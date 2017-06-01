﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour {

    public GameObject[] characterPrefabs;
    public UIInput nameInPut;//用来得到输入的文本
    private GameObject[] characterGameObjects;
    private int selectedIndex = 0;
    private int length;//所有共可选择的角色个数
	// Use this for initialization
	void Start () {
        length = characterPrefabs.Length;
        characterGameObjects = new GameObject[length];
		for(int i = 0; i < length;i++)
        {
            characterGameObjects[i] = GameObject.Instantiate(characterPrefabs[i],transform.position,transform .rotation )as GameObject;
        }
        UpdateCharacterShow();
	}

    void UpdateCharacterShow()
    {//更新所有角色的显示
        characterGameObjects[selectedIndex].SetActive(true);
        for(int i = 0;i < length;i++)
        {
            if (i != selectedIndex)
                characterGameObjects[i].SetActive(false);//把选择的角色设置为隐藏
        }
    }
    public  void OnNextButtonClick()
    {//当我们点击了下一个按钮
        selectedIndex++;
        selectedIndex %= length;
        UpdateCharacterShow();

    }
    public void OnPrevButtonClick()
    {//当我们点击了上一个按钮
        selectedIndex--; 
        if (selectedIndex == -1)
            selectedIndex = length - 1;
        UpdateCharacterShow();
    }
    public void OnOkButtonClick()
    { 
        PlayerPrefs.SetInt("SelectCharacterIndex", selectedIndex);//存储选择的角色
        PlayerPrefs.SetString("name", nameInPut.value);//存储输入的名字        
        //加载下一个场景
        Application.LoadLevel(2);
    }
}
