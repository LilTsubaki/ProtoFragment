using UnityEngine;
using System.Collections;

public class RuneSlot
{

    public Vector3 pos;
    public Rune runeBase;

    public RuneSlot(Vector3 p, GameObject r)
    {
        pos = p;
        runeBase = r.GetComponent<Rune>();
        runeBase.slot = this;
        runeBase.transform.position = pos;
        runeBase.gameObject.SetActive(true);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
