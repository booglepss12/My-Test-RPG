using System;

using UnityEngine;

using UnityEngine.AI;

using RPG.CameraUI;



namespace RPG.Characters

{

    [RequireComponent(typeof(NavMeshAgent))]

   

    public class CharacterMovement : MonoBehaviour

    {

        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = .7f;
        [SerializeField] float animationSpeedMultiplier = 1.5f;
        [SerializeField] float MovingTurnSpeed = 360;
        [SerializeField] float StationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;


        Vector3 clickPoint;

   

        NavMeshAgent agent;

        Animator animator;

        Rigidbody rigidBody;
        float turnAmount;
        float forwardAmount;



        void Start()

        {
            animator = GetComponent<Animator>();
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();

         


            rigidBody = GetComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            agent = GetComponent<NavMeshAgent>();

            agent.updateRotation = false;

            agent.updatePosition = true;

            agent.stoppingDistance = stoppingDistance;



            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;

            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;

        }



        void Update()

        {

            if (agent.remainingDistance > agent.stoppingDistance)

            {

                Move(agent.desiredVelocity);

            }

            else

            {

                Move(Vector3.zero);

            }

        }



        void OnMouseOverPotentiallyWalkable(Vector3 destination)

        {

            if (Input.GetMouseButton(0))

            {

                agent.SetDestination(destination);

            }

        }



        void OnMouseOverEnemy(Enemy enemy)

        {

            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))

            {

                agent.SetDestination(enemy.transform.position);

            }

        }
        public void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }
        public void Kill()
        {
            //to allow death signalling
        }
        void SetForwardAndTurn(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired direction.
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }

            var localMove = transform.InverseTransformDirection(movement);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeedMultiplier;
        }
        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(StationaryTurnSpeed, MovingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }
        void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
                velocity.y = rigidBody.velocity.y;
                rigidBody.velocity = velocity;
            }
        }
    }

}