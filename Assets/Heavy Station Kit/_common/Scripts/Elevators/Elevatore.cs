using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevatore : MonoBehaviour
{
    public GameObject elevatoreMesh;

	public void OnTriggerStay(Collider other)
	{
        elevatoreMesh.transform.position += elevatoreMesh.transform.up * Time.deltaTime;

    }
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
