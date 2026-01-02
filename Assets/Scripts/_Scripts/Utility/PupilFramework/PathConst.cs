using System.IO;
using UnityEngine;

namespace PupilFramework
{
    public class PathConst
    {
        //本地模拟(本地测试专用,测试结束打包时,需要手动关闭)
        public static bool LocalSimulation = true;

        /// <summary>
        /// 本地资源存放的路径（AssetBundle、xml、txt）
        /// </summary>
        public static string LocalResourcePath
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        return string.Concat(Application.persistentDataPath.Replace("/", @"\"), @"\", ManifestName, @"\");
                    case RuntimePlatform.OSXEditor:
                        return string.Concat(Application.persistentDataPath, "/", ManifestName, "/");
                    case RuntimePlatform.IPhonePlayer:
                        return string.Concat(Application.persistentDataPath, "/", ManifestName, "/");
                    case RuntimePlatform.WindowsPlayer:                  
                        return string.Concat(Application.persistentDataPath.Replace("/", @"\"), @"\", ManifestName, @"\");
                    default:
                        return string.Concat(Application.persistentDataPath, "/", ManifestName, "/");
                }
            }
        }

        /// <summary>
        /// 本地个人信息存放的路径
        /// </summary>
        public static string LocalInfoPath
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        return string.Concat(Application.persistentDataPath.Replace("/", @"\"), @"\", "Info", @"\",
                            SysConst.KEYFILE_PLAYINFO);
                    case RuntimePlatform.OSXEditor:
                        return string.Concat(Application.persistentDataPath, "/", "Info", "/", SysConst.KEYFILE_PLAYINFO);
                    case RuntimePlatform.WindowsPlayer:
                        return string.Concat(Application.persistentDataPath.Replace("/", @"\"), @"\", "Info", @"\",
                            SysConst.KEYFILE_PLAYINFO);
                    default:
                        return string.Concat(Application.persistentDataPath, "/", "Info", "/", SysConst.KEYFILE_PLAYINFO);
                }
            }
        }

        /// <summary>
        /// WWW本地资源存放的路径（AssetBundle、xml、txt）
        /// </summary>
        public static string LocalResourceWwwPath
        {
            get
            {
                Debug.Log("本地持久化文件夹路径" + Application.persistentDataPath);
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        return string.Concat("file://", Application.persistentDataPath, "/", ManifestName, "/");
                    case RuntimePlatform.IPhonePlayer:
                        return string.Concat("file://", Application.persistentDataPath, "/", ManifestName, "/");
                    case RuntimePlatform.OSXEditor:
                        return string.Concat("file://", Application.persistentDataPath, "/", ManifestName, "/");
                    case RuntimePlatform.OSXPlayer:
                        return string.Concat("file://", Application.persistentDataPath, "/", ManifestName, "/");
                    case RuntimePlatform.WindowsEditor:
                        return string.Concat("file://",Application.persistentDataPath.Replace("/", @"\"),
                            @"\" + ManifestName + @"\");
                    case RuntimePlatform.WindowsPlayer:
                        return string.Concat("file://",Application.persistentDataPath.Replace("/", @"\"),
                            @"\" + ManifestName + @"\");
                    default:
                        return string.Concat("file://", Application.persistentDataPath, "/", ManifestName, "/");
                }
            }
        }

        /// <summary>
        /// 本地配置文件存放的位置（第一次启动复制此路径中的资源到本地资源路径）
        /// </summary>
        /// <returns></returns>
        public static string LocalConfigPath
        {
            get
            {
                string outPath;
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        outPath = string.Concat(LocalResourcePath, "config", @"\");
                        if (!Directory.Exists(outPath))
                            Directory.CreateDirectory(outPath);
                        return outPath;
                    case RuntimePlatform.WindowsPlayer:
                        outPath = string.Concat(LocalResourcePath, "config", @"\");
                        if (!Directory.Exists(outPath))
                            Directory.CreateDirectory(outPath);
                        return outPath;
                    default:
                        outPath = string.Concat(LocalResourcePath, "config", "/");
                        if (!Directory.Exists(outPath))
                            Directory.CreateDirectory(outPath);
                        return outPath;
                }
            }
        }

        /// <summary>
        ///WWW本地配置文件存放的位置（第一次启动复制此路径中的资源到本地资源路径）
        /// </summary>
        /// <returns></returns>
        public static string LocalConfigWwwPath
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        return string.Concat(LocalResourceWwwPath, "config", @"\");
                    case RuntimePlatform.WindowsPlayer:
                        return string.Concat(LocalResourceWwwPath, "config", @"\");
                    default:
                        return string.Concat(LocalResourceWwwPath, "config", "/");
                }
            }
        }

        /// <summary>
        /// 初始资源存放的位置（第一次启动复制此路径中的资源到本地资源路径）
        /// </summary>
        public static string InitResourcePath
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.IPhonePlayer:
                        return string.Concat(Application.streamingAssetsPath, "/", ManifestName, "/");
                    case RuntimePlatform.OSXEditor:
                        return string.Concat(Application.streamingAssetsPath, "/", ManifestName, "/");
                    case RuntimePlatform.WindowsEditor:
                        return string.Concat("file://", Application.streamingAssetsPath, "/", ManifestName, "/");
                    default:
                        return string.Concat(Application.streamingAssetsPath, "/", ManifestName, "/");
                }
            }
        }

