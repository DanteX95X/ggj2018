using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class UnitController : MonoBehaviour
	{
		[SerializeField] private float initialLifetime = 20;

		[SerializeField] private float speed = 10.0f;
		
		private float lifetime;
		private Rigidbody2D body;
		
		void Start()
		{
			lifetime = initialLifetime;
			body = GetComponent<Rigidbody2D>();
		}

		void Update()
		{
			body.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed * Time.deltaTime;
			
			lifetime -= Time.deltaTime;
			if (lifetime <= 0)
				Debug.Log("Bang");
			else
				Debug.Log(""+ (int)lifetime);
		}
	}
}
