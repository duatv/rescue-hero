using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public List<Unit> gGems;
    public MapLevelManager.SPAWNTYPE _spawnType;
    public int totalGems;
    public Vector2 initSpeed = new Vector2(1f, -1.8f);
    //private List<Unit> lstWaterDrops = new List<Unit>();
    //private List<Unit> lstLavaDrops = new List<Unit>();
    private Unit gInstantiate;
    // int _ranIndex = 0;
    public bool loadObjectChild;
    private void OnValidate()
    {
        if (!loadObjectChild)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Unit u = transform.GetChild(i).gameObject.GetComponent<Unit>();
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
            GameManager.Instance.totalGems = totalGems;
        }
        if (_spawnType == MapLevelManager.SPAWNTYPE.LAVA)
        {
            if (SoundManager.Instance != null)
                StartCoroutine(PlaySoundLavaApear());
        }
        //SpawnAllGems();
        BeginSet();
    }
    void BeginSet()
    {
        for (int i = 0; i < gGems.Count; i++)
        {
            gGems[i].DisplayEffect(true, transform.position);
        }
    }
    IEnumerator PlaySoundLavaApear()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.acLavaApear);
    }
    //private void SpawnAllGems()
    //{
    //    for (int i = 0; i < totalGems; i++)
    //    {
    //        if (_spawnType == MapLevelManager.SPAWNTYPE.GEMS)
    //        {
    //            gInstantiate = ObjectPoolManagerHaveScript.Instance.gemPooler.GetUnitPooledObject();
    //            gInstantiate.DisplayEffect();
    //            gInstantiate.transform.position = new Vector2(transform.position.x + Random.Range(-0.01f, 0.01f), transform.position.y);

    //            gInstantiate.gameObject.SetActive(true);
    //            if (!GameManager.Instance.lstAllGems.Contains(gInstantiate))
    //                GameManager.Instance.lstAllGems.Add(gInstantiate);
    //        }

    //        if (_spawnType == MapLevelManager.SPAWNTYPE.WATER)
    //        {
    //            gInstantiate = ObjectPoolManagerHaveScript.Instance.waterPooler.GetUnitPooledObject();
    //            gInstantiate.DisplayEffect();
    //            gInstantiate.transform.position = new Vector2(transform.position.x + Random.Range(-0.01f, 0.01f), transform.position.y);
    //            gInstantiate.gameObject.SetActive(true);
    //        }
    //        if (_spawnType == MapLevelManager.SPAWNTYPE.LAVA)
    //        {

    //            gInstantiate = ObjectPoolManagerHaveScript.Instance.firePooler.GetUnitPooledObject();
    //            gInstantiate.transform.position = new Vector2(transform.position.x + Random.Range(-0.01f, 0.01f), transform.position.y);
    //            gInstantiate.gameObject.SetActive(true);
    //        }

    //    }
    //}

}
