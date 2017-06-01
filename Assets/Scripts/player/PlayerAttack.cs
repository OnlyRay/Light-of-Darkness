using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    ControlWalk,
    NormalAttack,
    SkillAttack,
    Death
}

public enum AttackState
{//攻击时候的状态
    Moving,
    Idle,
    Attack
}

public class PlayerAttack : MonoBehaviour
{

    public static PlayerAttack _instance;
    public PlayerState state = PlayerState.ControlWalk;
    public AttackState attack_state = AttackState.Idle;
    public string animname_normalattack;//普通攻击动画
    public string animname_idle;//休息的动画
    public string animname_now;//当前动画
    public float time_normalattack;//普通攻击时间
    public float rate_normalattack = 1;
    private float timer;//计时器
    public float min_distance = 5;
    private Transform target_normalattack;
    private PlayerMOve move;
    public GameObject effect;
    private bool showEffect = false;//是否已经显示了特效 
    private PlayerStatus ps;
    public float miss_rate = 0.25f;
    public GameObject hudtextPrefab;
    private GameObject hudtextGo;
    private HUDText hudtext;
    private GameObject hudtextFollow;
    public AudioClip miss_sound;
    public GameObject body;
    private Color normal;
    public string animname_death;
    public GameObject[] efxArray;
    private Dictionary<string, GameObject> efxDict = new Dictionary<string, GameObject>();//利用字典方便读取技能信息
    public bool isLockingTarget = false;//表示正在选择目标
    private SkillInfo info = null;

    void Awake()
    {
        _instance = this;
        move = this.GetComponent<PlayerMOve>();
        ps = this.GetComponent<PlayerStatus>();
        normal = body.GetComponent<Renderer>().material.color;
        
        hudtextFollow = transform.Find("HUDText").gameObject;

        //把信息放在字典里
        foreach (GameObject go in efxArray)
        {
            efxDict.Add(go.name, go);
        }
    }

    void Start()
    {
        hudtextGo = NGUITools.AddChild(HUDTextParent._instance.gameObject, hudtextPrefab);
        hudtext = hudtextGo.GetComponent<HUDText>();
        UIFollowTarget followTarget = hudtextGo.GetComponent<UIFollowTarget>();
        followTarget.target = hudtextFollow.transform;
        followTarget.gameCamera = Camera.main;
    }

    void Update()
    {
        if (isLockingTarget== false && Input.GetMouseButtonDown(0) && state != PlayerState.Death)
        {
            //做射线检测
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isCollider = Physics.Raycast(ray, out hitInfo);
            if (isCollider && hitInfo.collider.tag == Tags.enemy)
            {
                //当点击了一个敌人得时候
                target_normalattack = hitInfo.collider.transform;
                state = PlayerState.NormalAttack;//让主角进入普通攻击的状态          
                timer = 0;
                showEffect = false;

            }
            else
            {
                state = PlayerState.ControlWalk;
                target_normalattack = null;
            }
        }

        if (state == PlayerState.NormalAttack)
        {
            if (target_normalattack == null)
            {
                state = PlayerState.ControlWalk;
            }
            else
            {
                float distance = Vector3.Distance(transform.position, target_normalattack.position);
                if (distance <= min_distance)
                {//进行攻击
                    transform.LookAt(target_normalattack.position);
                    attack_state = AttackState.Attack;

                    timer += Time.deltaTime;
                    GetComponent<Animation>().CrossFade(animname_now);
                    if (timer >= time_normalattack)
                    {
                        animname_now = animname_idle;
                        if (showEffect == false)
                        {
                            showEffect = true;
                            //播放特效
                            GameObject.Instantiate(effect, target_normalattack.position, Quaternion.identity);
                            target_normalattack.GetComponent<WolfBaby>().TakeDamage(GetAttack());
                        }
                    }
                    if (timer >= (1f / rate_normalattack))
                    {
                        timer = 0;
                        showEffect = false;
                        animname_now = animname_normalattack;

                    }
                }
                else
                {//没有在攻击范围之内，走向敌人
                    attack_state = AttackState.Moving;
                    move.SimpleMove(target_normalattack.position);
                }
            }
        }
        else if (state == PlayerState.Death)
        {//如果死亡就播放死亡动画
            GetComponent<Animation>().CrossFade(animname_death);

        }
        if(Input.GetMouseButtonDown(0) && isLockingTarget)
        {
            OnLockTarget();
        }
    }

    public int GetAttack()
    {
        return (int)(EquipmentUI._instance.attack + ps.attack + ps.attack_plus);
    }

    public void TakeDamage(int attack)
    {
        if (state == PlayerState.Death)
        {
            return;
        }
        float def = EquipmentUI._instance.def + ps.def + ps.def_plus;
        float temp = attack * ((200 - def) / 200);
        if (temp < 1)
        {
            temp = 1;
        }
        float value = Random.Range(0f, 1f);
        if (value < miss_rate)
        {
            //播放miss的声音和miss的文字效果
            AudioSource.PlayClipAtPoint(miss_sound, transform.position);
            hudtext.Add("MISS", Color.gray, 1);
        }
        else
        {
            hudtext.Add("-" + temp, Color.red, 1);
            ps.hp_remain -= (int)temp;
            StartCoroutine(ShowBodyRed());
            if (ps.hp_remain <= 0)
            {
                state = PlayerState.Death;
                // GameObject.Destroy(hudtextGo);
                Destroy(this.gameObject, 2);//两秒后销毁尸体
            }
        }
        HeadStatusUI._instance.UpdateShow();
    }

