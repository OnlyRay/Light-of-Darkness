using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInfo : MonoBehaviour
{
    //用来管理所有的物品信息

    public static ObjectsInfo _instance;
    private Dictionary<int, ObjectInfo> objectInfoDict = new Dictionary<int, ObjectInfo>();//创建一个字典并且默认是空的 
    public TextAsset objectsInfoListText;//用力读取文本内容
                                         // Use this for initialization
    void Awake()
    {
        _instance = this;
        ReadInfo();
        //print(objectInfoDict.Keys.Count);
    }

    public ObjectInfo GetObjectInfoById(int id)//使用查找方法
    {
        ObjectInfo info;

        objectInfoDict.TryGetValue(id, out info);//得到了信息

        return info;//没有得到信息就为空
    }

    void ReadInfo()
    {
        string text = objectsInfoListText.text;//定义一个字符串来读取文本内容,按行读取，存储到字典里面
        string[] strArray = text.Split('\n');//先按行拆分
        //拆分文本里面的内容,根据逗号拆分
        foreach (string str in strArray )
        {
            string[] proArray = str.Split(',');
            ObjectInfo info = new ObjectInfo(); 
            int id = int.Parse(proArray[0]);
            string name = proArray[1];
            string icon_name = proArray[2];
            string str_type = proArray[3];
            //下面的属性不同，所以得先得到它的属性
            ObjectType type = ObjectType.Drug;
            switch(str_type)
            {
                case "Drug":
                    type = ObjectType.Drug;
                    break;
                case "Equip":
                    type = ObjectType.Equip;
                    break;
                case "Mat":
                    type = ObjectType.Mat;
                    break;
            }
            //下面开始存储一些信息如ID
            info.id = id;
            info.name = name;
            info.icon_name = icon_name;
            info.type = type;   

            if (type == ObjectType.Drug)
            {
                int hp = int.Parse(proArray[4]);
                int mp = int.Parse(proArray[5]);
                int price_sell = int.Parse(proArray[6]);
                int price_buy = int.Parse(proArray[7]);
                //存储一些信息如：HP
                info.hp = hp;
                info.hp = hp;
                info.price_sell = price_sell;
                info.price_buy = price_buy;
            }else if(type == ObjectType.Equip)
            {
                info.attack = int.Parse(proArray[4]);
                info.def = int.Parse(proArray[5]);
                info.speed = int.Parse(proArray[6]);
                info.price_sell = int.Parse(proArray[9]);
                info.price_buy = int.Parse(proArray[10]);
                string str_dresstype = proArray[7];
                switch (str_dresstype)
                {
                    case "Headgear":
                        info.dressType = DressType.Headgear;
                        break;
                    case "Armor":
                        info.dressType = DressType.Armor;
                        break;
                    case "LeftHand":
                        info.dressType = DressType.LeftHand;
                        break;
                    case "RightHand":
                        info.dressType = DressType.RightHand;
                        break;
                    case "Shoe":
                        info.dressType = DressType.Shoe;
                        break;
                    case "Accessory":
                        info.dressType = DressType.Accessory ;
                        break;
                }
                string str_apptype = proArray[8];
                switch (str_apptype)
                {
                    case "Swordman":
                        info.applicationType = ApplicationType.Swordman;
                        break;
                    case "Magician":
                        info.applicationType = ApplicationType.Magician;
                        break;
                    case "Common":
                        info.applicationType = ApplicationType.Common;
                        break;
                }
            }

            //读取到底额信息都保存在字典当中,id为key，可以很方便的根据ID查找到物品信息
            objectInfoDict.Add(id, info);

        }
    }

}

//0   Id
//1   名称
//2   Icon名称
//3   类型：药品
//4   加血量值
//5   加蓝量值
//6   出售价
//7   购买价
public enum ObjectType
{
    Drug,
    Equip,
    Mat
}

public enum DressType
{
    Headgear,
    RightHand,
    LeftHand,
    Accessory,
    Shoe,
    Armor
}

public enum ApplicationType
{
    Swordman,
    Magician,
    Common
}

public class ObjectInfo
{
    public int id;
    public string name;
    public string icon_name;//这个名称是存储在图形中的名称
    public ObjectType type;
    public int hp;
    public int mp;
    public int price_sell;
    public int price_buy;
    public int attack;
    public int def;
    public int speed;
    public DressType dressType;
    public ApplicationType applicationType;
}