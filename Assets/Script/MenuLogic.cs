using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    // Nama-nama panel
    public GameObject panelUtama;
    public GameObject panelOpsi;
    public GameObject panelCredit;

    // Metode untuk pindah ke scene
    public void PindahScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Metode untuk menampilkan panel utama
    public void TampilkanPanelUtama()
    {
        panelUtama.SetActive(true);
        panelOpsi.SetActive(false);
    }

    // Metode untuk menampilkan panel opsi
    public void TampilkanPanelOpsi()
    {
        panelUtama.SetActive(false);
        panelOpsi.SetActive(true);
    }
    public void TampilkanPanelCredit()
    {
        panelUtama.SetActive(false);
        panelCredit.SetActive(true);
    }
    // Metode untuk keluar dari game
    public void KeluarDariGame()
    {
        Application.Quit();
    }
}
