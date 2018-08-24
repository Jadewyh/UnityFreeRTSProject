using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingOld : MonoBehaviour
{
    public float Fuel;
    public NavMeshAgent agent;
    public Vector3 destination;
    public Quaternion angle;
    private Vector3 _direction;
    private bool atairbase;
    public GameObject airstrip;
    private Animator anim;
    private bool tookoff;
    public GameObject Rotator;
    private bool circling;
    public GameObject RotatorChild;
    private Vector3 temp;
    public static bool orderedtoreturn;
    private Vector3 directair;
    private Vector3 targetland;
    private Quaternion sangle;
    private Vector3 landing;
    public AnimationClip takeoffanim;
    public AnimationClip landingclip;

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(takeoffanim.length);
        tookoff = true;

    }
    IEnumerator Reset()
    {
        yield return new WaitForSeconds(landingclip.length);
        atairbase = true;
        orderedtoreturn = false;
        anim.SetInteger("istakingoff", 0);
    }
    // Use this for initialization
    void Start ()
    {
        landing = new Vector3(airstrip.transform.position.x + 72.7f, airstrip.transform.position.y + 20.74f, airstrip.transform.position.z);
        Fuel = 10;
        orderedtoreturn = false;
        circling = false;
        atairbase = true;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 20f;
        destination = transform.position;
        anim.enabled = false;
        agent.enabled = false;
        tookoff = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log("Fuel: " + Fuel + "% " + circling);
        if (atairbase == false)
        {
            if (Fuel >= 1)
            {
                Fuel = Fuel - (2 / (1.0f / Time.deltaTime));
            }
        }
        if (Fuel >= 1 && Input.GetMouseButtonDown(0) && LegacyMovementScript.selected == true && LegacyMovementScript.firsttimeclick == false)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                destination.Set(hit.point.x, 20, hit.point.z);
            }
            if (circling == true)
            {
                circling = false;
            }
        }
        if (Fuel >= 1 && atairbase == true && Input.GetMouseButtonDown(0) && LegacyMovementScript.selected == true && LegacyMovementScript.firsttimeclick == false)
        {
            atairbase = false;
            anim.enabled = true;
            anim.applyRootMotion = true;
            anim.SetInteger("istakingoff", 1);
            StartCoroutine(Delay());
        }
        if (atairbase == true)
        {
            transform.position = new Vector3(airstrip.transform.position.x - 13, airstrip.transform.position.y + 2.8f, airstrip.transform.position.z + 5f);
            if (Fuel < 100)
            {
                Fuel = Fuel + (10 / (1.0f / Time.deltaTime)); //Fuel increases by 10 units each second
            }
        }
        if(atairbase == false && tookoff == true)
        {
            agent.enabled = true;
        }
        if (agent.enabled == true && tookoff == true && circling == false && Fuel >= 1)
        {
            agent.SetDestination(destination);
            _direction = (transform.position - agent.steeringTarget).normalized;

            angle = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, 0);
            temp = new Vector3(agent.pathEndPosition.x, agent.pathEndPosition.y + 1, agent.pathEndPosition.z);
        }
        if(transform.position == temp && tookoff == true && circling == false && Fuel >= 1 && transform.position.x != landing.x && transform.position.z != landing.z)
        {
            Rotator.transform.rotation = transform.rotation;
            Rotator.transform.position = transform.position;
            Rotator.transform.Translate(-25,0,0);
            RotatorChild.transform.position = temp;
            circling = true;

        }
        if(circling == true && Fuel >= 1 && tookoff == true)
        {
            Rotator.transform.Rotate(0, -6.0f*10*Time.deltaTime,0);
            transform.position = RotatorChild.transform.position;
            transform.rotation = Rotator.transform.rotation;
            
        }
        if((Fuel < 1 && tookoff == true) || (orderedtoreturn == true && tookoff == true))
        {
            agent.SetDestination(landing);
            targetland.Set(airstrip.transform.position.x, transform.position.y, airstrip.transform.position.z);
            directair = (targetland - transform.position).normalized;
            sangle = Quaternion.LookRotation(directair);
            transform.rotation = Quaternion.Slerp(transform.rotation, sangle, 30f * Time.deltaTime);
        }
        if ( transform.position.x == landing.x && transform.position.z == landing.z)
        {
            circling = false;
            tookoff = false;
            agent.enabled = false;
            anim.enabled = true;
            anim.applyRootMotion = true;
            anim.SetInteger("istakingoff", 2);
            StartCoroutine(Reset());
            
        }
    }
}
