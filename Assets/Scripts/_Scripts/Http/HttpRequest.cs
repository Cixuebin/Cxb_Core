using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ResEntity;
using UnityEngine;
using UnityEngine.Networking;

namespace Http
{
    public class HttpRequest : MonoBehaviour
    {
        private static HttpRequest _instance;

        public static HttpRequest Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<HttpRequest>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("HttpRequest");
                        _instance = obj.AddComponent<HttpRequest>();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="res">委托回调</param>
        public void GetRequest(string url, Action<ResData> res)
        {
            StartCoroutine(SendGetRequest(url, res));
        }

        /// <summary>
        /// 发送GET请求
        /// </summary>
        private static IEnumerator SendGetRequest(string url, Action<ResData> res)
        {
            //Debug.Log(url);
            UnityWebRequest webRequest = paramsGetPublic(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                ResData resData = new ResData();
                resData.code = 509;
                resData.msg = "网络请求错误";
                res?.Invoke(resData);
                webRequest.Dispose();
            }
            else
            {
                var resData = JsonConvert.DeserializeObject<ResData>(webRequest.downloadHandler.text);
                // Debug.LogError(url);
                res?.Invoke(resData);
                webRequest.Dispose();
            }

        }

        /**
         * 用于发起 GET 请求的基础公共方法
         */
        public static UnityWebRequest paramsGetPublic(string url)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(GameManager.baseUrl + url);
            DownloadHandlerBuffer dhb = new DownloadHandlerBuffer();
            webRequest.downloadHandler = dhb;
            webRequest.timeout = 60;
            // 判断是否存在 token
            if (TokenUtils.getToken() != null && TokenUtils.getToken() != "")
            {
                webRequest.SetRequestHeader("Authorization", "Bearer " + TokenUtils.getToken());
            }
            return webRequest;
        }

        /// <summary>
        /// POST 请求
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="data">数据实体</param>
        /// <param name="res">委托回调</param>
        /// <typeparam name="T">传入的类型实体</typeparam>
        public void PostRequest<T>(string url, T data, Action<ResData> res)
        {
            StartCoroutine(SendPostRequest(url, data, res));
        }
        private static IEnumerator SendPostRequest<T>(string url, T data, Action<ResData> res)
        {
            // 创建 json 请求 header 类型
            var reqHeader = new Dictionary<string, string> { { "Content-Type", "application/json;charset=UTF-8" } };
            // 请求设置
            using (UnityWebRequest webRequest = paramsPostPublic(url, data, reqHeader))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    ResData resData = new ResData();
                    resData.code = 509;
                    resData.msg = "网络请求错误";
                    res?.Invoke(resData);
                    webRequest.Dispose();
                }
                else
                {
                    var resData = JsonConvert.DeserializeObject<ResData>(webRequest.downloadHandler.text);
                    res?.Invoke(resData);
                    webRequest.Dispose();
                }

            }
        }
        ///// <summary>
        ///// 发送Post请求
        ///// </summary>
        ///// <param name="data">请求实体</param>
        ///// <param name="res">委托回调</param>
        //private static IEnumerator SendPostRequest<T>(string url, T data, Action<ResData> res)
        //      {
        //          // 创建 json 请求 header 类型
        //          var reqHeader = new Dictionary<string, string> { { "Content-Type", "application/json;charset=UTF-8" } };
        //          // 请求设置
        //          using (UnityWebRequest webRequest = paramsPostPublic(url, data, reqHeader))
        //          {
        //              yield return webRequest.SendWebRequest();

        //              ResData resData;
        //              if (webRequest.result != UnityWebRequest.Result.Success)
        //              {
        //                  resData = new ResData
        //                  {
        //                      code = 509,
        //                      msg = "网络请求错误"
        //                  };
        //              }
        //              else
        //              {
        //                  resData = JsonConvert.DeserializeObject<ResData>(webRequest.downloadHandler.text);
        //              }

        //              res?.Invoke(resData);
        //          }
        //      }

        /**
         * 用于发起 POST 请求的基础公共方法
         */
        public static UnityWebRequest paramsPostPublic<T>(string url, T data, Dictionary<string, string> header = null)
        {
            // 转换请求数据
            string formFields = JsonConvert.SerializeObject(data);
            Debug.Log(GameManager.baseUrl + url);
            // 请求设置
            UnityWebRequest webRequest = UnityWebRequest.Post(GameManager.baseUrl + url, formFields);

            // 转换 body
            byte[] body = Encoding.UTF8.GetBytes(formFields);
            webRequest.uploadHandler = new UploadHandlerRaw(body);
            DownloadHandlerBuffer dhb = new DownloadHandlerBuffer();
            webRequest.downloadHandler = dhb;
            webRequest.timeout = 60;
            // 判断是否存在 token
            if (TokenUtils.getToken() != null && TokenUtils.getToken() != "")
            {
                webRequest.SetRequestHeader("Authorization", "Bearer " + TokenUtils.getToken());
            }
            if (header != null)
            {
                //添加头部内容
                foreach (KeyValuePair<string, string> kvp in header)
                {
                    webRequest.SetRequestHeader(kvp.Key, kvp.Value);
                }
            }
            return webRequest;

        }


    }
}

