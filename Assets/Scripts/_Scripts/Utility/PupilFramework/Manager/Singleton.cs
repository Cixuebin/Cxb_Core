using System;
using System.Threading;



/// <summary>
/// 不继承mono的单例基类，如果需要Update，可以将方法注册进MonoEvent的事件中
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : new()
{
    private static T _instance;
    private static readonly object Lock = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Monitor.Enter(Lock);
                try
                {
                    if (_instance == null)
                    {
                        _instance = Activator.CreateInstance<T>();
                    }
                }
                finally
                {
                    Monitor.Exit(Lock);
                }
            }
            return _instance;
        }
    }
}
