using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour {
    public static Status _instance;
    private TweenPosition tween;
    private bool isShow = false;

    private UILabel attackLabel;
    private UILabel defLabel;
    private UILabel speedLabel;
    private UILabel pointRemainLabel;
    private UILabel summaryLabel;

    private GameObject attackButtonGo;
    private GameObject defButtonGo;
    private GameObject speedButtonGo;

    private PlayerStatus playerstatus;

    void Awake()
    {
        //给属性赋值
        _instance = this;
        tween = this.GetComponent<TweenPosition>();
        attackLabel = transform.Find("attack").GetComponent<UILabel>();
        defLabel = transform.Find("def").GetComponent<UILabel>();
        speedLabel = transform.Find("speed").GetComponent<UILabel>();
        pointRemainLabel = transform.Find("point_remain").GetComponent<UILabel>();
        summaryLabel = transform.Find("summary").GetComponent<UILabel>();

        attackButtonGo = transform.Find("attack_plusbutton").gameObject;
        defButtonGo = transform.Find("def_plusbutton").gameObject;
        speedButtonGo = transform.Find("speed_plusbutton").gameObject;

        
    }
	
    void Start()
    {
        playerstatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
    }
    
	public void TransformState()
    {
        if(isShow == false )
        {
            UpdateShow();
            tween.PlayForward();
            isShow = true ;
        }else
        {
            tween.PlayReverse();
            isShow = false;
        }
    }


    void UpdateShow()
    {//更新显示  根据playerstatus的属性值去更新显示
        attackLabel.text = playerstatus.attack + " + " + playerstatus.attack_plus;
        defLabel.text = playerstatus.def + " + " + playerstatus.def_plus;
        speedLabel.text = playerstatus.speed + " + " + playerstatus.speed_plus;

        pointRemainLabel.text = playerstatus.point_remain.ToString();

        summaryLabel.text = "伤害：" + (playerstatus.attack + playerstatus.attack_plus)
            + "  " + "防御：" + (playerstatus.def + playerstatus.def_plus)
            + "  " + "速度" + (playerstatus.speed + playerstatus.speed_plus);
        
        if(playerstatus.point_remain > 0)
        {
            attackButtonGo.SetActive(true);
            defButtonGo.SetActive(true);
            speedButtonGo.SetActive(true);
        }else
        {
            attackButtonGo.SetActive(false );
            defButtonGo.SetActive(false );
            speedButtonGo.SetActive(false );

        }
    }
    //创建三个加号按钮的监听事件，在unity里面注册，点击要监听的button，把Status拖入OnClick中，选择方法
    public void OnAttackPlusClick()
    {
        bool success = playerstatus.GetPoint();
        if(success)
        {
            playerstatus.attack_plus++;
            UpdateShow();
        }
    }
    public void OnDeflusClick()
    {
        bool success = playerstatus.GetPoint();
        if (success)
        {
            playerstatus.def_plus++;
            UpdateShow();
        }
    }
    public void OnSpeedPlusClick()
    {
        bool success = playerstatus.GetPoint();
        if (success)
        {
            playerstatus.speed_plus++;
            UpdateShow();
        }
    }
}
