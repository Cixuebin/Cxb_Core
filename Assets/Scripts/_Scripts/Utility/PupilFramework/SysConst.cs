
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
    //角色信息
    public const string MVC_PLAYER = "MVC_PLAYER";
    //移动
    public const string MVC_PLAYER_MOVE = "MVC_PLAYER_MOVE";
    //跳跃
    public const string MVC_PLAYER_JUMP = "MVC_PLAYER_JUMP";
    //冲刺
    public const string MVC_PLAYER_SPRINT = "MVC_PLAYER_SPRINT";
    //x旋转
    public const string MVC_PLAYER_LOOK = "MVC_PLAYER_LOOK";

    //功能
    public const string MVC_FUNCTION = "MVC_FUNCTION";
    /// <summary>
    /// 第一、二、人称视角切换
    /// </summary>
    public const string MVC_FUNCTION_FIRSTTHIRDSWITCH = "MVC_FUNCTION_FirstThirdSwitch";




    //用户信息改变
    public const string MVC_USERDATA = "MVC_USERDATA";
    /// <summary>
    ///用户登录状态改变
    /// </summary>
    public const string MVC_USERDATA_USERLOGINSTATUSCHANGE = "MVC_USERDATA_UserLoginStatusChange";

    #endregion

    //pos get

}
