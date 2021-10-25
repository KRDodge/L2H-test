namespace Opsive.UltimateCharacterController.Demo.Objects
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Game;
    using Opsive.UltimateCharacterController.Objects;
    using Opsive.UltimateCharacterController.SurfaceSystem;
    using Opsive.UltimateCharacterController.Traits;
    using Opsive.UltimateCharacterController.Utility;


    public class FollowPlayer : MonoBehaviour
    {
        [Tooltip("rotation speed")]
        [SerializeField] protected float m_RotationSpeed = 5;
        [Tooltip("movementSpeed")]
        [SerializeField] protected float m_MoveSpeed = 2;

        [Tooltip("The location that the projectile should be fired.")]
        [SerializeField] protected Transform m_FireLocation;
        [Tooltip("The distance in which the enemy Attacks.")]
        [SerializeField] protected float m_AttackRange = 10;
        [Tooltip("The distance in which the enemy Follows.")]
        [SerializeField] protected float m_MaxFollowRange = 8;
        [Tooltip("The delay until the turret will fire again.")]
        [SerializeField] protected float m_FireDelay = 0.5f;
        [Tooltip("Is head in oposite direction")]
        [SerializeField] protected bool m_IsOposite = false;

        [Tooltip("The projectile that is fired.")]
        [SerializeField] protected GameObject m_Projectile;
        [Tooltip("The magnitude of the projectile velocity when fired. The direction is determined by the fire direction.")]
        [SerializeField] protected float m_VelocityMagnitude = 10;
        [Tooltip("A LayerMask of the layers that can be hit when fired at.")]
        [SerializeField] protected LayerMask m_ImpactLayers = ~(1 << LayerManager.IgnoreRaycast | 1 << LayerManager.TransparentFX | 1 << LayerManager.UI | 1 << LayerManager.Overlay);
        [Tooltip("The amount of damage to apply to the hit object.")]
        [SerializeField] protected float m_DamageAmount = 10;
        [Tooltip("How much force to apply to the hit object.")]
        [SerializeField] protected float m_ImpactForce = 0.05f;
        [Tooltip("The number of frames to add the force to.")]
        [SerializeField] protected int m_ImpactForceFrames = 1;
        [Tooltip("The Surface Impact triggered when the weapon hits an object.")]
        [SerializeField] protected SurfaceImpact m_SurfaceImpact;

        [Tooltip("Optionally specify a muzzle flash that should appear when the turret is fired.")]
        [SerializeField] protected GameObject m_MuzzleFlash;
        [Tooltip("The location that the muzzle flash should spawn.")]
        [SerializeField] protected Transform m_MuzzleFlashLocation;

        [Tooltip("Optionally specify an audio clip that should play when the turret is fired.")]
        [SerializeField] protected AudioClip m_FireAudioClip;

        private GameObject m_GameObject;
        private Transform m_Transform;
        private AudioSource m_AudioSource;
#if ULTIMATE_CHARACTER_CONTROLLER_MULTIPLAYER
        private INetworkInfo m_NetworkInfo;
#endif

        private Transform m_Target;
        private Health m_Health;
        private float m_LastFireTime;

		private void Awake()
		{
            m_GameObject = gameObject;
            m_Transform = transform;
            m_AudioSource = GetComponent<AudioSource>();
#if ULTIMATE_CHARACTER_CONTROLLER_MULTIPLAYER
            m_NetworkInfo = GetComponent<INetworkInfo>();
#endif
        }

        private void OnEnable()
        {
            m_LastFireTime = Time.time;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (m_Target == null)
            {
                return;
            }

            bool isAttack = CheckForAttack();
            
            if(!isAttack)
            {
                MoveTowardsTarget();
            }
        }

        public void MoveTowardsTarget()
        {
            var targetRotation = Quaternion.Euler(0,0,0);
            if (m_IsOposite == true)
            {
                targetRotation = Quaternion.LookRotation(m_Target.position - m_Transform.position);
                m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(m_Transform.position - m_Target.position);
                m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);
            }

            m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);


            if (Vector3.Distance(m_Transform.position, m_Target.position) > m_MaxFollowRange)
            {
                Vector3 followPostion = new Vector3(m_Target.position.x, transform.position.y, m_Target.position.z);
                m_Transform.position = Vector3.MoveTowards(transform.position, followPostion, m_MoveSpeed * Time.deltaTime);
            }
        }

        public bool CheckForAttack()
        {
            // The turret can attack if it hasn't fired recently and the target is in front of the turret.
            if (m_LastFireTime + m_FireDelay < Time.time && (m_Transform.position - m_Target.position).magnitude < m_AttackRange && (m_Health == null || m_Health.Value > 0))
            {
                Attack();
                return true;
            }
            return false;
        }

        public void Attack()
        {
            m_LastFireTime = Time.time;

            // Spawn a projectile which will move in the direction that the turret is facing
            var projectile = ObjectPoolBase.Instantiate(m_Projectile, m_FireLocation.position, m_Transform.rotation).GetCachedComponent<Projectile>();
            projectile.Initialize(m_FireLocation.forward * m_VelocityMagnitude, Vector3.zero, null, m_DamageAmount, m_ImpactForce, m_ImpactForceFrames,
                                    m_ImpactLayers, string.Empty, 0, m_SurfaceImpact, m_GameObject);
#if ULTIMATE_CHARACTER_CONTROLLER_MULTIPLAYER
            if (m_NetworkInfo != null) {
                NetworkObjectPool.NetworkSpawn(m_Projectile, projectile.gameObject, true);
            }
#endif

            // Spawn a muzzle flash.
            if (m_MuzzleFlash)
            {
                var muzzleFlash = ObjectPoolBase.Instantiate(m_MuzzleFlash, m_MuzzleFlashLocation.position, m_MuzzleFlashLocation.rotation, m_Transform).GetCachedComponent<MuzzleFlash>();
                muzzleFlash.Show(null, 0, true, null);
            }

            // Play a firing sound.
            if (m_FireAudioClip != null)
            {
                m_AudioSource.clip = m_FireAudioClip;
                m_AudioSource.Play();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_Target != null || !MathUtility.InLayerMask(other.gameObject.layer, 1 << LayerManager.Character))
            {
                return;
            }

            var characterLocomotion = other.GetComponentInParent<UltimateCharacterLocomotion>();
            if (characterLocomotion == null)
            {
                return;
            }

            m_Target = characterLocomotion.transform;
            m_Health = characterLocomotion.GetComponent<Health>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject != m_Target.gameObject)
            {
                return;
            }

            m_Target = null;
        }

    }
}
