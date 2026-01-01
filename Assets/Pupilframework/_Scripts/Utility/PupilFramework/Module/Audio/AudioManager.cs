using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoSingleton<AudioManager>
{
    /// <summary>
    /// 主音频源
    /// </summary>
    private AudioSource _mainAudioSource;

    /// <summary>
    /// 缓存容器
    /// </summary>
    private Dictionary<string, AudioClip> _audioDic;

    private void Awake()
    {
        _audioDic = new Dictionary<string, AudioClip>();
        _mainAudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 长音频播放(仅能播放当前场景的音频)
    /// </summary>
    /// <param name="audioName">音频名称(默认循环播放)</param>
    public void Play(string audioName)
    {
        if (GetAudio(audioName) != null)
        {
            if (_mainAudioSource.clip != null)
            {
                if (audioName == _mainAudioSource.clip.name)
                {
                    return;
                }
            }
            _mainAudioSource.clip = GetAudio(audioName);
            _mainAudioSource.loop = true;
            _mainAudioSource.Play();
        }
    }

    /// <summary>
    /// 长音频播放(仅能播放当前场景的音频)
    /// </summary>
    /// <param name="audioName">音频名称</param>
    /// <param name="isLoop">是否循环播放</param>
    public void Play(string audioName, bool isLoop)
    {
        if (GetAudio(audioName) != null)
        {
            _mainAudioSource.clip = GetAudio(audioName);
            _mainAudioSource.loop = isLoop;
            _mainAudioSource.Play();
        }
    }

    /// <summary>
    /// 短音频播放
    /// </summary>
    /// <param name="audioName">音频名称</param>
    public void PlayOneShot(string audioName)
    {
        if (GetAudio(audioName) != null)
        {
            _mainAudioSource.PlayOneShot(GetAudio(audioName));
        }
    }

    /// <summary>
    /// 短音频播放(自定义音量)
    /// </summary>
    public void PlayOneShot(string audioName, float volume)
    {
        if (GetAudio(audioName) != null)
        {
            _mainAudioSource.PlayOneShot(GetAudio(audioName), volume);
        }
    }

    /// <summary>
    /// 静音
    /// </summary>
    /// <returns></returns>
    public bool Mute()
    {
        if (_mainAudioSource.mute == true)
        {
            _mainAudioSource.mute = false;
            return false;
        }
        else
        {
            _mainAudioSource.mute = true;
            return true;

        }
    }
    /// <summary>
    /// 是否静音
    /// </summary>
    /// <returns></returns>
    public bool IsMute()
    {
        if (_mainAudioSource.mute == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 暂停声音
    /// </summary>
    public void Pause()
    {
        _mainAudioSource.Pause();
    }

    /// <summary>
    /// 继续播放
    /// </summary>
    public void Resume()
    {
        _mainAudioSource.Play();
    }

    /// <summary>
    /// 停止播放
    /// </summary>
    public void Stop()
    {
        _mainAudioSource.Stop();
    }

    /// <summary>
    /// 音量设置
    /// </summary>
    public void SetVolume(float value)
    {
        _mainAudioSource.volume = value;
    }

    /// <summary>
    /// 状态重置
    /// </summary>
    public void ResetState()
    {
        ClearAudioDic();
    }

    #region 容器操作

    /// <summary>
    /// 资源添加
    /// </summary>
    /// <param name="audioClip">音频</param>
    private void AddAudio(AudioClip audioClip)
    {
        if (!_audioDic.ContainsKey(audioClip.name))
        {
            _audioDic.Add(audioClip.name, audioClip);
        }
    }

    /// <summary>
    /// 单个资源获取(仅能获取当前场景的音频文件)
    /// </summary>
    /// <param name="audioName">音频名称</param>
    public AudioClip GetAudio(string audioName)
    {
        if (_audioDic.ContainsKey(audioName))
        {
            return _audioDic[audioName];
        }
        else
        {
            AudioClip audioClip =
                ResourceManager.LoadAudioAsset(audioName);
            if (audioClip != null)
            {
                return audioClip;
            }
            else
            {
                Debug.Log("未发现音频文件");
                return null;
            }
        }
    }

    /// <summary>
    /// 单个资源删除
    /// </summary>
    /// <param name="audioName">音频名称</param>
    private void RemoveAudio(string audioName)
    {
        if (_audioDic.ContainsKey(audioName))
        {
            _audioDic.Remove(audioName);
        }
    }

    /// <summary>
    /// 场景映射音频容器清空
    /// </summary>
    private void ClearAudioDic()
    {
        _audioDic.Clear();
    }

    #endregion
}
