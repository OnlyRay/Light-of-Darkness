using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemGrid : MonoBehaviour {

    public int id = 0;
    public int num = 0;
    public UILabel numLabel;
    public ObjectInfo info = null;

	// Use this for initialization
	void Start () {
        numLabel = this.GetComponentInChildren<UILabel>();//实例化numLabel	
	}
	
    public void SetId(int id,int num = 1)
    {//根据id和num来设置它的显示
        this.id = id;
        info = ObjectsInfo._instance.GetObjectInfoById(id);//通过id来获得信息
        InventoryItem item = this.GetComponentInChildren<InventoryItem>();
        item.SetIconName(id,info.icon_name);
        numLabel.enabled = true;//决定这个组件式否可用，这里是可用
        this.num = num;
        numLabel.text = num.ToString();
    }
    public void PlusNumber(int num = 1)
    {
        this.num += num;
        numLabel.text = this.num.ToString();
    }
    public bool MinusNumber(int num = 1)
    {
        if(this.num >= num)
        {
            this.num -= num;
            numLabel.text =this.num.ToString();
            if (this.num == 0)
            {
                //要清空这个物品格子
                ClearInfo();//清空存储的信息
                GameObject.Destroy(this.GetComponentInChildren<InventoryItem>().gameObject);//销毁物品格子
            }
            return true;
        }
        return false;
    }
    public void ClearInfo()
    {
        //当物品被托摘到不合理的位置时。清空信息
        id = 0;
        info = null;
        num = 0;
        numLabel.enabled = false;

    }

}
