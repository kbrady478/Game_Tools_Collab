// Tutorial used: https://www.youtube.com/watch?v=j1-OyLo77ss

using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class Enemy_FOV : MonoBehaviour
{
    public float FOV_Radius;
    [Range(0, 360)]
    public float FOV_Angle;
    
    public bool player_Visible;

    public GameObject player_Ref;
    public Vector3 last_Player_Location;
    public LayerMask target_Mask;
    public LayerMask obstacle_Mask;

    public Enemy_Movement enemy_Movement_Script;
    void Start()
    {
        player_Ref = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOV_Routine());
    } // end Start

    // Executes the FOV calculations 5 times per second instead of every frame
    private IEnumerator FOV_Routine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FOV_Check();
        }
    } // end FOV_Routine

    private void FOV_Check()
    {
        // Searches for objects only on target_Mask (the player)
        Collider[] range_Checks = Physics.OverlapSphere(transform.position, FOV_Radius, target_Mask);

        
        if (range_Checks.Length != 0)
        {
            Transform target = range_Checks[0].transform;
            Vector3 direction_To_Target = (target.position - transform.position).normalized;
            
            // Narrows down angle and checks if target is within distance
            if (Vector3.Angle(transform.forward, direction_To_Target) < (FOV_Angle / 2))
            {
                float distance_To_Target = Vector3.Distance(transform.position, target.position);
                
                // Checks if target is actually visible to enemy
                if (!Physics.Raycast(transform.position, direction_To_Target, distance_To_Target, obstacle_Mask))
                {
                    player_Visible = true;
                    // For use in Enemy_Movement Chase_Player
                    last_Player_Location = player_Ref.transform.position;
                    // Change state in Enemy_Movement to Chase_Player
                    enemy_Movement_Script.Change_State(2);
                }
                else
                    player_Visible = false;
            }
            else
                player_Visible = false;
            
        }// end range_Check detection
        // If player was previously visible and now isn't, sets bool to false
        else if (player_Visible == true)
            player_Visible = false;
    }// end FOV_Check

}// end Enemy_FOV
