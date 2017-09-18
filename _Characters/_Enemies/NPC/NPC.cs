﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    public class NPC : MonoBehaviour
    {

        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2.0f;
        [SerializeField] float waypointDwellTime = 2.0f;

        PlayerControl player = null;
        Character character;
        int nextWaypointIndex;
        float currentWeaponRange;
        float distanceToPlayer;

        enum State { idle, patrolling, chasing }
        State state = State.idle;

        void Start()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<PlayerControl>();
        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            bool inChaseRing = distanceToPlayer > currentWeaponRange && distanceToPlayer <= chaseRadius;
            bool outsideChaseRing = distanceToPlayer > chaseRadius;
            if (outsideChaseRing)
            {
                StopAllCoroutines();
                StartCoroutine(Patrol());
            }
            if (inChaseRing)
            {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }

        }

        IEnumerator Patrol()
        {
            state = State.patrolling;

            while (patrolPath != null)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPos);
                CycleWaypointWhenClose(nextWaypointPos);
                yield return new WaitForSeconds(waypointDwellTime);
            }
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPos)
        {
            if (Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
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

            // Draw chase sphere 
            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
