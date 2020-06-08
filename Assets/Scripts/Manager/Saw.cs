using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float RotationSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 10) * RotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "BodyPlayer")
        {
            if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
            {
                PlayerManager.Instance.OnPlayerDie(true);
            }
        }
    }
}
