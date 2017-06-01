using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private PlayerMOve move;
    private PlayerAttack attack;
    
    // Use this for initialization
	void Start () {
        move = this.GetComponent<PlayerMOve>();
        attack = this.GetComponent<PlayerAttack>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if(attack.state == PlayerState.ControlWalk)
        {
            if (move.state == ControlWalkState.Moving)
            {
                PlayAnim("Run");
            }
            else if (move.state == ControlWalkState.Idle)
            {
                PlayAnim("idle");
            }
        }
        else if(attack.state == PlayerState.NormalAttack)
        {
            if(attack.attack_state == AttackState.Moving)
            {
                PlayAnim("Run");
            }
        }
		
	}

    void PlayAnim(string animName)
    {
 
        GetComponent<Animation>().CrossFade(animName);
    }
}
