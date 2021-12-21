using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OpponentScript : MonoBehaviour
{
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
}
