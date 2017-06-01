using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShortCutType
{
    SKill,
    Drug,
    None
}

public class ShortCutGrid : MonoBehaviour {

    public KeyCode keyCode;//获取键盘上的按键
    private UISprite icon;
    private ShortCutType type = ShortCutType.None;
    private int id;
    private SkillInfo skillInfo;
    private ObjectInfo objectInfo;
    private PlayerStatus ps;//为了增加人物的属性
    private PlayerAttack playerAttack;
    void Awake()
    {
        icon = transform.Find("icon").GetComponent<UISprite>();
        icon.gameObject.SetActive(false);
    }

    void Start()
    {
        ps = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
        playerAttack = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerAttack>();
    }
    void Update()
    {
        if(Input.GetKeyDown(keyCode))//按下快捷键之后判断并且添加上属性
        {
            if(type == ShortCutType.Drug)
            {
                OnDrugUse();
            }
            else if(type == ShortCutType.SKill)
            {
                //释放技能
                //得到该技能需要的mp
                bool success = ps.TakeMP(skillInfo.mp);
                if(success == false)
                {//不够蓝释放技能
                    
                }
                else
                {
                    //获得MP后释放技能
                    playerAttack.UseSkill(skillInfo);
                }
            }
        }
    }

	public void SetSkill(int id)
    {
        this.id = id;
        this.skillInfo = SkillsInfo._instance.GetSkillInfoById(id);
        icon.gameObject.SetActive(true);
        icon.spriteName = skillInfo.icon_name;//更新图标的显示
        type = ShortCutType.SKill;
    }


    public void SetInventory(int id)
    {
        this.id = id;
        objectInfo = ObjectsInfo._instance.GetObjectInfoById(id);
        if (objectInfo.type == ObjectType.Drug)
        {
            icon.gameObject.SetActive(true);
            icon.spriteName = objectInfo.icon_name;//更新图标的显示
            type = ShortCutType.Drug;
        }
    }

    public void OnDrugUse()
    {
        bool success = Inventory._instance.MinusId(id, 1);
        if(success)
        {//如果使用成功的话
            Debug.Log(objectInfo.mp);
            ps.GetDrug(objectInfo.hp,objectInfo.mp);
        }
        else
        {
            type = ShortCutType.None;
            icon.gameObject.SetActive(false);
            id = 0;
            skillInfo = null;
            objectInfo = null; 
        }
    }

    
}
