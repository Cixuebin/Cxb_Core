using LitJson;

namespace ResEntity
{
    public class ResData
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int code;
        /// <summary>
        /// 订单编号
        /// </summary>
        public int orderId;
		/// <summary>
		/// 消息提示
		/// </summary>
		public string msg;

        /// <summary>
        /// 数据
        /// </summary>
        public JsonData data;

        public static ResData ToResData(string text)
        {
            JsonData jsonData = JsonMapper.ToObject(text);
            int code = int.Parse(jsonData["code"].ToString());
            string msg = jsonData["msg"].ToString();
            JsonData resData = jsonData["data"];
            ResData data = new ResData()
            {
                code = code,
                msg = msg,
                data = resData
            };
            return data;
        }
    }
}