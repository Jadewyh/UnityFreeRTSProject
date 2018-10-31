using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUnit : MonoBehaviour
{
    [Header("Unit/Object settings")]
    private float _currentHealth;

    public float currentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
            if (HPBarFilling)
                HPBarFilling.fillAmount = _currentHealth / maxHealth;
        }
    }
    public float maxHealth;
    public float combatDamage;
    public float combatRangeMax;
    public float combatRangeMin = 0f; // example are trebuchet in AoE which cannot combat unless there is a distance between attacker and receiver
    public float movementSpeed = 1f;
    public float maxMovementSpeed = 1f;
    public bool canMove;
    public bool canCombat;
    // For example: Buildings or drone bots
    public MiddleButtonObjects[] actionObjects;
    public GameObject HPBar;


    [Header("Runtime Objects (do not change)")]
    public UnityEngine.UI.Image HPBarFilling;
    public PlayerHandler owner;
    [ReadOnly] public Vector3 constructionObjectSpawnPoint;
    [ReadOnly] public bool isMoving;
    [ReadOnly] public bool isSelected;
    [ReadOnly] public Vector3 moveTargetPoint;
    [ReadOnly] public GameManager gameManager;
    [ReadOnly] public Transform _destination;
    [ReadOnly] public UnityEngine.AI.NavMeshAgent agent;
    [ReadOnly] public double hyp;
    [ReadOnly] public Quaternion angle;
    [ReadOnly] public Vector3 _direction;
    [ReadOnly] public Vector3 targetpos;

    [Header("Audio")]
    public UnityEngine.AudioClip[] selectQuotes;
    public UnityEngine.AudioClip[] moveQuotes;
    public UnityEngine.AudioClip[] attackQuotes;
    public UnityEngine.AudioClip[] dyingQuotes;
    public UnityEngine.AudioClip[] createQuotes;



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
        }
        else
        {
            targetpos = agent.transform.position;
        }
        currentHealth = maxHealth;

        if (HPBar)
            foreach (UnityEngine.UI.Image i in HPBar.GetComponentsInChildren<UnityEngine.UI.Image>())
            {
                if (i.name == "HPBarFilling")
                    HPBarFilling = i;
            }

    }
    //OnMouseDown is called when the user clicks on the unit, thus toggling selection and changing the cursor...
    void OnMouseDown()
    {
        if (this.owner == gameManager.UIPlayer)
            gameManager.guiManagerInstance.selectedUnit = this.gameObject;

    }
    void Update()
    {
        if (!this.canMove || !this.isMoving) return;

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

        if (_direction != Vector3.zero)
            angle = Quaternion.LookRotation(_direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, angle, 0);

    }
}