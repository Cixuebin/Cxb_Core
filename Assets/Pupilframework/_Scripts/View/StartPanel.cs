

using PupilFramework;
using PupilFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.U2D;
public class StartPanel : BasePanel
{

    /// <summary>
    /// 开始按钮
    /// </summary>
    private GameObject _startBtn, _muteBtn;
    private GameObject _obj_DevelopmentImg;
    private bool _bool_isFirst = true;
    private Image _img_mute;
    /// <summary>
    /// 获取所需是图集
    /// </summary>
    private SpriteAtlas _atlas;
    /// <summary>
    /// 默认预制体可能是隐藏状态
    /// 组件初始化放到这里
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        //transform.FindRecursively  直接查找自己子物体下的对象
        _obj_DevelopmentImg = transform.FindRecursively("DevelopmentTeamImage").gameObject;
        _startBtn = RegisterBtnEventReturn("StartBtn", StartPanelBtnOnClick).gameObject;
        _img_mute = RegisterBtnEventReturn("MuteBtn", StartPanelBtnOnClick).GetComponent<Image>();
        _atlas = ResourceManager.LoadSpriteAtlas("DarkUI");
        RegisterBtnEvent("AboutBtn", StartPanelBtnOnClick);
        RegisterBtnEvent("DevelopmentTeamBtn", StartPanelBtnOnClick);
        RegisterBtnEvent("DevelopmentTeamCloseBtn", StartPanelBtnOnClick);
        // RegisterBtnEvent("RestartGameBtn", StartPanelBtnOnClick);
        //RegisterBtnEvent("RestartGameCloseBtn", StartPanelBtnOnClick);
        //RegisterBtnEvent("ConfirmButton", StartPanelBtnOnClick);
        // RegisterBtnEvent("CancelButton", StartPanelBtnOnClick);
        RegisterBtnEvent("PlayBtn", PlayBtnOnClick);
        //按照要求不用点击直接加载场景
        PlayBtnOnClick(null);
    }

    public override void OnEnter(object msg)
    {
        base.OnEnter(msg);
        Debug.Log("Set Audio");
        //AudioManager.Instance.Play("背景音乐");
        if (AudioManager.Instance.IsMute())
        {
            _img_mute.sprite = _atlas.GetSprite("AudioOff");
        }
        else
        {
            _img_mute.sprite = _atlas.GetSprite("AudioOn");
        }
        _obj_DevelopmentImg.gameObject.SetActive(false);
    }

    /// <summary>
    /// 加载异步场景
    /// </summary>
    /// <param name="go"></param>
    private void PlayBtnOnClick(GameObject go)
    {

        //SceneLoadManager.Instance.YooLoadScene(SysConst.ASSET_SCENE_MAIN, M =>
        //{
        //	UIManger.Instance.Push(SysConst.ASSET_UI_HUDPANEL);
        //});
        SceneLoadManager.Instance.LoadSceneAsync(SysConst.ASSET_SCENE_MAINSCENE, M =>
        {
            UIManger.Instance.Push(SysConst.ASSET_UI_HUDPANEL);
            //UIManger.Instance.Pop();
        });


    }
    private void Update()
    {


    }
    ///// <summary>
    ///// 初始化UI数据模型信息
    ///// </summary>
    //protected override void InitUIData()
    //   {
    //       UIInfo = new Model_UIDataModel
    //       {
    //           //默认遮罩自动响应出栈
    //           IsMaskBack = false,
    //           //默认面板类型
    //           Show = UIShowType.PopScreen,
    //           //默认遮罩透明度
    //           Mask = UIMaskType.NoEffect,
    //           //默认影响上一级类型
    //           Effect = UIEffectType.DisableOthers
    //       };
    //   }

    protected override void RegisterMessage()
    {

    }
    private void StartPanelBtnOnClick(GameObject obj)
    {
        switch (obj.name)
        {

            case "MuteBtn":
                if (AudioManager.Instance.Mute())
                {
                    _img_mute.sprite = _atlas.GetSprite("AudioOff");
                }
                else
                {
                    _img_mute.sprite = _atlas.GetSprite("AudioOn");
                }
                break;

            default:
                break;
        }
    }


}