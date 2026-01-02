using Http;
using Newtonsoft.Json;
using ResEntity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameRoot : MonoBehaviour
{
    public static GameRoot instance = null;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        //配置信息初始化
        InitGameConfig();
        //初始动作
        UIManger.Instance.Push(SysConst.ASSET_UI_STARTPANEL);
    }

    /// <summary>
    /// 配置信息初始化,有什么需要配置的参数可以再次界面进行配置
    /// </summary>
    private void InitGameConfig()
    {
        UpdateUserLoginSatus();
    }
    /// <summary>
    /// 更新用户登录状态
    /// </summary>
    public void UpdateUserLoginSatus()
    {
        /// HttpRequest.Instance.GetRequest(SysConst.GET_USERLOGINSTATE, GetStateCallback);
    }
    private void GetStateCallback(ResData data)
    {
        if (data.code == 200)
        {
            //	Debug.Log("获取用户数据成功:" + data.data);
            Logger.Log("User login Success" + data.data);
            GameManager.UserData = JsonConvert.DeserializeObject<MateCommunityUser>(data.data.ToString());
            //MateCommunityUser user = JsonConvert.DeserializeObject<MateCommunityUser>(data.data.ToString());// JsonUtils.ToObject<MateCommunityUser>(data.data);
            //Debug.LogError(GameManager.UserData.nickName+"---"+GameManager.UserData.phone+"--头像地址:"+ GameManager.UserData.avatarUrl);
            //GameManager.gameCurrency = user.gameCurrency;
            GameManager.isLogin = true;
            //Debug.Log("登录成功发送消息:----" + SysConst.MVC_PERSONALCENTERPANEL_USERLOGINSTATUSCHANGE);
            InitAllData();
            MessageData md = new MessageData(SysConst.MVC_USERDATA_USERLOGINSTATUSCHANGE, null);
            MsgCenter.SendMessage(SysConst.MVC_USERDATA, md);

        }
        else
        {
            GameManager.isLogin = false;
            Debug.LogError("获取数据失败回调" + data.msg);

        }
    }
    /// <summary>
    /// 初始化所有数据信息,需要登录,所以需要放到初始化用户数据之后
    /// </summary>
    public void InitAllData()
    {
        Logger.Log("开始初始化所有数据");


    }
    /// <summary>
    /// 加载图片
    /// </summary>
    /// <param name="showImage"></param>
    /// <param name="avatarUrl"></param>
    /// <returns></returns>
    public IEnumerator ILoadImage(Image showImage, string avatarUrl)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(avatarUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                showImage.sprite = sprite;
                showImage.preserveAspect = true; // 可选：保持宽高比
                                                 //加入缓存
                GameManager.StoreSpriteDataDic[avatarUrl] = sprite;
            }
            else
            {
                Debug.LogError("Failed to load avatar: " + request.error);
            }
        }
    }
    /// <summary>
    /// 加载图片
    /// </summary>
    /// <param name="img"></param>
    /// <param name="mark"></param>
    public void LoadSprite(Image img, string url)
    {
        if (url != null)
        {
            if (GameManager.StoreSpriteDataDic.ContainsKey(url))
            {
                Logger.Log("已有缓存,直接获取~");
                img.sprite = GameManager.StoreSpriteDataDic[url];
            }
            else
            {
                StartCoroutine(ILoadImage(img, url));
                Debug.LogError("没有的图片,进行缓存:url=" + url);
            }

        }
        else
        {
            Debug.LogError("url is null");
        }

    }
    /// <summary>
    /// OnOperationManualPanel，由Input System自动调用
    /// </summary>
    /// <param name="value"></param>
    public void OnOperationManualPanel(InputValue value)
    {
        Debug.Log("OnOperationManualPanel");
        // JumpInput(value.isPressed);
        UIManger.Instance.Push(SysConst.ASSET_UI_OPERATIONMANUALPANEL);
    }
}

