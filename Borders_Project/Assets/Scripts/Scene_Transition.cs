using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Scene_Transition : MonoBehaviour
{
    private int scene_Number;
    
    public Material black_Screen;

    [Range(0,1)]
    public float fade_Speed;

    public bool start_Sequence;
    public bool death_Sequence;
    private Color fade_Transparancy;
    
    
    //public float trans_Test;

    private void Start()
    {
        Color reset_Colour = black_Screen.color;
        reset_Colour.a = 1;
        black_Screen.color = reset_Colour;
        
        start_Sequence = true;
        death_Sequence = false;
        fade_Transparancy = black_Screen.color;
    }

    private void Update()
    {
        print("start: "+start_Sequence);
        print("death: "+death_Sequence);

        if (start_Sequence == true && black_Screen.color.a >= 0)
        {
            
            
            fade_Transparancy.a -= fade_Speed * Time.deltaTime;
            black_Screen.color = fade_Transparancy;
            
            if (black_Screen.color.a == 0)
            {
                start_Sequence = false;
            }
        }

        if (death_Sequence == true)
        {
            start_Sequence = false;
            //print("buh");
            if (black_Screen.color.a <= 1)
            {
                //print("ah");

                fade_Transparancy.a += fade_Speed * Time.deltaTime;
                black_Screen.color = fade_Transparancy;

                if (black_Screen.color.a >= 1)
                {
                    death_Sequence = false;
                    Scene currentScene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(currentScene.buildIndex);
                }
            }
        }
        
    }// end Update
    
}
