using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Traits;
using Opsive.UltimateCharacterController.Objects;
using Opsive.Shared.Game;
using UnityEngine;

public class Mine : MonoBehaviour
{
    protected GameObject m_Explosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
        var characterLocomotion = other.GetComponentInParent<UltimateCharacterLocomotion>();
        if(characterLocomotion != null)
        {
            var health = this.GetComponentInParent<Health>();
            if (health == null)
            {
                return;
            }

            var position = this.transform.position;
            health.ImmediateDeath(position, Vector3.down, 0);
        }

       
    }

}
