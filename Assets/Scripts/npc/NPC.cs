using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
    //记载NPC共有的功能属性
	public void OnMouseEnter()
    {
        CursorManager._instance.SetNpcTalk();
    }
    public void OnMouseExit()
    {
        CursorManager._instance.SetNormal();
    }

}
