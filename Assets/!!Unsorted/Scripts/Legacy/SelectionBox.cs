using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionBox : MonoBehaviour {
    public RectTransform selectboxrect;
    public Vector2 startdrawpos;
    public Vector2 enddrawpos;
    public Image selectionbox;
    public Vector2 unitscreenpos;
    public GameObject Unit;
    public GameObject test1;

    [SerializeField]
	// Use this for initialization
	void Start () {
        selectionbox = GetComponent<Image>();
        selectboxrect = GetComponent<RectTransform>();
        selectionbox.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startdrawpos = Input.mousePosition;
        }

            if (Input.GetMouseButtonUp(0))
            {
            unitscreenpos = Camera.main.WorldToScreenPoint(Unit.transform.position);
            if ((unitscreenpos.x >= startdrawpos.x && unitscreenpos.y >= startdrawpos.y && unitscreenpos.x <= enddrawpos.x
                && unitscreenpos.y <= enddrawpos.y)||(unitscreenpos.x >= startdrawpos.x && unitscreenpos.y <= startdrawpos.y && unitscreenpos.x <= enddrawpos.x
                && unitscreenpos.y >= enddrawpos.y) || (unitscreenpos.x <= startdrawpos.x && unitscreenpos.y >= startdrawpos.y && unitscreenpos.x >= enddrawpos.x
                && unitscreenpos.y <= enddrawpos.y)||(unitscreenpos.x <= startdrawpos.x && unitscreenpos.y <= startdrawpos.y && unitscreenpos.x >= enddrawpos.x
                && unitscreenpos.y >= enddrawpos.y))
            {
                LegacyMovementScript.selected = true;
                LegacyMovementScript.firsttimeclick = true;
            }
            selectionbox.enabled = false;
        }
            if (Input.GetMouseButton(0))
            {
                if(selectionbox.enabled == false)
            {
                selectionbox.enabled = true;
            }
                enddrawpos = Input.mousePosition;
                Vector2 boxdrawstart = startdrawpos;
                float width = Mathf.Abs(boxdrawstart.x - enddrawpos.x);
                float height = Mathf.Abs(boxdrawstart.y - enddrawpos.y);
                Vector2 boxcenter = (boxdrawstart + enddrawpos) / 2f;
                selectboxrect.sizeDelta = new Vector2(width, height);
                selectboxrect.position = boxcenter;
            }
        Debug.Log(LegacyMovementScript.selected);
	}
}
