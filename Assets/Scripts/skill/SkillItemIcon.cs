using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItemIcon : UIDragDropItem {

    private int skillId;

    protected override void OnDragDropStart()
    {
        //在克隆的icon上调用的
        base.OnDragDropStart();
        skillId = transform.parent.GetComponent<SkillItem>().id;
        transform.parent = transform.root;
        this.GetComponent<UISprite>().depth = 30;
    }
    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        if (surface != null && surface.tag == Tags.shortcut)
        {//当一个技能拖到快捷方式上,根据ID来识别技能
            surface.GetComponent<ShortCutGrid>().SetSkill(skillId);
        }

    }
}
