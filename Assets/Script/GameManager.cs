using Lean.Gui;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;
    public InputActionReference cameraInputActionReference;
    public AudioSource musicAudioSource;
    public AudioSource enemyaudio;
    public AudioSource pauseSoundAudioSource;
    public GameObject pauseMenuUI; // Reference to your pause menu UI Canvas
    bool GameHasEnded = false;
    public GameObject GameOverL;
    public GameObject GameOverW;
    public GameObject GameOver;
    public PlayerLogic Logic;
    public EnemyLogic EnemyLogic;
    public Rigidbody kameraRoot;
    public Collider kameracol;
    public Collider PlayerCapsul;
    public GameObject enemy;
    public GameObject mc;
    public GameObject tutorkak;
    LeanWindow window;

   
    void Start()
    {
        var canvasnya = tutorkak.GetComponentInChildren<Canvas>();
        var modal = canvasnya.GetComponent<Canvas>();
        var modalwindow = modal.GetComponentInChildren<LeanWindow>();
        window = modalwindow;
        window.TurnOn();    

        // Lock and hide cursor at the start of the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Assuming you have an AudioSource component attached to the same GameObject
        if (musicAudioSource == null)
        {
            musicAudioSource = GetComponent<AudioSource>();
        }

        // Assuming pauseMenuUI is assigned in the Inspector
        if (pauseMenuUI == null)
        {
            Debug.LogError("Pause menu UI is not assigned!");
        }
        else
        {
            pauseMenuUI.SetActive(false); // Ensure the pause menu is initially hidden
        }
    }

    void Update()
    {
        //handeling death game result
        if (Logic.Hitpoint <= 0 && isPaused == false)
        {            
            Invoke("PauseGame",0.5f);                        
            GameOverL.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            GameHasEnded = true;
            isPaused = true;
            Logic.enabled = false;
            kameracol.enabled = true;
            kameraRoot.useGravity = true;                    
        }
        else if (EnemyLogic.hitPoints <= 0 && isPaused == false)
        {
            Invoke("PauseGame", 10f);                       
            GameHasEnded = true;
            isPaused = true;
            Logic.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            window.TurnOn();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PindahScene(string sceneName)
    {
        //handle exit to menu
        Logic.enabled = false;
        mc.GetComponent<CharacterController>().enabled = false;
        enemy.GetComponent<EnemyLogic>().enabled=false;
        enemy.GetComponent<NavMeshAgent>().enabled = false;
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        Debug.Log("PauseGame()");
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
        Logic.hitscreen.SetActive(false);
        

        // Disable the camera input action
        if (cameraInputActionReference != null)
        {
            cameraInputActionReference.action.Disable();
        }

        // Pause the music
        if (musicAudioSource != null)
        {
            musicAudioSource.Pause();
        }

        // Play the pause sound
        if (pauseSoundAudioSource != null)
        {
            pauseSoundAudioSource.Play();
        }
       
        if (pauseMenuUI != null && Logic.Hitpoint > 0 && GameHasEnded != true)
        {
            pauseMenuUI.SetActive(true);
        }
        else if (Logic.Hitpoint <= 0 && GameHasEnded == true)
        {
            GameOverL.SetActive(true);
            pauseMenuUI.SetActive(false);
        }
        else if (EnemyLogic.hitPoints <= 0 && GameHasEnded == true)
        {
            GameOverW.SetActive(true);
            pauseMenuUI.SetActive(false);
        }

        // Add any additional pause-related logic here
    }

    public void ResumeGame()
    {
        Debug.Log("ResumeGame()");
        Time.timeScale = 1f; // Set the time scale back to 1 to resume the game
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
        Logic.hitscreen.SetActive(true);
        // Enable the camera input action
        if (cameraInputActionReference != null)
        {
            cameraInputActionReference.action.Enable();
        }

        // Resume the music
        if (musicAudioSource != null)
        {
            musicAudioSource.UnPause();
        }
        if (pauseSoundAudioSource != null)
        {
            pauseSoundAudioSource.Pause();
        }

        // Hide the pause menu
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        } 

        // Add any additional resume-related logic here
    }


    public void RetryGame()
    {
        if (GameHasEnded == true)
        {
            GameHasEnded = true;
            GameOver.SetActive(true);
            Restart();
            Debug.Log("Restarting");
        }
    }

    public void Restart()
    {
        
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);        
    }
}

