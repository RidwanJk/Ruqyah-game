using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

public class Inventory : MonoBehaviour
{
    public InputActionReference cameraInputActionReference;
    public GameObject inventoryUi;
    public PlayerLogic Logic;
    public List<GameObject> box;
    
    public object GameObject;
    private bool isopen = false;

    void Update()
    {
        if (!isopen)
        {
            // Check for item pickup when inventory is not open
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryPickupItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.I)&& Logic.isQuran == true)
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isopen = !isopen;

        if (isopen)
        {
            OpenInventory();
        }
        else
        {
            CloseInventory();
        }
    }

    void TryPickupItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(Logic.ShootCamera.transform.position, Logic.ShootCamera.transform.forward, out hit, 10f))
        {
            if (hit.transform.tag.Equals("Al-Quran"))
            {
                Logic.quran++;
                Destroy(hit.collider.gameObject);
                Debug.Log("quran: "+Logic.quran);
            }
            else if (hit.transform.tag.Equals("surah"))
            {
                Logic.surah++;
                Destroy(hit.collider.gameObject);
                Debug.Log("surah: " + Logic.surah);
            }
            else if (hit.transform.tag.Equals("tasbih"))
            {
                Logic.tasbih++;
                Destroy(hit.collider.gameObject);
                Debug.Log("tasbih: " + Logic.tasbih);
            }
        }

    }

    void CloseInventory()
    {
        Debug.Log("invenclose()");
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor    
        Logic.enabled = true;
        // Hide the inventory UI
        if (inventoryUi != null)
        {
            inventoryUi.SetActive(false);
        }
        if (cameraInputActionReference != null)
        {
            cameraInputActionReference.action.Enable();
        }
    }

    void OpenInventory()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true;
        Logic.enabled = false;
        // Show the inventory UI
        if (inventoryUi != null)
        {
            inventoryUi.SetActive(true); 
        }

        // Disable camera input action
        if (cameraInputActionReference != null)
        {
            cameraInputActionReference.action.Disable();
        }
    }
}
