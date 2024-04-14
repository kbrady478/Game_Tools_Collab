using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_States : MonoBehaviour
{

    public float wander_Duration;
    
    void Start()
    {
        
    }

    private enum State
    {
        Wander,
        Chase_Player
    }
    
    
}
