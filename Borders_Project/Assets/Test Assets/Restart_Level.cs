using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart_Level : MonoBehaviour
{
    public void restart_Level()
    {
        int current_Scene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(current_Scene);
    }
}
