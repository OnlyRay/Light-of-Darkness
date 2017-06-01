using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDrugNPC : NPC {

    public void OnMouseOver()
    {//当鼠标在这个游戏物体上的时候，会一直调用这个方法
        if (Input.GetMouseButtonDown(0))
        {
            transform.GetComponent<AudioSource>().Play();
            ShopDrug._instance.TransformState();
        }
    } 
}
