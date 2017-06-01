using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject magicianPrefab;
    public GameObject swordmanPrefab;

    void Awake()
    { 
        int selectedIndex = PlayerPrefs.GetInt("SelectCharacterIndex");
        string name = PlayerPrefs.GetString("name");

        GameObject go = null;
        if(selectedIndex == 0)
        {
           go =  GameObject.Instantiate(magicianPrefab) as GameObject;
        }
        else if (selectedIndex == 1)
        {
           go =  GameObject.Instantiate(swordmanPrefab) as GameObject;
        }

        go.GetComponent<PlayerStatus>().name = name;
    }
}
