using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class WaypointModel
{

    //public Transform Waypoint;
    public Vector3 Pos;
    public float TimeToWait = 0.0f;
    public bool HasTime = false;
    public bool WithoutRotate = false;

    public bool IsDirectWay = false;

    public WaypointModel()
    {
        
    }

    public WaypointModel(Transform waypoint, float timeToWait = 0.0f, bool withoutRot = false)
    {
        //Waypoint = waypoint;
        Pos = waypoint.position;
        TimeToWait = timeToWait;
        HasTime = TimeToWait > 0.0f;
        WithoutRotate = withoutRot;
    }
    public WaypointModel(Vector3 pos, float timeToWait = 0.0f, bool withoutRot = false)
    {
        Pos = pos;
        TimeToWait = timeToWait;
        HasTime = TimeToWait > 0.0f;
        WithoutRotate = withoutRot;
    }

    public WaypointModel(WaypointLogic waypoint)
    {
        if (waypoint == null) return;
        //Waypoint = waypoint;
        Pos = waypoint.gameObject.transform.position;
        TimeToWait = waypoint.Timer;
        HasTime = TimeToWait > 0.0f;
        WithoutRotate = waypoint.WithoutRotate;
    }

    public Vector3 Position(bool inversed)
    {
        //var lx = -6.0f;
        //var rx = 6.0f;
        //var pos = Waypoint.position;
        //var x = pos.x < 0.0f ? 
        var dir = inversed && !IsDirectWay ? -1 : 1;
        //return new Vector3(Waypoint.position.x * dir, Waypoint.position.y, Waypoint.position.z);
        return new Vector3(Pos.x * dir, Pos.y, Pos.z);
    }

}



public class ParentWaypointModel
{
    public Int32 LoopStartIndex = 0;
    public List<WaypointModel> Waypoints = new List<WaypointModel>();
    
    public ParentWaypointModel()
    {
        
    }

}