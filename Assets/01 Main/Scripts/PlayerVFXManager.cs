using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXManager : MonoBehaviour
{
    public VisualEffect footStep;
    public ParticleSystem Blade01;

    public void Update_FootStep(bool state)
    {
        if (footStep != null)
        {
            if (state)
            {
                footStep.Play();
            } else
            {
                footStep.Stop();
            }
        }
    }

    public void PlayBlade01()
    {
        if (Blade01 != null)
        {
            Blade01.Play();
        }
    }
}
