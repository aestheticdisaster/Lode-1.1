using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] ships;
    
    public Button nextButton;
    public Button rotateButton;

    private bool shipSetupComplete = false;
    private bool playerTurn = true;
    private int shipIndex = 0;
    private LodScript lodScript;
    public OpponentScript opponentScript;

    void Start()
    {
        lodScript = ships[shipIndex].GetComponent<LodScript>();
        nextButton.onClick.AddListener(() => NextShipClicked());
        rotateButton.onClick.AddListener(() => RotateShipClicked());
    }


    void Update()
    {
        
    }

    public void PlackaClicked(GameObject placka)
    {
        if (shipSetupComplete && playerTurn)
        {
            // shoď dělovou kouli bum prásk 
        }
        else if (!shipSetupComplete)
        {
            PlaceShip(placka);
            lodScript.SetClickedPlacka(placka);
        }
    }

    private void NextShipClicked()
    {
        if (shipIndex <= ships.Length - 2)
        {
            shipIndex++;
            lodScript = ships[shipIndex].GetComponent<LodScript>();
        }
        else
        {
            opponentScript.PlaceOpponentShips();
        }
    }

    private void PlaceShip(GameObject placka)
    {
        lodScript = ships[shipIndex].GetComponent<LodScript>();
        lodScript.ClearPlackaList();
        Vector3 vector = lodScript.GetOffsetVector(placka.transform.position);
        ships[shipIndex].transform.localPosition = vector;
    }

    void RotateShipClicked()
    {
        lodScript.RotateShip();
    }
}
