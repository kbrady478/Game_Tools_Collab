using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Scene_Transition : MonoBehaviour
{
    private int scene_Number;
    
    public Material black_Screen;
    public Color object_Colour;

    [Range(0,1)]
    public float fade_Speed;

    private void Start()
    {
        
    }

    public void Start_Scene(int scene_Int)
    {
        scene_Number = scene_Int;
        StartCoroutine(Fade_From_Black());
    }

    private IEnumerator Fade_From_Black()
    {
        Color screen_Transparancy = black_Screen.color;
        while (screen_Transparancy.a > 0)
        {
            screen_Transparancy.a -= fade_Speed * Time.deltaTime;
            black_Screen.color = screen_Transparancy;
        }
        
    }
    
    
}
