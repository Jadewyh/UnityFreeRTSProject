using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthsystem : MonoBehaviour
{
    public float health; //public static to be modified later in another script(if unit takes damage)
    public GameObject healthbaroutlineobj;
    public GameObject healthbarobj;
    private Image healthbaroutline;
    private Image healthbar;
    private Vector2 unitscreenpos;
    private RectTransform barrect;
    private RectTransform outlinerect;
    private float barwidth;

	// Use this for initialization
	void Start ()
    {
        health = 100;
        healthbaroutline = healthbaroutlineobj.GetComponent<Image>();
        healthbar = healthbarobj.GetComponent<Image>();
        barrect = healthbarobj.GetComponent<RectTransform>();
        outlinerect = healthbaroutlineobj.GetComponent<RectTransform>();
        healthbaroutline.enabled = false;
        healthbar.enabled = false;
        outlinerect.sizeDelta = new Vector2(30, 3);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (LegacyMovementScript.selected == true)
        {
            unitscreenpos = Camera.main.WorldToScreenPoint(transform.position);
            unitscreenpos.Set(unitscreenpos.x, unitscreenpos.y + 30);
            outlinerect.position = unitscreenpos;
            barwidth = health * 28 / 100;
            barrect.sizeDelta = new Vector2(barwidth, 1);
            barrect.position = new Vector2(outlinerect.position.x - 14 + barwidth/2, outlinerect.position.y);
            healthbaroutline.enabled = true;
            healthbar.enabled = true;

            if (health > 0 && health <= 20)
            {
                healthbar.color = Color.red;
            }
            if (health > 20 && health <= 40)
            {
                healthbar.color = Color.yellow;
            }
            if (health > 40)
            {
                healthbar.color = Color.green;
            }
        }
        if (LegacyMovementScript.selected == false)
        {
            healthbaroutline.enabled = false;
            healthbar.enabled = false;
        }
        if (health == 0f)
        {
            Destroy(gameObject);
        }
	}
}
