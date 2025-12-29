using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger 
{
	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void Log(object message)
	{
		Debug.Log(message);
	}
	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void LogError(object message)
	{
		Debug.LogError(message);
	}
	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void LogWarning(object message)
	{
		Debug.LogWarning(message);
	}
}
