using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {

        public GameObject humanPrefab;//玩家角色预制体
        public BaseHuman myHuman;//自身脚本
        public Dictionary<string, BaseHuman> otherHumans;//存储其他角色
        
        
        private void Start()
        {   
            //初始化
            otherHumans=new Dictionary<string, BaseHuman>();
            
            //绑定
            NetManager.AddListener("Enter",OnEnter);
            NetManager.AddListener("Move",OnMove);
            NetManager.AddListener("Leave",OnLeave);
            
            NetManager.Connect("127.0.0.1",2233);    //连接
            
            //添加一个角色
            var go = Instantiate(humanPrefab);
            var x = Random.Range(-5, 5);
            var z = Random.Range(-5, 5);
            go.transform.position=new Vector3(x,0,z);
            myHuman = go.AddComponent<CtrlHuman>();
            myHuman.desc = NetManager.GetDesc();
            
            //发送协议
            var pos = myHuman.transform.position;
            var eul = myHuman.transform.eulerAngles;
            var sendStr = "Enter|";
            sendStr += NetManager.GetDesc() + ",";
            sendStr += pos.x + ",";
            sendStr += pos.y + ",";
            sendStr += pos.z + ",";
            sendStr += eul.y;
            NetManager.Send(sendStr);
        }


        private void Update()
        {
            NetManager.Update();
        }


        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="msg"></param>
        void OnEnter(string msg)
        {

            Debug.Log("OnEnter:"+msg);
            //解析参数
            var data = msg.Split(',');
            var desc = data[0];
            var x = float.Parse(data[1]);
            var y = float.Parse(data[2]);
            var z = float.Parse(data[3]);
            var eulY = float.Parse(data[4]);
            
            //只对非自身角色进行处理
            if (desc==NetManager.GetDesc())
            {
                return;
            }
            
            //创建同步角色
            var go = Instantiate(humanPrefab);
            go.transform.position=new Vector3(x,eulY,z);
            var human = go.AddComponent<SyncHuman>();
            human.desc = desc;
            
           otherHumans.Add(desc,human);
            
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