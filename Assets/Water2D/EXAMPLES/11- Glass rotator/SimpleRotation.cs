using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public  Water2D.Water2D_Spawner water2d;


    Rigidbody2D rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    float angle = 0f;
    float wise = 1f;
    int deadFrames = 10;
    int deadFramesCount = 0;
    bool canRotate = false;
    void FixedUpdate()
    {
        if (!canRotate)
            return;

        if (rb)
        {

            if (deadFramesCount <= deadFrames)
            {
                deadFramesCount++;
                return;
            }


            rb.MoveRotation(angle);
            angle+=.8f * wise;


            if (angle >= 140f)
            {
                wise *= -1;
                deadFramesCount *= 0;
            }

            if (angle <= 0f)
            {
                wise *= -1;
                canRotate = false;
                //water2d.Spawn();
            }

        }
    }

    public void StartRotation()
    {
        canRotate = true;
        print("end spawn");
    }
}
