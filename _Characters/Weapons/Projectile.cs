using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO consider re-wire
using RPG.Core;

namespace RPG.Characters
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed;
        [SerializeField] GameObject shooter; // So can inspected when paused

        const float DESTROY_DELAY = 0.01f;
       

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        public void SetDamage(float damage)
        {
            float damageCaused = damage;
        }

        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }

        void OnCollisionEnter(UnityEngine.Collision collision)
        {
            var layerCollidedWith = collision.gameObject.layer;
            if (shooter && layerCollidedWith != shooter.layer)
            {
                //DamageIfDamageable(collision);
            }
        }

        
    }
}