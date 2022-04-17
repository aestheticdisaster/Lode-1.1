using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] ships;
    
    public Button nextButton;
    public Button rotateButton;
    public Button replayButton;
    public Text topText;
    public Text playerShipText;
    public Text opponentShipText;

    private bool shipSetupComplete = false;
    private bool playerTurn = true;
    private int shipIndex = 0;
    private LodScript lodScript;
    public OpponentScript opponentScript;
    private List<int[]> opponentShips;
    private List<GameObject> playerFires = new List<GameObject>();
    private List<GameObject> opponentFires = new List<GameObject>();
    public List<PlackaScript> allPlackaScript;

    public GameObject raketa;
    public GameObject opponentRaketa;
    public GameObject dock;
    public GameObject fire;
    public GameObject raketaFire;
    public GameObject opponentRaketaFire;

    private int opponentShipCount = 5;
    private int playerShipCount = 5;
    void Start()
    {
        lodScript = ships[shipIndex].GetComponent<LodScript>();
        nextButton.onClick.AddListener(() => NextShipClicked());
        rotateButton.onClick.AddListener(() => RotateShipClicked());
        replayButton.onClick.AddListener(() => ReplayClicked());
        opponentShips = opponentScript.PlaceOpponentShips();
    }

    public void PlackaClicked(GameObject placka)
    {
        if (shipSetupComplete && playerTurn)
        {
            Vector3 plackaPosition = placka.transform.position;
            plackaPosition.y += 400;
            playerTurn = false;
            Instantiate(raketa, plackaPosition, raketa.transform.rotation);
            Instantiate(raketaFire, plackaPosition, raketaFire.transform.rotation);
        }
        else if (!shipSetupComplete)
        {
            PlaceShip(placka);
            lodScript.SetClickedPlacka(placka);
        }
    }

    private void NextShipClicked()
    {
        if (!lodScript.OnBoard())
        {

        }
        else
        {
            if (shipIndex <= ships.Length - 2)
            {
                shipIndex++;
                lodScript = ships[shipIndex].GetComponent<LodScript>();
                Debug.Log(shipIndex);
   
            }
            else
            {
                rotateButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);
                dock.gameObject.SetActive(false);
                topText.text = "Vyber předpokládané pole protivníka";
                shipSetupComplete = true;
                for (int i = 0; i < ships.Length; i++)
                {
                    ships[i].SetActive(false);
                }
            }
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

    public void CheckHit(GameObject placka)
    {
        int plackaNum = Int32.Parse(Regex.Match(placka.name, @"\d+").Value);
        int hitCount = 0;

        foreach (int[] plackaNumArray in opponentShips)
        {
            if (plackaNumArray.Contains(plackaNum))
            {
                for (int i = 0; i < plackaNumArray.Length; i++)
                {
                    if (plackaNumArray[i] == plackaNum)
                    {
                        plackaNumArray[i] = -5;
                        hitCount++;
                    }
                    else if (plackaNumArray[i] == -5)
                    {
                        hitCount++;
                    }
                }
                if (hitCount == plackaNumArray.Length)
                {
                    opponentShipCount--;
                    topText.text = "Loď potopena!";
                    opponentFires.Add(Instantiate(fire, placka.transform.position, Quaternion.identity));
                    placka.GetComponent<PlackaScript>().SetPlackaColor(1, new Color32(255, 255, 255, 255));
                    placka.GetComponent<PlackaScript>().ChangeColor(1);
                }
                else
                {
                    topText.text = "Zásah!";
                    placka.GetComponent<PlackaScript>().SetPlackaColor(1, new Color32(0, 255, 0, 255));
                    placka.GetComponent<PlackaScript>().ChangeColor(1); 
                }
                break;
            }
        }
        if (hitCount == 0)
        {
            placka.GetComponent<PlackaScript>().SetPlackaColor(1, new Color32(0, 0, 0, 255));
            placka.GetComponent<PlackaScript>().ChangeColor(1);
            topText.text = "Raketa minula, žádný zásah";
        }
        Invoke("EndPlayerTurn", 1.0f);
    }

    public void OpponentHit(Vector3 placka, int plackaNum, GameObject hitObject) 
    {
        opponentScript.RaketaHit(plackaNum);
        placka.y += 5.0f;
        playerFires.Add(Instantiate(fire, placka, Quaternion.identity));
        if (hitObject.GetComponent<LodScript>().HitCheckSank())
        {
            playerShipCount--;
            playerShipText.text = playerShipCount.ToString();
            opponentScript.SunkPlayer();
        }
        Invoke("EndOpponentTurn", 2.0f);
    }

    private void EndPlayerTurn()
    {
        for (int i = 0; i < ships.Length; i++)
        {
            ships[i].SetActive(true);
        }
        foreach (GameObject fire in playerFires)
        {
            fire.SetActive(true);
        }
        foreach (GameObject fire in opponentFires)
        {
            fire.SetActive(false);
        }
        opponentShipText.text = opponentShipCount.ToString();
        topText.text = "Na tahu je protivník";
        opponentScript.OpponentTurn();
        ColorAllPlacka(0);
        if (playerShipCount < 1)
        {
            GameOver("Prohrál jsi!");
        }
    }

    public void EndOpponentTurn()
    {
        for (int i = 0; i < ships.Length; i++)
        {
            ships[i].SetActive(false);
        }
        foreach (GameObject fire in playerFires)
        {
            fire.SetActive(false);
        }
        foreach (GameObject fire in opponentFires)
        {
            fire.SetActive(true);
        }
        playerShipText.text = playerShipCount.ToString();
        topText.text = "Vyber předpokládané pole protivníka";
        playerTurn = true;
        ColorAllPlacka(1);
        if (opponentShipCount < 1)
        {
            GameOver("Vyhrál jsi!");
        }
    }

    private void ColorAllPlacka(int colorIndex)
    {
        foreach (PlackaScript plackaScript in allPlackaScript)
        {
            plackaScript.ChangeColor(colorIndex);
        }
    }

    void GameOver(string winner)
    {
        topText.text = "Konec hry. " + winner;
        replayButton.gameObject.SetActive(true);
    }

    void ReplayClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
