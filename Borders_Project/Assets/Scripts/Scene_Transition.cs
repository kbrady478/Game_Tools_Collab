using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Scene_Transition : MonoBehaviour
{
    private int scene_Number;
    
    public Material black_Screen;

    [Range(0,1)]
    public float fade_Speed;

    public bool start_Sequence = true;
    public bool death_Sequence = false;
    
    //public float trans_Test;

    private void Start()
    {
        Color reset_Colour = black_Screen.color;
        reset_Colour.a = 1;
        black_Screen.color = reset_Colour;
    }

    private void Update()
    {
        if (start_Sequence = true && black_Screen.color.a >= 0)
        {
            Color fade_Transparancy = black_Screen.color;
            
            fade_Transparancy.a -= fade_Speed * Time.deltaTime;
            black_Screen.color = fade_Transparancy;
            
            if (black_Screen.color.a == 0)
            {
                start_Sequence = false;
            }
        }

        if (death_Sequence = true && black_Screen.color.a <= 1)
        {
            
        }
    }
}
