using System;

using UnityEngine;

using UnityEngine.AI;

using RPG.CameraUI;



namespace RPG.Characters

{

  

   
    [SelectionBase]
    public class Character : MonoBehaviour

    {

        [Header("Animator")]
        [SerializeField]RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;

        [Header("Audio")]
        [SerializeField] float audioSourceSpatialBlend = 0.5f;

        [Header("Movement")]
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = .7f;
        [SerializeField] float animationSpeedMultiplier = 1.5f;
        [SerializeField] float MovingTurnSpeed = 360;
        [SerializeField] float StationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;

        [Header("Collider")]
        [SerializeField]  Vector3 colliderCenter = new Vector3(0, 1.03f, 0);
        [SerializeField] float colliderRadius = 0.2f;
        [SerializeField] float colliderHeight = 2.03f;

        [Header("Nav Mesh Agent")]
        [SerializeField] float navMeshAgentSteeringSpeed = 1.0f;
        [SerializeField] float navMeshAgentStoppingDistance = 1.3f;

        Vector3 clickPoint;

   

        NavMeshAgent navMeshAgent;

        Animator animator;

        Rigidbody rigidBody;
        float turnAmount;
        float forwardAmount;

        private void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;
            rigidBody = gameObject.AddComponent<Rigidbody>(); 
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;
            animator = gameObject.AddComponent<Animator> ();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.speed = navMeshAgentSteeringSpeed;
            navMeshAgent.stoppingDistance = navMeshAgentStoppingDistance;
            navMeshAgent.autoBraking = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updatePosition = true;
           
        }

        void Start()

        {
            
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();

         


           

            



            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;

            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;

        }



        void Update()

        {

            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)

            {

                Move(navMeshAgent.desiredVelocity);

            }

            else

            {

                Move(Vector3.zero);

            }

        }


        //TODO Move to Player Control
        void OnMouseOverPotentiallyWalkable(Vector3 destination)

        {

            if (Input.GetMouseButton(0))

            {

                navMeshAgent.SetDestination(destination);

            }

        }



        //TODO Move to Player Control
        void OnMouseOverEnemy(Enemy enemy)

        {

            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))

            {

                navMeshAgent.SetDestination(enemy.transform.position);

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