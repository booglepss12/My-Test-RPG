﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core; // TODO consider re-wire

namespace RPG.Characters
{
    [RequireComponent (typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]
    public class EnemyAI : MonoBehaviour
    { 
        //TODO remove
        [SerializeField] float chaseRadius = 6f;
        

        enum State { idle, patrolling, attacking, chasing}
        State state = State.idle;
        PlayerMovement player = null;
        float currentWeaponRange;
        float distanceToPlayer;
        Character character;
        void Start()
        {
            player = FindObjectOfType<PlayerMovement>();
            character = GetComponent<Character>();
        }

        public void TakeDamage(float amount)
        {
            // todo remove
        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                //stop what we are doing
                StopAllCoroutines();
                //start patrolling
                state = State.patrolling;
            }
            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                //stop what we are doing
                StopAllCoroutines();
                //chase the player
                StartCoroutine(ChasePlayer());

            }
            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
                //stop what we are doing
                StopAllCoroutines();
                //attack the player
                state = State.attacking;
            }
            
        }
        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
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