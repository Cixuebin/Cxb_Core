using Http;
using ResEntity;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
	//获取验证码按钮
	public Button getMobileButton;
	//登录按钮
	public Button loginAndRegisterButton;
	//login界面的提示
	//private TextMeshProUGUI loginPanelTips;
	public TMP_Text loginPanelTips;
	//手机号输入框
	private TMP_InputField UserPhoneInputField;
	//手机验证码
	private TMP_InputField MobileCodeInputField;
	public TextMeshProUGUI countdownTimeText;
	// 倒计时时长（单位：秒）
	private const int countdownTime = 60;

	protected override void RegisterMessage()
	{


	}



	protected override void Awake()
	{
		base.Awake();
		//查找组件/添加点击事件

		getMobileButton = transform.FindRecursively("GetMobileCodeBtn").GetComponent<Button>();
		getMobileButton.onClick.AddListener(GetMobileButtonClickEvent);
		countdownTimeText = transform.FindRecursively("CountdownTimeText").GetComponent<TextMeshProUGUI>();
		loginPanelTips = transform.FindRecursively("LoginPanelTips_Text").GetComponent<TMP_Text>();
		UserPhoneInputField = transform.FindRecursively("UserPhoneInputField").GetComponent<TMP_InputField>();
		MobileCodeInputField = transform.FindRecursively("MobileCodeInputField").GetComponent<TMP_InputField>();
		loginAndRegisterButton = transform.FindRecursively("LoginAndRegisterButton").GetComponent<Button>();
		RegisterBtnEvent("LoginAndRegisterButton", LoginAndRegisterBtnOnClick);
		RegisterBtnEvent("CloseButton", CloseButtonOnClick);

	}

	private void CloseButtonOnClick(GameObject go)
	{
		UIManger.Instance.Pop();
	}

	//点击登录/注册
	private void LoginAndRegisterBtnOnClick(GameObject go)
	{
		loginAndRegisterButton.enabled = false;
		// 创建用户实体类
		MateCommunityUser user = new MateCommunityUser();
		// 装载数据
		user.phone = UserPhoneInputField.text;
		user.mobileVerificationCode = MobileCodeInputField.text;
		Debug.Log(user.phone + "--" + user.password);
		// 发起登录请求
		//HttpRequest.Instance.PostRequest(SysConst.POST_USERLOGIN, user, LoginRequestRes);
	}
	//登录注册的回调
	private void LoginRequestRes(ResData data)
	{

		if (data.code == 200)
		{
			Debug.Log("登录/注册成功回调");
			// Debug.LogError(JsonUtility.ToJson(data.data));

			// 请求成功，获取到 token
			string token = data.data["token"].ToString();
			// 记录 token
			TokenUtils.setToken(token);
			GameRoot.instance.UpdateUserLoginSatus();
			UIManger.Instance.Pop();

			LoginSuccessful();
		}
		else
		{
			Debug.Log("登录失败回调:" + data.msg);
			loginAndRegisterButton.enabled = true;
		}
		loginPanelTips.text = data.msg;
	}
	//登录注册成功
	public void LoginSuccessful()
	{

	}
	public override void OnEnter(object msg)
	{
		base.OnEnter(msg);
	}
	protected override void InitUIData()
	{
		base.InitUIData();
	}
	//获取手机验证码
	private void GetMobileButtonClickEvent()
	{
		Debug.Log("获取短信验证码,手机号:" + UserPhoneInputField.text);
		////本地测试倒计时
		//getMobileButton.interactable = false;
		//StartCoroutine(CountdownCoroutine());
		// 创建用户实体类
		MateCommunityUser user = new MateCommunityUser();
		// 装载数据
		user.phone = UserPhoneInputField.text;
		//暂时没有地址
		//HttpRequest.Instance.PostRequest(SysConst.POST_GETPHONECODE, user, GetPhoneCodeDataCallback);
	}
	/// <summary>
	/// 获取手机验证码
	/// </summary>
	/// <param name="data"></param>
	private void GetPhoneCodeDataCallback(ResData data)
	{
		if (data.code == 200)
		{
			Debug.Log("获取数据成功回调");
			getMobileButton.interactable = false;
			StartCoroutine(CountdownCoroutine());
		}
		loginPanelTips.text = data.msg;
	}
	// 倒计时协程,60秒才能再次获取
	private IEnumerator CountdownCoroutine()
	{
		int remainingTime = countdownTime;

		while (remainingTime > 0)
		{
			// 更新按钮文本为剩余时间
			if (countdownTimeText != null)
			{
				countdownTimeText.text = $"重新发送({remainingTime})";
			}

			// 等待1秒
			yield return new WaitForSeconds(1f);

			// 减少剩余时间
			remainingTime--;
		}

		// 倒计时结束，恢复按钮状态
		if (countdownTimeText != null)
		{
			countdownTimeText.text = "获取验证码";
		}
		getMobileButton.interactable = true;
	}
}
