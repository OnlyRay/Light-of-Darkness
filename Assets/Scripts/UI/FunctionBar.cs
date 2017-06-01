using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionBar : MonoBehaviour {
    //监听这五个按钮
    public void OnStatusButtonClick()
    {
        Status._instance.TransformState();
    }
    public void OnBagButtonClick()
    {
        Inventory._instance.TransformState();
    }
    public void OnEquipButtonClick()
    {
        EquipmentUI._instance.TransformState();
    }
    public void OnSkillButtonClick()
    {
        SkillUI._instance.TransformState();
    }
    public void OnSettingButtonClick()
    {

    }
    
}
