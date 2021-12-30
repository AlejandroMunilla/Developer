///My favourite script: AI. Sort of, just Virtual Inteligence to give an ilusion of inteligence. 
///Designed as a couroutine - switch - case it allows us;
///1) Build in additional behaviour; sleepng, perfoming any action, etc. just by adding a new enum value and case. 
///2) Control different behaviour in diferent times (new waitforseconds) to optimize expensive calls over funcions
///You may check a bit more complex AI behaviours on my GItHub, check DUA project EnemyAI. 
///Alejandro Munilla, Dec 30th, 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class CharacterAI : MonoBehaviour
{
    public float speed = 0.4f;
    private bool alive = true;
    public GameObject target;
    public Transform handTransform;
    private float distanceToTarget = 0.8f;
    private Animator animator;
    private AnimatorGUI animGUI;
    private NavMeshAgent nav;
    private Rig rig;

    public enum State
    {
        idle,
        move,
        ragdoll
    }
    public State state;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rig = transform.Find ("Rig").GetComponent<Rig>();
        animGUI = GameObject.FindGameObjectWithTag ("GameController").GetComponent<AnimatorGUI>();

    }

    // Start is called before the first frame update
    void OnEnable ()
    {
        state = State.idle;
        StartCoroutine("FSM");
    }

    private IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.idle:
                    Idle();
                    yield return new WaitForSeconds(0.05f);
                    break;

                case State.move:
                    Move();
                    yield return new WaitForSeconds(0.05f);
                    break;

                case State.ragdoll:
                    Radgoll();
                    yield return new WaitForSeconds(0.1f);
                    break;
            }
        }
    }


    private void Idle()
    {
        nav.isStopped = true;
        nav.speed = 0;
        animator.SetFloat("Forward", 0);
        CheckBeingDragged();

        if (target != null)
        {
            Transform ta = null;
            if (animGUI.beingDragged == true)
            {
                if (handTransform != null)
                {
                    ta = handTransform;
                }
                else
                {
                    ta = transform;
                    Debug.Log(gameObject.name + " doesnt have a hand correctly setup to be used to be dragged away. Gameobject root transform being used instead");
                }
            }
            else
            {
                ta = transform;
            }

            float distToTarget = Vector3.Distance (target.transform.position, ta.position);
            Debug.Log("Idle " + distToTarget);
            if (distToTarget > distanceToTarget)
            {
                nav.isStopped = false;
                nav.destination = target.transform.position;
                state = State.move;
            }

        }
    }


    private void Move()
    {
        if (target != null)
        {

            CheckBeingDragged();
            Transform ta = null;
            if (handTransform != null && animGUI.beingDragged == true)
            {
                ta = handTransform;
                rig.weight = 1;
            }
            else
            {
                ta = transform;
            }
            float distToTarget = Vector3.Distance(target.transform.position, ta.position);
            Debug.Log("Move " + distToTarget + "/" + distanceToTarget);
            if (distToTarget <= distanceToTarget)
            {
                nav.isStopped = true;
                nav.speed = 0;
                state = State.idle;
                animator.SetFloat("Forward", 0);
            }
            else
            {
                nav.speed = speed;
                animator.SetFloat("Forward", speed);
                nav.destination = target.transform.position;                
            }
        }
        else
        {
            state = State.idle;
        }
    }

    private void Radgoll ()
    {
        nav.isStopped = true;
        nav.speed = 0;
    }

    private void CheckBeingDragged ()
    {
        if (animGUI.beingDragged == true)
        {
            if (rig.weight == 0) rig.weight = 1;
        }
        else
        {
            if (rig.weight != 0) rig.weight = 0;
        }
    }

    public void RigOffTrigger ()
    {
        rig.weight = 0;
    }
}
