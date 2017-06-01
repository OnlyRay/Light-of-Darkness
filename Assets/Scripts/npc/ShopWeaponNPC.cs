using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWeaponNPC : NPC{

    public void OnMouseOver()
    {//当鼠标在这个游戏物体上的时候，会一直调用这个方法
        if (Input.GetMouseButtonDown(0))//弹出武器商店的列表 
        {
            transform.GetComponent<AudioSource>().Play();
            ShopWeaponUI._instance.TransformState();
        }
    }




}
