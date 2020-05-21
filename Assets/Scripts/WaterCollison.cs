using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollison : MonoBehaviour
{
    public MapLevelManager.SPAWNTYPE _spawnType;
    public Sprite sprLavaStone;
    public string id = "";
    public bool isLava;
    public bool isStone = false;
    private Sprite sprCur;

    private void Awake()
    {
        sprCur = GetComponent<SpriteRenderer>().sprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Lava_Pr"))
        {
            collision.gameObject.SetActive(false);
            if (!isStone) {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.acLavaOnWater);
                }
            }
            isStone = true;
            GetComponent<SpriteRenderer>().sprite = sprLavaStone;
            MapLevelManager.Instance.dAllStone[id].transform.position = transform.position;
            gameObject.SetActive(false);
            MapLevelManager.Instance.dAllStone[id].SetActive(true);
        }
    }
}
