using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplayLogic : MonoBehaviour
{
    public Image HealthBar;
    public Text HealthText;

    public void UpdateHealth(float CurrentHealth, float Hitpoint)
    {
        HealthBar.fillAmount = CurrentHealth / Hitpoint;

        if (CurrentHealth == 150)
        {
            HealthBar.color = Color.green;
        }else if(CurrentHealth < 150 && CurrentHealth > 50)
        {
            HealthBar.color= Color.yellow;
        }else if(CurrentHealth <= 50)
        {
            HealthBar.color= Color.red;
        }
    }
}
