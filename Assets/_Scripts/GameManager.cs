using System;
using UnityEngine;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {   
            //绑定
            NetManager.AddListener("Enter",OnEnter);
            NetManager.AddListener("Move",OnMove);
            NetManager.AddListener("Leave",OnLeave);
            
            NetManager.Connect("127.0.0.1",2233);    //连接
            
        }

        
        
        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="msg"></param>
        void OnEnter(string msg)
        {
            Debug.Log("OnEnter:"+msg);
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="msg"></param>
        void OnMove(string msg)
        {
            Debug.Log("OnMove:"+msg);

        }

        /// <summary>
        /// 离开
        /// </summary>
        /// <param name="msg"></param>
        void OnLeave(string msg)
        {
            Debug.Log("OnLeave:"+msg);

        }
    }
}