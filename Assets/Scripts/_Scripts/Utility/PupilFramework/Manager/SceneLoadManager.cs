using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoadManager : MonoSingleton<SceneLoadManager>
{
    /// <summary>
    /// 虚拟进度
    /// </summary>
    private int _displayProcess;

    /// <summary>
    /// 当前进度
    /// </summary>
    private int _currentProcess;

    /// <summary>
    /// 回调委托
    /// </summary>
    /// <param name="data"></param>
    public delegate void SceneCallBackHandler(object data);

    /// <summary>
    /// 同步场景切换
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        //状态重置
        // ResetManager();
        //场景加载
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// 同步场景切换
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="callBackEvent">回调</param>
    /// <param name="data">数据参数</param>
    public void LoadScene(string sceneName, SceneCallBackHandler callBackEvent, object data = null)
    {
        //状态重置
        // ResetManager();
        //开始同步场景加载
        StartCoroutine(LoadingScene(sceneName, callBackEvent, data));
    }

    /// <summary>
    /// 同步场景加载
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="callBackEvent"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    IEnumerator LoadingScene(string sceneName, SceneCallBackHandler callBackEvent, object data = null)
    {
        //场景加载
        SceneManager.LoadScene(sceneName);
        yield return new WaitForEndOfFrame();
        //执行回调
        if (callBackEvent != null)
        {
            callBackEvent.Invoke(data);
        }
    }

    /// <summary>
    /// 异步切换场景
    /// </summary>
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadingSceneAsync(sceneName, null, null, LoadSceneMode.Single));
    }

    /// <summary>
    /// 异步切换场景
    /// </summary>
    public void LoadSceneAsync(string sceneName, LoadSceneMode model)
    {
        StartCoroutine(LoadingSceneAsync(sceneName, null, null, model));
    }

    /// <summary>
    /// 异步切换场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="callBackEvent">回调</param>
    /// <param name="dataValue">回调参数</param>
    /// <param name="model"></param>
    public void LoadSceneAsync(string sceneName, SceneCallBackHandler callBackEvent, object dataValue,
        LoadSceneMode model = LoadSceneMode.Single)
    {
        StartCoroutine(LoadingSceneAsync(sceneName, callBackEvent, dataValue, model));
    }

    /// <summary>
    /// 异步切换场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="callBackEvent">回调</param>
    public void LoadSceneAsync(string sceneName, SceneCallBackHandler callBackEvent)
    {
        StartCoroutine(LoadingSceneAsync(sceneName, callBackEvent, null, LoadSceneMode.Single));
    }


    /// <summary>
    /// 异步切换场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="callBackEvent">回调</param>
    /// <param name="model"></param>
    public void LoadSceneAsync(string sceneName, SceneCallBackHandler callBackEvent,
        LoadSceneMode model)
    {
        StartCoroutine(LoadingSceneAsync(sceneName, callBackEvent, null, model));
    }

    /// <summary>
    /// 场景异步加载
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="callBackEvent">回调</param>
    /// <param name="dataValue">回调参数</param>
    /// <param name="mode">加载模式</param>
    /// <returns></returns>
    IEnumerator LoadingSceneAsync(string sceneName, SceneCallBackHandler callBackEvent, object dataValue,
        LoadSceneMode mode)
    {
        Debug.Log("场景切换");
        //状态重置
        _displayProcess = 0;
        //Loading界面进栈
        UIManger.Instance.PushTransition(SysConst.ASSET_UI_LOADINGPANEL);
        //降低GC开销
        WaitForEndOfFrame delay = new WaitForEndOfFrame();
        MessageData data = new MessageData(SysConst.MVC_LOADINGPANEL_UPDATEPRORESS, _displayProcess);
        //必须要等一帧 不然加载界面刷不出来
        yield return new WaitForSecondsRealtime(3);
        // ResetManager();
        //开始异步加载
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, mode);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            _currentProcess = (int)(operation.progress * 100);
            while (_displayProcess < _currentProcess)
            {
                _displayProcess += 1;
                UpdateProcess(SysConst.MVC_LOADINGPANEL, data);
                yield return delay;
            }

            yield return delay;
        }

        _currentProcess = 100;
        while (_displayProcess < _currentProcess)
        {
            _displayProcess += 1;
            UpdateProcess(SysConst.MVC_LOADINGPANEL, data);
            yield return delay;
        }

        yield return delay;

        //Loading界面出栈
        data.Key = SysConst.MVC_LOADINGPANEL_ENDPRORESS;
        data.Values = "加载完毕,即将切换场景";
        MsgCenter.SendMessage(SysConst.MVC_LOADINGPANEL, data);
        yield return new WaitForSeconds(2f);
        //执行回调
        if (callBackEvent != null)
        {
            Debug.Log("执行回调");
            operation.allowSceneActivation = true;
            callBackEvent.Invoke(dataValue);

        }
    }

    /// <summary>
    /// 管理类状态重置
    /// </summary>
    private void ResetManager()
    {
        UIManger.Instance.ResetState();
    }

    /// <summary>
    /// 更新进度条数值
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="data"></param>
    private void UpdateProcess(string msg, MessageData data)
    {
        data.Values = _displayProcess;
        MsgCenter.SendMessage(msg, data);
    }
}
