using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Hexagon hexa;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.origin, ray.direction * 20);
            RaycastHit rch;
            //int layermask = (1 << LayerMask.NameToLayer("Default"));
            int layermask =LayerMask.GetMask("Terrain");
            if (Physics.Raycast(ray, out rch, Mathf.Infinity, layermask))
            {
                Hexagon hexa = rch.collider.GetComponent<Hexagon>();
                if(hexa != null)
                {
                    Debug.Log(hexa.x);
                    Debug.Log(hexa.y);
                }
            }
        }
    }
}
