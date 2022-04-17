using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlackaScript : MonoBehaviour
{
    GameManager gameManager;
    Ray ray;
    RaycastHit hit;
    private bool raketaHit = false;
    Color32[] hitColor = new Color32[2];

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hitColor[0] = gameObject.GetComponent<MeshRenderer>().material.color;
        hitColor[1] = gameObject.GetComponent<MeshRenderer>().material.color;
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0) && hit.collider.gameObject.name == gameObject.name)
            {
                if (raketaHit == false)
                {
                    gameManager.PlackaClicked(hit.collider.gameObject);
                }
            }
        }
    }

    public void SetPlackaColor (int index, Color32 color)
    {
        hitColor[index] = color;
    }

    public void ChangeColor(int colorIndex)
    {
        GetComponent<Renderer>().material.color = hitColor[colorIndex];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Raketa"))
        {
            raketaHit = true;
        }
        else if (collision.gameObject.CompareTag("OpponentRaketa"))
        {
            hitColor[0] = new Color32(0, 0, 0, 255);
            GetComponent<Renderer>().material.color = hitColor[0];
        }
    }
}
