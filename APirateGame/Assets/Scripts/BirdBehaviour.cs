using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour {

    private Rigidbody2D rb;
    public float Xspeed;
    public float Amplitude;
    public float Frequency;
    public float resetX;
    private Vector2 StartingPosition;
    private AudioSource audioSource;

    private float audioPlayThreshold;
    private bool audioPlayed;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Xspeed = -0.7f;
        rb.velocity = new Vector2(Xspeed, 0.0f);
        Amplitude = 0.1f;
        Frequency = 1f;
        StartingPosition = rb.position;
        resetX = 9f;

        audioSource = GetComponent<AudioSource>();
        audioPlayThreshold = 6.5f;
        audioPlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newposition = new Vector2(rb.position.x + Xspeed * Time.deltaTime, StartingPosition.y + Amplitude * Mathf.Sin(Time.fixedTime * Frequency));

        if (newposition.x<audioPlayThreshold && !audioPlayed)
        {
            audioSource.Play();
            audioPlayed = true;
        }

        if (newposition.x <= -resetX)
        {
            newposition.x = StartingPosition.x;
            audioPlayed = false;
        }

        rb.MovePosition(newposition);


    }
}
