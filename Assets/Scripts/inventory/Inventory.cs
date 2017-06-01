using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public static Inventory _instance;
    private TweenPosition tween;
    private int coinCount = 1000;//初始金币数量
    public List<InventoryItemGrid> itemGridList = new List<InventoryItemGrid>();
    public UILabel coinNumberLabel;
    public GameObject inventoryItem; 

    void Awake()
    {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();
       
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GetId(Random.Range(2001, 2023));
        }
       // coinNumberLabel.text = coinCount.ToString();//更新剩余金币的显示
    }

    public void GetId(int id,int count = 1)
    {
        //拾取到ID为id的物品并添加到物品栏 
        //第一步查找在所有的物品是否存在存在该物品
        //第二步如果存在，num+1
        //第三部如果不存在查找空的方格，然后把新创建的inventoryitem放在这个方格里面
        InventoryItemGrid grid = null;
        foreach(InventoryItemGrid temp in itemGridList)
        {
            if(temp.id == id)
            {
                grid = temp;
                break;
            }
        }
        if(grid != null)
        {//存在的情况
            grid.PlusNumber(count);
        }else
        {//不存在的情况
            //grid = null;
            foreach (InventoryItemGrid temp in itemGridList)
            {
                if(temp.id == 0)
                {
                    grid = temp;
                    break;
                }
            }

            if(grid != null)
            { //第三部如果不存在查找空的方格，然后把新创建的inventoryitem放在这个方格里面
                GameObject itemGo = NGUITools.AddChild(grid.gameObject, inventoryItem);
                itemGo.transform.localPosition = Vector3.zero; //确保物品放在格子的中间  
                itemGo.GetComponent<UISprite>().depth = 4;//先取到UISprite，并把depth设置为4
                grid.SetId(id,count); 
            }
        }
    }

    public bool MinusId(int id, int count = 1)
    {
        InventoryItemGrid grid = null;
        foreach (InventoryItemGrid temp in itemGridList)
        {
            if (temp.id == id)
            {
                grid = temp;
                break;
            }
        }

        if (grid == null)
        {
            return false;
        }
        else
        {
            bool isSuccess = grid.MinusNumber(count);
            return isSuccess;
        }
    }


    private bool isShow = false;

     void Show()
    {
        //显示动画
        isShow = true;
        tween.PlayForward();

    }
     void Hide()
    {
        //隐藏动画
        isShow = false;
        tween.PlayReverse();
    }
   
    public void TransformState()
    {// 转变动画状态，例把现实转化为隐藏
        if(isShow == false )
        {
            Show();
        }else
        {
            Hide();
        }
    }

    public void AddCoin(int count)
    {
        coinCount += count;
        coinNumberLabel.text = coinCount.ToString();//更新剩余金币的显示
    }
    //这个是取款方法，返回TRUE表示取款成功
    public bool GetCoin(int count)
    {
        if (coinCount >= count)
        {
            coinCount = coinCount - count; 
            coinNumberLabel.text = coinCount.ToString();//更新剩余金币的显示
            return true;
        }
        return false;
    } 
}
