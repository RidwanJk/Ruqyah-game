using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;

public class PlayerLogic : MonoBehaviour
{
    [Header("SFX")]
    public AudioClip ShootAudio;
    public AudioClip StepAudio;
    public AudioClip DeathAudio;
    AudioSource PlayerAudio;

   
    bool AIMMode = false, AIMWalk = false;
    public Animator anim;
    public float Hitpoint;
    Vector3 moveDirection;
    FirstPersonController fp;
    public Camera ShootCamera;
    public int key = 0;
    public int Weapon = 0;
    [SerializeField] float range = 1000f;
  
    // private bool isWalking = false;

    void Start()
    {
        PlayerAudio = this.GetComponent<AudioSource>();
        fp = GetComponent<FirstPersonController>();
    }

    void Update()
    {

       
        EquipWeapon(fp);
        equip(fp);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        ShootAnimation();
        // Check for player input to determine if the character is walking.
        float moveInput = Input.GetAxis("Horizontal") + Input.GetAxis("Vertical");
        if (Mathf.Abs(moveInput) > 0)
        {
            anim.SetBool("Walk", true);
        }
       

    }

    public void PlayerGetHit(float damage)
    {
        Debug.Log("Player Receive Damage -> " + damage);
        Hitpoint = Hitpoint - damage;
        anim.SetTrigger("GetHit");
        if (Hitpoint <= 0)
        {
            PlayerAudio.clip = DeathAudio;
            PlayerAudio.Play();
            anim.SetBool("Death", true);
        }
    }

    public void step()
    {
        Debug.Log("Step");
        PlayerAudio.clip = StepAudio;
        PlayerAudio.Play();
    }

    public void equip(FirstPersonController fp)
    {

        if (fp.Grounded)
        {
            if (AIMMode && fp._input.move != Vector2.zero)
            {
                anim.SetBool("AIMWalk", true);
                anim.SetBool("AIMMode", false);

            }
            else if (AIMMode && fp._input.move == Vector2.zero)
            {
                anim.SetBool("AIMWalk", false);
                anim.SetBool("AIMMode", true);

            }
        }
    }

    public void EquipWeapon(FirstPersonController fp)
    {
        if (fp.Grounded)
        {


            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Debug.Log("mouse1");
                if (AIMMode)
                {
                    AIMMode = false;
                    anim.SetBool("AIMMode", false);

                }
                else if (!AIMMode)
                {
                    AIMMode = true;
                    AIMWalk = false;
                    anim.SetBool("AIMMode", true);
                }
            }
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(ShootCamera.transform.position, ShootCamera.transform.forward, out hit, range))
        {            
            Debug.Log("Gotchaa!" + hit.transform.name);
            if (hit.transform.tag.Equals("Enemy"))
            {
                EnemyLogic target = hit.transform.GetComponent<EnemyLogic>();
                target.TakeDamage(50);
            }
        }
        else
        {
            return;
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = ShootCamera.transform.TransformDirection(Vector3.forward) * range;
        Gizmos.DrawRay(ShootCamera.transform.position, direction);
    }

    //ditaruh di keals senjata
    private void ShootAnimation()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {        
                anim.SetBool("IdleShoot", true);
                anim.SetBool("WalkShoot", false);            
        }
        else
        {
            anim.SetBool("IdleShoot", false);
            anim.SetBool("WalkShoot", false);
        }
    }
}
