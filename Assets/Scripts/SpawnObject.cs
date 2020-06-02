using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public List<Unit> gGems;
    public MapLevelManager.SPAWNTYPE _spawnType;
    [Range(10, 30)]
    public int totalGems;
    public Unit gInstantiate;
    public bool loadObjectChild;
    int randomDisplayEffect;

    private void OnValidate()
    {
        if (!loadObjectChild)
        {
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
                }
                if (!gGems.Contains(u))
                {
                    gGems.Add(u);
                }
            }
            loadObjectChild = true;
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

}
