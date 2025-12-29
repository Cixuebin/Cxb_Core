
using DG.Tweening;
using PupilFramework;
using PupilFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    /// <summary>
    /// 进度条
    /// </summary>
    private Image _loadingImage;

    ///// <summary>
    ///// 进度文本
    ///// </summary>
    //private TextMeshProUGUI _loadingDotText;

    /// <summary>
    /// 动画组件
    /// </summary>
    private Tweener _loadTextTwr;

    /// <summary>
    /// 黑屏
    /// </summary>
    private Image _blackViewImg;

    /// <summary>
    /// 黑屏队列
    /// </summary>
    private Sequence _blackViewSeq;

    /// <summary>
    /// 背景
    /// </summary>
    private GameObject _backGround;

    private TextMeshProUGUI _loadingText;

    protected override void Awake()
    {
        base.Awake();
        _blackViewImg = transform.FindComponent<Image>("BlackView");
        _backGround = transform.FindRecursively("BackGround").gameObject;
        _loadingImage = transform.FindRecursively("LoadingImg").GetComponent<Image>();
        _loadingText = transform.FindRecursively("LoadingDotText").GetComponent<TextMeshProUGUI>();
        //_loadTextTwr = DOTween.To(() => _loadingDotText.text, x => _loadingDotText.text = x,
        //    ".....", 1.5f).SetUpdate(true).SetLoops(-1, LoopType.Restart).SetAutoKill(false).Pause();

    }

    protected override void RegisterMessage()
    {
        MsgCenter.AddMsgListener(SysConst.MVC_LOADINGPANEL, p =>
        {
            switch (p.Key)
            {
                case SysConst.MVC_LOADINGPANEL_UPDATEPRORESS:
                    DisplayProgress((int)p.Values);
                    break;
                case SysConst.MVC_LOADINGPANEL_ENDPRORESS:
                    _loadingText.text = p.Values.ToString();
                    //_loadingText.text = "加载完毕即将进入场景";
                    //场景加载完成自己出站.暂时取消,由加载时候的回调出站
                    // UIManger.Instance.Pop();

                    break;
            }
        });
    }

    public override void OnEnter(object msg)
    {
        base.OnEnter(msg);
        //初始化
        _backGround.SetActive(false);
        _blackViewImg.gameObject.SetActive(true);
        _loadingImage.fillAmount = 0;
        _loadingText.text = "场景加载中请稍后~";
        //场景切换,使用真实进度条
        if (msg == null)
        {
            Debug.Log("Loading场景真实进度条" + msg);
            _blackViewSeq = DOTween.Sequence();
            _blackViewSeq.Append(_blackViewImg.DOFade(1, 0.5f)).AppendCallback(() =>
                                                                            {
                                                                                _backGround.SetActive(true);
                                                                            }).Append(_blackViewImg.DOFade(0, 1)).SetUpdate(true);
        }
        else
        {
            Debug.Log("Loading场景虚拟进度条" + msg);
            //UI切换,使用虚拟进度条
            _blackViewSeq = DOTween.Sequence();
            _blackViewSeq.Append(_blackViewImg.DOFade(1, 0.5f)).AppendCallback(() =>
                                                                            {
                                                                                _backGround.SetActive(true);
                                                                            }).Append(_blackViewImg.DOFade(0, 1)).
                                                                            Append(DOTween.To(() => _loadingImage.fillAmount, x => _loadingImage.fillAmount = x / 100, 100, 3)).SetUpdate(true).OnComplete(() =>
                                                                            {
                                                                            });
        }
    }
    public override void OnPause()
    {
        base.OnPause();
        OnClickBack(this.gameObject);
    }
    public override void OnExit()
    {
        base.OnExit();
        _loadTextTwr.Pause();
    }

    protected override void InitUIData()
    {
        UIInfo = new Model_UIDataModel
        {
            //默认遮罩自动响应出栈
            IsMaskBack = false,
            //默认面板类型
            Show = UIShowType.FullScreen,
            //默认遮罩透明度
            Mask = UIMaskType.Complete,
            //默认影响上一级类型
            Effect = UIEffectType.DisableOthers
        };
    }

    /// <summary>
    /// 进度展示
    /// </summary>
    /// <param name="value"></param>
    private void DisplayProgress(float value)
    {
        // _loadingText.text = "当前进度" + (value / 100);
        _loadingImage.fillAmount = value / 100;
    }
}