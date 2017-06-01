using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarNPC : NPC {

    public static BarNPC _instance;
    public TweenPosition questTween;
    public UILabel desLable;
    public GameObject acceptBtnGo;
    public GameObject okBtnGo;
    public GameObject cancelBtnGo;
    public bool isInTask = false;//表示是否在任务中
    public int killCount = 0;//表示任务进度已经杀死几只小野狼

    private PlayerStatus status;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        status = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
    }

	void OnMouseOver()
    {
        //当鼠标会与这个collider之上的时候，会在每一帧调用这个方法
        if(Input .GetMouseButtonDown(0))
        {
            transform.GetComponent<AudioSource>().Play();
            //当点击了老爷爷
            if (isInTask)
            {
                ShowTaskProgress();
            }
            else ShowTaskDes();
            ShowQuest();
        }
    }
    void ShowQuest()
    {
        questTween.gameObject.SetActive(true);
        questTween.PlayForward();//播放这个动画
     }

    void HideQuest()//隐藏显示框
    {
        questTween.PlayReverse(); //相互播放这个动画
    }

    void ShowTaskDes()//显示任务详细信息
    {
        desLable.text = "任务：\n杀死10)只狼\n\n奖励：\n1000金币";
        okBtnGo.SetActive(false);
        acceptBtnGo.SetActive(true);
        cancelBtnGo.SetActive(true);
    }

    void ShowTaskProgress()//显示任务进度信息
    {
        desLable.text = "任务：\n你已经杀死了(" + killCount + "\\10)只狼\n\n奖励：\n1000金币";
        okBtnGo.SetActive(true);
        acceptBtnGo.SetActive(false);
        cancelBtnGo.SetActive(false);
    }

    //任务系统 任务对话框上的点击按钮事件的处理
    public void OnCloseButtonClick()
    {
        HideQuest();
    }

    public void OnAcceptBuutonClick()
    {
        ShowTaskProgress();
        isInTask = true;
    }

    public void OnOkButtonClick()
    {
        if(killCount >= 10)//完成任务
        {
            Inventory._instance.AddCoin(1000);
            killCount = 0;
            ShowTaskDes();

        }else//没有完成任务
        {
            HideQuest();
        }
    }

    public void OnCancelClick()
    {
        HideQuest();
    }

    public void OnKillWolf()
    {
        if (isInTask)
        {//在任务当中
            killCount++;
        }
    }
}
