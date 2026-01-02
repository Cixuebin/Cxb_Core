using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;


public class ResourceManager
{

    /// <summary>
    /// 资源加载
    /// </summary>
    /// <param name="objName">目标名称全路径</param>
    /// <returns></returns>
    public static T Load<T>(string objName) where T : Object
    {
        return Resources.Load<T>(objName);
    }

    /// <summary>
    /// 音频资源加载
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="objName">目标名称</param>
    /// <returns></returns>
    public static AudioClip LoadAudioAsset(string sceneName, string objName)
    {
        string tempPath = string.Concat(sceneName, "/Audio/", objName);
        AudioClip tempAsset = Resources.Load<AudioClip>(tempPath);
        return tempAsset;
    }

    /// <summary>
    /// 音频资源加载
    /// </summary>
    /// <param name="objName">目标名称</param>
    /// <returns></returns>
    public static AudioClip LoadAudioAsset(string objName)
    {
        string tempPath = string.Concat(SceneManager.GetActiveScene().name, "/Audio/", objName);
        AudioClip tempAsset = Resources.Load<AudioClip>(tempPath);
        if (tempAsset == null)
        {
            tempPath = string.Concat(SysConst.ASSET_SCENE_SHARE, "/Audio/", objName);
            tempAsset = Resources.Load<AudioClip>(tempPath);
        }

        return tempAsset;
    }

    /// <summary>
    /// UI资源加载
    /// </summary>
    /// <param name="objName">目标名称</param>
    /// <param name="parent">实例化后的父类</param>
    /// <returns></returns>
    public static GameObject LoadUIAsset(string objName, Transform parent)
    {
        string tempPath = string.Concat(SceneManager.GetActiveScene().name, "/UIPrefab/", objName);
        Debug.LogError("LoadUIAsset:" + tempPath);
        GameObject tempAsset = Resources.Load<GameObject>(tempPath);
        if (tempAsset == null)
        {
            tempPath = string.Concat(SysConst.ASSET_SCENE_SHARE, "/UIPrefab/", objName);
            tempAsset = Resources.Load<GameObject>(tempPath);
        }
        GameObject tarObj = Object.Instantiate(tempAsset);
        tarObj.name = tarObj.name.Replace("(Clone)", "");
        tarObj.transform.SetParent(parent, false);
        return tarObj;
    }

    /// <summary>
    /// 文本资源加载
    /// </summary>
    /// <param name="objName">目标名称</param>
    /// <returns></returns>
    public static TextAsset LoadTextAsset(string objName)
    {
        string tempPath = string.Concat(SceneManager.GetActiveScene().name, "/Text/", objName);
        TextAsset tempAsset = Resources.Load<TextAsset>(tempPath);
        if (tempAsset == null)
        {
            tempPath = string.Concat(SysConst.ASSET_SCENE_SHARE, "/Text/", objName);
            tempAsset = Resources.Load<TextAsset>(tempPath);
        }
        return tempAsset;
    }

    /// <summary>
    /// 图集资源加载
    /// </summary>
    /// <param name="objName">目标名称</param>
    /// <returns></returns>
    public static SpriteAtlas LoadSpriteAtlas(string objName)
    {
        string tempPath = string.Concat(SceneManager.GetActiveScene().name, "/SpriteAtlas/", objName);
        SpriteAtlas tempAsset = Resources.Load<SpriteAtlas>(tempPath);
        if (tempAsset == null)
        {
            tempPath = string.Concat(SysConst.ASSET_SCENE_SHARE, "/SpriteAtlas/", objName);
            tempAsset = Resources.Load<SpriteAtlas>(tempPath);
        }
        return tempAsset;
    }

    /// <summary>
    /// 按键资源加载
    /// </summary>
    /// <param name="objName">目标名称</param>
    /// <param name="parent">实例化后的父类</param>
    /// <returns></returns>
    public static GameObject LoadNoteAsset(string objName, Transform parent)
    {
        string tempPath = string.Concat(SceneManager.GetActiveScene().name, "/NotePrefab/", objName);
        GameObject tempAsset = Resources.Load<GameObject>(tempPath);
        if (tempAsset == null)
        {
            tempPath = string.Concat(SysConst.ASSET_SCENE_SHARE, "/NotePrefab/", objName);
            tempAsset = Resources.Load<GameObject>(tempPath);
        }

        GameObject tarObj = Object.Instantiate(tempAsset);
        tarObj.name = tarObj.name.Replace("(Clone)", "");
        tarObj.transform.SetParent(parent, false);
        return tarObj;
    }
    /// <summary>
    /// 加载Txt文件,按行存储,文本放到StreamingAssets
    /// </summary>
    /// <param name="textName">文本的名字</param>
    /// <returns></returns>
    public static string[] loadText(string textName)
    {
        if (textName.Contains(".txt") == false)
            textName += ".txt";
        string[] result = File.ReadAllLines(Application.streamingAssetsPath + "/" + textName, Encoding.UTF8);
        return result;
    }
}

