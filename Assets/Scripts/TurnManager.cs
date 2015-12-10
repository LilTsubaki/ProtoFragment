using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    private int playerTurn = 1;
    
	// Use this for initialization
	void Start () {
	
	}
	
    void Awake()
    {
        Player p = player1.GetComponent<Player>();
        if (p != null)
        {
            p.fieldOfView();
            p.isMyTurn = true;
        }
            
       
    }

	// Update is called once per frame
	void Update ()
    {
        
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, Screen.width, Screen.height), "Tour du joueur n° : " + playerTurn);
    }

    public void changeTurn()
    {
        Player p1 = player1.GetComponent<Player>();
        if (p1 != null)
            p1.isMyTurn = !p1.isMyTurn;

        Player p2 = player2.GetComponent<Player>();
        if (p2 != null)
            p2.isMyTurn = !p2.isMyTurn;

        if (p1.isMyTurn)
        {
            playerTurn = 1;
            p1.fieldOfView();
        }  
        else
        {
            playerTurn = 2;
            p2.fieldOfView();
        }
            
    }
}
