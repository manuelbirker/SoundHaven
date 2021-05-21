using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFootstepSound : MonoBehaviour
{
    private AudioSource audioSource;
    public bool IsMoving;

    public float speed;
    public float _speed;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        _speed = speed;
    }

    void Update()
    {
        
        
        if (InventoryController.inventoryIsOpen)
        {
            return;
        }
        
        
        
        if (_speed > 0)
        {
            _speed -= Time.deltaTime;
        }

        if (_speed <= 0)
        {
           
            if (IsMoving)
            {
                audioSource.Play();
            } _speed = speed;
        }


        if (Input.GetAxisRaw("Vertical") != 0) IsMoving = true; // better use != 0 here for both directions
        else IsMoving = false;


        if (!IsMoving) audioSource.Stop();
    }
}