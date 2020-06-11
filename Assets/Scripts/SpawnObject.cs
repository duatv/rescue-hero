using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public List<Unit> gGems;
    public MapLevelManager.SPAWNTYPE _spawnType;
    [Range(10, 50)]
    public int totalGems;
    public Unit gInstantiate;
    public bool loadObjectChild;
    int randomDisplayEffect;

    private void OnValidate()
    {
        if (!loadObjectChild)
        {
            loadObjectChild = true;
            if (gInstantiate == null)
                return;
            for (int i = gGems.Count; i < totalGems; i++)
            {
                
                Unit u = Instantiate(gInstantiate);
                u.transform.parent = gameObject.transform;
                u.transform.position = new Vector2(transform.position.x + Random.Range(-0.2f, 0.2f), transform.position.y + Random.Range(-0.2f, 0.2f));
                if (u._spawnType == MapLevelManager.SPAWNTYPE.WATER)
                {
                    randomDisplayEffect = Random.Range(0, 100);

                    if (randomDisplayEffect <= 30)
                    {
                     //   Debug.LogError(randomDisplayEffect);
                        if (u.effect != null)
                            u.effect.gameObject.SetActive(true);
                    }
                }
                else if (u._spawnType == MapLevelManager.SPAWNTYPE.LAVA)
                {
                    randomDisplayEffect = Random.Range(0, 100);

                    if (randomDisplayEffect <= 20)
                    {
                        //   Debug.LogError(randomDisplayEffect);
                        if (u.effect2 != null)
                            u.effect2.gameObject.SetActive(true);
                    }


                }
                else if (u._spawnType == MapLevelManager.SPAWNTYPE.GEMS)
                {
                    u.DisableSprite();
                    randomDisplayEffect = Random.Range(0, 100);

                    if (randomDisplayEffect <= 30)
                    {
                        //   Debug.LogError(randomDisplayEffect);
                        if (u.effect != null)
                            u.effect.gameObject.SetActive(true);
                    }
                }
                else if(u._spawnType == MapLevelManager.SPAWNTYPE.GAS)
                {

                    // u.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
                   // randomDisplayEffect = Random.Range(0, 100);

                    if (/*randomDisplayEffect <= 50*/i % 2 == 0)
                    {
                        u.isGravity = false;
                    }
                    u.BeginCreateGas();
                }
                if (!gGems.Contains(u))
                {
                    gGems.Add(u);
                }
            }

        }
    }

    private void Start()
    {
        if (_spawnType == MapLevelManager.SPAWNTYPE.GEMS)
        {
            GameManager.Instance.totalGems = /*totalGems*/gGems.Count - (int)(gGems.Count * 0.3f);
        }
        if (_spawnType == MapLevelManager.SPAWNTYPE.LAVA)
        {
            if (SoundManager.Instance != null)
                StartCoroutine(PlaySoundLavaApear());
        }
    }

    IEnumerator PlaySoundLavaApear()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.acLavaApear);
    }

    private void Update()
    {
        if (_spawnType != MapLevelManager.SPAWNTYPE.GAS)
            return;
        var deltaTime = Time.deltaTime;
        for (int i = 0; i < gGems.Count; i++)
            gGems[i].OnUpdate(deltaTime);
    }

}
