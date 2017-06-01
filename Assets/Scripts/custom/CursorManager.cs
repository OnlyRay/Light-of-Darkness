using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {

    public static CursorManager _instance;
    public Texture2D cursor_normal;
    public Texture2D cursor_npc_talk;
    public Texture2D cursor_attack;
    public Texture2D cursor_lookTarget;
    public Texture2D cursor_pick;

    private Vector2 hotspot = Vector2.zero;
    private CursorMode mode = CursorMode.Auto;//自动适应是在软件还是硬件上实现功能


    void Start()
    {
        _instance = this; 
    }

    public void SetNormal()
    {
        Cursor.SetCursor(cursor_normal, hotspot, mode);

    }


    public void SetNpcTalk()
    {
        Cursor.SetCursor(cursor_npc_talk, hotspot, mode);
    }

    public void SetAttack()
    {
        Cursor.SetCursor(cursor_attack, hotspot, mode);
    }

    public void SetLockTarget()
    {
        Cursor.SetCursor(cursor_lookTarget, hotspot, mode);
    }
}
