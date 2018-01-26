using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class ProjectileController : MonoBehaviour
	{
		[SerializeField] private float speed = 10.0f;

		private Rigidbody2D body;

		private Vector2 velocity;
		
		public Vector2 Velocity
		{
			set { velocity = value * speed; }
		}
		
		void Start()
		{
			body = GetComponent<Rigidbody2D>();
		}

		void Update()
		{
			body.velocity = velocity * Time.deltaTime;
		}
	}
}