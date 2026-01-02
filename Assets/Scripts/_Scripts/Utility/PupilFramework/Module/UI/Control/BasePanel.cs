using UnityEngine;
using DG.Tweening;

public abstract class BasePanel : MonoBehaviour
{
    #region UI管理

    public Model_UIDataModel UIInfo;

    /// <summary>
    /// 消息体
    /// </summary>
    protected MessageData MsgData = new MessageData(null, null);

    /// <summary>
    /// 组管理器
    /// </summary>
    private CanvasGroup _canvasGroup;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            gameObject.AddComponent<CanvasGroup>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        InitUIData();
        RegisterMessage();
    }

    /// <summary>
    /// 初始化UI数据模型信息
    /// </summary>
    protected virtual void InitUIData()
    {
        UIInfo = new Model_UIDataModel
        {
            //默认遮罩自动响应出栈
            IsMaskBack = true,
            //默认面板类型
            Show = UIShowType.PopScreen,
            //默认遮罩透明度
            Mask = UIMaskType.Half,
            //默认影响上一级类型
            Effect = UIEffectType.PauseUpperLevel
        };
    }

    /// <summary>
    /// 开启
    /// </summary>
    public virtual void OnEnter(object msg)
    {
        //显示UI
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        //移动到最底端
        transform.SetAsLastSibling();
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public virtual void OnPause()
    {
        //遮罩已经有暂停作用了
    }

    /// <summary>
    /// 恢复
    /// </summary>
    public virtual void OnResume()
    {
        //移动到最底端
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        transform.SetAsLastSibling();
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public virtual void OnExit()
    {
        //显示UI
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    /// <summary>
    /// 消息的注册
    /// </summary>
    protected abstract void RegisterMessage();

    #endregion

    #region 事件注册

    /// <summary>
    /// 注册按钮事件
    /// </summary>
    /// <param name="buttonName">按钮节点名称</param>
    /// <param name="delHandle">委托：需要注册的方法</param>
    protected void RegisterBtnEvent(string buttonName, EventTriggerListener.VoidDelegate delHandle, bool Sacle = true)
    {
        GameObject goButton = transform.FindRecursively(buttonName).gameObject;
        //给按钮注册事件方法
        if (goButton != null)
        {
            EventTriggerListener.Get(goButton).onClick += delHandle;
            if (Sacle)
            {
                EventTriggerListener.Get(goButton).onClick += OnClickScale;
            }
            //默认点击遮罩没有音效
            if (buttonName.Equals("MaskBtn")) return;
            EventTriggerListener.Get(goButton).onClick += OnClickAudio;
        }
    }

    /// <summary>
    /// 注册按钮事件(自定义事件类型)
    /// </summary>
    /// <param name="buttonName">按钮节点名称</param>
    /// <param name="delHandle">委托：需要注册的方法</param>
    /// <param name="type">事件类型</param>
    protected void RegisterBtnEvent(string buttonName, EventTriggerListener.VoidDelegate delHandle, UGUIEventType type)
    {
        GameObject goButton = transform.FindRecursively(buttonName).gameObject;
        //给按钮注册事件方法
        if (goButton != null)
        {
            CustomClickEvent(goButton, delHandle, type);
        }
    }

    /// <summary>
    /// 注册按钮事件(自定义按键声音)
    /// </summary>
    /// <param name="transformParent">父类</param>
    /// <param name="buttonName">按钮节点名称</param>
    /// <param name="delHandle"></param>
    /// <param name="audioHandle">音效事件</param>
    protected void RegisterBtnEvent(Transform transformParent, string buttonName,
        EventTriggerListener.VoidDelegate delHandle, EventTriggerListener.VoidDelegate audioHandle)
    {
        GameObject goButton = transformParent.FindRecursively(buttonName).gameObject;
        //给按钮注册事件方法
        if (goButton != null)
        {
            EventTriggerListener.Get(goButton).onClick += delHandle;
            //默认点击遮罩没有音效
            if (buttonName.Equals("MaskBtn")) return;
            EventTriggerListener.Get(goButton).onClick += audioHandle;
        }
    }

    /// <summary>
    /// 注册按钮事件(自定义按键声音)(自定义事件类型)
    /// </summary>
    /// <param name="transformParent">父类</param>
    /// <param name="buttonName">按钮节点名称</param>
    /// <param name="delHandle"></param>
    /// <param name="audioHandle">音效事件</param>
    /// <param name="type">事件类型</param>
    protected void RegisterBtnEvent(Transform transformParent, string buttonName,
        EventTriggerListener.VoidDelegate delHandle, EventTriggerListener.VoidDelegate audioHandle,
        UGUIEventType type)
    {
        GameObject goButton = transformParent.FindRecursively(buttonName).gameObject;
        //给按钮注册事件方法
        if (goButton != null)
        {
            CustomClickEvent(goButton, delHandle, type);
            //默认点击遮罩没有音效
            if (buttonName.Equals("MaskBtn")) return;
            CustomClickEvent(goButton, audioHandle, type);
        }
    }

    /// <summary>
    /// 注册按钮事件(从指定父类查找按钮)
    /// </summary>
    /// <param name="transformParent">父类</param>
    /// <param name="buttonName">按钮节点名称</param>
    /// <param name="delHandle">委托：需要注册的方法</param>
    protected void RegisterBtnEvent(Transform transformParent, string buttonName,
        EventTriggerListener.VoidDelegate delHandle)
    {
        GameObject goButton = transformParent.FindRecursively(buttonName).gameObject;
        //给按钮注册事件方法
        if (goButton != null)
        {
            EventTriggerListener.Get(goButton).onClick += delHandle;
            //默认点击遮罩没有音效
            if (buttonName.Equals("MaskBtn")) return;
            EventTriggerListener.Get(goButton).onClick += OnClickAudio;
        }
    }

    /// <summary>
    /// 注册按钮事件(自定义按键声音)
    /// </summary>
    /// <param name="buttonName">按钮节点名称</param>
    /// <param name="delHandle"></param>
    /// <param name="audioHandle">音效事件</param>
    protected void RegisterBtnEvent(string buttonName,
        EventTriggerListener.VoidDelegate delHandle, EventTriggerListener.VoidDelegate audioHandle)
    {
        GameObject goButton = transform.FindRecursively(buttonName).gameObject;
        //给按钮注册事件方法
        if (goButton != null)
        {
            EventTriggerListener.Get(goButton).onClick += delHandle;
            //默认点击遮罩没有音效
            if (buttonName.Equals("MaskBtn")) return;
            EventTriggerListener.Get(goButton).onClick += audioHandle;
        }
    }

    /// <summary>
    /// 注册按钮事件(返回值)
    /// </summary>
    /// <param name="buttonName">按钮节点名称</param>
    /// <param name="delHandle">委托：需要注册的方法</param>
    protected Transform RegisterBtnEventReturn(string buttonName, EventTriggerListener.VoidDelegate delHandle, bool Sacle = true)
    {
        GameObject goButton = transform.FindRecursively(buttonName).gameObject;
        //给按钮注册事件方法
        if (goButton != null)
        {
            EventTriggerListener.Get(goButton).onClick += delHandle;
            if (Sacle)
            {
                EventTriggerListener.Get(goButton).onClick += OnClickScale;
            }
            //默认点击遮罩没有音效
            if (buttonName.Equals("MaskBtn"))
            {
                return goButton.transform;
            }

            EventTriggerListener.Get(goButton).onClick += OnClickAudio;
            return goButton.transform;
        }

        return null;
    }

    /// <summary>
    /// 注册按钮事件(同名按键注册)
    /// </summary>
    /// <param name="buttonName">按钮节点名称</param>
    /// <param name="delHandle">委托：需要注册的方法</param>
    protected void RegisterAllBtnEvents(string buttonName, EventTriggerListener.VoidDelegate delHandle, bool Sacle = true)
    {
        var btns = transform.FindRecursivelyArray(buttonName);

        for (int i = 0; i < btns.Length; i++)
        {
            if (btns[i] != null)
            {
                EventTriggerListener.Get(btns[i].gameObject).onClick += delHandle;
                //默认点击遮罩没有音效
                if (buttonName.Equals("MaskBtn")) return;
                EventTriggerListener.Get(btns[i].gameObject).onClick += OnClickAudio;
            }
        }
    }

    /// <summary>
    /// 注册按钮事件(同名按键注册)(自定义事件类型)
    /// </summary>
    /// <param name="buttonName">按钮节点名称</param>
    /// <param name="delHandle">委托：需要注册的方法</param>
    /// <param name="type">事件类型</param>
    protected void RegisterAllBtnEvents(string buttonName, EventTriggerListener.VoidDelegate delHandle,
        UGUIEventType type)
    {
        var btns = transform.FindRecursivelyArray(buttonName);

        for (int i = 0; i < btns.Length; i++)
        {
            if (btns[i] != null)
            {
                CustomClickEvent(btns[i].gameObject, delHandle, type);
            }
        }
    }

    /// <summary>
    /// 自定义事件
    /// </summary>
    private void CustomClickEvent(GameObject goButton, EventTriggerListener.VoidDelegate delHandle, UGUIEventType type)
    {
        switch (type)
        {
            case UGUIEventType.OnClick:
                EventTriggerListener.Get(goButton).onClick += delHandle;
                break;
            case UGUIEventType.OnUp:
                EventTriggerListener.Get(goButton).onUp += delHandle;
                break;
            case UGUIEventType.OnDown:
                EventTriggerListener.Get(goButton).onDown += delHandle;
                break;
            case UGUIEventType.OnEnter:
                EventTriggerListener.Get(goButton).onEnter += delHandle;
                break;
            case UGUIEventType.OnExit:
                EventTriggerListener.Get(goButton).onExit += delHandle;
                break;
            case UGUIEventType.OnSelect:
                EventTriggerListener.Get(goButton).onSelect += delHandle;
                break;
            case UGUIEventType.OnUpdateSelect:
                EventTriggerListener.Get(goButton).onUpdateSelect += delHandle;
                break;
        }
    }

    #endregion

    #region UI栈扩展

    /// <summary>
    /// UI进栈
    /// </summary>
    /// <param name="UIName">名称</param>
    /// <param name="Msg">消息体</param>
    protected void ShowUI(string UIName, object Msg = null)
    {
        UIManger.Instance.Push(UIName, Msg);
    }



    /// <summary>
    /// UI进栈
    /// </summary>
    /// <param name="UIName">名称</param>
    /// <param name="Msg">消息体</param>
    protected void ShowUITransition(string UIName, object Msg = null)
    {
        UIManger.Instance.PushTransition(UIName);
    }

    /// <summary>
    /// 返回按钮(出栈)
    /// </summary>
    protected virtual void OnClickBack(GameObject obj)
    {
        //出栈
        //UIManger.Instance.Pop();
    }

    #endregion

    #region 自定义扩展

    /// <summary>
    /// 播放按钮点击音效
    /// </summary>
    protected virtual void OnClickAudio(GameObject obj)
    {
        AudioManager.Instance.PlayOneShot(SysConst.ASSET_AUDIO_CLICK);
    }
    protected virtual void OnClickScale(GameObject obj)
    {
        obj.transform.DOScale(1, 0);
        obj.transform.DOScale(1.2f, 0.1f).OnComplete(() =>
        {
            obj.transform.DOScale(1, 0.1f).SetUpdate(true);
        }).SetUpdate(true);
    }
    ///// <summary>
    ///// 显示语言
    ///// </summary>
    ///// <param name="id"></param>
    //public string Show(string id)
    //{
    //    string strResult = string.Empty;

    //    strResult = LauguageMgr.GetInstance().ShowText(id);
    //    return strResult;
    //} 

    #endregion
}
