using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace RPG.Characters {
    public class HealthSystem : MonoBehaviour {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Image healthBar;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;
        [SerializeField] float deathVanishSeconds= 2.0f;
        const string DEATH_TRIGGER = "Death";

        float currentHealthPoints = 0;
        Animator animator;
        AudioSource audioSource = null;
        CharacterMovement characterMovement;

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        // Use this for initialization
        void Start() {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<CharacterMovement>();

            currentHealthPoints = maxHealthPoints;

        }

        // Update is called once per frame
        void Update()
        {
            UpdateHealthBar();
        }

        void UpdateHealthBar()
        {
            if (healthBar) //enemies may not have health bars to update
            {
                healthBar.fillAmount = healthAsPercentage;
            }
        }
        public void TakeDamage(float damage)
        {
            bool characterDies = (currentHealthPoints - damage <= 0);
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.PlayOneShot(clip);
            if (currentHealthPoints <= 0)
            {
                StartCoroutine(KillCharacter());
            }
        }
        public void Heal(float points)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0f, maxHealthPoints);
        }

        IEnumerator KillCharacter()
        {
            StopAllCoroutines();
            characterMovement.Kill();
            animator.SetTrigger(DEATH_TRIGGER);
            var playerComponent = GetComponent<Player>();
            if (playerComponent && playerComponent.isActiveAndEnabled) //relying on lazy evaluation 
            {
                audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
                audioSource.Play(); //overriding existing sounds
            }
            else //assime is enemy for now, reconsider on other NPCs
            {
                DestroyObject(gameObject, deathVanishSeconds);
            }
            yield return new WaitForSecondsRealtime(audioSource.clip.length);

            SceneManager.LoadScene(0);
        }
    }
}
