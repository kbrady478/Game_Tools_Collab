// Tutorial used for patrolling state: https://www.youtube.com/watch?v=vS6lyX2QidE&t=238s

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Movement : MonoBehaviour
{
    private NavMeshAgent enemy_Nav_Agent;
    
    // Patrolling set area
    public Transform[] patrol_Points;
    public int target_Point;
    
    // Searching for Player
    private GameObject player_Object;
    public Enemy_FOV enemy_FOV_Script;

    // Walk speeds etc must be changed in the NavMeshAgent, depends on Agent type
    private enum State
    {
        // Patrol between set points
        Patrol,
        // Hunt player when in sight + go to last known location
        Chase_Player,
        // When player first goes out of sight, enemy will do a short search
        // May not keep
        Search_For_Player
        
    }
    private State current_State = State.Patrol;
    private int previous_State = 1;
    
    
    private void Start()
    {
        enemy_Nav_Agent = GetComponent<NavMeshAgent>();
        player_Object = GameObject.FindWithTag("Player");
        target_Point = 0;
        
    }// end Start

    private void Update()
    {
        switch (current_State)
        {
            case State.Patrol:
                Patrol_State();
                break;
            
            case State.Chase_Player:
                Chase_Player_State();
                break;
            
            case State.Search_For_Player:

                break;
        }// end State switch
    }// end Update

    private void Patrol_State()
    {
        float distance_To_Waypoint = Vector3.Distance(patrol_Points[target_Point].position, transform.position);

        // Checks if close enough to target waypoint, then changes to next
        if (distance_To_Waypoint <= 3)
        {
            target_Point = (target_Point + 1) % patrol_Points.Length;
        }
        
        enemy_Nav_Agent.SetDestination(patrol_Points[target_Point].position);

    }// end Patrol_State

    private void Chase_Player_State()
    {
        if (enemy_FOV_Script.player_Visible == true)
        {
            enemy_Nav_Agent.SetDestination(player_Object.transform.position);
        }
        else
        {
            enemy_Nav_Agent.SetDestination(enemy_FOV_Script.last_Player_Location);
        }
    }// end Chase_Player_State
    
    
    
    // 1 is Patrol, 2 is Chase, 3 is Search
    public void Change_State(int new_State)
    {
        //Debug.Log(previous_State);
        if (previous_State == new_State)
            return;
        
        Debug.Log("Changing state");
        
        if (new_State == 1)
            current_State = State.Patrol;
        
        if (new_State == 2)
        {
            // play surprised animation + sound
            current_State = State.Chase_Player;
        }

        if (new_State == 3)
            current_State = State.Search_For_Player;

        previous_State = new_State;
    }// end Change_States

}
