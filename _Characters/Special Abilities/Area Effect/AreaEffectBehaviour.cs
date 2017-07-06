using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using RPG.Characters;

using RPG.Core;
using System;

public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility
{



    AreaEffectConfig config;
    ParticleSystem myParticleSystem;



    public void SetConfig(AreaEffectConfig configToSet)

    {

        this.config = configToSet;

    }



    // Use this for initialization

    void Start()
    {

       

    }





    public void Use(AbilityUseParams useParams)

    {
        DealRadialDamage(useParams);
        PlayParticleEffect();

    }

    private void PlayParticleEffect()
    {
       
        //instantiate a particle system attached to player
        var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
        //TODO decide if particle system attaches to player
        //Get the particle system component
        myParticleSystem = prefab.GetComponent<ParticleSystem>();
        //play the particle system
        myParticleSystem.Play();
        //destroy after duration
        Destroy(prefab, myParticleSystem.main.duration);
    }

    private void DealRadialDamage(AbilityUseParams useParams)
    {
       

        // Static sphere cast for targets

        RaycastHit[] hits = Physics.SphereCastAll(

            transform.position,

            config.GetRadius(),

            Vector3.up,

            config.GetRadius()

        );



        foreach (RaycastHit hit in hits)

        {

            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            bool hitPlayer = hit.collider.gameObject.GetComponent <Player>();
            if (damageable != null && !hitPlayer)

            {

                float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget(); // TODO ok Rick?

                damageable.TakeDamage(damageToDeal);

            }

        }
    }
}
