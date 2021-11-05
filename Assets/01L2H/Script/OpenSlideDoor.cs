using Opsive.UltimateCharacterController.Character;
using Opsive.Shared.Game;
using UnityEngine;
using UnityEngine.UI;

public class OpenSlideDoor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected Text UIText;
    [SerializeField] private float speed = 1.0f;

    private Vector3 newPosition;
    private bool moveObject;
    private bool m_triggerOn = false;

    void Start()
    {
        UIText.text = "Press" + " [f]";
        newPosition = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("f") && m_triggerOn)
        {
            moveObject = true;
        }

        if (moveObject == true)
        {
            MoveObject();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        var characterLocomotion = other.gameObject.GetCachedParentComponent<UltimateCharacterLocomotion>();
        if (characterLocomotion != null)
        {
            UIText.enabled = true;
            m_triggerOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var characterLocomotion = other.gameObject.GetCachedParentComponent<UltimateCharacterLocomotion>();
        if (characterLocomotion != null)
        {
            UIText.enabled = false;
            m_triggerOn = true;
        }
    }

    private void MoveObject()
    {
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
        if (transform.position == newPosition)
            moveObject = false;
    }
}
