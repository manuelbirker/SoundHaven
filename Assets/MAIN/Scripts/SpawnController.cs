using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public List<GameObject> objects;
    public Transform activePosition;


    public GameObject currentSpawn;

    public PlayerLook pl;


    public bool canSpawn;
    public bool spawnMode;

    public KeyCode placeObject = KeyCode.Mouse0;
    public KeyCode ignoreObject = KeyCode.Escape;
    public KeyCode pickupObject = KeyCode.Mouse1;

    public GameObject GetCard(int id)
    {
        return objects[id];
    }


    public void Spawn(int id)
    {
        spawnMode = true;


        foreach (GameObject o in objects)
        {
            if (o.GetComponent<Object>().codeID == id)
            {
                currentSpawn = Instantiate(o);
                currentSpawn.transform.position = new Vector3(activePosition.transform.position.x,
                    activePosition.transform.position.y ,
                    activePosition.transform.position.z);
             //   currentSpawn.transform.SetParent(activePosition);

                return;
            }
        }
    }


    public void Pickup(GameObject o)
    {
        pl.currentSelected = o;
        Spawn(pl.currentSelected.GetComponent<Object>().codeID);

        Destroy(pl.currentSelected);

        pl.currentSelected = null;
    }


    void Update()
    {
        if (Input.GetKeyDown(pickupObject))
        {
            if (!currentSpawn && !spawnMode)
            {
                Pickup(pl.currentSelected);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Spawn(0);
        }

        if (currentSpawn)
        {
            currentSpawn.transform.position = new Vector3(activePosition.transform.position.x,
                activePosition.transform.position.y ,
                activePosition.transform.position.z);


            if (Input.GetKeyDown(placeObject))
            {
                if (canSpawn)
                {
                    currentSpawn.transform.position = new Vector3(activePosition.transform.position.x,
                        activePosition.transform.position.y ,
                        activePosition.transform.position.z);
                    currentSpawn.transform.parent = null;
                    currentSpawn.GetComponent<Object>().isPlaced = true;
                    currentSpawn.GetComponent<Collider>().enabled = true;
                    currentSpawn = null;
                    canSpawn = false;
                    spawnMode = false;
                }
            }

            if (Input.GetKeyDown(ignoreObject))
            {
                currentSpawn.transform.parent = null;
                Destroy(currentSpawn);
                currentSpawn = null;
                canSpawn = false;
                spawnMode = false;
            }
        }
    }
}