using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentRaketaScript : MonoBehaviour
{
    private GameManager gameManager;
    private OpponentScript opponentScript;
    public Vector3 targetPlackaLocation;
    private int targetPlacka = -1;
    
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        opponentScript = GameObject.Find("OpponentScript").GetComponent<OpponentScript>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            gameManager.OpponentHit(targetPlackaLocation, targetPlacka, collision.gameObject);
        }
        else
        {

        }
        Destroy(gameObject);
    }

    public void SetTarget(int target)
    {
        targetPlacka = target;
    }
}
