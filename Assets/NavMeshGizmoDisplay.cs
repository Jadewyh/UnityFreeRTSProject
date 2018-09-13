using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshGizmoDisplay : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent nav;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDrawGizmos()
    {

        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (nav == null) {
          Debug.Log("No NavAgent found.");
            return;
          }
          if (nav.path == null){
            Debug.Log("Could not find Path!");
            return;
          }


        UnityEngine.AI.NavMeshPath path = nav.path;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
    }
}
