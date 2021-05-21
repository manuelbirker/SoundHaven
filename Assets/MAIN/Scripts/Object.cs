using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using UnityEngine.UI;

public class Object : MonoBehaviour
{
    public int codeID;
    public Color outlineColor;
    private Outline outline;
    private float outlineThickness = 5f;

    public bool currentInteracting;

    [HideInInspector] public PlayerLook pl;

    public AudioClip ac;

    public bool isPlaced;

    private AudioSource audioSource;

    public GameObject musicNotes;
    private GameObject _musicNotes;


    public Sprite objectThumbnail;

    public float soundRadius;

    public bool enableCare;
    public float careTimer;
    private float _careTimer;
    public int minLevel;
    public int maxLevel;
    public int currentLevel;
    public bool needWatering;


    public Image watering;
    private Image _watering;

    public Vector3 offset;

    void Start()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = outlineColor;
        pl = GameObject.Find("Player").GetComponent<PlayerLook>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = ac;
        _musicNotes = Instantiate(musicNotes, transform);

        soundRadius = audioSource.maxDistance;

        if (enableCare)
        {
            _watering = Instantiate(watering, GameObject.Find("GameUI").transform).GetComponent<Image>();
        }
    }


    void Watering()
    {
        _watering.enabled = true;
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position + offset);
        _watering.transform.position = pos;
    }

    void CareController()
    {
        if (_watering != null)
        {
            if (needWatering)
            {
                Watering();
            }
        }
    }

    private void Update()
    {
        if (enableCare)
        {
            CareController();
        }


        if (pl.currentSelected == this.gameObject)
        {
            currentInteracting = true;
        }
        else
        {
            audioSource.clip = ac;
            currentInteracting = false;
        }

        Outlines();
        if (ac & isPlaced && !audioSource.isPlaying)
        {
            _musicNotes.GetComponent<ParticleSystem>().Play();
          //  StartCoroutine(blink(1, 0.2f, 1f));
            PlayMusic();
        }
    }

    public IEnumerator blink(int count, float onDuration, float offDuration)
    {
        WaitForSeconds onWait = new WaitForSeconds(onDuration);

        WaitForSeconds offWait = new WaitForSeconds(offDuration);
        for (int i = 0; i < count; ++i)
        {
            TurnOnLight(true);
            yield return onWait;
            TurnOnLight(false);
            yield return offWait;
        }
    }

    void TurnOnLight(bool on)
    {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;
        Color baseColor = Color.white;
        float emission;

        Color finalColor;
        if (on)
        {
            emission = 1;
            finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
        }

        else
        {
            emission = 0;
            finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
        }

        mat.SetColor("_EmissionColor", finalColor);
    }

    void PlayMusic()
    {
        audioSource.Play();
    }


    void Outlines()
    {
        if (currentInteracting)
        {
            outline.OutlineWidth = outlineThickness;
        }

        else
        {
            outline.OutlineWidth = 0;
        }
    }
}