using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossRocketPattern : MonoBehaviour
{
    [SerializeField] protected RocketZone m_rocketZone;
    [SerializeField] protected Transform[] m_randomPoints;

    private float DeltaTime;

    private void Update()
	{
        if (DeltaTime > 5)
        {
            int spawnPoint = Random.Range(0, 23);
            m_rocketZone.transform.position = m_randomPoints[spawnPoint].position;
            m_rocketZone.PlayParticle();
            DeltaTime = 0;
        }

        DeltaTime += Time.deltaTime;
    }
   
}
