using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollison : MonoBehaviour
{
    public MapLevelManager.SPAWNTYPE _spawnType;
  //  public Sprite sprLavaStone;
    public string id = "";
    public bool isLava;
    public bool isStone = false;
    private Sprite sprCur;

    //private void Awake()
    //{
    //    sprCur = GetComponent<SpriteRenderer>().sprite;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Lava_Pr"))
        {
            GameManager.Instance.PlaySoundLavaOnWater();
            collision.gameObject.SetActive(false);
            isStone = true;
          //  /* GetComponent<SpriteRenderer>().sprite*/sprCur = sprLavaStone;
         //   MapLevelManager.Instance.dAllStone[id].transform.position = transform.position;
            gameObject.SetActive(false);
            //  MapLevelManager.Instance.dAllStone[id].SetActive(true);

            Unit stone = ObjectPoolManagerHaveScript.Instance.stonePooler.GetUnitPooledObject();
            stone.transform.position = gameObject.transform.position;

            stone.DisplayEffect(false,Vector2.zero);

            stone.gameObject.SetActive(true);

            //Unit effectwaterfire = ObjectPoolManagerHaveScript.Instance.effectWaterFirePooler.GetUnitPooledObject();
            //effectwaterfire.transform.position = gameObject.transform.position;
            //effectwaterfire.gameObject.SetActive(true);
        }
    }
}
