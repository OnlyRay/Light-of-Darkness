using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : MonoBehaviour {


    private UISprite sprite;
    public int id;
    private bool isHover = false;

    void Awake()
    {
        sprite = this.GetComponent<UISprite>();
    }

    void Update()
    {
        if(isHover)
        {
            //当鼠标移动到这个装备栏之上的时候，监测鼠标右键的点击
            if(Input.GetMouseButtonDown(1))
            {//鼠标右键的点击之后，表示卸下该装备
                EquipmentUI._instance.TakeOff(id,this.gameObject);
                
            }
        }
    }
	// Use this for initialization
	public void SetId(int id)
    {
        this.id = id;
        ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(id);
        SetInfo(info);
    }
    public void SetInfo(ObjectInfo info)
    {
        this.id = info.id;
        sprite.spriteName = info.icon_name;
    }
    public void OnHover(bool isOver)
    {
        isHover = isOver;
    }
}
