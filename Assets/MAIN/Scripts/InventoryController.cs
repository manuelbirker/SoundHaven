using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class InventoryController : MonoBehaviour
{
    [SerializeField] KeyCode openInventory = KeyCode.Tab;

    public static bool inventoryIsOpen = false;
    public Canvas SidePanel;


    public float respawnTimer; // 100 - x/s
    public float _respawnTimer = 100;

    public SpawnController spawnController;
    public ItemPanel[] itemPanels;


    public Image objReloadBar;


    private void Start()
    {
        CloseInventory();
        inventoryIsOpen = false;
        _respawnTimer = 100;

        RespawnInventory();
    }

    void RespawnInventory()
    {
        //TODO Play Refresh Sound, Show Refresh Message
        foreach (ItemPanel ip in itemPanels)
        {
            ip.spawnID = -1;
            ip.spawnID = spawnController.objects[Random.Range(0, spawnController.objects.Count)].GetComponent<Object>()
                .codeID; //TODO Algorithm for object selection?

            ip.Reset();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_respawnTimer <= 0)
        {
            _respawnTimer = 100;
            RespawnInventory();
        }
        else
        {
            _respawnTimer -= respawnTimer;
        }


        objReloadBar.fillAmount = _respawnTimer / 100;

        if (Input.GetKeyDown(openInventory))
        {
            inventoryIsOpen = !inventoryIsOpen;


            if (inventoryIsOpen)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    public void OpenInventory()
    {
        SidePanel.enabled = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        inventoryIsOpen = true;
    }

    public void CloseInventory()
    {
        SidePanel.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inventoryIsOpen = false;
    }
}