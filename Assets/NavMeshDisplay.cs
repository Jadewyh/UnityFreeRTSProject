
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class NavMeshDisplay : MonoBehaviour {
    public LineRenderer line; //to hold the line Renderer
    public Vector3 target; //to hold the transform of the target
    public NavMeshAgent agent ; //to hold the agent of this gameObject


    IEnumerator GetPath()
    {
        line.SetPosition(0, transform.position); //set the line's origin

        //agent.SetDestination(target); //create the path
        yield return new WaitForEndOfFrame(); //wait for the path to generate

        DrawPath(agent.path);

        agent.isStopped = true;//add this if you don't want to move the agent
    }
    void DrawPath(UnityEngine.AI.NavMeshPath path){
        if (path.corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;

        //line.SetVertexCount(path.corners.Length); //set the array of positions to the amount of corners

        for (var i = 1; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i]); //go through each corner and set that to the line renderer's position
        }
    }
    // Use this for initialization
    void Start () {
        line = GetComponent<LineRenderer>(); //get the line renderer
        agent = GetComponent<NavMeshAgent>(); //get the agent
        GetPath();
    }

}
