﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AITank : MonoBehaviour {

    public float radius = 10;
    public int numWaypoints = 5;
    public int current = 0;
    List<Vector3> waypoints = new List<Vector3>();
    public float speed = 10;
    public Transform player;    

    public void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            float thetaInc = (Mathf.PI * 2.0f) / numWaypoints;
            for (int i = 0; i < numWaypoints; i++)
            {
                float theta = i * thetaInc;
                Vector3 pos = new Vector3(Mathf.Sin(theta) * radius, 0, Mathf.Cos(theta) * radius);
                pos = transform.TransformPoint(pos);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(pos, 1);
            }
        }
    }

    // Use this for initialization
    void Awake () {
        float thetaInc = (Mathf.PI * 2) / numWaypoints;
        for (int i = 0; i < numWaypoints; i++)
        {
            float theta = i * thetaInc;
            Vector3 pos = new Vector3(Mathf.Sin(theta) * radius, 0, Mathf.Cos(theta) * radius);
            pos = transform.TransformPoint(pos);
            waypoints.Add(pos);
        }
    }

    // Update is called once per frame
    void Update () {
        Vector3 toTarget = waypoints[current] - transform.position;
        if (toTarget.magnitude < 1)
        {
            current = (current + 1) % waypoints.Count;            
        }
        toTarget.Normalize();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(toTarget), Time.deltaTime * 5);
        transform.Translate(toTarget * speed * Time.deltaTime, Space.World);

        Vector3 toPlayer = player.position - transform.position;
        if (Vector3.Dot(transform.forward, toPlayer) < 0)
        {
            GameManager.Log("Player is behind");
        }
        else
        {
            GameManager.Log("Player is in front");
        }
        float angle = Mathf.Acos(Vector3.Dot(transform.forward, toPlayer) / toPlayer.magnitude) * Mathf.Rad2Deg;
        GameManager.Log("Angle to player 1: " + angle);
        //Log("Angle to player 2: " + Vector3.Angle(transform.forward, toPlayer));
        if (angle < 45)
        {
            GameManager.Log("Player is inside the FOV");
        }
        else
        {
            GameManager.Log("Player is outside the FOV");
        }
    }
}
