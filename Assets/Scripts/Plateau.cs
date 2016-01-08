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

    private int nbRunes = 0;
    public GameObject p;
    private Color lightGreen = new Color(0.564706f, 0.933333f, 0.564706f, 1);

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
                    lignes[i].cases[j].spellable = false;
                }
            }
        }
    }

    public void makeSpell()
    {
        makeSpell(nbRunes);
    }

    public void makeSpell(int nbRune)
    {
        Color lightGrey= new Color(0.8f,0.8f,0.8f,1);
        nbRunes = nbRune;
        if(nbRune > 0)
        {
            List<Hexagon> neighbours = p.GetComponent<Player>().hexagon.GetNeighbours();
            foreach (Hexagon hexa in neighbours)
            {
                hexa.transform.GetChild(0).GetComponent<Renderer>().material.color = lightGrey;
                hexa.previous = lightGrey;
                hexa.spellable = true;
            }
        }   
    }

    public void drawSpell(Hexagon firstHexa)
    {
        spell(firstHexa, nbRunes);
    }

    public void resetSpell(Hexagon firstHexa)
    {
        resetHexaSpell(firstHexa, nbRunes);
    }

    public List<Hexagon> getSpellHexa(Hexagon firstHexa, int spellNumber)
    {
        Hexagon.Direction dir = firstHexa.getDirection(p.GetComponent<Player>().hexagon, firstHexa);

        List<Hexagon> spell = new List<Hexagon>();
        switch (spellNumber)
        {
            case 1 :

                Hexagon devant = firstHexa.getHexa(dir);
                Hexagon droite = firstHexa.getHexa(firstHexa.getRight(dir));
                Hexagon gauche = firstHexa.getHexa(firstHexa.getLeft(dir));


                if (devant != null && !devant.IsBusy)
                {
                    spell.Add(devant);
                    Hexagon devant2 = firstHexa.getHexa(dir).getHexa(dir);
                    if (devant2 != null && !devant2.IsBusy)
                    {
                        spell.Add(devant2);
                    }
                }

                if (droite != null && !droite.IsBusy)
                    spell.Add(droite);
                if (gauche != null && !gauche.IsBusy)
                    spell.Add(gauche);

                break;

            case 2:
                Hexagon droite2 = firstHexa.getHexa(firstHexa.getRight(dir));
                Hexagon gauche2 = firstHexa.getHexa(firstHexa.getLeft(dir));

                if (droite2 != null && !droite2.IsBusy)
                    spell.Add(droite2);
                if (gauche2 != null && !gauche2.IsBusy)
                    spell.Add(gauche2);
                break;

            case 3:

                Hexagon front = firstHexa.getHexa(dir);
                if (front != null && !front.IsBusy)
                {
                    spell.Add(front);
                    Hexagon front2 = front.getHexa(dir);
                    if (front2 != null && !front2.IsBusy)
                    {
                        spell.Add(front2);


                        Hexagon droite3 = front2.getHexa(front2.getRight(dir));
                        Hexagon gauche3 = front2.getHexa(front2.getLeft(dir));

                        if (droite3 != null && !droite3.IsBusy)
                            spell.Add(droite3);
                        if (gauche3 != null && !gauche3.IsBusy)
                            spell.Add(gauche3);


                        Hexagon front3 = front2.getHexa(dir);
                        if (front3 != null && !front3.IsBusy)
                        {
                            spell.Add(front3);
                        }
                    }
                }
                break;

            case 4:

                Hexagon front1 = firstHexa.getHexa(dir);
                if (front1 != null && !front1.IsBusy)
                {
                    spell.Add(front1);
                    Hexagon front2 = front1.getHexa(dir);
                    if (front2 != null && !front2.IsBusy)
                    {
                        spell.Add(front2);

                        Hexagon front3 = front2.getHexa(dir);
                        if (front3 != null && !front3.IsBusy)
                        {
                            spell.Add(front3);
                            Hexagon front4 = front3.getHexa(dir);
                            if (front4 != null && !front4.IsBusy)
                            {
                                spell.Add(front4);
                            }
                        }
                    }
                }
                break;
        }
        return spell;
    }


    public void spell(Hexagon firstHexa, int spellNumber)
    {
        List<Hexagon> spell = getSpellHexa(firstHexa, spellNumber);
        spell.Add(firstHexa);

        foreach (Hexagon hexa in spell)
        {
            hexa.transform.GetChild(0).GetComponent<Renderer>().material.color = lightGreen;
        }
    }

    public void resetHexaSpell(Hexagon firstHexa, int spellNumber)
    {
        List<Hexagon> spell = getSpellHexa(firstHexa, spellNumber);

        foreach (Hexagon hexa in spell)
        {
            hexa.transform.GetChild(0).GetComponent<Renderer>().material.color = hexa.previous;
        }
    }

}
