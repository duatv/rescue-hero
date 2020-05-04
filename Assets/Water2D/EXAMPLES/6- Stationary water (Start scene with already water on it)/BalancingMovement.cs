using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancingMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    float angle = 0f;
    float wise = 1f;
    int deadFrames = 10;
    int deadFramesCount;
    void FixedUpdate()
    {
        if (rb)
        {

            if (deadFramesCount <= deadFrames)
            {
                deadFramesCount++;
                return;
            }


            rb.MoveRotation(angle);
            angle+=.8f * wise;


            if (angle >= 30f || angle <= -30f)
            {
                wise *= -1;
                deadFramesCount *= 0;
            }

        }
    }
}
