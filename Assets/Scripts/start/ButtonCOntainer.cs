using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCOntainer : MonoBehaviour {

	//1 游戏数据的保存和场景之间游戏数据的传递使用PlayPrefs
    //2. 场景分类
      //2.1开始场景
      //2.2角色选择界面
      //2.3游戏玩家打怪的界面。就是游戏实际运行的场景  有村庄，有野兽。。。。


    //开始新游戏
	public  void OnNewGame()
    {
        //加载我们选择角色的场景2
        PlayerPrefs.SetInt("DateFromSave", 0);
        Application.LoadLevel(1);
    }
    //加载已经已保存的游戏
    public void OnLoadGame()
    {
        //加载我们选择角色的场景3
        PlayerPrefs.SetInt("DateFromSave",1);//DateFromSave表示数据来自保存

    }

}
