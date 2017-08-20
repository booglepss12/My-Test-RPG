﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core; // TODO consider re-wire

namespace RPG.Characters
{
    [RequireComponent (typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    { 
        //TODO remove
        [SerializeField] float chaseRadius = 6f;
        

        bool isAttacking = false; //TODO a more rich state
        PlayerMovement player = null;
        float currentWeaponRange;

        void Start()
        {
            player = FindObjectOfType<PlayerMovement>();
        }

        public void TakeDamage(float amount)
        {
            // todo remove
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
            
        }

        

        void OnDrawGizmos()
        {
            // Draw attack sphere 
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            // Draw chase sphere 
            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}