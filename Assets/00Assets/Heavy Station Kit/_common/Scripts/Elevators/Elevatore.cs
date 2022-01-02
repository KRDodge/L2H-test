using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevatore : MonoBehaviour
{
    public GameObject elevatorMesh;
    public float elevatorSpeed = 2.0f;
    private bool elevatorActive = false;

	public void OnTriggerStay(Collider other)
	{
        elevatorMesh.transform.position += elevatorMesh.transform.up * Time.deltaTime * elevatorSpeed;
    }

    public void OnTriggerEnter(Collider other)
    {
        elevatorActive = true;
    }
    public void OnTriggerExit(Collider other)
	{
        elevatorActive = false;
    }

	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (elevatorSpeed < 3 && elevatorActive == true)
            elevatorSpeed += 0.1f * Time.deltaTime;
    }
}
