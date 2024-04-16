// Tutorial used for patrolling state: https://www.youtube.com/watch?v=vS6lyX2QidE&t=238s

//using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Enemy_Movement : MonoBehaviour
{
    private NavMeshAgent enemy_Nav_Agent;
    
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
    }// end Start

    
    private void Update()
    {
        //Debug.Log(search_Point_Placed);
        switch (current_State)
        {
            case State.Patrol:
                Patrol_State();
                break;
            
            case State.Chase_Player:
                Chase_Player_State();
                break;
            
            case State.Search_For_Player:
                //Search_For_Player_State();
                timer += Time.deltaTime;
                TEMP_WANDER();
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

    // TEMPORARY WANDER STATE JUST TO HAVE IT WORKING ***********************
    private void TEMP_WANDER()
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
        
    }// end TEMP_WANDER
    

    
    private void Search_For_Player_State()
    {
        //Debug.Log("enter search state");
        if (search_Point_Placed == false)
            Determine_Search_Point();
        //else if (search_Point == null)
         //   Determine_Search_Point();
        else
        {
            float dist_To_Search_Point = Vector3.Distance(transform.position, search_Point.transform.position);

            if (dist_To_Search_Point <= 3)
            {
                //Destroy_Search_Point();
                Debug.Log("destroyed search point");
                Determine_Search_Point();
            }
            else
            {
                enemy_Nav_Agent.SetDestination(search_Point.transform.position);
            }
        }        

    }// end Search_For_Player_State

    
    private void Determine_Search_Point()
    {
        Ray search_Point_Placer = new Ray(transform.position, transform.forward);
        
        //Debug.Log("enter Determine_Search_Point");
        
        if (!Physics.Raycast(search_Point_Placer, out RaycastHit hit, enemy_FOV_Script.FOV_Radius, enemy_FOV_Script.obstacle_Mask, QueryTriggerInteraction.Ignore))
        {
            Vector3 search_Point_Spawn_Pos = Calculate_Spawn_Point(transform.position, transform.forward, enemy_FOV_Script.FOV_Radius);
            
            Debug.Log("search point spawned");
            search_Point_Placed = true;
            search_Point = Instantiate(search_Point_Prefab, search_Point_Spawn_Pos, Quaternion.identity);
            
        }
        
    }// end Determine_Search_Point

    
    // why god wont you just delete without breaking everything
    private void Destroy_Search_Point()
    {
        //GameObject spawned_Search_Point = GameObject.FindWithTag("Search Point");
        if (GameObject.FindWithTag("Search Point"))
        {
            Destroy(GameObject.FindWithTag("Search Point"));
            //Destroy_Search_Point();
        }
        else
        {
            Debug.Log("No object with tag \"Search Point\" found");
            return;
        }
    }

    private Vector3 Calculate_Spawn_Point(Vector3 enemy_Pos, Vector3 enemy_Dir, float FOV_Range)
    {
        // Calculate random direction FOV
        Vector3 randomDirection = Quaternion.AngleAxis(Random.Range(-enemy_FOV_Script.FOV_Angle / 2f, enemy_FOV_Script.FOV_Angle / 2f), transform.up) * enemy_Dir;

        // Calculate random distance within FOV_Range
        float randomDistance = Random.Range(min_Search_Distance, FOV_Range);

        // Calculate spawn position
        return enemy_Pos + randomDirection * randomDistance;
    }// end Calculate_Spawn_Point
    
    
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

    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player caught");
            // trigger death sequence
        }
    }
}// end Enemy_Movement


    