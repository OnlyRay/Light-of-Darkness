using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroType
{
    Swordman,
    Magician
}

public class PlayerStatus : MonoBehaviour {

    public HeroType heroType;

    public int level = 1;//100+level*30，满足就能升级
    public string name = "默认名称";
    public int hp = 100;//hp最大值
    public int mp = 100;//mp最大值
    public float hp_remain = 100;
    public float mp_remain = 100;
    public float exp = 0;//当前已经获得的经验


    public float attack = 20;
    public int attack_plus = 0;
    public float def = 20;
    public int def_plus = 0;
    public float speed = 20;
    public int speed_plus = 0;


    public int point_remain = 0;//剩余的加点数

    void Start()
    {
        GetExp(0);
    }

    public void GetDrug(int hp,int mp)//吃药后
    {
        hp_remain += hp;
        mp_remain += mp;
        
        if(hp_remain > this.hp)
        {
            hp_remain = hp;
        }
        if (mp_remain > this.mp)
        {
            mp_remain = mp;
        }
        HeadStatusUI._instance.UpdateShow();
    }

    
    public bool GetPoint (int point = 1)// 获得点数
    {
        if(point_remain >= point)
        {
            point_remain -= point;
            return true;
        }
        return false;
    }

    public void GetExp(int exp)
    {
        this.exp += exp;
        int total_exp = 100 + level * 30;
        while(this.exp >= total_exp )
        {//升级
            this.level++;
            this.exp -= total_exp;
        }

        ExpBar._instance.SetValue(this.exp / total_exp);
       
    }

    public bool TakeMP(int count)
    {
        if(mp_remain > count)
        {
            mp_remain -= count;
            HeadStatusUI._instance.UpdateShow();
            return true;
        }
        else
        {
            return false;
        }
    }

}
