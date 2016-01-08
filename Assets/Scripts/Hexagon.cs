using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Hexagon : MonoBehaviour , IAStar<Hexagon>
{
    public enum Direction
    {
        EAST = 0, WEST = 3, NORTHEAST = 5, NORTHWEST = 4, SOUTHEAST = 1, SOUTHWEST = 2, UNDEFINED = 7
    }

    public Direction getLeft(Direction dir)
    {
        return getById((((int)dir - 1)+6) % 6);
    }

    public Direction getRight(Direction dir)
    {
        return getById(((int)dir + 1) % 6);
    }

    public Direction getBack(Direction dir)
    {
        return getById(((int)dir + 3) % 6);
    }

    private Direction getById(int id)
    {
        return (Direction)id;
    }

    public Hexagon getHexa(Direction dir)
    {
        switch (dir)
        {
            case Direction.EAST:
                return getEast();
            case Direction.WEST:
                return getWest();
            case Direction.NORTHEAST:
                return getNorthEast();
            case Direction.NORTHWEST:
                return getNorthWest();
            case Direction.SOUTHEAST:
                return getSouthEast();
            case Direction.SOUTHWEST:
                return getSouthWest();
            case Direction.UNDEFINED:
                return null;
        }
        return null;
    }

    public Direction getDirection(Hexagon source, Hexagon destination)
    {
        int diffX = destination.x - source.x;
        int diffY = destination.y - source.y;

        if (diffX > 0)
        {
            if (diffY > 0)
            {
                return Direction.NORTHEAST;
            }
            return Direction.EAST;
        }

        if (diffX < 0)
        {
            if (diffY < 0)
            {
                return Direction.SOUTHWEST;
            }
            return Direction.WEST;
        }

        if(diffX == 0)
        {
            if (diffY > 0)
                return Direction.NORTHWEST;
            if(diffY < 0)
                return Direction.SOUTHEAST;
        }
        return Direction.UNDEFINED;
    }


public int x, y;
    public Plateau plateau;
    public bool isBusy;
    public bool IsBusy { get { return isBusy; }
        set
        {
            isBusy = value;
            if (isBusy)
            {
                gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
                previous = gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color;
            }  
            else
            {
                gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
                previous = gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color;
            }
               
        }
    }
	// Use this for initialization

    private Color lightGrey = new Color(0.6f, 0.6f, 0.6f, 1);

    public Color previous = Color.white;
    private int cptMouseOver = 0;
    public bool spellable = false;

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
        previous = gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color;
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).GetComponent<Renderer>().enabled = false;
    }

    public void onPlayerExit(Player p)
    {
        IsBusy = false;
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void OnMouseOver()
    {
        if(cptMouseOver == 0)
        {
            cptMouseOver++;
            previous = gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color;
        }
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = lightGrey;
        if(spellable)
        {
            plateau.drawSpell(this);
        }
    }

    public void OnMouseExit()
    {
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = previous;
        cptMouseOver = 0;
        if (spellable)
        {
            plateau.resetSpell(this);
        }
    }
}
