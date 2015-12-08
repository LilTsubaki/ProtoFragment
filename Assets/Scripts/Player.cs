using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public Hexagon hexagon;
    public bool isMoving=false;
    List<Hexagon> path;
    private int currentStep;
    public bool isMyTurn=false;

    // Use this for initialization
    void Awake ()
    {
        hexagon.IsBusy= true;
        hexagon.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(isMyTurn)
        {
            if (Input.GetMouseButtonDown(0) && !isMoving)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rch;
                int layermask = LayerMask.GetMask("Players");
                if (Physics.Raycast(ray, out rch, Mathf.Infinity, layermask))
                {
                    Player player = rch.collider.GetComponent<Player>();
                    if (player != null && player != this)
                    {
                        Debug.Log("pew pew");
                        hexagon.plateau.GetComponent<TurnManager>().changeTurn();
                    }
                }
            }
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
                    if (hexa != null && hexa != hexagon && !hexa.IsBusy)
                    {
                        path = hexagon.plateau.Path(hexagon, hexa);
                        currentStep = 0;
                        if (path != null && path.Count > 0)
                            isMoving = true;
                        Move();
                    }
                }
            }
            if (isMoving)
            {
                Move();
            }
        }

        if (Input.GetMouseButtonDown(2) && !isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawLine(ray.origin, ray.direction * 20);
            RaycastHit rch;
            //int layermask = (1 << LayerMask.NameToLayer("Default"));
            int layermask = LayerMask.GetMask("Terrain");
            if (Physics.Raycast(ray, out rch, Mathf.Infinity, layermask))
            {
                Hexagon hexa = rch.collider.GetComponent<Hexagon>();
                if (hexa != null && hexa != hexagon && !hexa.IsBusy)
                {
                    hexa.IsBusy = !hexa.IsBusy;
                }
            }
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
                    hexagon.plateau.GetComponent<TurnManager>().changeTurn();
                }
            }
        }
    }
}
