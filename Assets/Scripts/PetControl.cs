using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PetControl : MonoBehaviour
{
    [Header("跟随设置")]
    public Transform target; // 跟随目标
    public float followDistance = 2.0f; // 跟随距离


    [Header("动画设置")]
    public string moveAnimationName = "isMove"; // 移动动画名称
    public string idleAnimationName = "Idle"; // 待机动画名称

    private UnityEngine.AI.NavMeshAgent navAgent;
    private Animator animator;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        // 获取组件
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
        // CheckMouseClick();
    }

    /// <summary>
    /// 跟随目标逻辑
    /// </summary>
    void FollowTarget()
    {
        // 如果目标不为空，则跟随目标
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // 如果距离目标足够远，开始跟随
            // if (distanceToTarget > followDistance)
            if (distanceToTarget > navAgent.stoppingDistance)
            {
                navAgent.SetDestination(target.position);

                // 播放移动动画
                if (animator != null && !isMoving)
                {
                    animator.SetBool(moveAnimationName, true);
                    isMoving = true;
                }
            }
            else
            {
                // 距离足够近，停止移动并播放待机动画
                if (animator != null && isMoving)
                {
                    animator.SetBool(moveAnimationName, false);
                    isMoving = false;
                }
            }
        }
        else
        {
            // 目标为空，停止移动并播放待机动画
            if (navAgent != null)
            {
                navAgent.ResetPath();
            }

            if (animator != null && isMoving)
            {
                animator.SetBool(moveAnimationName, false);
                isMoving = false;
            }
        }
    }
    private void OnMouseDown()
    {

        Debug.Log("鼠标点击");
        animator.SetTrigger("Tickle");
    }


    // /// <summary>
    // /// 检测鼠标点击并执行相应操作
    // /// </summary>
    // void CheckMouseClick()
    // {
    //     // 检测鼠标左键是否按下
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         // 发送一条从摄像机到鼠标位置的射线
    //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //         RaycastHit hit;

    //         // 进行射线检测
    //         if (Physics.Raycast(ray, out hit))
    //         {
    //             Debug.Log("鼠标点击" + hit.transform.name);
    //             // 判断击中的目标是否有"dog"标签
    //             if (hit.transform.tag == "dog")
    //             {
    //                 Debug.Log("鼠标点击了dog目标");
    //                 animator.SetTrigger("Tickle");
    //             }
    //         }
    //     }
    // }


}
