using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class PlayerLogic : MonoBehaviour
{
    [Header("SFX")]
    public AudioClip ShootAudio;
    public AudioClip StepAudio;
    public AudioClip DeathAudio;
    AudioSource PlayerAudio;

    public GameObject Enemy;
    public bool AIMMode = false;
    public Animator anim;
    public float Hitpoint;
    Vector3 moveDirection;
    FirstPersonController fp;
    public Camera ShootCamera;
    public GameObject cameffect;
    public int quran = 0;
    public int surah = 0;
    public int tasbih = 0;
    public int lentera = 0;
    public int fullItem = 0;

    public bool isTasbih = false;
    public bool isLentera = false;
    public bool isQuran = false;

    public List<GameObject> item;

    [SerializeField] float range = 1000f;
    NavMeshAgent enemyagent;
    CameraShake cshake;
    Rigidbody rbnyaeffect;
    public GameObject hitscreen;

    // private bool isWalking = false;

    void Start()
    {
        PlayerAudio = this.GetComponent<AudioSource>();
        fp = GetComponent<FirstPersonController>();
        enemyagent = Enemy.GetComponent<NavMeshAgent>();
        cshake = this.GetComponent<CameraShake> ();
        rbnyaeffect = cameffect.GetComponent<Rigidbody>();
    }

    void Update()
    {
      
        if (hitscreen != null)
        {
            if (hitscreen.GetComponentInChildren<Image>().color.a > 0)
            {
                var color = hitscreen.GetComponentInChildren<Image>().color;
                color.a -= 0.01f;
                hitscreen.GetComponentInChildren<Image>().color = color;
            }
        }

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

        pilihWeapon();
       

    }
    void gethurt()
    {
        //hitscreen
        var color = hitscreen.GetComponentInChildren<Image>().color;
        color.a = 0.8f;
        hitscreen.GetComponentInChildren<Image>().color = color;
    }

    public void PlayerGetHit(float damage)
    {
        Debug.Log("Player Receive Damage -> " + damage);

        gethurt();
       
        Hitpoint = Hitpoint - damage;
        cshake.shakeDuration = 0.5f;
        anim.SetTrigger("GetHit");
        if (Hitpoint <= 0)
        {
            hitscreen.SetActive(false);
            PlayerAudio.clip = DeathAudio;
            PlayerAudio.Play();
            anim.SetBool("Death", true);
            this.GetComponent<PlayerLogic>().enabled = false;
            enemyagent.enabled = false;
            this.GetComponent<CharacterController>().enabled = false;
            rbnyaeffect.AddForce(Vector3.up * 50, ForceMode.Impulse);

        }
    }

    public void step()
    {
        PlayerAudio.clip = StepAudio;
        PlayerAudio.Play();
    }

    public void equip(FirstPersonController fp)
    {

        if (fp.Grounded)
        {
            if (AIMMode && fp._input.move != Vector2.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("AIMWalk", true);
                anim.SetBool("AIMMode", false);
                anim.SetBool("AimRun", false);             
            }
            else if (AIMMode && fp._input.move == Vector2.zero)
            {
                anim.SetBool("AIMWalk", false);
                anim.SetBool("AIMMode", true);
                anim.SetBool("AimRun", false);
            }else if (AIMMode && fp._input.move != Vector2.zero && Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("AIMWalk", false);
                anim.SetBool("AIMMode", false);
                anim.SetBool("AimRun", true);
            }
        }
    }

    public void EquipWeapon(FirstPersonController fp)
    {
        if (fp.Grounded)
        {

            
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (AIMMode)
                {
                    AIMMode = false;
                    anim.SetBool("AIMMode", false);
                }
                else if (!AIMMode)
                {
                    AIMMode = true;
                    anim.SetBool("AIMMode", true);

                }
            } 
        }
    }

    public void pilihWeapon()
    {
        if (!AIMMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                isLentera = true;
                isTasbih = false;
                isQuran = false;

                item[0].SetActive(true);
                item[1].SetActive(false);
                item[2].SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && tasbih == 1)
            {
                isLentera = false;
                isTasbih = true;
                isQuran = false;

                item[0].SetActive(false);
                item[1].SetActive(true);
                item[2].SetActive(false);

                Debug.Log(item[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)&& quran == 1)
            {
                isLentera = false;
                isTasbih = false;
                isQuran = true;

                item[0].SetActive(false);
                item[1].SetActive(false);
                item[2].SetActive(true);
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
