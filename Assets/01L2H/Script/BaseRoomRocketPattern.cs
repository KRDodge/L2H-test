
using UnityEngine;

public class BaseRoomRocketPattern : MonoBehaviour
{
    [SerializeField] protected RocketZone block0;
    [SerializeField] protected RocketZone block1;
    [SerializeField] protected RocketZone block2;
    [SerializeField] protected RocketZone block3;

    private float DeltaTime;

    // Start is called before the first frame update
    void Start()
    {
        DeltaTime = 14.0f;
    }

    // Update is called once per frame
    void Update()
    {
        int time = Mathf.RoundToInt(DeltaTime);
        if (time % 40 == 0 || time % 40 == 18 || time % 40 == 36)
        {
            block0.PlayParticle();
            DeltaTime += 1;
        }
        if (time % 40 == 0 || time % 40 == 9 || time % 40 == 27 || time % 40 == 36)
        {
            block1.PlayParticle();
            DeltaTime += 1;
        }
        if (time % 40 == 18 || time % 40 == 36)
        {
            block2.PlayParticle();
            DeltaTime += 1;
        }
        if (time % 40 == 9 || time % 40 == 27)
        {
            block3.PlayParticle();
            DeltaTime += 1;
        }
        DeltaTime += Time.deltaTime;
    
    }

    private void PlayBaseRoom()
    {
        int time = (int)DeltaTime;

        if (time % 40 == 0 || time%40 == 6 || time%40 == 12)
        {
            block0.PlayParticle();
        }
        if (time % 40 == 0 || time % 40 == 3 || time % 40 == 9 || time % 40 == 12)
        {
            block1.PlayParticle();
        }
        if (time % 40 == 6 || time % 40 == 12)
        {
            block2.PlayParticle();
        }
        if (time % 40 == 3 || time % 40 == 9)
        {
            block3.PlayParticle();
        }
    }
}
