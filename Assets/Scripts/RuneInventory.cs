using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneInventory : MonoBehaviour {

    public int slotsNb = 4;
    public List<RuneSlot> slots;
    public GameObject runeTest;

	// Use this for initialization
	void Awake () {
        slots = new List<RuneSlot>();
        for(int i = 0; i < slotsNb; ++i)
        {
            slots.Add(new RuneSlot(gameObject.transform.position + new Vector3(0 ,0 ,-i * (1 + 0.2f)), Instantiate(runeTest)));

        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            if(Physics.Raycast(r, out info, Mathf.Infinity, LayerMask.GetMask("runes")))
            {
                GameObject held;
                held = info.collider.gameObject;
                held.GetComponent<Rune>().follow = true;
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            if (Physics.Raycast(r, out info, Mathf.Infinity, LayerMask.GetMask("runes")))
            {
                GameObject held;
                held = info.collider.gameObject;
                held.GetComponent<Rune>().follow = false;
                if(Physics.Raycast(r, out info, Mathf.Infinity, LayerMask.GetMask("caseRunier")))
                {
                    Vector3 newPos = info.transform.position;
                    newPos.y += 2;
                    held.transform.position = newPos;
                }
                else
                {
                    held.transform.position = held.GetComponent<Rune>().slot.pos;
                }
                
            }

            if (Physics.Raycast(r, out info, Mathf.Infinity, LayerMask.GetMask("caseRunier")))
            {
                Debug.Log(info.collider.gameObject.name);
            }
        }


    }
}