        /// <summary>
        /// WWW初始资源存放的位置（第一次启动复制此路径中的资源到本地资源路径）
        /// </summary>
        public static string InitResourceWwwPath
        {
            get
            {
#if (UNITY_2017)
            return string.Concat(Application.streamingAssetsPath, "/", ManifestName, "/");
#else
                switch (Application.platform)
                {
                    case RuntimePlatform.IPhonePlayer:
                        return string.Concat("file://", Application.streamingAssetsPath, "/", ManifestName, "/");
                    case RuntimePlatform.OSXEditor:
                        return string.Concat("file://", Application.streamingAssetsPath, "/", ManifestName, "/");
                    case RuntimePlatform.WindowsEditor:
                        return string.Concat("file://", Application.streamingAssetsPath, "/", ManifestName, "/");
                    case RuntimePlatform.Android:
                        return string.Concat(Application.streamingAssetsPath, "/", ManifestName, "/");
                    default:
                        return string.Concat("file://", Application.streamingAssetsPath, "/", ManifestName, "/");
                }
#endif
            }
        }

        /// <summary>
        /// 初始配置文件存放的位置（第一次启动复制此路径中的资源到本地资源路径）
        /// </summary>
        /// <returns></returns>
        public static string InitConfigPath
        {
            get
            {
                string outPath = string.Concat(InitResourcePath, "config", "/");
                if (!Directory.Exists(outPath))
                    Directory.CreateDirectory(outPath);
                return outPath;
            }
        }

        /// <summary>
        ///WWW初始配置文件存放的位置（第一次启动复制此路径中的资源到本地资源路径）
        /// </summary>
        /// <returns></returns>
        public static string InitConfigWwwPath
        {
            get { return string.Concat(InitResourceWwwPath, "config", "/"); }
        }

        /// <summary>
        /// 服务器资源存放的位置（只读）
        /// </summary>
        public static string NetServerResourcePath
        {
            get
            {
                return LocalSimulation
                    ? InitResourceWwwPath
                    : string.Concat("http://127.0.0.1/", ManifestName, "/");
            }
        }

        /// <summary>
        /// 服务器配置文件存放的位置（只读）
        /// </summary>
        public static string NetServerConfigPath
        {
            get
            {
                return LocalSimulation
                    ? InitConfigWwwPath
                    : string.Concat("http://127.0.0.1/", ManifestName, "/", "config", "/");
            }
        }

        /// <summary>
        /// 根据运行平台返回Manifest文件名称
        /// </summary>
        public static string ManifestName
        {
            get
            {
                if (LocalSimulation)
                {
                    return "IOS";
                }
                else
                {
                    switch (Application.platform)
                    {
                        case RuntimePlatform.Android:
                            return "Android";
                        case RuntimePlatform.IPhonePlayer:
                            return "IOS";
                        case RuntimePlatform.OSXEditor:
                            return "IOS";
                        case RuntimePlatform.OSXPlayer:
                            return "IOS";
                        case RuntimePlatform.WindowsEditor:
                            return "Windows";
                        case RuntimePlatform.WindowsPlayer:
                            return "Windows";
                        default:
                            return "Windows";
                    }
                }
            }
        }
    }
}