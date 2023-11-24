using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenu : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("1 - Menu");
        //SceneManager.LoadScene("VilleScene");
    }
}
