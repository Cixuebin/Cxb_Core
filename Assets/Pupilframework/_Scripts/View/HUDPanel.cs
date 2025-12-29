using System;
using System.Collections;
using System.Collections.Generic;
using PupilFramework;
using PupilFramework.UI;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class HUDPanel : BasePanel
{
    private Image _img_mute;
    private SpriteAtlas _atlas;
    protected override void Awake()
    {
        base.Awake();
        // RegisterBtnEvent("ReturnButton", ReturnBtnOnClickEvent);
        //RegisterBtnEvent("PersonalCenterButton", PersonalCenterBtnOnClickEvent);
        _img_mute = RegisterBtnEventReturn("MuteButton", MuteButtonOnClick).GetComponent<Image>();
        _atlas = ResourceManager.LoadSpriteAtlas("DarkUI");
    }

    private void PersonalCenterBtnOnClickEvent(GameObject go)
    {
        //UIManger.Instance.Push(SysConst.ASSET_UI_PERSONALCENTERPANEL);
    }

    public override void OnEnter(object msg)
    {
        base.OnEnter(msg);
        if (AudioManager.Instance.IsMute())
        {
            _img_mute.sprite = _atlas.GetSprite("AudioOff");
        }
        else
        {
            _img_mute.sprite = _atlas.GetSprite("AudioOn");
        }
    }
    private void MuteButtonOnClick(GameObject go)
    {
        if (AudioManager.Instance.Mute())
        {
            _img_mute.sprite = _atlas.GetSprite("AudioOff");
        }
        else
        {
            _img_mute.sprite = _atlas.GetSprite("AudioOn");
        }
    }

    private void ReturnBtnOnClickEvent(GameObject go)
    {
        Debug.Log("返回按钮点击!");
        // SceneLoadManager.Instance.YooLoadScene(SysConst.ASSET_SCENE_STOREGROUP, M => 
        // {
        // 	UIManger.Instance.Pop();
        // });
        //SceneLoadManager.Instance.YooLoadScene(SysConst.ASSET_SCENE_START, ChangeUI);
    }

    /// <summary>
    /// 初始化UI数据模型信息
    /// </summary>
    protected override void InitUIData()
    {
        UIInfo = new Model_UIDataModel
        {
            //默认遮罩自动响应出栈
            IsMaskBack = false,
            //默认面板类型
            Show = UIShowType.FullScreen,
            //默认遮罩透明度
            Mask = UIMaskType.NoEffect,
            //默认影响上一级类型
            Effect = UIEffectType.DisableOthers
        };
    }

    protected override void RegisterMessage()
    {

    }

    private void ChangeUI(object data)
    {
        UIManger.Instance.Push(SysConst.ASSET_UI_STARTPANEL);
    }
}
