using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Sprite[] spriteGem;
    public MapLevelManager.SPAWNTYPE _spawnType;
    public GameObject effect;
    public SpriteRenderer sp;
    public Rigidbody2D rid;
    public void DisableSprite()
    {

        if (spriteGem.Length > 0)
        {
            randomDisplayEffect = Random.Range(0, spriteGem.Length);
            sp.sprite = spriteGem[randomDisplayEffect];
        }

    }
    private void OnValidate()
    {
        if (sp == null)
        {
            sp = GetComponent<SpriteRenderer>();
        }

        if (rid == null)
            rid = GetComponent<Rigidbody2D>();
    }
    int randomDisplayEffect;

}
