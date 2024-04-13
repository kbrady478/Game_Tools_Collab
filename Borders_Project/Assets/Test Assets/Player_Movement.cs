using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Player_Movement : MonoBehaviour
{
    // Player Movement vars
    public CharacterController controller;
    public float movement_speed = 12f;
    public float gravity = -9.81f;
    public float jump_height = 3f;
    public float normal_Height, crouch_Height;
    // To change how much the grav boots affect player
    public float grav_Boot_Multiplier;
    private bool grav_Boot_Active = false;
    
    // Ground Check vars
    //public Transform ground_Check;
    public float ground_Distance = 0.4f;
    // To register things only in the ground layer, ie, not a player
    //public LayerMask ground_Mask;
    private bool is_Grounded;
    
    // Gravity vars
    private Vector3 velocity;
    
    private void Update()
    {
        /*
        // Ground Check
        // Creates invisible sphere at object, detects collisions with ground_Mask layer
        is_Grounded = Physics.CheckSphere(ground_Check.position, ground_Distance, ground_Mask);

        if (is_Grounded && velocity.y < 0)
        {
            // Not 0 because sphere protrudes  down, would cause floating effect
            velocity.y = -2f;
        }
        */
        // Movement
        float x_axis = Input.GetAxis("Horizontal");
        float z_axis = Input.GetAxis("Vertical");
        
        // Move player locally along axis
        Vector3 movement = transform.right * x_axis + transform.forward * z_axis;
        controller.Move(movement * movement_speed * Time.deltaTime);
        
       /* 
        // Jump
        if (grav_Boot_Active == false)
        {
            if (Input.GetButtonDown("Jump") && is_Grounded)
            {
                velocity.y = MathF.Sqrt(jump_height * -2f * gravity);
            }
        }
        */
        
        // Crouch / Grav Boots
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            controller.height = crouch_Height;
            //gravity = gravity * grav_Boot_Multiplier;
            //grav_Boot_Active = true;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            controller.height = normal_Height;
            gravity = gravity / grav_Boot_Multiplier;
            grav_Boot_Active = false;
        }
        
        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }// end Update
    
}// end Player_Movement
