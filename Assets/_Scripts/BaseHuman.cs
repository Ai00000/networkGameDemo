using JetBrains.Annotations;
using UnityEngine;

namespace _Scripts
{
    public class BaseHuman : MonoBehaviour
    { 
        //参数
        protected bool IsMoving;
        private Vector3 _targetPosition;
        private float _speed=1.2f;
        public string desc = "";
       
       
       
     
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
           transform.position=Vector3.MoveTowards(pos,_targetPosition,_speed*Time.deltaTime);
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
