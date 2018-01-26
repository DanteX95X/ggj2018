using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class UnitController : MonoBehaviour
	{
		[SerializeField]
		private float initialLifetime = 20;

		private float lifetime;
		void Start()
		{
			lifetime = initialLifetime;
		}

		void Update()
		{
			lifetime -= Time.deltaTime;
			if (lifetime <= 0)
				Debug.Log("Bang");
			else
				Debug.Log(""+ (int)lifetime);
		}
	}
}
