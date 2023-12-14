using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplayLogic : MonoBehaviour
{
    public Image HealthBar;
    public Text HealthText;

    public void UpdateHealth(float CurrentHealth, float Hitpoint)
    {
        HealthBar.fillAmount = CurrentHealth / Hitpoint;
        
    }
}
