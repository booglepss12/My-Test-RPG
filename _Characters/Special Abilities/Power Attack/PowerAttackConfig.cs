using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Power Attack"))]
    public class PowerAttackConfig : SpecialAbilityConfig
    {
        [Header("Power Attack Extra Damage")]
        [SerializeField] float energyDamage = 10f;

        public override ISpecialAbility AddComponent(GameObject gameObjectToattachTo)
        {
            var behaviorComponent = gameObjectToattachTo.AddComponent<PowerAttackBehaviour>();
            behaviorComponent.SetConfig(this);
            return behaviorComponent;
        }
    }
}
