using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;


public enum ScreenOrientation
{
    Portrait,
    Landscape
}

public class UIManger : Singleton<UIManger>
{

    //显示UI的Canvas
    private Transform _uiParent;


    //栈
    private Stack<BasePanel> _uiStack = new Stack<BasePanel>();

    /// <summary>
    /// 已经显示的UI
    /// </summary>
    private Dictionary<string, BasePanel> _uiDic = new Dictionary<string, BasePanel>();

    //UI操控组件
    private BasePanel _basePanel;

    //提示模块
    private TextMeshProUGUI _gameTipsMeshPro;
    // 动画参数，可以在 Inspector 面板中调整
    [Header("动画设置")]
    [Tooltip("移动的目标位置（相对于初始位置的偏移）")]
    public Vector3 moveOffset = new Vector3(0, 400, 0);
    [Tooltip("动画持续时间（秒）")]
    public float duration = 1.5f;
    [Tooltip("动画延迟开始时间（秒）")]
    public float delay = 0f;
    [Tooltip("动画曲线")]
    public Ease easeType = Ease.OutQuad;
    public void GameTips(string str, float duration = 1.5f, Action ac = null)
    {

        //如果重复执行就杀死之前的进程
        _gameTipsMeshPro.transform.DOKill();
        _gameTipsMeshPro.DOKill();
        _gameTipsMeshPro.gameObject.SetActive(false);
        // 将文本位置重置到初始位置
        _gameTipsMeshPro.transform.localPosition = Vector3.zero;

        //设置文本内容
        _gameTipsMeshPro.text = str;
        _gameTipsMeshPro.transform.SetAsLastSibling();

        // 将文本颜色重置为完全不透明
        _gameTipsMeshPro.color = new Color(_gameTipsMeshPro.color.r, _gameTipsMeshPro.color.g, _gameTipsMeshPro.color.b, 1f);
        _gameTipsMeshPro.gameObject.SetActive(true);
        // 2. 创建移动动画
        // DOBlendableLocalMoveBy 可以将移动叠加，避免与其他可能的移动动画冲突
        //var moveTween = _gameTipsMeshPro.transform.DOLocalMove(moveOffset, duration);
        // 3. 创建淡出动画
        // 将文本的 alpha 通道从 1 变为 0
        var fadeTween = _gameTipsMeshPro.DOFade(0f, duration);

        // 4. 设置动画参数
        // 为两个动画设置相同的缓动效果和延迟
        fadeTween.SetEase(easeType).SetDelay(delay);
        //缓慢消失,但是会导致不是很明显,先去掉
        //fadeTween.SetEase(easeType).SetDelay(delay);

        // 5. 动画完成后执行操作
        // 当淡出动画完成时，调用 OnAnimationComplete 方法
        fadeTween.OnComplete(() => { OnAnimationComplete(ac); });
    }

    private void OnAnimationComplete(Action ac = null)
    {
        //Debug.Log("动画播放完毕");
        // 动画播放完毕后，隐藏对象
        _gameTipsMeshPro.gameObject.SetActive(false);
        if (ac != null)
        {
            //  Debug.Log("动画播放完毕执行函数");
            ac();
        }

    }
    public UIManger()
    {
        _uiParent = GameObject.Find(SysConst.ASSET_UI_PARENTCANVAS).transform;

        _gameTipsMeshPro = GameObject.Find("GameTipsText").GetComponent<TextMeshProUGUI>();
        _gameTipsMeshPro.gameObject.SetActive(false);
    }

    /// <summary>
    /// 状态重置
    /// </summary>
    public void ResetState()
    {
        //foreach (var value in _uiDic.Values)
        //{
        //    Destroy(value.gameObject);
        //}

        ClearStack();
        ClearUIDic();
    }

    /// <summary>
    /// 进栈
    /// </summary>
    public void Push(string uiName, object msg = null)
    {
        Debug.Log(uiName + "进栈");
        //查询UI是否已经显示出来
        if (!IsConUI(uiName))
        {
            //如果没有发现目标UI则从缓存中单独实例化出来
            GameObject obj =
                ResourceManager.LoadUIAsset(uiName, _uiParent);
            AddUI(obj.GetComponent<BasePanel>());
        }



        _basePanel = GetUI(uiName);
        if (_basePanel != null)
        {
            //消息栏不参与进栈/遮罩处理/上一级处理

            //遮罩处理
            OperatorMask(_basePanel);
            //上一层级处理
            OperatorUpperLevelPush(_basePanel);
            //进栈
            PushStack(_basePanel);


            _basePanel.OnEnter(msg);
        }
        else
        {
            Debug.Log("UI组件为空");
        }
    }



    /// <summary>
    /// 带过渡效果的UI进栈
    /// </summary>
    public void PushTransition(string uiName, object msg = null)
    {
        Sequence pushSeq = DOTween.Sequence();
        pushSeq.AppendCallback(() => { Push(SysConst.ASSET_UI_LOADINGPANEL, "UseUnRealValue"); }).AppendInterval(0.5f)
               .AppendCallback(() => { Push(uiName, msg); }).SetUpdate(true);
    }

