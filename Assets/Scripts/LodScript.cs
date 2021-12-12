using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LodScript : MonoBehaviour
{
    List<GameObject> touchPlacka = new List<GameObject>();
    public float xOffset = 0;
    public float zOffset = 0;
    private float nextYRotation = 90f;
    private GameObject clickedPlacka;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ClearPlackaList()
    {
        touchPlacka.Clear();
    }

    public Vector3 GetOffsetVector(Vector3 plackaPosition)
    {
        return new Vector3(plackaPosition.x + xOffset, 100, plackaPosition.z + zOffset);
    }

    public void RotateShip()
    {
        touchPlacka.Clear();
        transform.localEulerAngles += new Vector3(0, nextYRotation, 0);
        nextYRotation *= -1;
        float temp = xOffset;
        xOffset = zOffset;
        zOffset = temp;
        SetPosition(clickedPlacka.transform.position);
    }

    public void SetPosition(Vector3 newVector)
    {
        transform.localPosition = new Vector3(newVector.x + xOffset, 100, newVector.z + zOffset);
    }

    public void SetClickedPlacka(GameObject placka)
    {
        clickedPlacka = placka;
    }
}
