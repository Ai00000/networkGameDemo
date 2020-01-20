using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts
{
    /// <summary>
    /// 基础角色类
    /// </summary>
    public class BaseHuman : MonoBehaviour
    { 
        //参数
        
        [Header("移动速度")]
        public float speed=1.2f;
        [Header("描述")]
        public string desc = "";
       
        protected bool IsMoving;
        private Vector3 _targetPosition;
       
     
        //组件
        private Animator _animator;
        private static readonly int Moving = Animator.StringToHash("isMoving");
        

       protected void MoveTo(Vector3 pos)
       {
           _targetPosition = pos;
           IsMoving = true;
           _animator.SetBool(Moving,true);
       }

       protected void MoveUpdate()
       {
           if (!IsMoving)
           {
               return;
           }

           var pos = transform.position;
           
           transform.position=Vector3.MoveTowards(pos,_targetPosition,speed*Time.deltaTime);
           transform.LookAt(_targetPosition);
           if (Vector3.Distance(pos,_targetPosition)<=0.05f)
           {
               IsMoving = false;
               _animator.SetBool(Moving,false);
           }
       }
        protected void Start()
        {
            _animator = GetComponent<Animator>();
        }

    
        protected void Update()
        {
            MoveUpdate();
        }
    }
}
