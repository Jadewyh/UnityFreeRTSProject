using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GenericUnit : MonoBehaviour
{
    public float maxHealth;
    public bool canCombat;
    public float combatDamage;
    public float combatRangeMax;
    public float combatRangeMin = 0f; // example are trebuchet in AoE which cannot combat unless there is a distance between attacker and receiver


    public float currentHealth;
    public float movementSpeed = 1f;
    public float maxMovementSpeed = 1f;

    // For example: Buildings or drone bots
    public GenericConstructionObject[] availableConstructionObjects;
    public Vector3 constructionObjectSpawnPoint;

    public bool isMoving;
    public bool canMove;
    public bool isSelected;

    public Vector3 moveTargetPoint;

    public UnityEngine.AudioClip[] selectQuotes;
    public UnityEngine.AudioClip[] moveQuotes;
    public UnityEngine.AudioClip[] attackQuotes;
    public UnityEngine.AudioClip[] dyingQuotes;
    public UnityEngine.AudioClip[] createQuotes;

    private GameManager gameManager;

    private Transform _destination;
    private UnityEngine.AI.NavMeshAgent agent;
    public double hyp;
    public Quaternion angle;
    private Vector3 _direction;
    public Vector3 targetpos;

    // Use this for initialization
    void Start()
    {
        gameManager = transform.root.GetComponentInChildren<GameManager>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        targetpos = agent.transform.position;
    }
    //OnMouseDown is called when the user clicks on the unit, thus toggling selection and changing the cursor...
    void OnMouseDown()
    {
        gameManager.selectedUnit = this.gameObject;
    }
    void Update()
    {
        if (!this.canMove) return;

        hyp = System.Math.Sqrt((agent.pathEndPosition.x - transform.position.x) * (agent.pathEndPosition.x - transform.position.x)
           + (agent.pathEndPosition.y - transform.position.y) * (agent.pathEndPosition.y - transform.position.y)
           + (agent.pathEndPosition.z - transform.position.z) * (agent.pathEndPosition.z - transform.position.z));
        if (Input.GetMouseButtonDown(0) && this.moveTargetPoint != Vector3.zero)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                targetpos.Set(hit.point.x, transform.position.y, hit.point.z);

                agent.SetDestination(targetpos);
            }
        }
        _direction = (agent.steeringTarget - transform.position).normalized;

        if (_direction != Vector3.zero)
        {
            angle = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, 0);
        }
        /* 
        // moving transistion
        if (this.isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTargetPoint, movementSpeed * Time.deltaTime);
            
            Quaternion _lookRotation;
         Vector3 _direction;
             //find the vector pointing from our position to the target
         _direction = (moveTargetPoint - transform.position);
         //_direction.z = _direction.z * -1;
 
         //create the rotation we need to be in to look at the target
         _lookRotation = Quaternion.LookRotation(_direction);
 
         //rotate us over time according to speed until we are in the required rotation
         transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, Time.deltaTime * this.movementSpeed);

            if (transform.position == moveTargetPoint)
            {
                // halt tank
                this.movementSpeed = 0;
                this.isMoving = false;
            }
        }*/
    }
}