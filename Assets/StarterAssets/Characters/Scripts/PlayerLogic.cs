using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    FirstPersonController fp;
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerGetHit(100f);
        }

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

}
