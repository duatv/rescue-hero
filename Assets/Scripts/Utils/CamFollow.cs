﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject objectToFollow;
    public Camera _myCam;
    public float speed = 2.0f;
    public bool beginFollow;
    void Update()
    {
        if (beginFollow) {

            if (Vector3.Distance(transform.position, objectToFollow.transform.position) == 10)
            {
                beginFollow = false;
            }
            else {
                float interpolation = speed * Time.deltaTime;
                _myCam.orthographicSize = 2.17f;
                Vector3 position = this.transform.position;
                position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y, interpolation);
                position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x, interpolation);
                this.transform.position = position;
            }
        }
    }
}
