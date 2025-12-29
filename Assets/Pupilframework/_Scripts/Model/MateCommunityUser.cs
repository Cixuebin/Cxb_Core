using System;
using UnityEngine;

[Serializable]
public class MateCommunityUser
{
    /// <summary>
    /// 用户手机号
    /// </summary>
    public string phone
    {
        get; set;
    } = null;

    /// <summary>
    /// 用户头像
    /// </summary>
    public string avatarUrl;

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string nickName;

    /// <summary>
    /// 用户密码
    /// </summary>
    public string password
    {
        get; set;
    } = null;

	/// <summary>
	/// 确认密码
	/// </summary>
	public string confirmPassword;
    /// <summary>
    /// 邮箱
    /// </summary>
    public string email;
    /// <summary>
    /// 手机验证码
    /// </summary>
    public string mobileVerificationCode;
	/// <summary>
	/// 用户余额
	/// </summary>
	public decimal balance;
    /// <summary>
    /// 用户虚拟货币
    /// </summary>
    public long gameCurrency;

    /// <summary>
    /// 注册时间字符串
    /// </summary>
    public String createTimeStr;

	/// <summary>
	/// 默认选中的宠物的数据名称
	/// </summary>
	public string defaultSelectedPetName;

	/// <summary>
	/// 默认选中的皮肤数据名称
	/// </summary>
	public string defaultSelectedSkinName;
    /// <summary>
    /// 是否登录
    /// </summary>
	public string loginStatus = "NOT";
	/// <summary>
	/// 是否实名认证 0 未实名,1 已实名
	/// </summary>
	public long isRealName;
}
