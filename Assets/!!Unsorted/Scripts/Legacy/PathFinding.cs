using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinding : MonoBehaviour {
    [SerializeField]
    Transform _destination;
    NavMeshAgent agent;
    public double hyp;
    public Quaternion angle;
    private Vector3 _direction;
    private Vector3 targetpos;
    public static Vector3 unitpos;
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        targetpos = agent.transform.position;
       
	}
	
	// Update is called once per frame
	void Update () {
        hyp = System.Math.Sqrt((agent.pathEndPosition.x - transform.position.x)* (agent.pathEndPosition.x - transform.position.x)
            + (agent.pathEndPosition.y - transform.position.y)* (agent.pathEndPosition.y - transform.position.y)
            + (agent.pathEndPosition.z - transform.position.z)* (agent.pathEndPosition.z - transform.position.z));
        if (Input.GetMouseButtonDown(0) && LegacyMovementScript.selected == true && LegacyMovementScript.firsttimeclick == false)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                targetpos.Set(hit.point.x, transform.position.y, hit.point.z);
            }
        }
        agent.SetDestination(targetpos);
        _direction = (agent.steeringTarget - transform.position).normalized;
        
        angle = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, 0);
    }
}
