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

    private void OnValidate()
    {
        if (_spawnType == MapLevelManager.SPAWNTYPE.GEMS)
        {
            if (sp == null)
            {
                sp = GetComponent<SpriteRenderer>();

                if (spriteGem.Length > 0)
                {
                    randomDisplayEffect = Random.Range(0, spriteGem.Length);
                    sp.sprite = spriteGem[randomDisplayEffect];
                }
            }
        }
        if (rid == null)
            rid = GetComponent<Rigidbody2D>();
    }
    private void OnDisable()
    {
        if (effect != null)
        {
            effect.SetActive(false);
        }
    }
    int randomDisplayEffect;
    Vector2 originPos;
    public virtual void DisplayEffect(bool setPos, Vector2 _originPos)
    {
        if (_spawnType == MapLevelManager.SPAWNTYPE.WATER)
        {
            randomDisplayEffect = Random.Range(0, 3);
            //    Debug.LogError("=========random nuoc:" + randomDisplayEffect);
            if (randomDisplayEffect == 0)
            {
                if (effect != null)
                    effect.SetActive(true);
            }
        }

        else if (_spawnType == MapLevelManager.SPAWNTYPE.STONE)
        {
            randomDisplayEffect = Random.Range(0, 100);
            if (randomDisplayEffect <= 2)
            {
                if (effect != null)
                    effect.SetActive(true);
            }
        }
        if (setPos)
        {

            originPos.x = _originPos.x + Random.Range(-0.01f, 0.01f);
            originPos.y = _originPos.y + Random.Range(-0.01f, 0.01f);
            rid.velocity = originPos;
            //transform.position = originPos;

            //Debug.LogError("=======set pos de=====");
        }
    }
}
