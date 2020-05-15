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
        if (collision.gameObject.GetComponent<EnemyBase>() != null) {
            collision.gameObject.GetComponent<EnemyBase>().OnDie_();
        }
        if (collision.gameObject.GetComponent<PlayerManager>() != null) {
            if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
            {
                collision.gameObject.GetComponent<PlayerManager>().OnPlayerDie();
            }
        }
        if (collision.gameObject.GetComponent<HostageManager>() != null)
        {
            if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
            {
                collision.gameObject.GetComponent<HostageManager>().OnDie_();
            }
        }
    }
}
