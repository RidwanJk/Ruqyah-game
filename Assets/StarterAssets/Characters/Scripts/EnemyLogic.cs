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
    }
    private void Update()
    {

        DistancetoTarget = Vector3.Distance(target.position, transform.position);
        DistancetDefault = Vector3.Distance(DefaultPosition, transform.position);    
        
        if (DistancetoTarget <= ChaseRange)
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
        else if (DistancetoTarget >= ChaseRange * 2)
        {
            agent.SetDestination(DefaultPosition);
            FaceTarget(DefaultPosition);
            if (DistancetDefault <= agent.stoppingDistance)
            {
                Debug.Log("welp to far");              
               
                    anim.SetBool("Idle", true);
                    anim.SetBool("Run", false);
                    anim.SetBool("Attack", false);
                
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
            Destroy(gameObject, 3f);
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
