using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	/// <summary>
	/// 服务器 -> 基础请求地址
	/// </summary>
	//public static string baseUrl = "http://192.168.0.106:8004";//
	//public static string baseUrl = "http://192.168.0.103:8004";
	public static string baseUrl = "https://www.starsharkmeta.com/api";


	// //本地存储当前宠物名字 宠物暂时使用EasySave3
	public static string CurrentPetKey = "CurrentPet";//
													  // public static string CurrentPetName; 
	public static bool isUserServer = true;
	//虚拟货币 
	public static long gameCurrency;
	//是否显示UI
	public static bool isShowUI;
	public static string videoUrl = "https://stream.mux.com/4XYzhPXzqArkFI8d1vDsScBLD69Gh1b2.m3u8";

	public static bool isLogin = false;
	//当前对象的mark值
	public static string currentOpenObj;
	//用户信息
	public static MateCommunityUser UserData;

	/// <summary>
	/// 图片缓存容器
	/// </summary>
	public static Dictionary<string, Sprite> StoreSpriteDataDic = new Dictionary<string, Sprite>();


}
