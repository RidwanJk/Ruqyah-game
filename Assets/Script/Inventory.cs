using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Inventory : MonoBehaviour
{
    public InputActionReference cameraInputActionReference;
    public GameObject inventoryUi;
    public PlayerLogic Logic;
<<<<<<< Updated upstream

    private bool isopen = false;

    // Start is called before the first frame update
    void Start()
    {

=======
    private bool isopen = false;
    // Start is called before the first frame update
    void Start()
    {
>>>>>>> Stashed changes
    }
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
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
    void TryPickupItem()
    {
        // Raycast to detect the item with the "Key" tag
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            if (hit.collider.CompareTag("key"))
            {
                // Perform the pickup logic here
<<<<<<< Updated upstream

                // Destroy the item in the environment
                Destroy(hit.collider.gameObject);

                // Add the item to the inventory (you might want to implement an inventory system)

=======
                // Destroy the item in the environment
                Destroy(hit.collider.gameObject);
                // Add the item to the inventory (you might want to implement an inventory system)
>>>>>>> Stashed changes
                Debug.Log("Item picked up!");
            }
        }
    }
    void CloseInventory()
    {
        Debug.Log("invenclose()");
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor    
<<<<<<< Updated upstream
            Logic.enabled = true;        
=======
        Logic.enabled = true;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
    void OpenInventory()
    {
        Debug.Log("invenopen()");
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
<<<<<<< Updated upstream
        Cursor.visible = true;              
            Logic.enabled = false;        
=======
        Cursor.visible = true;
        Logic.enabled = false;
>>>>>>> Stashed changes
        // Show the inventory UI
        if (inventoryUi != null)
        {
            inventoryUi.SetActive(true);
        }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
        // Disable camera input action
        if (cameraInputActionReference != null)
        {
            cameraInputActionReference.action.Disable();
        }
    }
}