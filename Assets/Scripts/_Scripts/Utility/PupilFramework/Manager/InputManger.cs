using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManger : MonoBehaviour
{
    [Header("角色输入值")]
    public Vector2 move;        // 移动输入向量（X轴和Y轴）
    public Vector2 look;        // 视角输入向量（鼠标或手柄右摇杆）
    public bool jump;           // 跳跃输入状态
    public bool sprint;         // 冲刺/奔跑输入状态
    private bool _isMouseRightPressed;  // 鼠标右键按下

    [Header("移动设置")]
    public bool analogMovement; // 模拟移动开关（用于区分键盘和手柄输入）

    //移动
    [SerializeField] private InputActionReference player_MoveAction;
    [SerializeField] private InputActionReference player_LookAction;
    [SerializeField] private InputActionReference player_JumpAction;
    [SerializeField] private InputActionReference player_SprintAction;
    [SerializeField] private InputActionReference player_MouseRightAction;
    //功能
    //视角切换
    [SerializeField] private InputActionReference player_FirstThirdSwitch;

    [SerializeField] private InputActionReference ui_panelOperationManualAction;
    [SerializeField] private InputActionReference ui_panelPersonalCenterAction;
    [SerializeField] private InputActionReference ui_PlayerMoveAction;

    void Awake()
    {
        // 启用所有动作
        EnableAllActions();

        // 绑定事件监听器
        // BindActionEvents();
    }

    private void EnableAllActions()
    {
        if (player_MoveAction != null)
            player_MoveAction.action.Enable();
        if (player_LookAction != null)
            player_LookAction.action.Enable();
        if (player_JumpAction != null)
            player_JumpAction.action.Enable();
        if (player_SprintAction != null)
        {
            Logger.Log("Enable Sprint");
            player_SprintAction.action.Enable();
        }
        if (player_MouseRightAction != null)
            player_MouseRightAction.action.Enable();
        if (player_FirstThirdSwitch != null)
            player_FirstThirdSwitch.action.Enable();
        if (ui_panelOperationManualAction != null)
            ui_panelOperationManualAction.action.Enable();
        if (ui_panelPersonalCenterAction != null)
            ui_panelPersonalCenterAction.action.Enable();
        if (ui_PlayerMoveAction != null)
            ui_PlayerMoveAction.action.Enable();
    }

    private void OnEnable()
    {
        BindActionEvents();
    }

    private void OnDisable()
    {
        UnbindActionEvents();
    }

    private void BindActionEvents()
    {
        // Player Actions
        if (player_MoveAction != null)
        {
            player_MoveAction.action.started += OnPlayerMoveStarted;
            player_MoveAction.action.performed += OnPlayerMovePerformed;
            player_MoveAction.action.canceled += OnPlayerMoveCanceled;
        }
        if (player_LookAction != null)
        {
            player_LookAction.action.started += OnPlayerLookStarted;
            player_LookAction.action.performed += OnPlayerLookPerformed;
            player_LookAction.action.canceled += OnPlayerLookCanceled;
        }
        if (player_JumpAction != null)
        {
            player_JumpAction.action.started += OnPlayerJumpStarted;
            player_JumpAction.action.canceled += OnPlayerJumpCanceled;
        }
        if (player_SprintAction != null)
        {
            player_SprintAction.action.performed += OnPlayerSprintStarted;
            player_SprintAction.action.canceled += OnPlayerSprintCanceled;
            Logger.Log("Binding Sprint actions");
        }
        if (player_MouseRightAction != null)
        {
            player_MouseRightAction.action.performed += OnMouseRightPerformed;
            player_MouseRightAction.action.canceled += OnMouseRightCanceled;
        }
        if (player_FirstThirdSwitch != null)
        {
            player_FirstThirdSwitch.action.performed += OnFirstThirdSwitchPerformed;
        }
        // UI Actions
        if (ui_panelOperationManualAction != null)
            ui_panelOperationManualAction.action.performed += OnOperationManualPerformed;
        if (ui_panelPersonalCenterAction != null)
            ui_panelPersonalCenterAction.action.performed += OnPersonalCenterPerformed;
        if (ui_PlayerMoveAction != null)
            ui_PlayerMoveAction.action.performed += OnPlayerMovePerformed;
    }

    private void OnFirstThirdSwitchPerformed(InputAction.CallbackContext context)
    {
        MsgCenter.SendMessage(SysConst.MVC_FUNCTION, new MessageData(SysConst.MVC_FUNCTION_FIRSTTHIRDSWITCH, null));
    }


    private void OnMouseRightCanceled(InputAction.CallbackContext context)
    {
        Logger.Log("Mouse Right Canceled");
        _isMouseRightPressed = false;
    }


    private void OnMouseRightPerformed(InputAction.CallbackContext context)
    {
        Logger.Log("Mouse Right Canceled");
        _isMouseRightPressed = true;
    }


    private void UnbindActionEvents()
    {
        // Player Actions
        if (player_MoveAction != null)
        {
            player_MoveAction.action.started -= OnPlayerMoveStarted;
            player_MoveAction.action.performed -= OnPlayerMovePerformed;
            player_MoveAction.action.canceled -= OnPlayerMoveCanceled;
        }
        if (player_LookAction != null)
        {

            player_LookAction.action.started -= OnPlayerLookStarted;
            player_LookAction.action.performed -= OnPlayerLookPerformed;
            player_LookAction.action.canceled -= OnPlayerLookCanceled;
        }
        if (player_JumpAction != null)
        {
            player_JumpAction.action.started -= OnPlayerJumpStarted;
            player_JumpAction.action.canceled -= OnPlayerJumpCanceled;
        }
        if (player_SprintAction != null)
        {
            player_SprintAction.action.started -= OnPlayerSprintStarted;
            player_SprintAction.action.canceled -= OnPlayerSprintCanceled;
        }
        if (player_MouseRightAction != null)
        {
            player_MouseRightAction.action.performed -= OnMouseRightPerformed;
            player_MouseRightAction.action.canceled -= OnMouseRightCanceled;
        }
        // UI Actions
        if (ui_panelOperationManualAction != null)
            ui_panelOperationManualAction.action.performed -= OnOperationManualPerformed;
        if (ui_panelPersonalCenterAction != null)
            ui_panelPersonalCenterAction.action.performed -= OnPersonalCenterPerformed;
        if (ui_PlayerMoveAction != null)
            ui_PlayerMoveAction.action.performed -= OnPlayerMovePerformed;
    }

    // 玩家移动输入处理
    private void OnPlayerMoveStarted(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        MsgCenter.SendMessage(SysConst.MVC_PLAYER, new MessageData(SysConst.MVC_PLAYER_MOVE, move));
        //   Logger.Log("Move:Started ");
    }

    private void OnPlayerMovePerformed(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        MsgCenter.SendMessage(SysConst.MVC_PLAYER, new MessageData(SysConst.MVC_PLAYER_MOVE, move));
        // Logger.Log("Move:Performed ");

    }

    private void OnPlayerMoveCanceled(InputAction.CallbackContext context)
    {
        move = Vector2.zero;
        MsgCenter.SendMessage(SysConst.MVC_PLAYER, new MessageData(SysConst.MVC_PLAYER_MOVE, move));
        //Logger.Log("Move:Canceled");
    }

    // 玩家视角输入处理
    private void OnPlayerLookStarted(InputAction.CallbackContext context)
    {
        if (_isMouseRightPressed)
        {
            look = context.ReadValue<Vector2>();
            MsgCenter.SendMessage(SysConst.MVC_PLAYER, new MessageData(SysConst.MVC_PLAYER_LOOK, look));
        }

    }

    private void OnPlayerLookPerformed(InputAction.CallbackContext context)
    {
        // look = context.ReadValue<Vector2>();
        if (_isMouseRightPressed)
        {
            look = context.ReadValue<Vector2>();
            MsgCenter.SendMessage(SysConst.MVC_PLAYER, new MessageData(SysConst.MVC_PLAYER_LOOK, look));
        }
        // look = context.ReadValue<Vector2>();

    }

    private void OnPlayerLookCanceled(InputAction.CallbackContext context)
    {
        look = Vector2.zero;
        MsgCenter.SendMessage(SysConst.MVC_PLAYER, new MessageData(SysConst.MVC_PLAYER_LOOK, look));
        //Logger.Log("Player Look Canceled: " + look);
    }

    // 玩家跳跃输入处理
    private void OnPlayerJumpStarted(InputAction.CallbackContext context)
    {
        jump = true;
        MsgCenter.SendMessage(SysConst.MVC_PLAYER, new MessageData(SysConst.MVC_PLAYER_JUMP, jump));

    }

    private void OnPlayerJumpCanceled(InputAction.CallbackContext context)
    {
        jump = false;
        MsgCenter.SendMessage(SysConst.MVC_PLAYER, new MessageData(SysConst.MVC_PLAYER_JUMP, jump));
    }

    // 玩家冲刺输入处理
    private void OnPlayerSprintStarted(InputAction.CallbackContext context)
    {
        sprint = true;
        MsgCenter.SendMessage(SysConst.MVC_PLAYER, new MessageData(SysConst.MVC_PLAYER_SPRINT, sprint));
    }

    private void OnPlayerSprintCanceled(InputAction.CallbackContext context)
    {
        sprint = false;
        MsgCenter.SendMessage(SysConst.MVC_PLAYER, new MessageData(SysConst.MVC_PLAYER_SPRINT, sprint));
    }

    // 原有的UI相关方法
    private void OnPlayerMovePerformedUI(InputAction.CallbackContext context)
    {
        Logger.Log("Player Move UI:" + context.ReadValue<Vector2>());
        Debug.Log("Player Move UI:" + context.ReadValue<Vector2>());
    }

    private void OnPersonalCenterPerformed(InputAction.CallbackContext context)
    {
        UIManger.Instance.Push(SysConst.ASSET_UI_OPERATIONMANUALPANEL);
    }

    private void OnOperationManualPerformed(InputAction.CallbackContext context)
    {
        UIManger.Instance.Push(SysConst.ASSET_UI_PERSONALCENTERPANEL);
    }
}