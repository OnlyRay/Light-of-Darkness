using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ControlWalkState
{
    Moving,
    Idle
}


public class PlayerMOve : MonoBehaviour {
    public float speed = 3;
   
    public ControlWalkState state = ControlWalkState.Idle;
    private PlayerDir dir;
    private CharacterController controller;
    private PlayerAttack attack;
    public bool isMoving = false;
    void Start()
    {
        dir = this.GetComponent<PlayerDir>();
        controller = this.GetComponent<CharacterController>();
        attack = this.GetComponent<PlayerAttack>();
    }
	
	void Update () {
        if (attack.state == PlayerState.ControlWalk)
        {
            float distance = Vector3.Distance(dir.targetPosition, transform.position);
            if (distance > 0.3f)
            {
                isMoving = true;
                state = ControlWalkState.Moving;
                controller.SimpleMove(transform.forward * speed);
            }
            else
            {
                isMoving = false;
                state = ControlWalkState.Idle;
            }
        }
	}


    public void SimpleMove(Vector3 targetPos)
    {
        transform.LookAt(targetPos);
        controller.SimpleMove(transform.forward * speed);
    }
}
