
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ²Ù×÷ÊÖ²á
/// </summary>
public class OperationManualPanel : BasePanel
{
	protected override void RegisterMessage()
	{
	}
	protected override void Awake()
	{
		base.Awake();
		RegisterBtnEvent("CloseButton", CloseButtonOnClickEvent);
	}

	private void CloseButtonOnClickEvent(GameObject go)
	{
		UIManger.Instance.Pop();
	}

	public override void OnEnter(object msg)
	{
		base.OnEnter(msg);
	}
}

