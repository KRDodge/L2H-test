using Opsive.Shared.Game;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Game;
using Opsive.UltimateCharacterController.Traits;
using Opsive.UltimateCharacterController.Utility;
using UnityEngine;

public class RocketZone : MonoBehaviour
{
    [Tooltip("The amount of damage to apply during each damage event.")]
    [SerializeField] protected float m_DamageAmount = 20;
    [Tooltip("The interval between damage events.")]
    [SerializeField] protected float m_rocketDamageInterval = 0.2f;
    [Tooltip("The interval between damage events.")]
    [SerializeField] protected float m_rocketPlayInterval = 3f;
    [Tooltip("RocketHitLocation")]
    [SerializeField] protected Transform m_rocketHitLocation;
    [Tooltip("GameObject")]
    [SerializeField] protected GameObject m_rocketProjectile;

    private Health m_Health;
    private Transform m_HealthTransform;
    private ScheduledEventBase m_ScheduledDamageEvent;

    private float particlePlayTime;
    private bool isDamageOn;

	private void Awake()
	{
    }

	private void Update()
	{
        if(particlePlayTime < 3)
        {
            particlePlayTime += Time.deltaTime;
            isDamageOn = true;
        }
        else
        {
            isDamageOn = false;
        }
    }
    public void PlayParticle()
    {
        particlePlayTime = 0.0f;
        Instantiate(m_rocketProjectile, m_rocketHitLocation.position, Quaternion.identity);
    }

	private void OnTriggerEnter(Collider other)
    {
        if (m_Health != null)
        {
            return;
        }

        // A main character collider is required.
        if (!MathUtility.InLayerMask(other.gameObject.layer, 1 << LayerManager.Character))
        {
            return;
        }

        // The object must be a character.
        var characterLocomotion = other.GetComponentInParent<UltimateCharacterLocomotion>();
        if (characterLocomotion == null)
        {
            return;
        }

        // With a health component.
        var health = characterLocomotion.GetComponent<Health>();
        if (health == null)
        {
            return;
        }

        m_Health = health;
        m_HealthTransform = health.transform;
        if(isDamageOn)
        {
            m_ScheduledDamageEvent = SchedulerBase.Schedule(0, Damage);
        }
    }

    /// <summary>
    /// Apply damage to the health component.
    /// </summary>
    private void Damage()
    {
        m_Health.Damage(m_DamageAmount, m_HealthTransform.position + Random.insideUnitSphere, Vector3.zero, 0);

        // Apply the damage again if the object still has health remaining.
        if (m_Health.Value > 0)
        {
            m_ScheduledDamageEvent = SchedulerBase.Schedule(m_rocketDamageInterval, Damage);
        }

        m_ScheduledDamageEvent = SchedulerBase.Schedule(0, Damage);
    }

    /// <summary>
    /// An object has exited the trigger.
    /// </summary>
    /// <param name="other">The collider that exited the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        // A main character collider is required.
        if (!MathUtility.InLayerMask(other.gameObject.layer, 1 << LayerManager.Character))
        {
            return;
        }

        var health = other.GetComponentInParent<Health>();
        if (health == m_Health)
        {
            // The object has left the trigger - stop applying damage.
            SchedulerBase.Cancel(m_ScheduledDamageEvent);
            m_Health = null;
        }
    }
}
