using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] gGems;
    public MapLevelManager.SPAWNTYPE _spawnType;
    public int totalGems;
    public Vector2 initSpeed = new Vector2(1f, -1.8f);
    private List<GameObject> lstWaterDrops = new List<GameObject>();
    private List<GameObject> lstLavaDrops = new List<GameObject>();
    private void OnEnable()
    {
        SpawnAllGems();
    }
    private GameObject gInstantiate;
    private void SpawnAllGems()
    {
        for (int i = 0; i < totalGems; i++)
        {
            gInstantiate = Instantiate(gGems[Random.Range(0, gGems.Length)], transform.position, Quaternion.identity);
            gInstantiate.transform.position = Vector3.zero;
            gInstantiate.transform.SetParent(transform, false);
            if (_spawnType != MapLevelManager.SPAWNTYPE.STONE)
                gInstantiate.SetActive(true);
            if (_spawnType == MapLevelManager.SPAWNTYPE.WATER)
            {
                gInstantiate.GetComponent<WaterCollison>().id = "W" + i;
                lstWaterDrops.Add(gInstantiate);
                Spawn(lstWaterDrops);
            }
            if (_spawnType == MapLevelManager.SPAWNTYPE.LAVA)
            {
                lstLavaDrops.Add(gInstantiate);
                Spawn(lstLavaDrops);
            }
            if (_spawnType == MapLevelManager.SPAWNTYPE.STONE)
            {
                MapLevelManager.Instance.dAllStone.Add("W" + i, gInstantiate);
            }
        }
    }

    int DefaultCount;
    bool _breakLoop = false;
    private void Spawn(List<GameObject> lst)
    {
        Spawn(DefaultCount, lst);
    }
    public void Spawn(int count, List<GameObject> lst)
    {
        StartCoroutine(loop(gameObject.transform.position, lst, initSpeed, count));
    }
    IEnumerator loop(Vector3 _pos, List<GameObject> lst, Vector2 _initSpeed, int count = -1, float delay = 0f, bool waitBetweenDropSpawn = true)
    {
        yield return new WaitForSeconds(delay);
        _breakLoop = false;
        int auxCount = 0;
        for (int i = 0; i < lst.Count; i++)
        {
            if (_breakLoop)
                yield break;

            if (_initSpeed == Vector2.zero)
                _initSpeed = initSpeed;
            lst[i].GetComponent<Rigidbody2D>().velocity = _initSpeed;

            lst[i].SetActive(true);
            if (count > -1)
            {
                auxCount++;
                if (auxCount >= count)
                {
                    yield break;
                }
            }
        }

        yield return new WaitForEndOfFrame();
    }
}
