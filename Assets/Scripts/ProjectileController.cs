using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class ProjectileController : MonoBehaviour
	{
		[SerializeField] private float speed = 10.0f;

		private Rigidbody body;

		private Vector2 velocity;

		private GameObject owner;

		private bool collisionEnabled = false;
		
		public Vector2 Velocity
		{
			set { velocity = value * speed; GetComponent<Rigidbody>().AddForce(velocity); }
		}

		public GameObject Owner
		{
			set { owner = value; }
		}

		private void Start()
		{
			body = GetComponent<Rigidbody>();
			collisionEnabled = false;
		}

		private void Update()
		{
		}

		private void OnCollisionEnter(Collision other)
		{
			UnitController unit = other.collider.gameObject.GetComponent<UnitController>();
			if (unit != null)
			{
				Debug.Log("Destroyed");
				if(!unit.IsIll)
					unit.IsIll = true;
				Destroy(gameObject);
			}
			else if(!collisionEnabled)
			{
				Physics.IgnoreCollision(GetComponent<Collider>(), owner.GetComponent<Collider>(), false);
				collisionEnabled = true;
			}
		}
	}
}