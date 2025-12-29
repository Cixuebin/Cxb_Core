
public static class SysConst
{
    #region 常用字段

    //玩家信息文本名称(加密密钥)
    public const string KEYFILE_PLAYINFO = "PlayerInfo.txt?encrypt=true&password=396275959";

    #endregion

    #region 资源名称

    //音频Bundle文件名称
    public const string ASSET_BUNDLE_AUDIO = "Audio";


    //场景名称
    public const string ASSET_SCENE_SHARE = "ShareScene";

    public const string ASSET_SCENE_MAINSCENE = "MainScene";



    //UI资源名称
    //Canvas/Panel

    public const string ASSET_UI_PARENTCANVAS = "UICanvas";
    public const string ASSET_UI_MASKPANEL = "MaskPanel";
    public const string ASSET_UI_LOADINGPANEL = "LoadingPanel";
    public const string ASSET_UI_STARTPANEL = "StartPanel";
    public const string ASSET_UI_HUDPANEL = "HUDPanel";
    /// <summary>
    /// 登录界面
    /// </summary>
    public const string ASSET_UI_LOGINPANEL = "LoginPanel";

    /// <summary>
    /// 操作说明.手册
    /// </summary>
    public const string ASSET_UI_OPERATIONMANUALPANEL = "OperationManualPanel";
    /// <summary>
    /// 个人中心
    /// </summary>
    public const string ASSET_UI_PERSONALCENTERPANEL = "PersonalCenterPanel";
    //音频资源名称

    /// <summary>
    /// 按键点击
    /// </summary>
    public const string ASSET_AUDIO_CLICK = "按键";



    /// <summary>
    /// 胜利音效
    /// </summary>
    public const string ASSET_AUDIO_BA_VICTORY = "Battle_Victory";


    #endregion

    #region PureMVC 通信常量

    //UI信息更新请求
    public const string MVC_LOADINGPANEL = "MVC_LoadingPanel";
    public const string MVC_LOADINGPANEL_UPDATEPRORESS = "MVC_LoadingPanel_UpdateProgress";
    public const string MVC_LOADINGPANEL_ENDPRORESS = "MVC_LoadingPanel_EndProgress";

    /// <summary>
    /// UI信息
    /// </summary>
    public const string MVC_UI = "MVC_UI";
    /// <summary>
    /// ui出栈
    /// </summary>
    public const string MVC_UI_POP = "MVC_UI_POP";

    //用户信息改变
    public const string MVC_USERDATA = "MVC_USERDATA";
    /// <summary>
    ///用户登录状态改变
    /// </summary>
    public const string MVC_USERDATA_USERLOGINSTATUSCHANGE = "MVC_USERDATA_UserLoginStatusChange";

    #endregion

    //pos get
    /// <summary>
    /// 获取用户登录状态
    /// </summary>
    public const string GET_USERLOGINSTATE = "/unity/api/mate_community/user/getUserInfo";

    /// <summary>
    /// 用户登录.注册
    /// </summary>
    public const string POST_USERLOGIN = "/unity/api/mate_community/user/loginOrRegister";

    /// <summary>
    /// 获取手机验证码
    /// </summary>
    public const string POST_GETPHONECODE = "/unity/api/mate_community/user/pushPhoneRegisterSmsCode";
}
