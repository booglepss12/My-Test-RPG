using System;

using System.Collections;

using UnityEngine;

using UnityEngine.UI;



namespace RPG.Characters

{

    public class Energy : MonoBehaviour

    {

        [SerializeField] RawImage energyBar = null;

        [SerializeField] float maxEnergyPoints = 100f;

        [SerializeField] float regenPointsPerSecond = 1f;
        



        bool isRegenerating = false;

        const float REGEN_INTERVAL_S = .1f;

        float currentEnergyPoints;

        CameraUI.CameraRaycaster cameraRaycaster;



        // Use this for initialization

        void Start()

        {

            currentEnergyPoints = maxEnergyPoints;

        }
        void Update()
        {
           if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoints();
                UpdateEnergyBar();
            }
        }



        public bool IsEnergyAvailable(float amount)

        {

            return amount <= currentEnergyPoints;

        }



        public void ConsumeEnergy(float amount)

        {

            float newEnergyPoints = currentEnergyPoints - amount;

            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);

            UpdateEnergyBar();

            if (!isRegenerating)

            {

                StartCoroutine(RegenerateEnergy());

            }

        }



        IEnumerator RegenerateEnergy()

        {

            while (currentEnergyPoints < maxEnergyPoints)

            {

                AddEnergyPoints();

                UpdateEnergyBar();

                yield return new WaitForSeconds(REGEN_INTERVAL_S);

            }

        }



        private void AddEnergyPoints()

        {

            var energyToAdd = regenPointsPerSecond * REGEN_INTERVAL_S;

            var newEnergyPoints = currentEnergyPoints + energyToAdd;

            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0f, maxEnergyPoints);

        }



        private void UpdateEnergyBar()

        {

            // TODO remove magic numbers

            float xValue = -(EnergyAsPercent() / 2f) - 0.5f;

            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);

        }



        float EnergyAsPercent()

        {

            return currentEnergyPoints / maxEnergyPoints;

        }

    }

}