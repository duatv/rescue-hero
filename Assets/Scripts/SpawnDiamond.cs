using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDiamond : MonoBehaviour
{
    public GameObject[] gGems;
    public int totalGems;
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
            gInstantiate.SetActive(true);
        }
    }
}
