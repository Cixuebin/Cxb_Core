
using Http;
using PupilFramework;
using ResEntity;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersonalCenterPanel : BasePanel
{
	//login界面的提示
	private TextMeshProUGUI loginStatusText;
	private TextMeshProUGUI userPhoneText;
	private TextMeshProUGUI userNameText;
	//头像
	private Image userAvatar;
	//设置用户名按钮
	//private Transform setNameButton;
	//退出登录按钮
	private Transform logOutButton;
	[Header("检查配置")]
	[Tooltip("支付检查持续的总时长（分钟）")]
	private int totalDurationMinutes = 5;

	[Tooltip("每次检查的时间间隔（秒）")]
	private float intervalSeconds = 1.0f;

	// 私有变量，用于控制协程
	private Coroutine checkCoroutine;
	protected override void RegisterMessage()
	{

	}




	protected override void Awake()
	{
		base.Awake();
		//登录按钮事件
		RegisterBtnEvent("LoginStatusButton", LoginStatusBtnOnClickEvent);
		//关闭事件
		RegisterBtnEvent("CloseBtn", CloseBtnOnClickEvent);

		//退出登录按钮事件 PurchaseHistoryButton
		RegisterBtnEvent("LogOutButton", LogOutButtonOnClickEvent);
		logOutButton = transform.FindRecursively("LogOutButton");
		//修改用户名
		RegisterBtnEvent("SetUserNameButton", SetUserNameButtonOnClickEvent);

		//打开操作说明 OperationManualButton
		RegisterBtnEvent("OperationManualButton", OperationManualButtonOnClickEvent);


		loginStatusText = transform.FindRecursively("LoginStatusText").GetComponent<TextMeshProUGUI>();
		userPhoneText = transform.FindRecursively("UserPhoneText").GetComponent<TextMeshProUGUI>();
		userNameText = transform.FindRecursively("UserNameText").GetComponent<TextMeshProUGUI>();
		userAvatar = transform.FindRecursively("UserAvatarImage").GetComponent<Image>();
	}





	private void OperationManualButtonOnClickEvent(GameObject go)
	{
		UIManger.Instance.Push(SysConst.ASSET_UI_OPERATIONMANUALPANEL);
	}


	private void SetUserAvatarCallback(ResData data)
	{
		Debug.Log("选择头像回调");
		if (data.code == 200)
		{
			Debug.Log(data.data);
			string url = LitJson.JsonMapper.ToJson(data.data);
			//Debug.LogError(url);
			//Application.OpenURL(url);
			string cleanUrl = url.Trim('"', '\'').Trim();

#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL环境 - 使用JS新标签页打开
        Application.ExternalCall("window.open", cleanUrl, "_blank");
#else
			// 其他平台 - 使用传统方式
			Application.OpenURL(cleanUrl);
#endif
			// 如果已经有一个协程在运行，先停止它
			if (checkCoroutine != null)
			{
				StopCoroutine(checkCoroutine);
			}
			//开始检测用户是否上传成功
			checkCoroutine = StartCoroutine(PerformChecksCoroutine());
		}
		Debug.Log(data.msg);
	}
	/// <summary>
	/// 执行检查的协程,持续检查头像是否替换成功
	/// </summary>
	private IEnumerator PerformChecksCoroutine()
	{
		// 计算检查结束的时间点
		float endTime = Time.time + (totalDurationMinutes * 60f);

		// 循环直到达到结束时间
		while (Time.time < endTime)
		{
			// 1. 发送一次你的 GET 请求
			// 注意：这里我们只发送请求，不等待它返回。你的 UserBuyCallback 会在请求完成时被自动调用。
			//Debug.Log($"判断是否上传成功...");
			//	HttpRequest.Instance.GetRequest(SysConst.GET_CHECKAVATARUPDATED, UserPaymentCallback);

			// 2. 等待指定的时间间隔
			// yield return new WaitForSeconds 会暂停协程的执行，等待指定秒数后再继续下一次循环
			yield return new WaitForSeconds(intervalSeconds);
		}

		//Debug.Log("判断是否上传成功时间结束。");
		//	UIManger.Instance.GameTips("超时");
		checkCoroutine = null; // 协程正常结束，清空引用

	}

	private void UserPaymentCallback(ResData data)
	{

		if (data.code == 200)
		{
			Debug.Log(data.data);
			bool t_result = (bool)data.data;
			if (!t_result)
			{
				UIManger.Instance.GameTips(data.msg);
			}
			StopCoroutine(checkCoroutine);
			checkCoroutine = null; // 协程正常结束，清空引用
			Debug.LogError("头像替换完成,这个写更新的逻辑");
			//GameRoot.instance.UpdateUserLoginSatus();

		}
		else
		{

			//UIManger.Instance.GameTips(data.msg);
		}
	}
	private void SetUserNameButtonOnClickEvent(GameObject go)
	{
		if (GameManager.isLogin)
		{
			//打开修改用户名界面
			//UIManger.Instance.Push(SysConst.ASSET_UI_SETUSERNAMEPANEL);

		}
		else
		{
			UIManger.Instance.GameTips("请先登录!");
		}
	}


	private void LogOutButtonOnClickEvent(GameObject go)
	{
		//退出登录测试用
		//HttpRequest.Instance.PostRequest(SysConst.POST_USEROUT, new MateCommunityUser(), UserLoginOut);
	}

	private void UserLoginOut(ResData data)
	{
		if (data.code == 200)
		{
			// 删除 token 数据
			TokenUtils.removeToken();
			Debug.Log("删除 token 数据");
			GameManager.isLogin = false;

			//MessageData md = new MessageData(SysConst.MVC_PERSONALCENTERPANEL_USERLOGINSTATUSCHANGE, null);
			//MsgCenter.SendMessage(SysConst.MVC_PERSONALCENTERPANEL, md);
			UIManger.Instance.Pop();
			UIManger.Instance.GameTips("账号已退出");
		}
	}



	private void CloseBtnOnClickEvent(GameObject go)
	{
		UIManger.Instance.Pop();
	}
	/// <summary>
	/// 登录状态事件,如果没有登录打开登录Panel
	/// </summary>
	/// <param name="go"></param>
	private void LoginStatusBtnOnClickEvent(GameObject go)
	{
		if (GameManager.isLogin) return;
		//UIManger.Instance.Push(SysConst.ASSET_UI_LOGINPANEL);
	}

	public override void OnEnter(object msg)
	{
		base.OnEnter(msg);
		// 如果已经有一个协程在运行，先停止它
		if (checkCoroutine != null)
		{
			StopCoroutine(checkCoroutine);
		}
		SetLoginStatus();
	}
	public void SetLoginStatus()
	{
		//Debug.Log("收到登录成功的消息:----进行UI显示" );
		if (GameManager.isLogin)
		{
			//领导需求,登录后不需要显示
			//loginStatusText.text = "已登录";
			userPhoneText.text = "账号:" + GameManager.UserData.phone;
			userNameText.text = "昵称:" + GameManager.UserData.nickName; ;
			loginStatusText.transform.parent.gameObject.SetActive(false); ;
			//setNameButton.gameObject.SetActive(true);
			logOutButton.gameObject.SetActive(true);

			GameRoot.instance.LoadSprite(userAvatar, GameManager.UserData.avatarUrl);
			//StartCoroutine(LoadAvatar());
		}
		else
		{
			loginStatusText.transform.parent.gameObject.SetActive(true);
			//setNameButton.gameObject.SetActive(false);
			logOutButton.gameObject.SetActive(false);
			loginStatusText.text = "未登录";
			userPhoneText.text = "账号:";
			userNameText.text = "昵称:";
		}

	}
	IEnumerator LoadAvatar()
	{
		using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(GameManager.UserData.avatarUrl))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.Success)
			{
				Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
				Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
				userAvatar.sprite = sprite;
				userAvatar.preserveAspect = true; // 可选：保持宽高比
			}
			else
			{
				Debug.LogError("Failed to load avatar: " + request.error);
			}
		}
	}
}
