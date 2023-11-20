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
    public GameObject GameOverMenu;
    public PlayerLogic Logic;
    public Rigidbody kameraRoot;
    public Collider kameracol;
    public Collider PlayerCapsul;
    public GameObject enemy;
    public GameObject mc;
    void Start()
    {
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
        //handeling death
        if (Logic.Hitpoint <= 0 && isPaused == false)
        {            
            Invoke("PauseGame",1.5f);                        
            GameOverMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            GameHasEnded = true;
            isPaused = true;
            Logic.enabled = false;
            kameracol.enabled = true;
            kameraRoot.useGravity = true;    
            PlayerCapsul.enabled = false;          

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
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
        Logic.enabled = false;
        mc.GetComponent<CharacterController>().enabled = false;
        enemy.GetComponent<EnemyLogic>().enabled=false;
        enemy.GetComponent<NavMeshAgent>().enabled = false;
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        Debug.Log("PauseGame()");
        Time.timeScale = 0f; // Set the time scale to 0 to pause the game
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
     

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

        // Show the pause menu
        if (pauseMenuUI != null && Logic.Hitpoint > 0)
        {
            pauseMenuUI.SetActive(true);
        }
        else if (Logic.Hitpoint <= 0)
        {
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
            GameOverMenu.SetActive(true);
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

