using System.Collections.Generic;
using System.Linq;
using UnityEngine;


    public static class TransformExtension
    {
        #region 查找物体

        /// <summary>
        /// 从自己的子物体中递归查找名字为name的物体的Transform
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="targetName">要查找的子节点名称</param>
        /// <returns></returns>
        public static Transform FindRecursively(this Transform root, string targetName)
        {
            if (root.name == targetName)
            {
                return root; //传进来的是自己，直接返回自己
            }
            foreach (Transform child in root)
            {
                Transform t = FindRecursively(child, targetName);
                if (t != null)
                {
                    return t; //从子物体中递归查找,找到第一个名字相同的返回
                }
            }
            return null;
        }

        /// <summary>
        /// 从自己的子物体中递归查找所有名字为name的物体的Transform
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="targetName">要查找的子节点名称</param>
        /// <param name="value">是否查找隐藏物体</param>
        /// <returns></returns>
        public static Transform[] FindRecursivelyArray(this Transform root, string targetName,bool value=true)
        {
            List<Transform> targetList = new List<Transform>();
            List<Transform> tempList   = root.GetComponentsInChildren<Transform>(value).ToList();
            for (int i = 0; i < tempList.Count; i++)
            {
                if (tempList[i].name == targetName)
                {
                    targetList.Add(tempList[i]);
                }
            }

            return targetList.ToArray();
        }
        #endregion

        #region 查找组件

        /// <summary>
        /// 从子名为targetName物体中查找继承自MonoBehaviour的类
        /// </summary>
        /// <typeparam name="T"> 查找的继承自MonoBehaviour的类的类型 </typeparam>
        /// <param name="root"> 根节点 </param>
        /// <param name="targetName"> 查找目标的名字 </param>
        /// <returns></returns>
        public static T FindComponent<T>(this Transform root, string targetName) where T : Object
        {
            Transform child = FindRecursively(root, targetName);

            if (child == null)
            {
                return null;
            }

            T target = child.GetComponent<T>();
            if (target == null)
            {
                Debug.LogError(targetName + " is not has component ");
            }

            return target;
        }

        /// <summary>
        /// 从自己的子物体中递归查找所有名字为name的物体且包含T组件
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="targetName">要查找的子节点名称</param>
        /// <param name="isActive">是否包含隐藏物体</param>
        /// <returns></returns>
        public static T[] FindComponentArray<T>(this Transform root, string targetName,bool isActive=false) where T : Object
        {
            List<T> targetList = new List<T>();
            List<Transform> tempList = root.GetComponentsInChildren<Transform>(isActive).ToList();
            for (int i = 0; i < tempList.Count; i++)
            {
                if (tempList[i].name == targetName)
                {
                    if (tempList[i].GetComponent<T>() != null)
                    {
                        targetList.Add(tempList[i].GetComponent<T>());
                    }
                }
            }

            return targetList.ToArray();
        }

        /// <summary>
        /// 获取子物体的所有相关组件(通过排除第一个的方式排除自身组件) 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static T[] GetComponentsInRealChildren<T>(this Transform obj, bool includeInactive = false)
        {
            List<T> Tlist = new List<T>();
            Tlist.AddRange(obj.GetComponentsInChildren<T>(includeInactive));
            Tlist.RemoveAt(0);
            return Tlist.ToArray();
        } 
        #endregion
    }
