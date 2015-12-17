﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunesBoard : MonoBehaviour {

    public Dictionary<int, RuneSlot> dict;
	
	void Awake () {
        dict = new Dictionary<int, RuneSlot>();


		foreach (RuneSlot rs in gameObject.transform.GetComponentsInChildren<RuneSlot> ()) {
			string[] strs = rs.name.Split(',');
			int id = int.Parse(strs[0])*4 + int.Parse(strs[1]);
			dict.Add(id, rs);
		}
        /*Hexagon center = new Hexagon();
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
        }*/

	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			foreach(int i in dict.Keys) {
				Debug.Log(i + " : " + dict[i].runeBase);
			}
		}
	}

	public List<RuneSlot> GetNeighboors(int id) {
		List<int> ids = new List<int> ();
		ids.Add (id + 1);
		ids.Add (id + 4);
		ids.Add (id + 5);
		ids.Add (id - 1);
		ids.Add (id - 4);
		ids.Add (id - 5);
		
		List<RuneSlot> neighboors = new List<RuneSlot> ();
		foreach (int i in ids) {
			if(dict.ContainsKey(i) && dict[i].runeBase != null) {
				neighboors.Add(dict[i]);
			}
		}

		return neighboors;
	}

	public List<RuneSlot> GetNeighboorsWithout(int id, RuneSlot rwo) {
		List<int> ids = new List<int> ();
		ids.Add (id + 1);
		ids.Add (id + 4);
		ids.Add (id + 5);
		ids.Add (id - 1);
		ids.Add (id - 4);
		ids.Add (id - 5);
		
		List<RuneSlot> neighboors = new List<RuneSlot> ();
		foreach (int i in ids) {
			if(dict.ContainsKey(i) && !dict[i].Equals(rwo) && dict[i].runeBase != null) {
				neighboors.Add(dict[i]);
			}
		}
		
		return neighboors;
	}

	public int CountRunes() {
		int i = 0;
		foreach(RuneSlot rs in dict.Values) {
			if(rs.runeBase != null)
				i++;
		}

		return i;
	}

	public bool CanPlaceRune (GameObject after, GameObject before)
	{
		if (after.GetComponent<RuneSlot> () == null) {
			Debug.LogError("Rune not placed on a proper RuneSlot");
			return false;
		}

		string name = after.name;
		string[] strs = name.Split(',');
		RuneSlot wo = before.GetComponent<RuneSlot> ();

		if (strs.Length == 1) {
			if (CountRunes() > 1) {
				foreach(int i in dict.Keys){
					if(dict[i].runeBase != null && GetNeighboorsWithout(i, wo).Count < 1){
						return false;
					}
				}
			}
			return true;
		}
		int id = int.Parse(strs[0])*4 + int.Parse(strs[1]);

		// Preventing two runes in the same slot
		if (dict [id].runeBase != null) {
			return false;
		}

		bool empty = true;
		foreach(int i in dict.Keys) {
			if(dict[i].runeBase != null) {
				empty = false;
			}
		}

		if (empty) {
			if(id == 0)
				return true;
			else
				return false;
		}

		// If more than one rune on the board, each slot must have at least one neighboor

		if (CountRunes() > 1) {
			foreach(int i in dict.Keys){
				if(dict[i].runeBase != null && GetNeighboorsWithout(i, wo).Count < 1){
					return false;
				}
			}
		}

		List<int> ids = new List<int> ();
		ids.Add (id + 1);
		ids.Add (id + 4);
		ids.Add (id + 5);
		ids.Add (id - 1);
		ids.Add (id - 4);
		ids.Add (id - 5);

		bool neighboor = false;
		foreach (int i in ids) {
			if(dict.ContainsKey(i) && dict[i].runeBase != null) {
				neighboor = true;
				break;
			}
		}

		return neighboor;
	}
}
