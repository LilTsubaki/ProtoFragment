﻿using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject plateau;
    public GameObject runesboard;
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
            plateau.GetComponent<Plateau>().p = player1;
            p.GetComponent<Renderer>().material.color = Color.blue;
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

        plateau.GetComponent<Plateau>().resetAll();
        if (p1.isMyTurn)
        {
            playerTurn = 1;
            plateau.GetComponent<Plateau>().p = player1;
            p1.fieldOfView();
            p1.GetComponent<Renderer>().material.color = Color.blue;
            p2.GetComponent<Renderer>().material.color = Color.white;
        }  
        else
        {
            playerTurn = 2;
            plateau.GetComponent<Plateau>().p = player2;
            p2.fieldOfView();
            p1.GetComponent<Renderer>().material.color = Color.white;
            p2.GetComponent<Renderer>().material.color = Color.blue;
        }
        runesboard.GetComponent<RunesBoard>().resetBoard();
        
    }
}
