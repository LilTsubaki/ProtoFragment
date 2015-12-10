using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Hexagon : MonoBehaviour , IAStar<Hexagon>
{
    public int x, y;
    public Plateau plateau;
    public bool isBusy;
    public bool IsBusy { get { return isBusy; } set { isBusy = value;  if(isBusy) gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red; else gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white; } }
	// Use this for initialization
	void Start () {
        //IsBusy = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    /*if(isBusy)
            gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
        else
            gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;*/
    }
    
    public Hexagon getNorthEast()
    {
        return plateau.GetCase(x + 1, y + 1);
    }
    public Hexagon getNorthWest()
    {
        return plateau.GetCase(x, y + 1);
    }
    public Hexagon getEast()
    {
        return plateau.GetCase(x + 1, y);
    }
    public Hexagon getWest()
    {
        return plateau.GetCase(x - 1, y);
    }
    public Hexagon getSouthEast()
    {
        return plateau.GetCase(x, y - 1);
    }
    public Hexagon getSouthWest()
    {
        return plateau.GetCase(x - 1, y - 1);
    }

    public List<Hexagon> GetNeighbours()
    {
        List<Hexagon> neighbours = new List<Hexagon>();

        Hexagon NE = getNorthEast();
        Hexagon NW = getNorthWest();
        Hexagon E = getEast();
        Hexagon W = getWest();
        Hexagon SE = getSouthEast();
        Hexagon SW = getSouthWest();


        if (NE != null && !NE.IsBusy)
            neighbours.Add(NE);

        if (NW != null && !NW.IsBusy)
            neighbours.Add(NW);

        if (E != null && !E.IsBusy)
            neighbours.Add(E);

        if (W != null && !W.IsBusy)
            neighbours.Add(W);

        if (SE != null && !SE.IsBusy)
            neighbours.Add(SE);

        if (SW != null && !SW.IsBusy)
            neighbours.Add(SW);

        return neighbours;
    }

    public int Distance(Hexagon t)
    {
        int diffX = t.x - x;
        int diffY = t.y - y;
        if ((diffX >= 0 && diffY >= 0) || (diffX <= 0 && diffY <= 0))
            return Math.Max(Math.Abs(diffX), Math.Abs(diffY));

        return Math.Abs(diffX) + Math.Abs(diffY);
    }

    public int Cost()
    {
        return 1;
    }

    public void onPlayerEnter(Player p)
    {
        IsBusy = true;
        if (p.hexagon != null)
            p.hexagon.onPlayerExit(p);
        p.hexagon = this;
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).GetComponent<Renderer>().enabled = false;
    }

    public void onPlayerExit(Player p)
    {
        IsBusy = false;
        transform.GetChild(1).gameObject.SetActive(false);
    }

}
