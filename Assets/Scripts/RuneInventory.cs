using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneInventory : MonoBehaviour {

    public int slotsNb = 4;
	public GameObject slot;
    public GameObject runeTest;

	// Use this for initialization
	void Awake () {
        for(int i = 0; i < slotsNb; ++i)
        {
			GameObject newSlot = (GameObject) Instantiate(slot, gameObject.transform.position + new Vector3(0 ,0 ,-i * (1 + 0.2f)), Quaternion.identity);
			newSlot.transform.SetParent(gameObject.transform);
			newSlot.layer = LayerMask.NameToLayer("caseRunier");
			RuneSlot rs = newSlot.AddComponent<RuneSlot>();

			GameObject newRune = Instantiate(runeTest);
			newRune.transform.SetParent(rs.transform);
			newRune.GetComponent<Rune>().ResetPosition();
			rs.runeBase = newRune.GetComponent<Rune>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
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
        if (Input.GetMouseButtonUp(0))
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
					GameObject slot = info.collider.gameObject;
					GameObject before = held.transform.GetComponentInParent<RuneSlot>().gameObject;
					RunesBoard rb = slot.GetComponentInParent<RunesBoard>();
					if(rb != null) {
						if(rb.CanPlaceRune(slot, before)) {
							held.transform.GetComponentInParent<RuneSlot>().runeBase = null;
							held.transform.SetParent(slot.transform);
							slot.GetComponent<RuneSlot>().runeBase = held.GetComponent<Rune>();
						}
					}
					else {
						RunesBoard rbbefore = before.GetComponentInParent<RunesBoard>();
						if(rbbefore != null) {
							if(rbbefore.CanPlaceRune(slot, before)) {
								held.transform.GetComponentInParent<RuneSlot>().runeBase = null;
								held.transform.SetParent(slot.transform);
								slot.GetComponent<RuneSlot>().runeBase = held.GetComponent<Rune>();
							}
						}
						else{
							held.transform.GetComponentInParent<RuneSlot>().runeBase = null;
							held.transform.SetParent(slot.transform);
							slot.GetComponent<RuneSlot>().runeBase = held.GetComponent<Rune>();
						}
					}

				}
				held.GetComponent<Rune>().ResetPosition();
            }

        }


    }
}