    IEnumerator ShowBodyRed()
    {
        body.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(1f);
        body.GetComponent<Renderer>().material.color = normal;
    }

    public void UseSkill(SkillInfo info)
    {
        if (ps.heroType == HeroType.Magician)
        {
            if (info.applicableRole == ApplicableRole.Swordman)
            {//不能使用技能
                return;
            }
            else
            {

            }
        }
        if (ps.heroType == HeroType.Swordman)
        {
            if (info.applicableRole == ApplicableRole.Magician)
            {//不能使用技能
                return;
            }
            else
            {

            }
        }

        switch (info.applyType)
        {
            case ApplyType.Passive:
                StartCoroutine(OnPassiveSkillUse(info));
                break;
            case ApplyType.Buff:
                StartCoroutine(OnBuffSkillUse(info));
                break;
            case ApplyType.SingleTarget:
                OnSingleTargetSkillUse(info);
                break;
            case ApplyType.MultiTarget:
                OnMultiTargetSkillUse(info);
                break;
        }


    }

    //处理增益技能
    IEnumerator OnPassiveSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        GetComponent<Animation>().CrossFade(info.aniname);
        yield return new WaitForSeconds(info.anitime);
        state = PlayerState.ControlWalk;
        int hp = 0;
        int mp = 0;
        if (info.applyProperty == ApplyProperty.Hp)
        {
            hp = info.applyValue;
        }
        else if (info.applyProperty == ApplyProperty.Mp)
        {
            mp = info.applyValue;
        }
        ps.GetDrug(hp, mp);
        //实例化特效
        GameObject prefab = null;
        efxDict.TryGetValue(info.efx_name, out prefab);
        GameObject.Instantiate(prefab, transform.position, Quaternion.identity);
    }
    //处理增强技能
    IEnumerator OnBuffSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        GetComponent<Animation>().CrossFade(info.aniname);
        yield return new WaitForSeconds(info.anitime);
        //实例化特效
        GameObject prefab = null;
        efxDict.TryGetValue(info.efx_name, out prefab);
        GameObject.Instantiate(prefab, transform.position, Quaternion.identity);
        state = PlayerState.ControlWalk;
        switch (info.applyProperty)
        {
            case ApplyProperty.Attack:
                ps.attack *= (info.applyValue / 100f);
                break;
            case ApplyProperty.AttackSpeed:
                rate_normalattack *= (info.applyValue / 100f);
                break;
            case ApplyProperty.Def:
                ps.def *= (info.applyValue / 100f);
                break;
            case ApplyProperty.Speed:
                move.speed *= (info.applyValue / 100f);
                break;
        }
        yield return new WaitForSeconds(info.applyTime);
        state = PlayerState.ControlWalk;
        switch (info.applyProperty)
        {
            case ApplyProperty.Attack:
                ps.attack /= (info.applyValue / 100f);
                break;
            case ApplyProperty.AttackSpeed:
                rate_normalattack /= (info.applyValue / 100f);
                break;
            case ApplyProperty.Def:
                ps.def /= (info.applyValue / 100f);
                break;
            case ApplyProperty.Speed:
                move.speed /= (info.applyValue / 100f);
                break;
        }
    }

    //准备选择某个目标
    void OnSingleTargetSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        //先得修改图标
        CursorManager._instance.SetLockTarget();
        isLockingTarget = true;
        this.info = info;
    }
    //选择目标完成，技能释放
    void OnLockTarget()
    {
        isLockingTarget = false;
        switch (info.applyType)
        {
            case ApplyType.SingleTarget:
                StartCoroutine(OnLockSingleTarget());
                break;
            case ApplyType.MultiTarget:
                StartCoroutine(OnLockMultiTarget());
                break;
        }
    }
    IEnumerator OnLockSingleTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//进行射线检测
        RaycastHit hitInfo;
        bool isCollider = Physics.Raycast(ray, out hitInfo);
        if (isCollider && hitInfo.collider.tag == Tags.enemy)
        {//选择了敌人
            GetComponent<Animation>().CrossFade(info.aniname);
            yield return new WaitForSeconds(info.anitime);
            state = PlayerState.ControlWalk;
            //实例化特效
            GameObject prefab = null;
            efxDict.TryGetValue(info.efx_name, out prefab);
            GameObject.Instantiate(prefab, hitInfo.collider.transform.position, Quaternion.identity);

            hitInfo.collider.GetComponent<WolfBaby>().TakeDamage((int)(GetAttack() * (info.applyValue / 100f)));
        }
        else
        {//当做技能取消
            state = PlayerState.NormalAttack;
        }
        CursorManager._instance.SetNormal();
    }

    void OnMultiTargetSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        //先得修改图标
        CursorManager._instance.SetLockTarget();
        isLockingTarget = true;
        this.info = info;
    }
    IEnumerator OnLockMultiTarget()
    {
        CursorManager._instance.SetNormal();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//进行射线检测
        RaycastHit hitInfo;
        bool isCollider = Physics.Raycast(ray, out hitInfo,11);
        if(isCollider)
        {
            GetComponent<Animation>().CrossFade(info.aniname);
            yield return new WaitForSeconds(info.anitime);
            state = PlayerState.ControlWalk;
            //实例化特效
            GameObject prefab = null;
            efxDict.TryGetValue(info.efx_name, out prefab);
            GameObject go = GameObject.Instantiate(prefab, hitInfo.point + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
            go.GetComponent<MagicSphere>().attack = GetAttack() * (info.applyValue / 100f);
        }
        else
        {
            state = PlayerState.ControlWalk;
        }
    }
}