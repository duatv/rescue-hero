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
    public GameObject stone, check;
    public Collider2D colliderBegin;
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
            colliderBegin.gameObject.layer = 9;
            rid.sharedMaterial = GameManager.Instance.matStone;
            colliderBegin.sharedMaterial = GameManager.Instance.matStone;
            activeChangeStone = true;

            StartCoroutine(DelayChangeStone());
        }
        //    Debug.LogError("zooooooooooooo");
    }
    IEnumerator DelayChangeStone()
    {
        yield return new WaitForSeconds(0.1f);

        if (check != null)
            check.SetActive(false);
        stone.SetActive(true);
        sp.gameObject.SetActive(false);
        if (effect != null)
            effect.SetActive(false);
        if (effect2 != null)
            effect2.SetActive(false);
        int randomeffect = Random.Range(0, 100);
        if (randomeffect < 10)
        {
            if (ObjectPoolerManager.Instance != null)
            {
                GameManager.Instance.PlaySoundLavaOnWater();
                GameObject g = ObjectPoolerManager.Instance.effectWaterFirePooler.GetPooledObject();
                g.transform.position = transform.position;
                g.SetActive(true);
            }
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


    public float speedMove;
    public bool isGravity;
    public float timeFly = 0;
    private float ranGas;
    // Start is called before the first frame update
    public void BeginCreateGas()
    {
        ranGas = Random.Range(0.5f, 1);
        if (isGravity)
            rid.gravityScale = 0;
        else
            rid.gravityScale = 0.1f;
        transform.localScale = new Vector3(ranGas, ranGas, ranGas);
        speedMove = Random.Range(1f, 1.5f);
    }
    public virtual void OnUpdate(float deltaTime)
    {

    }
}
