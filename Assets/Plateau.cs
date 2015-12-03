using UnityEngine;
using System.Collections;

public class Plateau : MonoBehaviour {

    public Ligne[] lignes;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameObject GetCase(int x, int y)
    {
        return lignes[y].cases[x];
    }
}
