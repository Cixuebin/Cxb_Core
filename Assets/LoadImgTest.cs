using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadImgTest : MonoBehaviour
{
	public RawImage _avatarImage;
	public Image _avatar;
	private string avatarUrl = "https://mate-community.eos-beijing-4.cmecloud.cn/2025/10/28/20251028103242_avatar_8.png?AWSAccessKeyId=H956H0TFW9P6QDXBOI7T&Expires=2076978764&Signature=Xn9BOPq3%2FeEBX8sqncHhfXCeSyY%3D";
	public void Start()
	{
		StartCoroutine(LoadAvatar());
	}
	IEnumerator LoadAvatar()
	{
		using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(avatarUrl))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.Success)
			{
				Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
				Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
				_avatar.sprite = sprite;
				_avatar.preserveAspect = true; // 可选：保持宽高比
			}
			else
			{
				Debug.LogError("Failed to load avatar: " + request.error);
			}
		}
	}

	public void LoadImgClick() 
	{
		// 通过JavaScript函数来触发文件选择对话框
		Application.ExternalEval(@"document.getElementById('upload').click(); ");
	}
	// 在文件选择对话框中选择完成后的回调函数
	public void OnFileSelected(string info)
	{
		FileInfo result = JsonConvert.DeserializeObject<FileInfo>(info);
		Debug.Log("Selected File: " + result.Filename + ", path: " + result.Path);
		StartCoroutine(LoadData(result.Path));
	}

	IEnumerator LoadData(string url)
	{
		UnityWebRequest request = UnityWebRequest.Get(url);
		DownloadHandlerBuffer handler = new DownloadHandlerBuffer();
		request.downloadHandler = handler;
		yield return request.SendWebRequest();
		if (request.result != UnityWebRequest.Result.Success)
		{
			Debug.Log(request.error);
			yield break;
		}
		Debug.Log("File loaded! " + url);
		byte[] data = handler.data;
		// 根据需要进行处理 data 的操作
		// 将 byte[] 转为 Texture2D
		Texture2D tex = new Texture2D(2, 2);
		if (tex.LoadImage(data))
		{
			Debug.Log($"图片加载成功，尺寸: {tex.width}x{tex.height}");

			// 示例：将纹理应用到 RawImage（UI）或上传到服务器
			_avatarImage.texture = tex;

			// 如果需要上传到服务器，可将 imageData 作为 POST 数据发送
			//StartCoroutine(UploadImageToServer(imageData));
		}
		else
		{
			Debug.LogError("图片加载失败！");
		}
	}

	[Serializable]
	public class FileInfo
	{
		public string Path;
		public string Filename;
	}
}
