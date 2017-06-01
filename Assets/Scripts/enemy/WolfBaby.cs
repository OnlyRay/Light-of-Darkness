using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WolfState
{//用枚举类型来表示wolf的状态
    Idle,
    Walk,
    Attack,
    Death
}


public class WolfBaby : MonoBehaviour {
    public WolfState state = WolfState.Idle;
    public int hp = 100;
    public int exp = 20;
    public int attack = 10;
    public float miss_rate = 0.2f;
    public string animname_death;
    public string animname_idle;
    public string animname_walk;
    public string animname_now;
    //轮换状态保存到animname_now里，需要计时器
    public float time = 3;//默认时间
    private float timer = 0;//用来计时
    private CharacterController cc;
    public float speed = 1;

    private Color normal;
    private bool is_attacked = false;
    public AudioClip miss_sound;


    public GameObject hudtextGo;
    public GameObject hudtextFollow;
    public GameObject hudtextPrefab;

    private HUDText hudtext;
    private UIFollowTarget followTarget;
    public    GameObject body;

    public string animname_normalattack;
    public float time_normalattack;//普通攻击的时间
    public string animname_crazyattack;
    public float time_crazyattack;//疯狂攻击的时间
    public float crazyattack_rate;//疯狂攻击得概率
    public Transform target;//设置攻击的目标

    public string animname_attack_now;//存储是哪种得攻击状态

    public int attack_rate = 1;//攻击速率 //每秒
    private float attack_timer = 0;//攻击得计时器

    public float minDistance = 2;//最小攻击距离
    public float maxDistance = 5;//最大攻击距离

    public WolfSpawn spawn;
    private  PlayerStatus ps;


    void Awake()
    {
        animname_now = animname_idle;
        
        cc = this.GetComponent<CharacterController>();
        normal = body.GetComponent<SkinnedMeshRenderer>().materials[0].color;
        hudtextFollow = transform.Find("HUDText").gameObject;//跟随这个物体

    }

    void Start()
    {
        //hudtextGo = GameObject.Instantiate(hudtextPrefab, Vector3.zero, Quaternion.identity);
        //hudtextGo.transform.parent = HUDTextParent._instance.gameObject.transform;
        hudtextGo = NGUITools.AddChild(HUDTextParent._instance.gameObject, hudtextPrefab);

        hudtext = hudtextGo.GetComponent<HUDText>();
        followTarget = hudtextGo.GetComponent<UIFollowTarget>();
        followTarget.target = hudtextFollow.transform;
        followTarget.gameCamera = Camera.main;

        ps = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
        // followTarget.uiCamera = UICamera.current.GetComponent<Camera>();
        //followTarget.uiCamera = UICamera.mainCamera.GetComponent<Camera>();
    }

    void Update()
    {
        if(state == WolfState.Death)
        {//死亡状态
            GetComponent<Animation>().CrossFade(animname_death); 
        }
        else if( state == WolfState.Attack)
        {//处于自动攻击状态
            
            AutoAttack();
        }
        else
        {//巡逻状态
            GetComponent<Animation>().CrossFade(animname_now);//播放当前动画
            if(animname_now == animname_walk)
            {
                cc.SimpleMove(transform.forward * speed );//用CharacterController来控制移动,这里只是往前走而已
            }

            timer += Time.deltaTime;
           if( timer >= time)
            {//计时结束,切换状态
                timer = 0;
                RandomState();
            }
        }
        //进行测试而已
        //if(Input.GetMouseButtonDown(1))
        //{
        //   TakeDamage(1);
        //}

    }

    void RandomState()
    {
        int value = Random.Range(0 , 2);//生成随机数0,1
         if(value == 0)
        {
            animname_now = animname_idle;
        }
         else
        {
            //在状态转变的时候可以转变它的方向
            if (animname_now != animname_walk)
            {
                transform.Rotate(transform.up * Random.Range(0,360));//绕着Y轴旋转
            }
            
            animname_now = animname_walk;
        }
    }


    public void TakeDamage( int attack)
    {//受到伤害时
        if (state == WolfState.Death) return;
        target = GameObject.FindGameObjectWithTag(Tags.player).transform;
        state = WolfState.Attack;
        float value = Random.Range(0f ,2f);
        if(value < miss_rate)
        {//没打中miss了
            AudioSource.PlayClipAtPoint(miss_sound, transform.position);
            hudtext.Add("Miss", Color.gray, 5);
        }
        else
        {//打中了的效果
            hudtext.Add("-"+attack, Color.red, 5);
            this.hp -= attack;
            StartCoroutine(ShowBodyRed());
            if(hp <=0)
            {
                state = WolfState.Death;
                spawn.MinusNumber();
                ps.GetExp(exp);
                BarNPC._instance.OnKillWolf();
                GameObject.Destroy(hudtextGo);
                Destroy(this.gameObject, 2);//两秒后销毁尸体
            }
        }
    }
    //使用协程来控制红色的身体的显示
    IEnumerator ShowBodyRed ()
    {
        body.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(1f);
        body.GetComponent<Renderer>().material.color = normal;
    }

    void AutoAttack()
    {
        if(target != null)
        {
            PlayerState playerState = target.GetComponent<PlayerAttack>().state;
            if(playerState == PlayerState.Death)
            {
                target = null;
                state = WolfState.Idle;
                return;
            }
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance > maxDistance)
            {//超出最大攻击距离，停止攻击
                target = null;
                state = WolfState.Idle;
            }
            else if (distance <= minDistance)
            {//自动攻击
                attack_timer += Time.deltaTime;
                GetComponent<Animation>().CrossFade(animname_attack_now);
                if(animname_attack_now == animname_normalattack)
                {//正常攻击，进行计时
                    if(attack_timer > time_normalattack)
                    {
                        target.GetComponent<PlayerAttack>().TakeDamage(attack);
                        //攻击结束，产生伤害
                        animname_attack_now = animname_idle;
                    }
                }
                else if (animname_attack_now == animname_crazyattack)
                {
                    if (attack_timer > time_crazyattack)
                    {
                        //疯狂攻击结束，产生伤害
                        target.GetComponent<PlayerAttack>().TakeDamage(attack);
                        animname_attack_now = animname_idle;
                    }
                }

                if (attack_timer > (1f / attack_rate))
                {
                    //再次进行攻击
                    RandomAttack();
                    attack_timer = 0;
                }
            }
            else
            {
                //朝着主角移动
                transform.LookAt(target);//先是朝向主角
                cc.SimpleMove(transform.forward * speed);
                GetComponent<Animation>().CrossFade(animname_walk);
            }
        }
        else
        {
            state = WolfState.Idle; 
        }
    }

    void RandomAttack()
    {
        float value = Random.Range(0, 1);
        if(value < crazyattack_rate)
        {//进行疯狂攻击
            animname_attack_now = animname_crazyattack;
        }
        else
        {//进行普通攻击
            animname_attack_now = animname_normalattack;
        }
    }

    void OnMouseEnter()
    {
        if(PlayerAttack._instance.isLockingTarget == false)
        {
            CursorManager._instance.SetAttack();
        }
        
    }
    void OnMouseExit()
    {
        if (PlayerAttack._instance.isLockingTarget == false)
        {
            CursorManager._instance.SetNormal();
        }
    }
}
