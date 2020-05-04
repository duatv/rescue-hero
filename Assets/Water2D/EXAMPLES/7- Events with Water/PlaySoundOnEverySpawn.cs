using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnEverySpawn : MonoBehaviour
{

    public AudioSource sfx;

    public void PlaySound() {
        if (sfx)
            sfx.Play();
    }
}
