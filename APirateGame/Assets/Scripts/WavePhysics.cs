using UnityEngine;
using System.Collections;

public class WavePhysics : MonoBehaviour
{
    private Rigidbody2D rb;
    public float Xspeed;
    public float Amplitude;
    public float Frequency;
    public float resetX;
    private Vector2 StartingPosition;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Xspeed = 0.7f;
        rb.velocity = new Vector2 (Xspeed,0.0f);
        Amplitude = 0.7f;
        Frequency =1f;
        StartingPosition = rb.position;
        resetX = 4.5f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newposition = new Vector2(rb.position.x + Xspeed * Time.deltaTime, StartingPosition.y + Amplitude * Mathf.Sin(Time.fixedTime * Frequency));

        if (newposition.x >= resetX)
        {
            newposition.x = StartingPosition.x;
        }

        rb.MovePosition(newposition);


    }
}
