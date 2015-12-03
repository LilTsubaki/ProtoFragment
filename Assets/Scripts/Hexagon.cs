using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Hexagon : MonoBehaviour , IAStar<Hexagon>
{
    public int x, y;
    public Plateau plateau;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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


        if (NE != null)
            neighbours.Add(NE);

        if (NW != null)
            neighbours.Add(NW);

        if (E != null)
            neighbours.Add(E);

        if (W != null)
            neighbours.Add(W);

        if (SE != null)
            neighbours.Add(SE);

        if (SW != null)
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
}
