using UnityEngine;

namespace _Scripts
{
    /// <summary>
    /// 玩家控制角色
    /// </summary>
    public class CtrlHuman:BaseHuman
    {
        public GameObject pointer;
        
        /*使用new 对父类方法进行覆写，隐藏父类方法*/
        new void Start()
        {
           
            base.Start();
        }

        new void Update()
        {
            base.Update();

            if (Input.GetMouseButtonDown(1))
            {
                /*
                 * 从主摄像机发射一道射线，获得屏幕上点对应的三维世界的点
                 */
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (hit.collider.CompareTag("Plane"))
                {
                    MoveTo(hit.point);
                    //创建指示标
                    var go = Instantiate(pointer, hit.point, Quaternion.identity);
                    NetManager.Send("Enter|127.1.1.1,100,200,300,45");
                    Destroy(go,5f);
                }
            }
        }
        
        
    }
}