using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plateau : MonoBehaviour {

    public Ligne[] lignes;
    AStar<Hexagon> astar = new AStar<Hexagon>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Hexagon GetCase(int x, int y)
    {
        if( y < lignes.Length && y>=0)
        {
            if(x<lignes[y].cases.Length&& x >= 0)
            {
                return lignes[y].cases[x];
            }
        }

        return null;
    }

    public List<Hexagon> Path(int x1, int y1, int x2, int y2)
    {
        return Path(GetCase(x1, y1), GetCase(x2, y2));
    }

    public List<Hexagon> Path(Hexagon h1,Hexagon h2)
    {
        astar.reset();
        return astar.CalculateBestPath(h1, h2);
    }
}
