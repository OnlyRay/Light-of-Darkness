using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {

    private Camera minimaoCamera;

    void Start()
    {
        minimaoCamera = GameObject.FindGameObjectWithTag(Tags.minimap).GetComponent<Camera>();
    }

    public void OnZoomInCllick()
    {//放大小地图
        minimaoCamera.orthographicSize--;
    }
    public void OnZoomOutCllick()
    {// 缩小小地图
        minimaoCamera.orthographicSize++;
    }
}
