using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public InputActionReference cameraInputActionReference;
    public GameObject inventoryUi;

    bool isopen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToogleInventory();
        }
    }
    void ToogleInventory()
    {
        isopen = !isopen;

        if (isopen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }
    void CloseInventory()
    {
        Debug.Log("invenclose()");   
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
        // Hide the pause menu
        if (inventoryUi != null)
        {
            inventoryUi.SetActive(false);
        }
    }
    void OpenInventory()
    {
       
        Debug.Log("invenopen()");
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; //
                               // Hide the pause menu
        if (cameraInputActionReference != null)
        {
            cameraInputActionReference.action.Disable();
        }
        if (inventoryUi != null)
        {
            inventoryUi.SetActive(true);
        }
    }
}
