using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Camera : MonoBehaviour
{
    private float rotation_X = 0f;
    public float mouse_Sensitivty = 100f;

    public Transform player_Body;
    
    private void Start()
    {
        // Locks + hides cursor when in window
        Cursor.lockState = CursorLockMode.Locked;
        
    }// end Start

    private void Update()
    {
        // Gets axis inputs from mouse
        float mouse_X = Input.GetAxis("Mouse X") * mouse_Sensitivty * Time.deltaTime;
        float mouse_Y = Input.GetAxis("Mouse Y") * mouse_Sensitivty * Time.deltaTime;
        
        // Handles X rotation
        rotation_X -= mouse_Y;
        // Stops you from flipping
        rotation_X = Mathf.Clamp(rotation_X, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(rotation_X, 0f, 0f);
        player_Body.Rotate(Vector3.up * mouse_X);
        
        
    }// end Update
    
} // end Player_Camera