    /// <summary>
    /// 出栈
    /// </summary>
    public void Pop(object msg = null, bool isCloseHUD = false)
    {
        if (_uiStack.Count <= 0) return;
        Debug.Log("" + PeekStack().name);
        //如果栈内只有hud切面，并且不是关闭HUD，就停止出栈
        if (!isCloseHUD && _uiStack.Count == 1 && PeekStack().name == SysConst.ASSET_UI_HUDPANEL)
        {
            return;
        }
        BasePanel currentUI = PopStack();
        currentUI.OnExit();


        BasePanel upperLevelUI = PeekStack();
        OperatorMask(upperLevelUI);
        //上一层级UI处理
        OperatorUpperLevelPop(currentUI, upperLevelUI, msg);
      
    }

    /// <summary>
    /// 进栈时上一级处理
    /// </summary>
    private void OperatorUpperLevelPush(BasePanel uiPanel)
    {
        //处理上一层级
        if (_uiStack.Count > 0)
        {
            switch (uiPanel.UIInfo.Effect)
            {
                case UIEffectType.PauseUpperLevel:
                    PeekStack().OnPause();
                    break;
                case UIEffectType.DisableUpperLevel:
                    PopStack().OnExit();
                    break;
                case UIEffectType.DisableOthers:
                    foreach (BasePanel basePanel in _uiDic.Values)
                    {
                        if (basePanel.name != uiPanel.name)
                        {
                            basePanel.OnExit();
                        }
                    }

                    //清空栈
                    ClearStack();
                    break;
            }
        }
    }

    /// <summary>
    /// 出栈时上一级处理
    /// </summary>
    private void OperatorUpperLevelPop(BasePanel currentUI, BasePanel upperLevelUI, object msg)
    {
        switch (currentUI.UIInfo.Effect)
        {
            case UIEffectType.DisableUpperLevel:
                upperLevelUI.OnEnter(msg);
                break;
            case UIEffectType.DisableOthers:
                //UI设计中如果UI进栈之前移除了之前所有UI,当前UI界面不能有出栈操作按钮!
                //如果有出栈按钮,则在完成出栈后,由于栈内成员数量等于零,直接return返回!
                break;
            case UIEffectType.PauseUpperLevel:
                upperLevelUI.OnResume();
                break;
        }
    }

    /// <summary>
    /// 遮罩处理
    /// </summary>
    private void OperatorMask(BasePanel uiPanel)
    {

    }


    #region 容器管理

    /// <summary>
    /// 标记已经显示出来的UI资源
    /// </summary>
    /// <param name="ui"></param>
    private void AddUI(BasePanel ui)
    {

        if (_uiDic == null)
        {
            _uiDic = new Dictionary<string, BasePanel>();
        }
        else
        {
            if (!_uiDic.ContainsKey(ui.name))
            {
                _uiDic.Add(ui.name, ui);
            }
        }
    }

    /// <summary>
    /// 移除已经显示出来的UI资源
    /// </summary>
    /// <param name="uiName"></param>
    private void RemoveUI(string uiName)
    {
        if (_uiDic == null)
        {
            _uiDic = new Dictionary<string, BasePanel>();
        }
        else
        {
            if (_uiDic.ContainsKey(uiName))
            {
                _uiDic.Remove(uiName);
            }
        }
    }

    /// <summary>
    /// 获取已经显示出来的UI资源
    /// </summary>
    /// <param name="uiName"></param>
    private BasePanel GetUI(string uiName)
    {
        if (_uiDic.ContainsKey(uiName))
        {
            return _uiDic[uiName];
        }
        else
        {
            Debug.Log("目标UI不存在!");
            return null;
        }
    }

    /// <summary>
    /// 清空已经显示出来的UI资源
    /// </summary>
    private void ClearUIDic()
    {
        if (_uiDic != null)
        {
            _uiDic.Clear();
        }
    }

    /// <summary>
    /// 标记已经显示出来的UI资源
    /// </summary>
    /// <param name="uiName"></param>
    private bool IsConUI(string uiName)
    {
        if (_uiDic == null)
        {
            _uiDic = new Dictionary<string, BasePanel>();
            return false;
        }
        else
        {
            if (_uiDic.ContainsKey(uiName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    #endregion

    #region 栈管理

    /// <summary>
    /// UI进栈
    /// </summary>
    private void PushStack(BasePanel uiBase)
    {
        if (_uiStack == null)
        {
            _uiStack = new Stack<BasePanel>();
        }

        if (!_uiStack.Contains(uiBase))
        {
            _uiStack.Push(uiBase);
        }
    }

    /// <summary>
    /// UI出栈(获取栈顶信息)
    /// </summary>
    private BasePanel PopStack()
    {
        return _uiStack.Count <= 0 ? null : _uiStack.Pop();
    }

    /// <summary>
    /// 获取栈顶信息
    /// </summary>
    private BasePanel PeekStack()
    {
        return _uiStack.Count <= 0 ? null : _uiStack.Peek();
    }

    /// <summary>
    /// 清空栈
    /// </summary>
    private void ClearStack()
    {
        if (_uiStack == null) return;
        _uiStack.Clear();
    }

    #endregion
}
