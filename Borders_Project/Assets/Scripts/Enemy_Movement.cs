// Tutorial used for patrolling state: https://www.youtube.com/watch?v=vS6lyX2QidE&t=238s

//using System;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Enemy_Movement : MonoBehaviour
{
    private NavMeshAgent enemy_Nav_Agent;

    public Animator enemy_Animator;
    // For if I reuse and don't hardcode
    //public String[] enemy_Animator_Bool;

    public float patrol_Speed;
    public float chase_Speed;
    
    // Patrolling set area
    public int target_Point; // Next patrol point to walk to
    
    public Transform[] patrol_Points; // Array of patrol waypoints, added in editor
    
    // Searching for Player
    private bool search_Point_Placed;
    
    public float min_Search_Distance;
    
    private GameObject player_Object;
    private GameObject search_Point = null;
    
    public GameObject search_Point_Prefab;
    public Enemy_FOV enemy_FOV_Script;

    
    private enum State
    {
        // Patrol between set points
        Patrol,
        // Hunt player when in sight + go to last known location
        Chase_Player,
        // When player first goes out of sight, enemy will do a short search
        Search_For_Player
        // 
        
    }
    private State current_State = State.Patrol;
    private int previous_State = 1;

    // TEMP WANDER VARS **************
    public float wander_Duration;
    public int wander_Amount; // How many time the enemy will wander before going back to patrol
    private int wander_Inc = 0; // Increment for wander_Amount;
    public float timer;
    
    
    private void Start()
    {
        enemy_Nav_Agent = GetComponent<NavMeshAgent>();
        player_Object = GameObject.FindWithTag("Player");
        target_Point = 0;
        search_Point_Placed = false;
        enemy_Animator.SetBool("Walking State", true);
        enemy_Nav_Agent.speed = patrol_Speed;
    }// end Start

    
    private void Update()
    {
        //Debug.Log(search_Point_Placed);
        switch (current_State)
        {
            case State.Patrol:
                if (enemy_Animator.GetBool("Walking State") == false)
                {
                    enemy_Nav_Agent.speed = patrol_Speed;
                    enemy_FOV_Script.FOV_Angle = enemy_FOV_Script.temp_Searching_FOV_Angle;
                    enemy_Animator.SetBool("Searching State", false);
                    enemy_Animator.SetBool("Chasing State", false);
                    enemy_Animator.SetBool("Walking State", true);
                }
                Patrol_State();
                break;
            
            case State.Chase_Player:
                if (enemy_Animator.GetBool("Chasing State") == false)
                {
                    enemy_Nav_Agent.speed = chase_Speed;
                    enemy_FOV_Script.FOV_Angle = enemy_FOV_Script.searching_FOV_Angle;
                    enemy_Animator.SetBool("Walking State", false);
                    enemy_Animator.SetBool("Searching State", false);
                    enemy_Animator.SetBool("Chasing State", true);
                }
                Chase_Player_State();
                break;

            case State.Search_For_Player:
                if (enemy_Animator.GetBool("Searching State") == false)
                {
                    enemy_Animator.SetBool("Chasing State", false);
                    enemy_Animator.SetBool("Searching State", true);
                }
                //Search_For_Player_State();
                timer += Time.deltaTime;
                Search_State();
                break;
        }// end State switch
    }// end Update

    
    private void Patrol_State()
    {
        if (search_Point != null)
        {
            search_Point_Placed = false;
            //Destroy_Search_Point();
        }
        
        float distance_To_Waypoint = Vector3.Distance(patrol_Points[target_Point].position, transform.position);

        // Checks if close enough to target waypoint, then changes to next
        if (distance_To_Waypoint <= 3)
        {
            // put timer here **************
            target_Point = (target_Point + 1) % patrol_Points.Length;
        }
        
        enemy_Nav_Agent.SetDestination(patrol_Points[target_Point].position);

    }// end Patrol_State

    
    private void Chase_Player_State()
    {
        if (search_Point != null)
        {
            search_Point_Placed = false;
            //Destroy_Search_Point();
        }
        
        float dist_To_Last_Known_Location = Vector3.Distance(transform.position, enemy_FOV_Script.last_Player_Location);
        
        //Debug.Log("enter chase state");
        
        if (dist_To_Last_Known_Location <= 3)
        {
            Debug.Log("reached last known");
            Change_State(3);
            return;
        }
        
        // Chases player instead of just mindlessly walking towards the first given location
        if (enemy_FOV_Script.player_Visible == true)
        {
            enemy_Nav_Agent.SetDestination(player_Object.transform.position);
        }
        else
        {
            enemy_Nav_Agent.SetDestination(enemy_FOV_Script.last_Player_Location);
        }
        
    }// end Chase_Player_State

    
    private void Search_State()
    {
        if (timer >= wander_Duration && wander_Inc < wander_Amount)
        {
            Debug.Log("new wander");
            Vector3 random_Point = Random.insideUnitCircle * enemy_FOV_Script.FOV_Radius;
            enemy_Nav_Agent.SetDestination(random_Point);
            timer = 0;
            wander_Inc++;
        }
        else if (wander_Inc >= wander_Amount)
        {
            Debug.Log(("back to patrol"));
            wander_Inc = 0;
            Change_State(1); // go back to patrol
        }
        
    }// end Search_State
    
    
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player caught");
            // trigger death sequence
        }
    }
}// end Enemy_Movement


    