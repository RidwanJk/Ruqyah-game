using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    public void pindahScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
