    using System.Collections;
    using System.Collections.Generic;
using System.Linq;
using UnityEngine;
    using UnityEngine.AI;
public class EnemyLogic : MonoBehaviour
{

        [Header("Enemy Setting")]
        public float hitPoints = 100f;
        public float turnSpeed = 15f;
        public Transform target;        
        public float ChaseRange;
        private NavMeshAgent agent;
        private float DistancetoTarget, DistancetDefault;
        public PlayerLogic PL;

              
        private Animator anim;
        Vector3 DefaultPosition;

        [Header("Enemy SFX")]
        public AudioClip GethitAudio;
        public AudioClip StepAudio;
        public AudioClip Step1Audio;
        public AudioClip AttackSwingAudio;
        public AudioClip AttackConnectAudio;
        public AudioClip DeathAudio;
        public AudioClip Iseeyou;
        public AudioSource EnemyAudio;
        public AudioSource EnemySFX;
        bool playingalready = false;

        [Header("Enemy VFX")]
        public ParticleSystem DeathEffect;
        

        [Header("Do Not Edit")]
        int currentWaypoint = 0;
        public Transform[] waypoints;
        public void SetWaypoints(List<Transform> mazeWaypoints)
        {
            waypoints = mazeWaypoints.ToArray();
        }

  

    private void Start()
        {
       
        agent = this.GetComponent<NavMeshAgent>();
            anim = this.GetComponentInChildren<Animator>();
            anim.SetFloat("Hitpoint", hitPoints);           
            DefaultPosition = this.transform.position;
        MazeLogic mazeLogic = FindObjectOfType<MazeLogic>();
        if (mazeLogic != null)
        {
            SetWaypoints(mazeLogic.WaypointsList);
        }
        else
        {
            Debug.LogError("MazeLogic not found in the scene.");
        }

        SetNextDestination();
       
    }
        private void Update()
        {

            DistancetoTarget = Vector3.Distance(target.position, transform.position);
            DistancetDefault = Vector3.Distance(DefaultPosition, transform.position);

            if (DistancetoTarget <= ChaseRange && hitPoints != 0)
            {
                FaceTarget(target.position);
                if (DistancetoTarget > agent.stoppingDistance + 0.5f)
                {
                    ChaseTarget();
                               
                }
                else if (DistancetoTarget <= agent.stoppingDistance)
                {
                    Attack();                

            }
        }
            else if (DistancetoTarget >= ChaseRange + 2f)
            {

                playingalready = false;

                if (!agent.pathPending && agent.remainingDistance <= 3f)
                {
                
                SetNextDestination();
                anim.SetBool("Idle", false);
                anim.SetBool("Run", true);
                anim.SetBool("Attack", false);
                }                                                                                                     
            }
        }


    private void FaceTarget(Vector3 destination)
        {
            Vector3 direction = (destination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }
        public void Attack()
        {            
            anim.SetBool("Run", false);
            anim.SetBool("Attack", true);
        }
        public void ChaseTarget()
        {
            agent.SetDestination(target.position);
            anim.SetBool("Run", true);
            anim.SetBool("Attack", false);
        
        if (!playingalready)
        {
            EnemySFX.clip = Iseeyou;
            EnemySFX.Play();
            playingalready = true;
        } 
    }

    private void SetNextDestination()
    {
        anim.SetBool("Run", true);        
        if (waypoints != null && waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypoint].position);

            // Increment currentWaypoint by 1
            currentWaypoint++;

            if (currentWaypoint == waypoints.Length)
            {
                
                currentWaypoint = 0;
            }
        }
        else
        {
            Debug.LogWarning("Waypoints array is null or empty in EnemyLogic.");
        }
    }

    private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ChaseRange);
        }
        public void TakeDamage(float damage)
        {
        
        if(PL.isTasbih)
        {

            PL.counter++;

            if (PL.counter <= 3) 
            {
                var x = Random.Range(0, 19);
                agent.transform.position = waypoints[x].position;
                agent.ResetPath();
                Debug.Log(x);
                
                 if (PL.counter == 3)
                {                    
                    PL.anim.SetBool("AIMMode", false);
                    PL.AIMMode = false;
                    PL.isTasbih = false;    
                    PL.item[1].SetActive(false);
                    Debug.Log("Tasbih Hancur " + PL.counter);
                }

            }
           
           
        } else if (PL.isQuran) 
        {
            hitPoints -= damage;
            EnemyAudio.clip = GethitAudio;
            EnemyAudio.Play();
            anim.SetTrigger("GetHit");
            anim.SetFloat("Hitpoint", hitPoints);
            if (hitPoints <= 0)
            {
                EnemyAudio.clip = DeathAudio;
                DeathEffect.Play();
                EnemyAudio.Play();
                anim.SetBool("Death", true);
                Destroy(gameObject, 10f);
            }
        }
           
            
        }

        public void HitConnect()
        {

            EnemyAudio.clip = AttackSwingAudio;
            EnemyAudio.Play();
            if (DistancetoTarget <= agent.stoppingDistance)
            {
                EnemyAudio.clip = AttackConnectAudio;
                EnemyAudio.Play();
                target.GetComponent<PlayerLogic>().PlayerGetHit(50f);

            }
        }
        public void step()
        {
            EnemyAudio.clip = StepAudio;
            EnemyAudio.Play();
        }
        public void step1()
        {
            EnemyAudio.clip = Step1Audio;
            EnemyAudio.Play();
        }
    }
