using UnityEngine;

namespace Http
{
    public class TokenUtils
    {
        /// <summary>
        /// Token 键
        /// </summary>
        public static string TokenKey = "Admin-Token";

        /// <summary>
        /// 设置 token 数据
        /// </summary>
        /// <param name="tokenValue">token值</param>
        public static void setToken(string tokenValue)
        {
            PlayerPrefs.SetString(TokenKey,tokenValue);
        }

        /// <summary>
        /// 获取 token 值
        /// </summary>
        /// <returns>token string</returns>
        public static string getToken()
        {
            return PlayerPrefs.GetString(TokenKey);
        }

        public static void removeToken()
        {
            PlayerPrefs.DeleteKey(TokenKey);
        }
    }
}