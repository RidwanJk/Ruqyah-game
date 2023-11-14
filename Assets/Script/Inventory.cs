using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

public class Inventory : MonoBehaviour
{
    public InputActionReference cameraInputActionReference;
    public GameObject inventoryUi;
    public PlayerLogic Logic;
    public object GameObject;
    private bool isopen = false;

    // Update is called once per frame
    
    
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

        if (Input.GetKeyDown(KeyCode.I))
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
        

        if (Logic == null)
        {
            Debug.LogError("PlayerLogic component not found!");
            return;
        }
        if (Logic.ShootCamera == null)
        {
            Debug.LogError("ShootCamera not assigned in PlayerLogic!");
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(Logic.ShootCamera.transform.position, Logic.ShootCamera.transform.forward, out hit, 10f))
        {
            // Your logic for handling the hit goes here
            // For example, checking the tag of the object hit
            if (hit.transform.tag.Equals("Key"))
            {
                Logic.key++;
                // Perform pickup logic here
                Destroy(hit.collider.gameObject);
                Debug.Log("Item picked up!");
            }
            if (hit.transform.tag.Equals("Weapon"))
            {
                Logic.Weapon++;
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
        Debug.Log("invenopen()");
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
