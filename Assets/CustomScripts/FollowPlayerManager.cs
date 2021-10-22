using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerManager : MonoBehaviour
{
	public GameObject player;

	#region Singleton

	public static FollowPlayerManager instance;

	private void Awake()
	{
		instance = this;
	}

	#endregion

}
