using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbility
    {
        SelfHealConfig config;
        Player player;
        private void Start()
        {
            player = GetComponent<Player>();
        }
        public void SetConfig(SelfHealConfig configToSet)
        {
            this.config = configToSet;
        }

        private void PlayParticleEffect()

        {

            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);

            // TODO decide if particle system attaches to player

            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();

            myParticleSystem.Play();

            Destroy(prefab, myParticleSystem.main.duration);

        }

        public void Use(AbilityUseParams useParams)
        {
            print("Self heal used by:" + gameObject.name);
            player.AdjustHealth(-config.GetExtraHealth()); //note the negative
            PlayParticleEffect();
        }
    }
}
