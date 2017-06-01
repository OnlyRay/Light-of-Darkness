using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDir : MonoBehaviour {

    public Vector3 targetPosition = Vector3.zero;
    public GameObject effect_click_prefab;
    private bool isMoving = false;//表示鼠标是否按下
    private PlayerMOve PlayerMove;
    private PlayerAttack attack;

   void Start()
    {
        targetPosition = transform.position;
        PlayerMove = this.GetComponent<PlayerMOve>();
        attack = this.GetComponent<PlayerAttack>();
    }
	// Update is called once per frame
	void Update () {
        if(attack.state == PlayerState.Death)
        {
            return;
        }
		if(attack.isLockingTarget == false && Input .GetMouseButtonDown(0)&&UICamera .hoveredObject == null)//检测是否点击时有UI控件
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isCollider = Physics.Raycast(ray, out hitInfo);
            if(isCollider && hitInfo .collider .tag == Tags.ground )
            {
                isMoving = true;
                ShowClickEffect(hitInfo.point);
                LookAtTarget(hitInfo.point);
            } 
        }
        if(Input .GetMouseButtonUp(0))
        {
            isMoving = false;
        }
        if(isMoving )
        {
            //得到要移动的目标的位置
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isCollider = Physics.Raycast(ray, out hitInfo);
            if (isCollider && hitInfo.collider.tag == Tags.ground)
            {
                LookAtTarget(hitInfo.point);
            }
            //让主角朝向目标位置
            
        }
        else
        {
            if (PlayerMove.isMoving)
            {
                LookAtTarget(targetPosition);
            }
        }
	}
    
    //实例化出来点击的效果
    void ShowClickEffect(Vector3 hitPoint)
    {
        hitPoint = new Vector3(hitPoint.x, hitPoint.y + 0.1f, hitPoint.z); 
        GameObject.Instantiate(effect_click_prefab, hitPoint, Quaternion.identity);
    }
    //该表让主嚼朝向目标位置
    void LookAtTarget(Vector3 hitPoint)
    {
        targetPosition = hitPoint;
        targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);//让主角绕着Y轴旋转，其实就是与目标位置的y轴方向保持一致
        this.transform.LookAt(targetPosition);
    }
}
