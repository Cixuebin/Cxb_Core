using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

/* 注意：动画通过控制器为角色和胶囊体调用，使用了动画器空值检查
 */

namespace StarterAssets
{
    // 必须附加 CharacterController 组件
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    // 如果启用输入系统，则必须附加 PlayerInput 组件
    // [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("玩家")]
        [Tooltip("角色移动速度（米/秒）")]
        public float MoveSpeed = 2.0f;

        [Tooltip("角色冲刺速度（米/秒）")]
        public float SprintSpeed = 5.335f;

        [Tooltip("角色转向面对移动方向的速度")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("加速和减速")]
        public float SpeedChangeRate = 10.0f;

        // 音效相关
        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("玩家可以跳跃的高度")]
        public float JumpHeight = 1.2f;

        [Tooltip("角色使用自己的重力值。引擎默认值为-9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("再次跳跃前需要经过的时间。设为0f可立即再次跳跃")]
        public float JumpTimeout = 0.50f;

        [Tooltip("进入坠落状态前需要经过的时间。对于走下楼梯很有用")]
        public float FallTimeout = 0.15f;

        [Header("玩家接地检测")]
        [Tooltip("角色是否接地。这不是CharacterController内置的接地检查")]
        public bool Grounded = true;

        [Tooltip("对于不平整地面有用")]
        public float GroundedOffset = -0.14f;

        [Tooltip("接地检测的半径。应与CharacterController的半径匹配")]
        public float GroundedRadius = 0.28f;

        [Tooltip("角色视为地面的图层")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("Cinemachine虚拟相机中设置的跟随目标，相机会跟随此目标")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("相机可向上移动的最大角度")]
        public float TopClamp = 70.0f;

        [Tooltip("相机可向下移动的最大角度")]
        public float BottomClamp = -30.0f;

        [Tooltip("覆盖相机的额外角度。在锁定时微调相机位置很有用")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("锁定相机在所有轴上的位置")]
        public bool LockCameraPosition = false;

        // Cinemachine 相机相关变量
        private float _cinemachineTargetYaw;   // 相机目标的偏航角（Y轴旋转）
        private float _cinemachineTargetPitch; // 相机目标的俯仰角（X轴旋转）

        // 玩家相关变量
        private float _speed;                  // 当前移动速度
        private float _animationBlend;         // 动画混合参数
        private float _targetRotation = 0.0f;  // 目标旋转角度
        private float _rotationVelocity;       // 旋转速度（用于平滑旋转）
        private float _verticalVelocity;       // 垂直速度（用于跳跃和重力）
        private float _terminalVelocity = 53.0f; // 终端速度（最大下落速度）

        // 超时时间变量
        private float _jumpTimeoutDelta;       // 跳跃超时计时器
        private float _fallTimeoutDelta;       // 坠落超时计时器

        // 动画参数ID（使用哈希值提高性能）
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM 
        public PlayerInput _playerInput;      // 输入系统组件
#endif
        private Animator _animator;            // 动画控制器
        private CharacterController _controller; // 角色控制器
        public InputManger _input;    // 自定义输入管理器
        private GameObject _mainCamera;        // 主相机引用

        private const float _threshold = 0.01f; // 输入阈值（避免微小输入干扰）

        private bool _hasAnimator;             // 是否拥有动画控制器

        // 判断当前设备是否为鼠标键盘
        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// 在对象启用时调用，用于初始化
        /// </summary>
        private void Awake()
        {
            // 获取主相机引用
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        /// <summary>
        /// 在第一帧更新之前调用，用于初始化组件和变量
        /// </summary>
        private void Start()
        {
            // 初始化相机目标的初始旋转角度
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            // 尝试获取动画控制器组件
            _hasAnimator = TryGetComponent(out _animator);

            // 获取其他必要组件
            _controller = GetComponent<CharacterController>();
            _input = FindObjectOfType<InputManger>();
#if ENABLE_INPUT_SYSTEM
            //  _playerInput = GetComponent<PlayerInput>();
            _playerInput = FindObjectOfType<PlayerInput>();
#else
            Debug.LogError( "起始资源包缺少依赖项。请使用 Tools/Starter Assets/Reinstall Dependencies 来修复");
#endif

            // 分配动画参数ID
            AssignAnimationIDs();

            // 在开始时重置超时时间
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        /// <summary>
        /// 每帧调用一次，用于处理游戏逻辑更新
        /// </summary>
        private void Update()
        {
            // 检查是否拥有动画控制器
            _hasAnimator = TryGetComponent(out _animator);

            // 处理跳跃和重力
            JumpAndGravity();

            // 检查是否接地
            GroundedCheck();

            // 处理移动逻辑
            Move();
        }

        /// <summary>
        /// 在所有Update函数之后调用，用于处理相机旋转
        /// </summary>
        private void LateUpdate()
        {
            // 处理相机旋转逻辑
            CameraRotation();
        }

        /// <summary>
        /// 分配动画参数的哈希ID，提高性能
        /// </summary>
        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        /// <summary>
        /// 检查角色是否接地
        /// </summary>
        private void GroundedCheck()
        {
            // 设置球体位置，带偏移量
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);

            // 使用物理检测球体判断是否接触地面
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // 如果使用角色，更新动画器的接地状态
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        /// <summary>
        /// 处理相机旋转逻辑
        /// </summary>
        private void CameraRotation()
        {

            // 如果有输入且相机位置未固定
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                Logger.LogError("相机视角移动");
                // 不要将鼠标输入乘以Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                // 更新相机目标的旋转角度
                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // 限制旋转角度，使值限定在360度内
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // 应用旋转到Cinemachine相机目标
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        /// <summary>
        /// 处理角色移动逻辑
        /// </summary>
        private void Move()
        {
            // 根据移动速度、冲刺速度和是否按下冲刺键设置目标速度
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // 简化的加速和减速设计，易于移除、替换或迭代

            // 注意：Vector2的==运算符使用近似值，因此不会出现浮点误差问题，且比计算模长更高效
            // 如果没有输入，将目标速度设为0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // 获取玩家当前水平速度
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // 加速或减速到目标速度
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // 创建曲线结果而非线性结果，使速度变化更加自然
                // 注意Lerp中的T已被限制，所以我们不需要限制速度
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // 将速度四舍五入到小数点后3位
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            // 计算动画混合参数
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // 标准化输入方向
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // 注意：Vector2的!=运算符使用近似值，因此不会出现浮点误差问题，且比计算模长更高效
            // 如果有移动输入，在玩家移动时旋转玩家
            if (_input.move != Vector2.zero)
            {
                // 计算目标旋转角度（相对于相机方向）
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;

                // 平滑旋转到目标角度
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // 相对于相机位置旋转以面向输入方向
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            // 计算移动方向
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // 移动玩家
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // 如果使用角色，更新动画器参数
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        /// <summary>
        /// 处理跳跃和重力逻辑
        /// </summary>
        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // 重置坠落超时计时器
                _fallTimeoutDelta = FallTimeout;

                // 如果使用角色，更新动画器状态
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // 接地时停止垂直速度无限下降
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // 跳跃处理
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // H * -2 * G的平方根 = 达到期望高度所需的速度
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // 如果使用角色，更新动画器的跳跃状态
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // 跳跃超时处理
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // 重置跳跃超时计时器
                _jumpTimeoutDelta = JumpTimeout;

                // 坠落超时处理
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // 如果使用角色，更新动画器的自由落体状态
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // 如果未接地，则不能跳跃
                _input.jump = false;
            }

            // 如果低于终端速度，则随时间应用重力（乘以delta time两次以线性加速）
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        /// <summary>
        /// 限制角度在指定范围内
        /// </summary>
        /// <param name="lfAngle">要限制的角度</param>
        /// <param name="lfMin">最小角度</param>
        /// <param name="lfMax">最大角度</param>
        /// <returns>限制后的角度</returns>
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        /// <summary>
        /// 在Scene视图中绘制辅助Gizmos（仅在选中对象时显示）
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            // 根据是否接地设置不同的颜色
            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // 绘制接地检测球体的Gizmo
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        /// <summary>
        /// 脚步声回调函数（由动画事件触发）
        /// </summary>
        /// <param name="animationEvent">动画事件参数</param>
        private void OnFootstep(AnimationEvent animationEvent)
        {
            // 只有当动画权重较大时才播放脚步声
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                // 播放随机的脚步声音效
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        /// <summary>
        /// 着陆声回调函数（由动画事件触发）
        /// </summary>
        /// <param name="animationEvent">动画事件参数</param>
        private void OnLand(AnimationEvent animationEvent)
        {
            // 只有当动画权重较大时才播放着陆声
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}