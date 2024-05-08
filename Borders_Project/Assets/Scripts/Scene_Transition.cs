using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Scene_Transition : MonoBehaviour
{
    private int scene_Number;
    
    public GameObject black_Screen;
    public int fade_Speed;
    public Color object_Colour;

    private void Start()
    {
        object_Colour = black_Screen.GetComponent<Image>().tintColor;
    }

    public void Start_Scene(int scene_Int)
    {
        scene_Number = scene_Int;
        StartCoroutine(Fade_From_Black());
    }

    private IEnumerator Fade_From_Black(bool fade_Bool = true)
    {
        float fade_Amount;

        if (fade_Bool)
        {
            while (black_Screen.GetComponent<Image>().tintColor.a < 1)
            {
                fade_Amount = object_Colour.a + (fade_Speed * Time.deltaTime);

                object_Colour = new Color(object_Colour.r, object_Colour.g, object_Colour.b, fade_Amount);
                black_Screen.GetComponent<Image>().tintColor = object_Colour;
                yield return null;
            }
        }
        else{}
        
    }
    
    
}
