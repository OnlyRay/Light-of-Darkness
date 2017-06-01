using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    private Transform player;
    private Vector3 offsetPosition;//位置偏移
    private bool isRotating = false;

    public float rotateSpeed = 5;
    public float distance = 0;//用来存储player和相机之间的距离
    public float scrollSpeed = 10;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform ;
        transform.LookAt(player.position);
        offsetPosition = transform.position - player.position;//记录位置偏移
    }
	
	// Update is called once per frame
	void Update () {

        transform.position = offsetPosition + player.position;
        //处理视野的旋转
        RotateView();
        //处理视野的拉近和拉远
        ScrollView();
        
	}


    void ScrollView()
    {
        //Input.GetAxis("Mouse ScrollWheel")向前推滑轮是拉近视野，向后滑是拉远视野
        distance = offsetPosition.magnitude;
        distance -= Input.GetAxis("Mouse ScrollWheel")*scrollSpeed ;
        distance = Mathf.Clamp(distance, 3, 16);
        offsetPosition = offsetPosition.normalized * distance;

    }
    void RotateView()
    {
        //Input.GetAxis("Mouse X");得到鼠标水平方向的移动
        //Input.GetAxis("Mouse Y");得到鼠标垂直方向的移动
        if(Input .GetMouseButtonDown(1))
        {
            isRotating = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false ;
        }
        if(isRotating )
        {
            transform.RotateAround(player.position,player.up , rotateSpeed *  Input.GetAxis("Mouse X"));

            Vector3 originalPosition = transform.position;
            Quaternion originalRotation = transform.rotation;
            
            transform.RotateAround(player.position, transform .right , rotateSpeed * (-1) * Input.GetAxis("Mouse Y"));//影响的属性有两个，一个是position，一个是rotation
            float x = transform.eulerAngles.x;
            if(x > 80 || x < 10)
            {// 当超出范围之后，我们将属性归为原来的，就是旋转无效
                transform.position = originalPosition;
                transform.rotation = originalRotation;
            }
        }
        offsetPosition = transform.position - player.position;//记录位置偏移
    }
}
