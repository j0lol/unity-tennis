using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class NewBehaviourScript : MonoBehaviour
{
    private const double Variance = 3.0;
    public Rigidbody body;
    public int collisions;
    public bool mySide;
    public bool served;
    public int playerPoints;
    public int opponentPoints;
    private readonly Random _random = new Random();
    private TextMeshProUGUI score;

    public void Start()
    {
        body = GetComponent<Rigidbody>();
        served = false;
        score = FindAnyObjectByType<TextMeshProUGUI>();
    }

    public void Update()
    {
        var prevSide = mySide;
        mySide = body.position.x > 0;

        if (prevSide != mySide) collisions = 0; 

        if (mySide)
        {
            if (Input.GetButtonDown("Fire1")) 
            {
                var velocity = body.velocity;
                velocity.x -= 5;
                body.velocity = velocity;
                served = true;
            }
            
            // Debug, fire ball down
            if (Input.GetButtonDown("Fire2"))
            {
                var velocity = body.velocity;
                velocity.y -= 2;
                body.velocity = velocity;
            }
        }
        
        if (body.position.x <= -2)
        {
            var velocity = body.velocity;
            velocity.x = 5 + (float) (_random.NextDouble() * Variance - Variance/2);
            body.velocity = velocity;
        }
        
        score = FindAnyObjectByType<TextMeshProUGUI>();
        score.text = playerPoints + " - " + opponentPoints;

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (served) collisions++; 
        if (collisions > 1) BallReset();
    }

    private void BallReset()
    {
        if (mySide) opponentPoints++; else playerPoints++;
        
        body.position = new Vector3((float) 1.9, 3, 0);
        body.velocity = new Vector3(0, 1, 0);
        collisions = 0;
        served = false;
        mySide = true;
    }
}
