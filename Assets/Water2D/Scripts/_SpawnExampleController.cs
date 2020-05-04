namespace Water2D {

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _SpawnExampleController : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
		Water2D_Spawner.instance.Loop = true;
		Water2D_Spawner.instance.Spawn ();
		//Water2D_Spawner.instance.LifeTime = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}
