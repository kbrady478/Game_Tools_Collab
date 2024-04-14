// Tutorial used for patrolling state: https://www.youtube.com/watch?v=vS6lyX2QidE&t=238s
// Rest was the class tutorial

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Movement : MonoBehaviour
{
    // Patrolling set area
    public Transform[] patrol_Points;
    public int target_Point;
    public float patrol_Speed;
    
    // Searching for Player
    private GameObject player_Object;
    private NavMeshAgent enemy_Nav_Agent;
    
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
    private void Start()
    {
        enemy_Nav_Agent = GetComponent<NavMeshAgent>();
        player_Object = GameObject.FindWithTag("Player");
        target_Point = 0;
        
    }

    private void Update()
    {
        Patrol_State();
    }

    public void Patrol_State()
    {
        float distance_To_Waypoint = Vector3.Distance(patrol_Points[target_Point].position, transform.position);

        // Checks if close enough to target waypoint, then changes to next
        if (distance_To_Waypoint <= 3)
        {
            target_Point = (target_Point + 1) % patrol_Points.Length;
        }
        
        enemy_Nav_Agent.SetDestination(patrol_Points[target_Point].position);

    }// end Patrol_State


    public void Move_To_Player()
    {
        print("player detection recieved");
    }// end Move_To_Player
}
