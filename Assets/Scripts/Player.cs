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
    public GameObject plateau;

    // Use this for initialization
    void Awake ()
    {
        hexagon.IsBusy= true;
        hexagon.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
        hexagon.previous = hexagon.transform.GetChild(0).GetComponent<Renderer>().material.color;
        hexagon.transform.GetChild(1).gameObject.SetActive(hexagon.IsBusy);
        hexagon.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
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
                int layermask = LayerMask.GetMask("Terrain");
                if (Physics.Raycast(ray, out rch, Mathf.Infinity, layermask))
                {
                    Hexagon hexa = rch.collider.GetComponent<Hexagon>();
                    if (hexa != null && hexa != hexagon && !hexa.IsBusy && hexa.spellable)
                    {
                        List<Hexagon> spell = plateau.GetComponent<Plateau>().currentSpell;
                        if(spell !=null)
                        {
                            foreach(Hexagon h in spell)
                            {
                                if((h == hexagon.plateau.GetComponent<TurnManager>().player1.GetComponent<Player>().hexagon && hexagon.plateau.GetComponent<TurnManager>().player1 != this) ||
                                   ( h == hexagon.plateau.GetComponent<TurnManager>().player2.GetComponent<Player>().hexagon && hexagon.plateau.GetComponent<TurnManager>().player2 != this))
                                Debug.Log("pewpewpew sur un ennemi");
                            }
                        }
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
                        hexa.plateau.resetAll();
                        if (path != null && path.Count > 0)
                            isMoving = true;
                        Move();
                    }
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
                    if (hexa != null && hexa != hexagon && hexa != hexagon.plateau.GetComponent<TurnManager>().player2.GetComponent<Player>().hexagon
                        && hexa != hexagon.plateau.GetComponent<TurnManager>().player1.GetComponent<Player>().hexagon)
                    {
                        hexa.IsBusy = !hexa.IsBusy;
                        hexa.transform.GetChild(1).gameObject.SetActive(hexa.IsBusy);
                        hexa.transform.GetChild(1).GetComponent<Renderer>().enabled = true;
                        hexa.plateau.resetAll();
                        fieldOfView();
                    }
                }
            }

            if (isMoving)
            {
                Move();
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
                    //hexagon.plateau.GetComponent<TurnManager>().changeTurn();
                    fieldOfView();
                }
            }
        }
    }

    public void fieldOfView()
    {
        Color darkGrey = new Color(0.3f, 0.3f, 0.3f, 1);
        Plateau pl = hexagon.plateau;
        pl.makeSpell();
        for(int i = 0; i < pl.lignes.Length; i++)
        {
            for(int j = 0; j < pl.lignes[i].cases.Length; j++)
            {
                Hexagon dest = pl.lignes[i].cases[j];
                if (!dest.isBusy && hexagon != dest && !pl.fieldOfView(hexagon, dest))
                {
                    dest.transform.GetChild(0).GetComponent<Renderer>().material.color = darkGrey;
                }
                //break;
            }
            //break;
        }
    }
}
