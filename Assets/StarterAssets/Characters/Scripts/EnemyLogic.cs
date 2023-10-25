using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyLogic : MonoBehaviour
{
    public float hitPoints = 100f;
    public float turnSpeed = 15f;
    public Transform target;
    public float ChaseRange;
    private NavMeshAgent agent;
    private float DistancetoTarget, DistancetDefault;
    private Animator anim;
    Vector3 DefaultPosition;

    private void Start()
    {
        agent=this.GetComponent<NavMeshAgent>();
        anim=this.GetComponentInChildren<Animator>();
        anim.SetFloat("Hitpoint", hitPoints);
        DefaultPosition = this.transform.position;
        MoveForward();
    }
    private void Update()
    {
        DistancetoTarget = Vector3.Distance(target.position, transform.position);
        DistancetDefault = Vector3.Distance(DefaultPosition, transform.position);

        ObstacleHandling();
        if (DistancetoTarget <= ChaseRange && hitPoints > 0)
        {
           if (DistancetoTarget > agent.stoppingDistance)
           {
              ChaseTarget();
           }
           else if (DistancetoTarget <= agent.stoppingDistance)
           {
              Attack();
           }

        }
        else if (DistancetoTarget > ChaseRange *2)
        {
            MoveForward();
            ObstacleHandling();
        }


    }



    private void MoveForward()
    {
        agent.SetDestination(transform.position + transform.forward * 1000f);
        anim.SetBool("Idle", false);
        anim.SetBool("Run", true);

    }

    private void ObstacleHandling()
    {
        Vector3 forward = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, forward, out hit, 1f))
        {
            Debug.Log("I hit this thing: " + hit.transform.name);
                if (hit.collider != null && !hit.transform.tag.Equals("Player"))
                {
                    transform.Rotate(Vector3.up, 90f);
                    MoveForward();
                }
        }

       
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 direction =(destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    public void Attack()
    {
        Debug.Log("attack");        
        anim.SetBool("Run", false);
        anim.SetBool("Attack", true);
    }
    public void ChaseTarget()
    {
        agent.SetDestination(target.position);
        anim.SetBool("Run", true);
        anim.SetBool("Attack", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }
    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        anim.SetTrigger("GetHit");
        anim.SetFloat("Hitpoint", hitPoints);
        if (hitPoints <= 0)
        {
            anim.SetBool("Death",true);
            Destroy(gameObject, 10f);
        }
    }

    public void HitConnect()
    {
        if(DistancetoTarget <= agent.stoppingDistance)
        {
            target.GetComponent<PlayerLogic>().PlayerGetHit(50f);

        }
    }
}
