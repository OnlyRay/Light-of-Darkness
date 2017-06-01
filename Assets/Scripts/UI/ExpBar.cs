using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpBar : MonoBehaviour {

    public static ExpBar _instance;
    private UISlider progressBar;


    void Awake()
    {
        _instance = this;
        progressBar = this.GetComponent<UISlider>();
    }

    public void SetValue(float value)
    {//更新进度条的显示
        progressBar.value = value;
    }
}
