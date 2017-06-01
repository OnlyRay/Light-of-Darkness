using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawn : MonoBehaviour {

    // 用于不断产生小狼
    public int maxnum = 5;//限制小狼的最大数量
    private int currentnum = 0;

    public float time = 3;//产生小狼所需要的时间
    private float timer = 0;//使用计时器记录时间
    public GameObject prefab;
    
    void Update()
    {
        if(currentnum < maxnum)
        {
            timer += Time.deltaTime;
            if(timer > time)
            {//产生一个小狼
                Vector3 pos = transform.position;
                pos.x += Random.Range(-5, 5);
                pos.z += Random.Range(-5, 5);
                GameObject go = GameObject.Instantiate(prefab, pos, Quaternion.identity)as GameObject;//产生小狼
                go.GetComponent<WolfBaby>().spawn = this;
                timer  = 0 ;
                currentnum++;
            }
        }
    }


    public void MinusNumber()
    {
        this.currentnum--;
    }
}
