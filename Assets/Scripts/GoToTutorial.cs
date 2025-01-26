using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToTutorial : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
