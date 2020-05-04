using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTriggerBoundingBox : MonoBehaviour
{
    public ParticleSystem[] SteamPS;


    private void Start()
    {
        for (int i = 1; i < SteamPS.Length; i++)
        {
            SteamPS[i] = Instantiate(SteamPS[0]);
        }
    }

    public void DeleteOnCollide(GameObject drop, GameObject other)
    {

        Delete(drop, other);

    }

    Color c = new Color(1f, 0, 0, .5f);
    
    void Delete(GameObject drop, GameObject another)
    {

        if (another.GetInstanceID()  == gameObject.GetInstanceID()) {
            drop.GetComponent<MetaballParticleClass>().Active = false;
            for (int i = 0; i < SteamPS.Length; i++)
            {
                if (SteamPS[i].isPlaying)
                    continue;

                SteamPS[i].transform.position = drop.transform.position;
                SteamPS[i].Play();
                break;

            }
           
        }

    }

}
