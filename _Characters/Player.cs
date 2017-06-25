﻿using System;

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.Assertions;
using UnityEngine.SceneManagement;



// TODO consider re-wire...

using RPG.CameraUI;

using RPG.Core;

using RPG.Weapons;



namespace RPG.Characters

{

    public class Player : MonoBehaviour, IDamageable

    {

        [SerializeField] float maxHealthPoints = 100f;

        [SerializeField] float baseDamage = 10f;

        [SerializeField] Weapon weaponInUse = null;

        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;



        // Temporarily serialized for dubbing

        [SerializeField] SpecialAbility[] abilities;

        
        AudioSource audioSource;
        Animator animator;

        float currentHealthPoints;

        CameraRaycaster cameraRaycaster;

        float lastHitTime = 0f;



        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }



        void Start()

        {

            RegisterForMouseClick();

            SetCurrentMaxHealth();

            PutWeaponInHand();

            SetupRuntimeAnimator();

            abilities[0].AttachComponentTo(gameObject);
            audioSource = GetComponent<AudioSource>();
        }



        public void TakeDamage(float damage)

        {
            bool playerDies = currentHealthPoints - damage <= 0;
            if (playerDies) //player dies
            {
                ReduceHealth(damage);
                //kill player
                StartCoroutine(KillPlayer());     

            }
            else
            {
                ReduceHealth(damage);
                audioSource.clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
                audioSource.Play();
            }
         }
        IEnumerator KillPlayer()
        {
            //play sound
            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            //trigger death animation
            Debug.Log("Death Animation");
            //wait a bit
            yield return new WaitForSecondsRealtime(audioSource.clip.length); 
            //reload scene
            SceneManager.LoadScene(0);
        }
        private void ReduceHealth (float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            //play sounds
        }


        private void SetCurrentMaxHealth()

        {

            currentHealthPoints = maxHealthPoints;

        }



        private void SetupRuntimeAnimator()

        {

            animator = GetComponent<Animator>();

            animator.runtimeAnimatorController = animatorOverrideController;

            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); // remove const

        }



        private void PutWeaponInHand()

        {

            var weaponPrefab = weaponInUse.GetWeaponPrefab();

            GameObject dominantHand = RequestDominantHand();

            var weapon = Instantiate(weaponPrefab, dominantHand.transform);

            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;

            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;

        }



        private GameObject RequestDominantHand()

        {

            var dominantHands = GetComponentsInChildren<DominantHand>();

            int numberOfDominantHands = dominantHands.Length;

            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on Player, please add one");

            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on Player, please remove one");

            return dominantHands[0].gameObject;

        }



        private void RegisterForMouseClick()

        {

            cameraRaycaster = FindObjectOfType<CameraUI.CameraRaycaster>();

            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;

        }



        void OnMouseOverEnemy(Enemy enemy)

        {

            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))

            {

                AttackTarget(enemy);

            }

            else if (Input.GetMouseButtonDown(1))

            {

                AttemptSpecialAbility(0, enemy);

            }

        }



        private void AttemptSpecialAbility(int abilityIndex, Enemy enemy)

        {

            var energyComponent = GetComponent<Energy>();

            var energyCost = abilities[abilityIndex].GetEnergyCost();



            if (energyComponent.IsEnergyAvailable(energyCost))

            {

                energyComponent.ConsumeEnergy(energyCost);

                var abilityParams = new AbilityUseParams(enemy, baseDamage);

                abilities[abilityIndex].Use(abilityParams);

            }

        }



        private void AttackTarget(Enemy enemy)

        {

            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())

            {

                animator.SetTrigger("Attack"); // TODO make const

                enemy.TakeDamage(baseDamage);

                lastHitTime = Time.time;

            }

        }



        private bool IsTargetInRange(GameObject target)

        {

            float distanceToTarget = (target.transform.position - transform.position).magnitude;

            return distanceToTarget <= weaponInUse.GetMaxAttackRange();

        }

    }

}