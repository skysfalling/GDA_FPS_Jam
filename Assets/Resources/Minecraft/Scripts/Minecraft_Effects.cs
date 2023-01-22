using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecraft_Effects : MonoBehaviour
{
    GameObject player;
    FormController formController;
    Animator animator;

    public int currSlot = 0;
    public GameObject snowball_model;
    public GameObject tnt_model;
    public GameObject hand_model;

    [Space(10)]
    public AudioSource background_music;
    public List<AudioClip> music;

    [Space(10)]
    public bool isWalking;
    public AudioSource walking_audio;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        formController = player.GetComponent<FormController>();

        animator = GetComponent<Animator>();

        snowball_model.SetActive(false);
        tnt_model.SetActive(false);
        hand_model.SetActive(true);

        PlayRandomBackgroundMusic();
    }

    // Update is called once per frame
    void Update()
    {
        // Switch Slots
        if (formController._currentPrimaryIsPressed)
        {
            SetSlotModel(1);
        }
        else if(formController._currentSecondaryIsPressed)
        {
            SetSlotModel(2);
        }

        // << WALKING >>
        Vector3 moveDirection = player.GetComponent<PlayerController>().moveDirection;
        if (moveDirection.x > 0 || moveDirection.y > 0) { isWalking = true; }
        else { isWalking = false; }


        // animation
        if (isWalking)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        // play walk audio
        if ( !walking_audio.isPlaying && isWalking)
        {
            walking_audio.Play();
        }
        else if (walking_audio.isPlaying && !isWalking)
        {
            walking_audio.Pause();
        }

    }

    public void SetSlotModel(int slot)
    {
        if (slot == 0)
        {
            snowball_model.SetActive(false);
            tnt_model.SetActive(false);
            hand_model.SetActive(true);
        }
        else if (slot == 1)
        {
            snowball_model.SetActive(true);
            tnt_model.SetActive(false);
            hand_model.SetActive(false);
        }
        else if (slot == 2)
        {
            snowball_model.SetActive(false);
            tnt_model.SetActive(true);
            hand_model.SetActive(false);
        }
    }

    public void PlayRandomBackgroundMusic()
    {
        background_music.PlayOneShot(music[Random.Range(0, music.Count)]);
    }
}
