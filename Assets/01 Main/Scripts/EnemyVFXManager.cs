using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    public VisualEffect FootStep;
    public VisualEffect AttackVFX;


    public void Bite()
    {
        PlayAttackVFX();
    }
    public void PlayAttackVFX()
    {

            AttackVFX.SendEvent("OnPlay");

    }

    public void BurstFootStep()
    {

            FootStep.SendEvent("OnPlay");

    }
}
