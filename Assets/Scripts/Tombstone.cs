using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombstone : MonoBehaviour
{
	[SerializeField] private float time = 0.5f;
	
	void Start ()
	{
		gameObject.layer = 1;
	}
	
	void Update ()
	{
		time -= Time.deltaTime;
		if (time <= 0 && gameObject.layer == 1)
			gameObject.layer = 0;
	}
}
