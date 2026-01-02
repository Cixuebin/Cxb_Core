using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	/// <summary>
	/// 输入管理器类，用于处理玩家的各种输入操作
	/// </summary>
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("角色输入值")]
		public Vector2 move;        // 移动输入向量（X轴和Y轴）
		public Vector2 look;        // 视角输入向量（鼠标或手柄右摇杆）
		public bool jump;           // 跳跃输入状态
		public bool sprint;         // 冲刺/奔跑输入状态

		[Header("移动设置")]
		public bool analogMovement; // 模拟移动开关（用于区分键盘和手柄输入）

		[Header("鼠标光标设置")]
		public bool cursorLocked = true;        // 是否锁定鼠标光标
		public bool cursorInputForLook = true;  // 是否允许鼠标控制视角
												// public PlayerInput playerInput;
												// // 获取 InputActionAsset
												// public InputActionAsset inputActions;
												// private InputActionMap playerMap; // Player 模块
		void Awake()
		{

		}



		void Start()
		{
			// playerInput = FindObjectOfType<PlayerInput>();
			// inputActions = FindObjectOfType<PlayerInput>().actions;
			// //GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
			// playerMap = inputActions.FindActionMap("Player");


		}
#if ENABLE_INPUT_SYSTEM
		/// <summary>
		/// 移动输入回调函数，由Input System自动调用
		/// </summary>
		/// <param name="value">输入值（来自输入设备的移动向量）</param>
		public void OnMove(InputValue value)
		{
			Debug.Log("Move: " + value.Get<Vector2>());
			MoveInput(value.Get<Vector2>());
		}
		public void OnCanel()
		{
			Debug.Log("Cancel: ");
			if (GetComponent<PlayerInput>().currentActionMap.name == "Panel")
			//if (inputActions.FindActionMap("Panel").enabled)
			{
				GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
				//inputActions.FindActionMap("Player").Enable();
				//inputActions.FindActionMap("Panel").Disable();
				UIManger.Instance.Pop();

			}
			else
			{
				GetComponent<PlayerInput>().SwitchCurrentActionMap("Panel");
				UIManger.Instance.Push(SysConst.ASSET_UI_PERSONALCENTERPANEL);
				//inputActions.FindActionMap("Player").Disable();
				//inputActions.FindActionMap("Panel").Enable();
			}



		}
		/// <summary>
		/// 视角输入回调函数，由Input System自动调用
		/// </summary>
		/// <param name="value">输入值（来自鼠标或手柄右摇杆的视角向量）</param>
		public void OnLook(InputValue value)
		{
			//Debug.Log("Look: " + value.Get<Vector2>());
			// 只有当允许鼠标控制视角且按下鼠标右键时才处理视角输入
			if (cursorInputForLook && Input.GetMouseButton(1))
			{
				LookInput(value.Get<Vector2>());
			}
			else
			{
				// 否则将视角输入设为零向量
				LookInput(Vector2.zero);
			}
		}

		/// <summary>
		/// 跳跃输入回调函数，由Input System自动调用
		/// </summary>
		/// <param name="value">输入值（跳跃按钮的状态）</param>
		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		/// <summary>
		/// 冲刺输入回调函数，由Input System自动调用
		/// </summary>
		/// <param name="value">输入值（冲刺按钮的状态）</param>
		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif

		/// <summary>
		/// 处理移动输入
		/// </summary>
		/// <param name="newMoveDirection">新的移动方向向量</param>
		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		/// <summary>
		/// 处理视角输入
		/// </summary>
		/// <param name="newLookDirection">新的视角方向向量</param>
		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		/// <summary>
		/// 处理跳跃输入
		/// </summary>
		/// <param name="newJumpState">新的跳跃状态（按下或释放）</param>
		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		/// <summary>
		/// 处理冲刺输入
		/// </summary>
		/// <param name="newSprintState">新的冲刺状态（按下或释放）</param>
		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		/// <summary>
		/// 当应用程序获得或失去焦点时调用
		/// </summary>
		/// <param name="hasFocus">是否获得焦点</param>
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		/// <summary>
		/// 设置鼠标光标状态
		/// </summary>
		/// <param name="newState">新的光标锁定状态</param>
		private void SetCursorState(bool newState)
		{
			// 根据状态锁定或解锁鼠标光标
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}