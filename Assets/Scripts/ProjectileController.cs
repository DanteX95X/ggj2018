using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class ProjectileController : MonoBehaviour
	{
		[SerializeField] private float speed = 10.0f;

		[SerializeField] private float maxScaleFraction = 5;

		[SerializeField] private float growthTime = 10;

		private Rigidbody body;

		private Vector2 velocity;

		private GameObject owner;

		private bool collisionEnabled = false;

		private Vector3 scale;

		private float lifetime;
		
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
			scale = transform.localScale;
			lifetime = 0;
		}

		private void Update()
		{
			lifetime += Time.deltaTime;
			transform.localScale = scale *(1 + (maxScaleFraction-1) * Mathf.Clamp(lifetime/growthTime, 0, 1));
		}

		private void OnCollisionEnter(Collision other)
		{
			UnitController unit = other.collider.gameObject.GetComponent<UnitController>();
			if (unit != null && !unit.HasBall)
			{
				Debug.Log("Destroyed");
				if(owner != null)
					owner.GetComponent<UnitController>().IsIll = false;
				//if(!unit.IsIll)
				unit.HasBall = true;
				Destroy(gameObject);
			}
			else if(!collisionEnabled && owner != null)
			{
				Physics.IgnoreCollision(GetComponent<Collider>(), owner.GetComponent<Collider>(), false);
				collisionEnabled = true;
			}
		}
	}
}