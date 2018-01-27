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

		private GameObject owner;

		private bool collisionEnabled = false;

		[SerializeField] private float invincibilityTime = 0.1f;
		
		public Vector2 Velocity
		{
			set { velocity = value * speed; GetComponent<Rigidbody2D>().AddForce(velocity); }
		}

		public GameObject Owner
		{
			set { owner = value; }
		}

		private void Start()
		{
			body = GetComponent<Rigidbody2D>();
			collisionEnabled = false;
		}

		private void Update()
		{
			//body.velocity = velocity * Time.deltaTime;

			invincibilityTime -= Time.deltaTime;
			if (invincibilityTime < 0 && !collisionEnabled)
			{
				Physics2D.IgnoreCollision(GetComponent<Collider2D>(), owner.GetComponent<Collider2D>(), false);
				collisionEnabled = true;
			}
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			UnitController unit = other.collider.gameObject.GetComponent<UnitController>();
			if (unit != null)
			{
				Debug.Log("Destroyed");
				if(!unit.IsIll)
					unit.IsIll = true;
				Destroy(gameObject);
			}
		}
	}
}