using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDes : MonoBehaviour {

    public static InventoryDes _instance;
    private UILabel label;
    private float timer = 0;
    void Awake()
    {
        _instance = this;
        label = this.GetComponentInChildren<UILabel>();
        this.gameObject.SetActive(false);
    }


    void Update()
    {
        if(this .gameObject .activeInHierarchy == true)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
                this.gameObject.SetActive(false);   
        }
    }

    public void Show(int id)
    {
        this.gameObject.SetActive(true);
        timer = 0.1f;
        transform.position = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);//跟随物品而动
        ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(id);
        string des = "";
        switch (info.type )
        {
            case ObjectType.Drug:
                des = GetDrugDes(info);
                break;
            case ObjectType.Equip:
                des = GetEquipDes(info);
                break;
        }
        label.text = des;
    }

    string GetDrugDes(ObjectInfo info)
    {
        string str = "";
        str += "名称" + info.name + "\n";
        str += "+HP:" + info.hp + "\n";
        str += "+MP:" + info.mp + "\n";
        str += "出售价格：" + info.price_sell + "\n";
        str += "购买价格：" + info.price_buy + "\n";
        return str;
    }
    string GetEquipDes(ObjectInfo info)
    {
        string str1 = "";
        str1 += "名称" + info.name + "\n";
        switch(info.dressType)
        {
            case DressType.Headgear:
                str1 += "穿戴类型：头盔\n";
                break;
                case DressType.Armor:
                str1 += "穿戴类型：盔甲\n";
                break;
            case DressType.LeftHand :
                str1 += "穿戴类型：左手\n";
                break;
            case DressType.RightHand :
                str1 += "穿戴类型：右手\n";
                break;
            case DressType.Shoe:
                str1 += "穿戴类型：鞋子\n";
                break;
            case DressType.Accessory:
                str1 += "穿戴类型：首饰\n";
                break;
        }
        switch (info.applicationType)
        {
            case ApplicationType.Swordman:
                str1 += "适用类型：剑士\n";
                break;
            case ApplicationType.Magician :
                str1 += "适用类型：魔法师\n";
                break;
            case ApplicationType.Common:
                str1 += "适用类型：通用\n";
                break;
        }
        str1 += "伤害值：" + info.attack + "\n";
        str1 += "防御值：" + info.def + "\n";
        str1 += "速度值：" + info.speed + "\n";
        str1 += "出售价格：" + info.price_sell + "\n";
        str1 += "购买价格：" + info.price_buy;
        return str1;
    }
}
