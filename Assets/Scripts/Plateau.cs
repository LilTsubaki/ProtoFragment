using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plateau : MonoBehaviour {

    [Range(0,1)]
    public float percent;
    public Ligne[] lignes;
    AStar<Hexagon> astar = new AStar<Hexagon>();
    Vector3 centreTemp = new Vector3();
    float rayonTemp = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake()
    {

    }

    public Hexagon GetCase(int x, int y)
    {
        if( y < lignes.Length && y>=0)
        {
            if(x < lignes[y].cases.Length && x >= 0)
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

    public bool fieldOfView(Hexagon source, Hexagon destination)
    {
        Vector3 coll1 = new Vector3(source.transform.position.x, source.transform.position.y + 0.5f, source.transform.position.z);
        Vector3 coll2 = new Vector3(destination.transform.position.x, destination.transform.position.y + 0.5f, destination.transform.position.z);
        float rayonHexaCarre = (destination.transform.localScale.x / 2.0f) * (destination.transform.localScale.x / 2.0f)-(destination.transform.localScale.x / 4.0f)* (destination.transform.localScale.x / 4.0f);

        centreTemp = coll2;
        rayonTemp = Mathf.Sqrt(rayonHexaCarre);

        //centre de nouveau cercle
        Vector3 o1 = coll1 + (coll2 - coll1)*0.5f;
        float rayon = Vector3.Distance(o1, coll1);

        float xbMoinsXa = destination.transform.position.x - o1.x;
        float zbMoinsza = destination.transform.position.z - o1.z;

        float a = 2.0f * xbMoinsXa;
        float b = 2.0f * zbMoinsza;
        float c = xbMoinsXa * xbMoinsXa + zbMoinsza * zbMoinsza - rayonHexaCarre + rayon * rayon;
        float delta = (2.0f * a * c) * (2.0f * a * c) - 4.0f * (a * a + b * b) * (c * c - (b * b) * (rayon * rayon));

        Vector3 inter1 = new Vector3();
        Vector3 inter2 = new Vector3();

        inter1.x = o1.x + ((2.0f * a * c - Mathf.Sqrt(delta)) / (2.0f * (a * a + b * b)));
        inter2.x = o1.x + ((2.0f * a * c + Mathf.Sqrt(delta)) / (2.0f * (a * a + b * b)));
        inter1.y = 0.5f;
        inter2.y = 0.5f;

        if (b != 0 )
        {
            inter1.z = o1.z + ((c - a * (inter1.x - o1.x)) / b);
            inter2.z = o1.z + ((c - a * (inter2.x - o1.x)) / b);
        }
        else
        {
            float temp = (((2.0f*c)-(a*a))/(2.0f*a))* (((2.0f * c) - (a * a)) / (2.0f * a));
            inter1.z = o1.z + b / 2.0f + Mathf.Sqrt(rayonHexaCarre - temp);
            inter2.z = o1.z + b / 2.0f - Mathf.Sqrt(rayonHexaCarre - temp);
        }

        int nbRayon=20;
        int cptHit = 0;
        //Debug.DrawRay(coll1, (inter1-coll1) * 20, Color.red, 10);
        //Debug.DrawRay(coll1, (inter2 - coll1) * 20, Color.red, 10);
        for (float i = 0; i < 1f+1f/nbRayon; i +=1f/nbRayon)
        {
            Ray rayTest = new Ray(coll1, (inter1 * i + inter2 * (1- i)) - coll1);
            //Debug.DrawRay(rayTest.origin, rayTest.direction * 20, Color.cyan, 10);
            RaycastHit rch;
            int layermask = LayerMask.GetMask("FieldOfView");
            if (Physics.Raycast(rayTest, out rch, Vector3.Distance(coll1, coll2), layermask))
            {
                if (destination == rch.transform.parent.GetComponent<Hexagon>())
                {
                    ++cptHit;
                    continue;
                }
                else
                {
                    continue;
                }  
                
            }
            ++cptHit;
        }

        //Debug.Log(cptHit);
        return ((float)cptHit) / nbRayon > percent;
    }

    public void resetAll()
    {
        for (int i = 0; i < lignes.Length; i++)
        {
            for (int j = 0; j < lignes[i].cases.Length; j++)
            {
                if(!lignes[i].cases[j].isBusy)
                {
                    lignes[i].cases[j].transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(centreTemp, rayonTemp);
    }
}
