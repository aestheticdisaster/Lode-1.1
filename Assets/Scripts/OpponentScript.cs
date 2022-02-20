using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OpponentScript : MonoBehaviour
{
    char[] guess;
    List<int> potentialHits;
    List<int> currentHits;
    private int guess1;
    public GameObject opponentRaketa;

    private void Start()
    {
        potentialHits = new List<int>();
        currentHits = new List<int>();
        guess = Enumerable.Repeat('o', 100).ToArray();
    }

    public List<int[]> PlaceOpponentShips()
    {
        List<int[]> opponentShips = new List<int[]>
        {
            new int[]{-1,-1,-1,-1,-1},
            new int[]{-1,-1,-1,-1},
            new int[]{-1,-1,-1},
            new int[]{-1,-1,-1},
            new int[]{-1,-1},
        };
        int[] plackaNumbers = Enumerable.Range(1, 100).ToArray();
        bool taken = true;

        foreach (int[] plackaNumArray in opponentShips)
        {
            taken = true;

            while (taken == true)
            {
                taken = false;
                int shipNose = UnityEngine.Random.Range(0, 99);
                int rotate = UnityEngine.Random.Range(0, 2);
                int minusAmount = rotate == 0 ? 10 : 1;

                for (int i = 0; i < plackaNumArray.Length; i++)
                {
                    if ((shipNose - (minusAmount * i)) < 0 || plackaNumbers[shipNose - i * minusAmount] < 0)
                    {
                        taken = true;
                        break;
                    }
                    else if (minusAmount == 1 && shipNose /10 != ((shipNose - i * minusAmount) -1) / 10)
                    {
                        taken = true;
                        break;
                    }
                }
                if (taken == false)
                {
                    for (int j = 0; j < plackaNumArray.Length; j++)
                    {
                        plackaNumArray[j] = plackaNumbers[shipNose - j * minusAmount];
                        plackaNumbers[shipNose - j * minusAmount] = -1;
                    }
                }
            }
        }
        return opponentShips;
    }

    public void OpponentTurn()
    {
        List<int> hitIndex = new List<int>();
        for (int i = 0; i < guess.Length; i++)
        {
            if (guess[i] == 'h')
            {
                hitIndex.Add(i);
            }
        }
        if (hitIndex.Count > 1)
        {
            int diff = hitIndex[1] - hitIndex[0];
            int posNeg = Random.Range(0, 2) * 2 - 1;
            int nextIndex = hitIndex[0] + diff;

            while (guess[nextIndex] != 'o')
            {
                if (guess[nextIndex] == 'm' || nextIndex < 0 || nextIndex > 100)
                {
                    diff *= -1;
                }
                nextIndex += diff;
            }
            guess1 = nextIndex;
        }
        else if (hitIndex.Count == 1)
        {
            List<int> closeTiles = new List<int>();
            closeTiles.Add(1);
            closeTiles.Add(-1);
            closeTiles.Add(10);
            closeTiles.Add(-10);
            int index = Random.Range(0, closeTiles.Count);
            int possibleGuess = hitIndex[0] + closeTiles[index];
            bool onGrid = possibleGuess > -1 && possibleGuess < 100;
            while ((!onGrid || guess[possibleGuess] != 'o') && closeTiles.Count > 0)
            {
                closeTiles.RemoveAt(index);
                index = Random.Range(0, closeTiles.Count);
                possibleGuess = hitIndex[0] + closeTiles[index];
                onGrid = possibleGuess > -1 && possibleGuess < 100;
            }
            guess1 = possibleGuess;
        }
        else
        {
            int nextIndex = Random.Range(0, 100);
            while (guess[nextIndex] != 'o')
            {
                nextIndex = Random.Range(0, 100);
            }
            guess1 = nextIndex;
        }
        GameObject placka = GameObject.Find("placka (" + (guess1 + 1) + ")");
        guess[guess1] = 'm';
        Vector3 vector = placka.transform.position;
        vector.y += 100;
        GameObject raketa = Instantiate(opponentRaketa, vector, opponentRaketa.transform.rotation);
        raketa.GetComponent<OpponentRaketaScript>().SetTarget(guess1);
        raketa.GetComponent<OpponentRaketaScript>().targetPlackaLocation = placka.transform.position;
    }

    public void RaketaHit(int hit)
    {
        guess[guess1] = 'h'; 
    }

    public void SunkPlayer()
    {
        for (int i = 0; i < guess.Length; i++)
        {
            if (guess[i] == 'h')
            {
                guess[i] = 'x';
            }
        }
    }
}
