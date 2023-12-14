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
    public AudioClip Usir;
    public AudioClip Loot;
    public AudioClip Pengumuman;
    public AudioSource PlayerAudio;
    public AudioSource SFXsource;
    public AudioSource ShootSource;

    [Header("Core")]
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
    public int counter = 0;
    public Light lampu;

    public bool isTasbih = false;
    public bool isLentera = false;
    public bool isQuran = false;

    public List<GameObject> item;

    [Header("VFX")]
    [SerializeField] float range = 1000f;
    [SerializeField] public ParticleSystem ps;
    NavMeshAgent enemyagent;
    CameraShake cshake;
    Rigidbody rbnyaeffect;
    public GameObject hitscreen;
    public UIGameplayLogic UIGameplay;
    bool isCharging = false;
    float chargeDuration = 0f;
    float maxChargeDuration = 10f;
    

    void Start()
    {
        PlayerAudio = this.GetComponent<AudioSource>();
        fp = GetComponent<FirstPersonController>();
        enemyagent = Enemy.GetComponent<NavMeshAgent>();
        cshake = this.GetComponent<CameraShake> ();
        rbnyaeffect = cameffect.GetComponent<Rigidbody>();
        UIGameplay.UpdateHealth(Hitpoint, 150);
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
            isCharging = true;
            PlayerAudio.clip = ShootAudio;
            PlayerAudio.Play();
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (isCharging)
            {
                chargeDuration += Time.deltaTime;

                //  display charging effects
                
                if (chargeDuration >= maxChargeDuration)
                {
                    chargeDuration = maxChargeDuration;
                    ExecuteChargeAttack();
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (isCharging)
            {
                PlayerAudio.clip = ShootAudio;
                PlayerAudio.Stop();
                
            
                ExecuteChargeAttack();
                isCharging = false;
                chargeDuration = 0f; 
            }
        }

        

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
        UIGameplay.UpdateHealth(Hitpoint, 150);
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
            if (AIMMode)
            {
                if (fp._input.move != Vector2.zero && !fp._input.sprint)
                {
                    anim.SetBool("AIMWalk", true);
                    anim.SetBool("AIMMode", false);
                    anim.SetBool("AimRun", false);
                }               
                else if (fp._input.move != Vector2.zero && fp._input.sprint)
                {
                    anim.SetBool("AIMWalk", false);
                    anim.SetBool("AIMMode", false);
                    anim.SetBool("AimRun", true);
                }
                else if (fp._input.move == Vector2.zero)
                {
                    anim.SetBool("AIMWalk", false);
                    anim.SetBool("AIMMode", true);
                    anim.SetBool("AimRun", false);
                }
            }         
        }
        else
        {
            anim.SetBool("AIMWalk", false);
            anim.SetBool("AIMMode", false);
            anim.SetBool("AimRun", false);
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
                    anim.SetBool("AIMWalk", false);
                    anim.SetBool("AimRun", false);
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
                
                lampu.intensity = 2;
                item[0].SetActive(true);
                item[1].SetActive(false);
                item[2].SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && tasbih == 1 && counter < 2)
            {
                isLentera = false;
                isTasbih = true;
                isQuran = false;

                lampu.intensity = 1;
                item[0].SetActive(false);
                item[1].SetActive(true);
                item[2].SetActive(false);

                Debug.Log(item[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)&& quran == 1 && surah == 3)
            {
                isLentera = false;
                isTasbih = false;
                isQuran = true;              
                lampu.intensity = 1;
                item[0].SetActive(false);
                item[1].SetActive(false);
                item[2].SetActive(true);
            }
        }
       
    }
    void ExecuteChargeAttack()
    {
        if (isCharging)
        {
            float maxDamage = 50f;
            float calculatedDamage = maxDamage * Mathf.Clamp01(chargeDuration / maxChargeDuration);
            
            Shoot(calculatedDamage);

            anim.SetBool("Shoot", true);
            ps.Play();
            StartCoroutine(ResetShootAnimation());
        }
    }


    private void Shoot(float damage)
    {
        Debug.Log("shoot" + damage);
        float range = 10f;
        Vector3 direction = ShootCamera.transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(ShootCamera.transform.position, direction, out hit, range))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
               
                EnemyLogic target = hit.transform.GetComponent<EnemyLogic>();
                target.TakeDamage(damage);
            }
        }

        Debug.DrawRay(ShootCamera.transform.position, direction * range, Color.blue, 5f);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = ShootCamera.transform.TransformDirection(Vector3.forward) * range;
        Gizmos.DrawRay(ShootCamera.transform.position, direction);
    }
    IEnumerator ResetShootAnimation()
    {
        yield return new WaitForSeconds(2);
        anim.SetBool("Shoot", false); // Reset the charge attack animation state
    }

}
