using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaChannelPicker : MonoBehaviour
{
   public Water2D.Water2D_Spawner WSpawner;

    Color color;
    int wise = 1;

    // Update is called once per frame
    void Update()
    {
        color = WSpawner.FillColor;
        color.a += 0.01f * wise;

        if (color.a >= 1f || color.a <= 0f)
        {
            wise *= -1;
        }

        WSpawner.FillColor = color;
    }

}
