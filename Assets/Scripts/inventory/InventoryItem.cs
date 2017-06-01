using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : UIDragDropItem {//实现鼠标交互，实现拖拽功能

    private UISprite sprite;
    private int id;
    void Awake()
    {
        sprite = this.GetComponent<UISprite>();
    }

    void Update()
    {
        if(isHover)
        {
            //显示提示信息
            InventoryDes._instance.Show(id);
            if (Input.GetMouseButtonDown(1))
            {
                //处理穿戴功能
                bool success = EquipmentUI._instance.Dress(id);
                if(success)
                {
                    transform.parent.GetComponent<InventoryItemGrid>().MinusNumber();
                }
            }
        }
    }

	 protected override void OnDragDropRelease(GameObject surface)//重写监听事件的方法，拖拽玩调用这个方法 
    {
        base.OnDragDropRelease(surface);
        //三种情况
        //第一种：拖拽物品到其他空格子去，则把原来信息删除并且添加到新的格子去
        //第二种：拖拽物品到已有物品的格子去，则两个格子的信息全部交换
        //第三种：拖拽物品又回到原来的格子，信息不变
        if(surface  != null)
        {
            if (surface.tag == Tags.inventory_item_grid)
            {//第一种：拖拽物品到其他空格子去，则把原来信息删除并且添加到新的格子去
                if (surface == this.transform.parent.gameObject)
                {//第三种：拖拽物品又回到原来的格子，信息不变
                    
                }
                else
                {
                    InventoryItemGrid oldParent = this.transform.parent.GetComponent<InventoryItemGrid>();
                    this.transform.parent = surface.transform;//更换
                    ResetPosition();
                    InventoryItemGrid newParent = surface. GetComponent<InventoryItemGrid>();
                    newParent.SetId(oldParent.id, oldParent.num);
                    oldParent.ClearInfo();//把原来的信息清空
                }
            }
            else if (surface.tag == Tags.inventory_item)
            { //第二种：拖拽物品到已有物品的格子去，则两个格子的信息全部交换
              InventoryItemGrid grid1 = this.transform.parent.GetComponent<InventoryItemGrid>();
              InventoryItemGrid grid2 =surface.transform.parent.GetComponent<InventoryItemGrid>();
              int id = grid1.id;//记录下原来的id和num，只要交换这两个信息就好了
              int num = grid1.num;
              grid1.SetId(grid2.id,grid2.num);//把grid2的信息给了grid1
              grid2.SetId(id,num);//把记录下来的信息给了grid2
            }
            else if (surface.tag == Tags.shortcut)//拖到快捷方式上面
            {
                surface.GetComponent<ShortCutGrid>().SetInventory(id);
            }
            
        }
        ResetPosition();
    }
    void ResetPosition()
    {
        transform.localPosition = Vector3.zero;
    } 


    public void SetId(int id)
    {
        ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(id);//得到物品的ID
        sprite.spriteName = info.icon_name;//更新物品的sprite(显示）
    }
    public void  SetIconName(int id,string icon_name)
    {
        sprite.spriteName =icon_name;
        this.id = id;     
    }

    private bool isHover = false;
    public void OnHoverOver()
    {
        //鼠标移到物品上
        //print("123");
        isHover = true;
    }

    public void OnHoverOut()
    {
        //print("12222223");
        //鼠标移离物品
        isHover = false ;
    }
}
