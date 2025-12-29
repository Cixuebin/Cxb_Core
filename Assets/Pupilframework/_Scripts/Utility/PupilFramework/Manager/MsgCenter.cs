using System;
using System.Collections.Generic;

    public class MsgCenter
    {
     public static MessageData messageData = new MessageData("", null);
    //委托：消息传递
    public delegate void MessageHandler(MessageData kv);

        //消息中心缓存集合
        //<string : 数据大的分类，MessageHandler 数据执行委托>
        public static Dictionary<string, MessageHandler>
            _dicMessages = new Dictionary<string, MessageHandler>();

        /// <summary>
        /// 增加消息的监听。
        /// </summary>
        /// <param name="messageType">消息分类</param>
        /// <param name="handler">消息委托</param>
        public static void AddMsgListener(string messageType, MessageHandler handler)
        {
            if (!_dicMessages.ContainsKey(messageType))
            {
                _dicMessages.Add(messageType, null);
            }

            _dicMessages[messageType] += handler;
        }

        /// <summary>
        /// 取消消息的监听
        /// </summary>
        /// <param name="messageType">消息分类</param>
        /// <param name="handele">消息委托</param>
        public static void RemoveMsgListener(string messageType, MessageHandler handele)
        {
            if (_dicMessages.ContainsKey(messageType))
            {
                _dicMessages[messageType] -= handele;
            }
        }

        /// <summary>
        /// 移除消息的监听
        /// </summary>
        /// <param name="messageType">消息分类</param>
        public static void RemoveMsgListener(string messageType)
        {
            if (_dicMessages.ContainsKey(messageType))
            {
                Delegate[] delArray = _dicMessages[messageType].GetInvocationList();
                if (delArray.Length>0)
                {
                    for (int i = 0; i < delArray.Length; i++)
                    {
                        _dicMessages[messageType] -= delArray[i] as MessageHandler;
                    }
                }
                _dicMessages.Remove(messageType);
            }
        }

        /// <summary>
        /// 取消所有指定消息的监听
        /// </summary>
        public static void ClearALLMsgListener()
        {
            if (_dicMessages != null)
            {
                _dicMessages.Clear();
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageType">消息的分类</param>
        /// <param name="kv">键值对(对象)</param>
        public static void SendMessage(string messageType, MessageData kv)
        {
            if (_dicMessages.TryGetValue(messageType, out var msgHandler))
            {
                if (msgHandler != null)
                {
                    //调用委托
                    msgHandler(kv);
                }
            }
        }
    }

    /// <summary>
    /// 键值更新对
    /// 功能： 配合委托，实现委托数据传递
    /// </summary>
    public class MessageData
    {
        public string Key { get; set; }

        public object Values { get; set; }

        public MessageData(string key, object valueObj)
        {
            Key    = key;
            Values = valueObj;
        }
    }
