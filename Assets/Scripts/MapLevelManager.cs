using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLevelManager : MonoBehaviour
{
    public enum SPAWNTYPE { WATER,LAVA,STONE, GEMS}
    public static MapLevelManager Instance;
    public Dictionary<string, GameObject> dAllStone = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
