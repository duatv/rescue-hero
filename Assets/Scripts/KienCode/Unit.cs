using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Sprite[] spriteGem;
    public MapLevelManager.SPAWNTYPE _spawnType;
    public GameObject effect, effect2;
    public SpriteRenderer sp;
    public Rigidbody2D rid;
    public bool activeChangeStone;
    public GameObject stone, check, colliderBegin;
    public void DisableSprite()
    {
        if (spriteGem.Length > 0)
        {
            randomDisplayEffect = Random.Range(0, spriteGem.Length);
            sp.sprite = spriteGem[randomDisplayEffect];
        }
    }
    public virtual void ChangeStone()
    {
        if (!activeChangeStone)
        {
            activeChangeStone = true;
            StartCoroutine(DelayChangeStone());
        }
        //    Debug.LogError("zooooooooooooo");
    }
    IEnumerator DelayChangeStone()
    {
        yield return new WaitForSeconds(0.1f);
        colliderBegin.layer = 9;
        if (check != null)
            check.SetActive(false);
        stone.SetActive(true);
        sp.gameObject.SetActive(false);
        if (effect != null)
            effect.SetActive(false);
        if (effect2 != null)
            effect2.SetActive(false);
        int randomeffect = Random.Range(0, 100);
        //if (randomeffect < 10)
        //{
        //    GameObject g = ObjectPoolerManager.Instance.effectWaterFirePooler.GetPooledObject();
        //    g.transform.position = transform.position;
        //    g.SetActive(true);
        //}
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
