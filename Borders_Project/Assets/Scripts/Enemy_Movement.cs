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
    private GameObject player_Object;
    private GameObject search_Point = null;
    
    public Animator enemy_Animator;
    // For if I reuse and don't hardcode
    //public String[] enemy_Animator_Bool;
    
    public Transform[] patrol_Points; // Array of patrol waypoints, added in editor
    public Enemy_FOV enemy_FOV_Script;
    
    private enum State
    {
        // Patrol between set points
        Patrol,
        // Hunt player when in sight + go to last known location
        Chase_Player,
        // When player first goes out of sight, enemy will do a short search
        Search_For_Player,
        // When enemy reaches player
        Attack_Player,
        // Standing between walking to patrol points
        Patrol_Idle
    }
    
    private State current_State = State.Patrol;
    private int previous_State = 1;
    private bool search_Point_Placed;
    
    public float patrol_Speed;
    public float chase_Speed;
    
    // Patrolling set area
    private int target_Point; // Next patrol point to walk to
    public float patrol_Idle_Time; // Time spent at patrol point before moving
    private float patrol_Timer; 
    
    // Search State vars
    public float wander_Duration;
    public int wander_Amount; // How many times the enemy will wander before going back to patrol
    private int wander_Inc = 0; // Increment for wander_Amount;
    private float search_Timer;
    
    
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
                    enemy_Animator.SetBool("Idle State", false);
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
                    enemy_Animator.SetBool("Idle State", false);
                    enemy_Animator.SetBool("Chasing State", true);
                }
                Chase_Player_State();
                break;

            case State.Search_For_Player:
                if (enemy_Animator.GetBool("Searching State") == false)
                {
                    enemy_Animator.SetBool("Chasing State", false);
                    enemy_Animator.SetBool("Idle State", false);
                    enemy_Animator.SetBool("Searching State", true);
                } 
                
                Search_State();
                break;
            
            case State.Attack_Player:
                enemy_Animator.SetBool("Walking State", false);
                enemy_Animator.SetBool("Chasing State", false);
                enemy_Animator.SetBool("Searching State", false);
                enemy_Animator.SetBool("Idle State", false);
                enemy_Animator.SetBool("Attack State", true);

                Death_Sequence();
                break;
            
            case State.Patrol_Idle:
                enemy_Animator.SetBool("Walking State",false);
                enemy_Animator.SetBool("Chasing State",false);
                enemy_Animator.SetBool("Searching State",false);
                enemy_Animator.SetBool("Idle State",true);
                
                Idle_State();
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
        if (distance_To_Waypoint <= .7)
        {
            Change_State(5);
        }
        
        enemy_Nav_Agent.SetDestination(patrol_Points[target_Point].position);

    }// end Patrol_State

    
    private void Idle_State()
    {
        if (patrol_Timer >= patrol_Idle_Time)
        {
            patrol_Timer = 0;
            target_Point = (target_Point + 1) % patrol_Points.Length;
            Change_State(1);
        }
        else
            patrol_Timer += Time.deltaTime;
    }// end Idle_State
    
    
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
        search_Timer += Time.deltaTime;
        
        if (search_Timer >= wander_Duration && wander_Inc < wander_Amount)
        {
            Debug.Log("new wander");
            Vector3 random_Point = Random.insideUnitCircle * enemy_FOV_Script.FOV_Radius;
            enemy_Nav_Agent.SetDestination(random_Point);
            search_Timer = 0;
            wander_Inc++;
        }
        else if (wander_Inc >= wander_Amount)
        {
            Debug.Log(("back to patrol"));
            wander_Inc = 0;
            Change_State(1); // go back to patrol
        }
        
    }// end Search_State
    
    
    private void Death_Sequence()
    {
        print("player died");
    }// end Attack_State
    
    
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

        if (new_State == 4)
            current_State = State.Attack_Player;

        if (new_State == 5)
            current_State = State.Patrol_Idle;
        
        previous_State = new_State;
    }// end Change_States


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player caught");
            Change_State(4);
        }
    }
}// end Enemy_Movement


    