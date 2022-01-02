
    using Opsive.UltimateCharacterController.Traits;
    using UnityEngine;

    /// <summary>
    /// Instantly kills the character if the character moves beneath the moving platform as it is moving down.
    /// </summary>
    public class MovingPlatformDeathZone : MonoBehaviour
    {
        private Transform m_Transform;
        private Vector3 m_PrevPosition;
        private bool m_DownwardMovement;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        private void Awake()
        {
        }

        /// <summary>
        /// Detect if the platform is moving downward.
        /// </summary>
        private void FixedUpdate()
        {
        }

        /// <summary>
        /// An
        /// </summary>
        /// <param name="other"></param>
        /// 
        private void OnCollisionEnter(Collision collision)
        {
            var health = collision.collider.GetComponent<CharacterHealth>();
            if (health == null)
            {
                return;
            }

            var position = m_Transform.position;
            health.ImmediateDeath(position, Vector3.down, (position - m_PrevPosition).magnitude);
        }
        private void Onclo(Collider other)
        {
            // Kill the character.
            var health = other.GetComponent<CharacterHealth>();
            if (health == null) {
                return;
            }

            var position = m_Transform.position;
            health.ImmediateDeath(position, Vector3.down, (position - m_PrevPosition).magnitude);
        }
    }
