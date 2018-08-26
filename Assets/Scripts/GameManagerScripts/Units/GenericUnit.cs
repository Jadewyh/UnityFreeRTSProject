using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericUnitAudio
{
    public UnityEngine.AudioClip[] selectQuotes;
    public UnityEngine.AudioClip[] moveQuotes;
    public UnityEngine.AudioClip[] attackQuotes;
    public UnityEngine.AudioClip[] dyingQuotes;
    public UnityEngine.AudioClip[] createQuotes;
}

[System.Serializable]
public class GenericUnitSettings
{
    public float combatDamage;
    public float maxHealth;
    public float combatRangeMax;
    public float combatRangeMin = 0f; // example are trebuchet in AoE which cannot combat unless there is a distance between attacker and receiver
    public float maxMovementSpeed = 1f;
    public bool canMove;
    public bool canCombat;

}
public class GenericUnit : MonoBehaviour
{
    public GenericUnitAudio AudioSettings;
    public GenericUnitSettings UnitSettings;

    public float currentHealth;
    public float movementSpeed = 1f;

    // For example: Buildings or drone bots
    public GenericConstructionObject[] availableConstructionObjects;
    public Vector3 constructionObjectSpawnPoint;

    public bool isMoving;
    public bool isSelected;

    public Vector3 moveTargetPoint;

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
        if (!gameManager)
        {
            gameManager = GameManager.myInstance;
            if (!gameManager)
                Debug.Log("Please add this object below gameManager!");
        }
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (!agent)
        {
            Debug.Log("Please add a NavMeshAgent to this object!");
        } else {
            targetpos = agent.transform.position;
        }
    }
    //OnMouseDown is called when the user clicks on the unit, thus toggling selection and changing the cursor...
    void OnMouseDown()
    {
        gameManager.guiManagerInstance.selectedUnit = this.gameObject;

    }
    void Update()
    {
        if (!this.UnitSettings.canMove || !this.isMoving) return;

        hyp = System.Math.Sqrt((agent.pathEndPosition.x - transform.position.x) * (agent.pathEndPosition.x - transform.position.x)
           + (agent.pathEndPosition.y - transform.position.y) * (agent.pathEndPosition.y - transform.position.y)
           + (agent.pathEndPosition.z - transform.position.z) * (agent.pathEndPosition.z - transform.position.z));

        if (Input.GetMouseButtonDown(0) && this.moveTargetPoint != Vector3.zero && isSelected)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                targetpos.Set(hit.point.x, transform.position.y, hit.point.z);

                agent.SetDestination(targetpos);
            }
        }
        agent.SetDestination(targetpos);
        _direction = (agent.steeringTarget - transform.position).normalized;

        angle = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, angle, 0);
        
    }
}