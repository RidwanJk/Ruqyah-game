using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHand : MonoBehaviour
{
    public Transform playerCamera;

    void Update()
    {
        if (playerCamera != null)
        {          
            transform.rotation = playerCamera.rotation;
        }
        else
        {
            Debug.LogWarning("Player camera reference not set!");
        }
    }
}
