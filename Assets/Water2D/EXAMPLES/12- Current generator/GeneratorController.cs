using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    public SpriteRenderer lamp, glow;
    public Light point;
    public Text _text;
    public Text _text2;
    public Water2D.Water2D_Spawner water2d;


    float battery;
    float currentLosesCoef = .975f;

    // Start is called before the first frame update
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    Color _c;
    void FixedUpdate()
    {
        if(rb)
        {
            battery += (rb.angularVelocity/360f);
           float rpm = rb.angularVelocity / 6.28f;
            battery *= currentLosesCoef;
            if (battery < .1f)
                battery *= 0;

            _c = lamp.color;
            _c.a =  Mathf.Lerp(0f, 1f, battery *.01f);
            lamp.color = _c;


            
           point.intensity= Mathf.Lerp(0f, 40f, battery * .003f);

            PrintResults(rpm, Mathf.Lerp(0f, 110f, _c.a));
           
        }
    }
    float _speed;
    private void Update()
    {
        if (water2d != null)
        {
            _speed = water2d.Speed;
            if (Input.GetMouseButton(0))
            {
                _speed += .1f;
            }
            if (Input.GetMouseButton(1)) //
            {
                _speed -= .1f;
            }
            if (_speed < 1f)
                _speed = 1f;

            if (_speed > 60f)
                _speed = 60f;

            water2d.Speed = _speed;
        }
    }

    int relaxFrames = 30;
    int _frames;

    float avgVolts;
    float avgRpm;

    void PrintResults(float rpm, float volts)
    {


        if (_frames > relaxFrames)
        {
            _frames *= 0;

            _text.text = "Volts \n" +(avgVolts/relaxFrames).ToString("00.00");
            _text2.text = "RPM \n " +(avgRpm/relaxFrames).ToString("0");

            avgVolts *= 0;
            avgRpm *= 0;
        }
        else {
            _frames++;

            avgVolts += volts;
            avgRpm += rpm;

        }

    }

}
