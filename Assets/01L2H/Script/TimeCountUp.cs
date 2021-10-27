using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountUp : MonoBehaviour
{
	public Text TimerText;
	public bool playing;
	public float Timer;
	private float blinkDuration;

	private void Start()
	{
		Timer = 0;
	}
	void Update()
	{

		if (playing == true)
		{

			Timer += Time.deltaTime;
			int hour = Mathf.FloorToInt(Timer / 3600);
			int minutes = Mathf.FloorToInt(Timer / 60F % 60);


			int seconds = Mathf.FloorToInt(Timer % 60F);
			TimerText.text = hour.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");

			if(Timer > 6600 && Timer < 7200)
			{
				TimerText.color = Color.red;
			}
			if (Timer > 6900 && Timer < 7200)
			{
				blinkDuration += Time.deltaTime;
				if (blinkDuration >= 0.5)
				{
					TimerText.enabled = false;
				}
				if(blinkDuration >= 1)
				{
					TimerText.enabled = true;
					blinkDuration = 0;
				}
			}
			else if(Timer > 7200)
			{
				TimerText.color = Color.gray;
			}
		}

	}
}
