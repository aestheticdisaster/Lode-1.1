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
    int hitCount = 0;
    public int shipSize;
    private Material[] allMaterials;
    List<Color> allColors = new List<Color>();

    private void Start()
    {
        /*allMaterials = GetComponent<Renderer>().materials;
        for (int i = 0; i < allMaterials.Length; i++)
        {
            allColors.Add(allMaterials[i].color);
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Placka"))
        {
            touchPlacka.Add(collision.gameObject);
        }
    }

    public void ClearPlackaList()
    {
        touchPlacka.Clear();
    }

    public Vector3 GetOffsetVector(Vector3 plackaPosition)
    {
        return new Vector3(plackaPosition.x + xOffset, 80, plackaPosition.z + zOffset);
    }

    public void RotateShip()
    {
        if (clickedPlacka == null)
        {
            return;
        }
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
        ClearPlackaList();
        transform.localPosition = new Vector3(newVector.x + xOffset, 80, newVector.z + zOffset);
    }

    public void SetClickedPlacka(GameObject placka)
    {
        clickedPlacka = placka;
    }

    public bool OnBoard()
    {
        return touchPlacka.Count == shipSize;
    }

    public bool HitCheckSank()
    {
        hitCount++;
        return shipSize <= hitCount;
    }

}
