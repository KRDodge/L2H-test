using Opsive.Shared.Game;
using Opsive.UltimateCharacterController.Character;
using Opsive.Shared.Input;
using UnityEngine;
using UnityEngine.UI;

public class SelectWeaponUI : MonoBehaviour
{
    [SerializeField] protected Canvas UICanvas;
    [SerializeField] protected Text UIText;
    [SerializeField] protected GameObject m_Character;
    [SerializeField] private bool m_triggerOn = false;

    // Start is called before the first frame update
    void Start()
    {
        UICanvas.enabled = false;
        UIText.enabled = false;
        UIText.text = "Press" + " [f]";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("f") && m_triggerOn)
        {
            var unityInput = m_Character.GetComponent<UnityInput>();
            if (UICanvas.enabled == true)
            {
                UICanvas.enabled = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                unityInput.OnApplicationFocusPublic(true);
            }
            else
            {
                UICanvas.enabled = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                unityInput.OnApplicationFocusPublic(false);
            }
        }
    }

	private void OnTriggerEnter(Collider other)
	{
        var characterLocomotion = other.gameObject.GetCachedParentComponent<UltimateCharacterLocomotion>();
        if(characterLocomotion != null)
        {
            UIText.enabled = true;
            m_triggerOn = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        var characterLocomotion = other.gameObject.GetCachedParentComponent<UltimateCharacterLocomotion>();
        var unityInput = m_Character.GetComponent<UnityInput>();
        if (characterLocomotion != null)
        {
            m_triggerOn = false;
            UIText.enabled = false;
            UICanvas.enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            unityInput.OnApplicationFocusPublic(true);
        }
    }
}
