﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunesBoard : MonoBehaviour {

    public Dictionary<int, RuneSlot> dict;
	public Dictionary<RuneSlot, List<RuneSlot>> parents;
	
	void Awake () {
        dict = new Dictionary<int, RuneSlot>();
		parents = new Dictionary<RuneSlot, List<RuneSlot>> ();

		foreach (RuneSlot rs in gameObject.transform.GetComponentsInChildren<RuneSlot> ()) {
			string[] strs = rs.name.Split(',');
			int id = int.Parse(strs[0])*4 + int.Parse(strs[1]);
			dict.Add(id, rs);
			parents.Add(rs, null);
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
		if (neighboors.Count == 0) {
			return null;
		}
		return neighboors;
	}

	public List<RuneSlot> GetParentsWithout(RuneSlot slot, RuneSlot rs) {
		if (parents [slot] == null) {
			return null;
		}
		List<RuneSlot> tmpParents = new List<RuneSlot> ();
		foreach (RuneSlot parentRs in parents[slot]) {
			if(!parentRs.Equals(rs)) {
				tmpParents.Add(parentRs);
			}
		}

		return tmpParents;
	}


	public int CountRunes() {
		int i = 0;
		foreach(RuneSlot rs in dict.Values) {
			if(rs.runeBase != null)
				i++;
		}

		return i;
	}

	private void RemoveRuneSlotParent(RuneSlot rs) {
		foreach (RuneSlot r in parents.Keys) {
			if(parents[r] != null && parents[r].Contains(rs)){
				parents[r].Remove(rs);
			}
		}
	}

	public bool CanPlaceRune (GameObject after, GameObject before)
	{
		if (after.GetComponent<RuneSlot> () == null) {
			Debug.LogError("Rune not placed on a proper RuneSlot");
			return false;
		}
		Debug.Log ("--------------------------------");
		string name = after.name;
		string[] strsAfter = name.Split(',');
		string[] strsBefore = before.name.Split (',');
		RuneSlot wo = before.GetComponent<RuneSlot> ();
		int id;
		int orphans = -1;

		// Slot not on board
		if (strsAfter.Length == 1) {
			// None of them are on the board
			if(strsBefore.Length == 1) {
				Debug.Log("Switch between inventory slots");
				return true;
			} // Before is on the board
			else {
				Debug.Log("Switch from board");
				foreach(RuneSlot tmpRs in parents.Keys) {
					List<RuneSlot> l = GetParentsWithout(tmpRs, wo);
					if(l != null) Debug.Log (l.Count);
					// Rune has at least a parent
					if(l != null && l.Count == 0) {
						orphans++;
					}
				}
				if(orphans > 0) {
					return false;
				}
				RemoveRuneSlotParent(wo);
				return true;
			}
		}

		id = int.Parse(strsAfter[0])*4 + int.Parse(strsAfter[1]);

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
		Debug.Log ("Parents count " + parents.Count);
		foreach(RuneSlot tmpRs in parents.Keys) {
			List<RuneSlot> l = GetParentsWithout(tmpRs, wo);
			if(l != null) Debug.Log (l.Count);
			// Rune has at least a parent
			if(l != null && l.Count == 0) {
				orphans++;
			}
		}
		Debug.Log ("Orphans " + orphans);
		if(orphans > 0) {
			return false;
		}

		bool neighboor = GetNeighboors (id).Count > 0;

		if (neighboor) {
			RemoveRuneSlotParent(wo);
			parents[dict[id]] = GetNeighboors(id);
		}

		return neighboor;
	}
}
