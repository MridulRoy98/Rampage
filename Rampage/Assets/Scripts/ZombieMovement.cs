using System;
//using System.Diagnostics;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject destroyer;
    PlayerMovement playermovement;

    [Header("Navmesh things")]
    private NavMeshAgent agent;
    public GameObject navmeshSurface;
    private NavMeshSurface surface;

    [Header("Zombie Stats")]
    private Animator zombieAnimator;
    private float rotationSpeed = 1f;
    public bool isChasing = false;
    private bool canDamage = false;

    public delegate void ZombieDamageEventHandler(object source, EventArgs args);
    public event ZombieDamageEventHandler InflictedDamage;

    private void Awake()
    {
        player = GameObject.Find("Character");
        playermovement = player.GetComponent<PlayerMovement>();
        zombieAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        surface = navmeshSurface.GetComponent<NavMeshSurface>();
    }
    private void Start()
    {
        destroyer = GameObject.Find("Zombie_Destroyer");
    }
    void Update()
    {
        ModeSelection();
        setSpeed();
        if (isChasing)
        {
            FollowMode();
        }
        else
        {
            AttackMode();
        }

    }

    //Deciding whether the zombie should chase or attack
    private void ModeSelection()
    {
        float distance = Vector3.Distance(transform.position, playermovement.GetPlayerPosition());
        if (distance > 1f)
        {
            zombieAnimator.SetBool("zombie_attack", false);
            zombieAnimator.SetBool("zombie_slam", false);
            isChasing = true;
        }
        else
        {
            isChasing=false;
        }
    }

    //Chasing
    private void FollowMode()
    {
        agent.SetDestination(playermovement.GetPlayerPosition());
        Vector3 lookDirection = playermovement.GetPlayerPosition() - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        chasingAnimation();
    }

    //Randomize chase animation
    private void chasingAnimation()
    {
        int animNumber = UnityEngine.Random.Range(0, 3);
        switch (animNumber)
        {
            case 0:
                zombieAnimator.SetBool("zombie_walk", true);
                break;

            case 1:
                zombieAnimator.SetBool("zombie_run", true);
                break;

            case 2:
                zombieAnimator.SetBool("zombie_crawl", true);
                break;
        }
    }

    //Attacking
    private void AttackMode()
    {
        Debug.Log("inAttackMode");
        isChasing = false;
        int animNumber = UnityEngine.Random.Range(0, 2);
        switch (animNumber)
        {
            case 0:
                zombieAnimator.SetBool("zombie_attack", true);
                break;
                
            case 1:
                zombieAnimator.SetBool("zombie_slam", true);
                break;
        }
    }

    //Randomize speed for different zombie animations
    private void setSpeed()
    {
        if (zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie_Walk"))
        {
            agent.speed = UnityEngine.Random.Range(0.4f, 0.5f);
        }
        else if (zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie_Run"))
        {
            agent.speed = UnityEngine.Random.Range(1.4f, 1.6f);
        }
        else if (zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Zombie_Crawl"))
        {
            agent.speed = UnityEngine.Random.Range(2.8f, 3f);
        }
    }

    //Destroys zombie when too far from player
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == destroyer.name)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnInflictedDamage()
    {
        if(InflictedDamage != null)
        {
            InflictedDamage(this, EventArgs.Empty);
        }
    }

    ///////////////////////////////////////////////////////////
    //Combat Mode//
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject == player && canDamage == true)
        {
            //Trigger Event
            OnInflictedDamage();
        }
    }

    //Animation Events//
    private bool DamageOn()
    {
        canDamage = true;
        return true;
    }
    private bool DamageOff()
    {
        canDamage = false;
        return true;
    }
    /////////////////////////
}
