using UnityEngine;
using System.Collections;

public class Rune : MonoBehaviour {

    internal bool follow;
    internal RuneSlot slot;
    internal RuneSlot initialSlot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(follow)
        {
            Plane p = new Plane(gameObject.transform.up, gameObject.transform.position);
            float dist;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (p.Raycast(r, out dist))
            {
                Vector3 pos = r.GetPoint(dist);
                gameObject.transform.position = pos;
            }
        }
	}

	public void ResetPosition() {
		Vector3 pos = gameObject.transform.localPosition;
		pos.x = 0;
		pos.z = 0;
		pos.y = 1;
		gameObject.transform.localPosition = pos;
	}
}
