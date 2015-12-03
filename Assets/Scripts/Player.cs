using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public Hexagon hexagon;
    public bool isMoving=false;
    List<Hexagon> path;
    private int currentStep;


    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetMouseButtonDown(1) && !isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawLine(ray.origin, ray.direction * 20);
            RaycastHit rch;
            //int layermask = (1 << LayerMask.NameToLayer("Default"));
            int layermask = LayerMask.GetMask("Terrain");
            if (Physics.Raycast(ray, out rch, Mathf.Infinity, layermask))
            {
                Hexagon hexa = rch.collider.GetComponent<Hexagon>();
                if (hexa != null && hexa != hexagon)
                {
                    path = hexagon.plateau.Path(hexagon, hexa);
                    currentStep = 0;
                    if (path != null && path.Count > 0)
                        isMoving = true;
                    Move();
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawLine(ray.origin, ray.direction * 20);
            RaycastHit rch;
            //int layermask = (1 << LayerMask.NameToLayer("Default"));
            int layermask = LayerMask.GetMask("Terrain");
            if (Physics.Raycast(ray, out rch, Mathf.Infinity, layermask))
            {
                Hexagon hexa = rch.collider.GetComponent<Hexagon>();
                if (hexa != null && hexa != hexagon)
                {
                    hexa.IsBusy = !hexa.IsBusy;
                }
            }
        }

        if (isMoving)
        {
            Move();
        }
    }

    bool goTo(Hexagon hexa)
    {
        //Debug.Log("go to : " + hexa.x + "  " + hexa.y);
        Vector3 temp = new Vector3(0.0f, 0.5f, 0.0f);
        transform.position = Vector3.MoveTowards(transform.position, hexa.transform.position + temp, 0.05f);
        return Mathf.Approximately(Vector3.SqrMagnitude(hexa.transform.position + temp  - transform.position), 0);
    }

    void Move()
    {
        if(path != null && path.Count > 0)
        {
            if (currentStep <= path.Count && goTo(path[path.Count -1 - currentStep]))
            {
                path[path.Count - 1 - currentStep].onPlayerEnter(this);
                currentStep++;
                if(currentStep == path.Count)
                {
                    isMoving = false;
                }
            }
        }
    }
}
