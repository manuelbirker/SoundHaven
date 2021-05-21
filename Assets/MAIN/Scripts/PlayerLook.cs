using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Cinemachine;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float sensX = 10f;

    public Camera maincam;

    [SerializeField] private float sensY = 10f;
    public float sensitivityMultiplier = 10f;


    [SerializeField] Transform cam = null;
    [SerializeField] Transform orientation = null;

    float mouseX;
    float mouseY;


    float xRotation;
    float yRotation;

    public GameObject activePosition;

    public SpawnController spawnController;

    public GameObject currentSelected;
    public GameObject lastSelected;
    public float interactionLength;
    public float placementLengthMin;
    public float placementLengthMax;


    [SerializeField] KeyCode rotateKey = KeyCode.R;

    public GameObject floorCircle;
    private GameObject _floorCircle;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void CurrentInteracting()
    {
        if (spawnController.currentSpawn)
        {
            lastSelected = currentSelected;
            currentSelected = null;
            if (_floorCircle != null)
            {
                Destroy(_floorCircle);
                _floorCircle = null;
            }

            return;
        }


        RaycastHit hit;
        if (Physics.Raycast(maincam.transform.position, maincam.transform.TransformDirection(Vector3.forward),
            out hit, interactionLength))
        {
            Debug.DrawRay(maincam.transform.position,
                maincam.transform.TransformDirection(Vector3.forward) * hit.distance,
                Color.yellow);


            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                currentSelected = hit.collider.gameObject;


                if (lastSelected == null)
                {
                    currentSelected = hit.collider.gameObject;
                    lastSelected = currentSelected;
                }
                else if (lastSelected != currentSelected)
                {
                    currentSelected = hit.collider.gameObject;
                }


                if (_floorCircle != null)
                {
                    if (currentSelected = hit.collider.gameObject)
                    {
                        // If object is changed (fix for objects that are too near to each other.
                        Destroy(_floorCircle);
                        _floorCircle = null;
                    }
                }

                if (_floorCircle == null)
                {
                    _floorCircle = Instantiate(floorCircle);
                    _floorCircle.transform.localScale = new Vector3(currentSelected.GetComponent<Object>().soundRadius,
                        currentSelected.GetComponent<Object>().soundRadius,
                        1);
                    _floorCircle.transform.position = new Vector3(currentSelected.transform.position.x,
                        currentSelected.transform.position.y+ 0.001f,
                        currentSelected.transform.position.z);
                }
            }
            else
            {
                lastSelected = currentSelected;
                currentSelected = null;
                if (_floorCircle != null)
                {
                    Destroy(_floorCircle);
                    _floorCircle = null;
                }
            }
        }
        else
        {
            lastSelected = currentSelected;
            currentSelected = null;

            if (_floorCircle != null)
            {
                Destroy(_floorCircle);
                _floorCircle = null;
            }
        }
    }


    private void Update()
    {
        if (InventoryController.inventoryIsOpen)
        {
            return;
        }


        CurrentInteracting();

        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");


        yRotation += mouseX * sensX * sensitivityMultiplier;
        xRotation -= mouseY * sensY * sensitivityMultiplier;


        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);


        if (Input.GetKey((rotateKey)))
        {
            activePosition.transform.Rotate(Vector3.up * 30 * Time.deltaTime);
        }


        RaycastHit hit;
        Ray ray = maincam.ScreenPointToRay(Input.mousePosition);
        LayerMask lm = LayerMask.GetMask("Ground");


        if (spawnController.spawnMode)
        {
            activePosition.SetActive(true);
        }
        else
        {
            activePosition.SetActive(false);
        }


        if (Physics.Raycast(ray, out hit, lm))
        {
            Debug.DrawRay(ray.origin, ray.direction * interactionLength, Color.yellow);
            float dist = Vector3.Distance(activePosition.transform.position, ray.origin);
            activePosition.transform.position = hit.point;

            if (dist > placementLengthMax)
            {
                spawnController.canSpawn = false;
                activePosition.GetComponent<Renderer>().material.color = new Color32(255, 0, 30, 50);
                activePosition.GetComponent<Renderer>().material
                    .SetColor("_EmissionColor", new Color32(255, 0, 30, 50));

                if (spawnController.currentSpawn != null)
                {
                    spawnController.currentSpawn.GetComponent<Renderer>().material
                        .SetColor("_EmissionColor", new Color32(255, 0, 30, 50));
                }
            }
            else if (dist < placementLengthMin)
            {
                spawnController.canSpawn = false;
                activePosition.GetComponent<Renderer>().material.color = new Color32(255, 0, 30, 50);
                activePosition.GetComponent<Renderer>().material
                    .SetColor("_EmissionColor", new Color32(255, 0, 30, 50));


                if (spawnController.currentSpawn != null)
                {
                    spawnController.currentSpawn.GetComponent<Renderer>().material
                        .SetColor("_EmissionColor", new Color32(255, 0, 30, 50));
                }
            }

            else
            {
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Interactable"))
                {
                    spawnController.canSpawn = true;
                    activePosition.GetComponent<Renderer>().material.color = new Color32(158, 195, 252, 50);
                    activePosition.GetComponent<Renderer>().material
                        .SetColor("_EmissionColor", new Color32(158, 195, 252, 50));

                    if (spawnController.currentSpawn != null)
                    {
                        spawnController.currentSpawn.GetComponent<Renderer>().material
                            .SetColor("_EmissionColor", new Color32(0, 0, 0, 0));
                    }
                }
                else
                {
                    spawnController.canSpawn = false;
                    activePosition.GetComponent<Renderer>().material.color = new Color32(255, 0, 30, 50);
                    activePosition.GetComponent<Renderer>().material
                        .SetColor("_EmissionColor", new Color32(255, 0, 30, 50));


                    if (spawnController.currentSpawn != null)
                    {
                        spawnController.currentSpawn.GetComponent<Renderer>().material
                            .SetColor("_EmissionColor", new Color32(0, 0, 0, 0));
                    }
                }
            }
        }
    }
}