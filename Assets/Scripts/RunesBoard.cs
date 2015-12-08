using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunesBoard : MonoBehaviour {

    public Dictionary<int, Hexagon> dict;
    public GameObject hexagon;

	// Use this for initialization
	void Start () {
        dict = new Dictionary<int, Hexagon>();

        Hexagon center = new Hexagon();
        center.x = 0;
        center.y = 0;
	    for(int i = -2; i <= 2; ++i)
        {
            for(int j = -2; j <= 2; ++j)
            {
                Hexagon input = new Hexagon();
                input.x = i;
                input.y = j;

                if(center.Distance(input) <= 2)
                {
                    dict.Add(i*4+j, input);
                    GameObject hex = Instantiate(hexagon);
                    hex.transform.name = i + "," + j;
                    Vector3 pos = new Vector3(0.75f*i, 0, j*0.866f-0.433f* i);
                    hex.transform.position = pos;
                    hex.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
