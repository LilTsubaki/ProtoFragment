using UnityEngine;
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

    public bool CanRemove(int rs)
    {
        if (rs == 0 && GetNeighboors(0) == null)
            return true;
        Rune r= dict[rs].runeBase;
        dict[rs].runeBase = null;
        List<int> ids = new List<int>();
        ids.Add(rs + 1);
        ids.Add(rs + 4);
        ids.Add(rs + 5);
        ids.Add(rs - 1);
        ids.Add(rs - 4);
        ids.Add(rs - 5);

        List<int> list = new List<int>();
        foreach (int i in ids)
        {
            if (dict.ContainsKey(i) && dict[i].runeBase != null)
            {
                list.Clear();
                if (!PathToZero(i, ref list))
                {
                    dict[rs].runeBase = r;
                    return false;
                }
                    
            }
        }

        dict[rs].runeBase = r;
        return true;
    }

    public bool PathToZero(int rs,ref List<int> list)
    {
        if (rs == 0&& dict[0].runeBase != null)
            return true;
        if(list.Contains(rs))
            return false;

        list.Add(rs);


        List<int> ids = new List<int>();
        ids.Add(rs + 1);
        ids.Add(rs + 4);
        ids.Add(rs + 5);
        ids.Add(rs - 1);
        ids.Add(rs - 4);
        ids.Add(rs - 5);

        foreach (int i in ids)
        {
            if (dict.ContainsKey(i) && dict[i].runeBase != null)
            {
                if (PathToZero(i, ref list))
                    return true;
            }
        }

        return false;
    }

    public bool CanPlaceRune(int target)
    {
        List<int> list = new List<int>();
        return dict[target].runeBase==null&& (target == 0||PathToZero(target, ref list));
    }
    public bool CanPlaceRune(int source, int target)
    {
        if(dict[target].runeBase!=null)
            return false;


        Rune r = dict[source].runeBase;
        dict[source].runeBase = null;
        bool cp=CanPlaceRune(target);
        dict[source].runeBase = r;

        dict[target].runeBase = r;
        bool cr = CanRemove(source);
        dict[target].runeBase = null;
        return cp&&cr;
    }

    public bool CanPlaceRune(int source, RuneSlot target)
    {
        return CanRemove(source) && target.runeBase==null;
    }

    public bool CanPlaceRune (GameObject after, GameObject before)
	{
		if (after.GetComponent<RuneSlot> () == null) {
			Debug.LogError("Rune not placed on a proper RuneSlot");
			return false;
		}
		string name = after.name;
		string[] strsAfter = name.Split(',');
		string[] strsBefore = before.name.Split (',');
		RuneSlot wo = before.GetComponent<RuneSlot> ();
		int id;
        int idBefore = int.MinValue;
		int orphans = 0;


        if (strsBefore.Length == 1)
        {
            if (strsAfter.Length == 1)
            {
                return false;
            }
                // after.GetComponent<RuneSlot>().runeBase == null;

            int i = int.Parse(strsAfter[0]) * 4 + int.Parse(strsAfter[1]);
            return CanPlaceRune(i);
        }
        else
        {
            int ib = int.Parse(strsBefore[0]) * 4 + int.Parse(strsBefore[1]);
            if (strsAfter.Length == 1)
                return CanPlaceRune(ib, after.GetComponent<RuneSlot>());

            int ia = int.Parse(strsAfter[0]) * 4 + int.Parse(strsAfter[1]);
            return CanPlaceRune(ib, ia);
        }
        /*
		// Slot not on board
		if (strsAfter.Length == 1) {
			// None of them are on the board
			if(strsBefore.Length == 1) {
				return true;
			} // Before is on the board
			else {
				foreach(RuneSlot tmpRs in parents.Keys) {
					List<RuneSlot> l = GetParentsWithout(tmpRs, wo);
					// Rune has at least a parent
					if(l != null && l.Count == 0) {
						orphans++;
					}
				}
				if(orphans > 0) {
					return false;
				}
                RemoveRuneSlotParent(wo);
                parents[wo] = null;
				return true;
			}
		}

		id = int.Parse(strsAfter[0])*4 + int.Parse(strsAfter[1]);
        if(strsBefore.Length != 1)
        {
            idBefore = int.Parse(strsBefore[0]) * 4 + int.Parse(strsBefore[1]);
        }

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
        
        if(idBefore == 0 && CountRunes() > 1)
        {
            return false;
        }

		// If more than one rune on the board, each slot must have at least one neighboor
		
		foreach(RuneSlot tmpRs in parents.Keys) {
			List<RuneSlot> l = GetParentsWithout(tmpRs, wo);
			// Rune has at least a parent
			if(l != null && l.Count == 0) {
				orphans++;
			}
		}
		if(orphans > 0) {
			return false;
		}

        List<RuneSlot> lneighboor = GetNeighboors(id);
        if (lneighboor != null) {

            RemoveRuneSlotParent(wo);
            if (strsBefore.Length != 1)
                parents[wo] = null;
            parents[dict[id]] = GetNeighboors(id);
            return true;
        }

        return false;*/
	}
}
