using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
    public SpawnController spawnController;
    public InventoryController inventoryController;
    public int spawnID; // -1 = empty

    public Image thumbnail;

    public void Spawn()
    {

        if (spawnID > -1)
        {
            inventoryController.CloseInventory();
            spawnController.Spawn(spawnID);
            spawnID = -1;
            thumbnail.sprite = null;
            thumbnail.enabled = false;
            //TODO Play select sound
        }
        else
        {
            //TODO Play Error Sound, Show Error Message
        }
    }


    public void OnMouseOver()
    {
        if (spawnID > -1)
        {
            GetComponent<Image>().color = new Color32(255, 255, 255, 150);
        }
    }

    public void OnMouseExit()
    {
        GetComponent<Image>().color = new Color32(0, 0, 0, 150);
    }


    public void Reset()
    {
        thumbnail.sprite = null;
        thumbnail.enabled = false;
        if (spawnID > -1)
        {
            thumbnail.sprite = spawnController.objects[spawnID].GetComponent<Object>().objectThumbnail;
            thumbnail.enabled = true;
        }
    }
}