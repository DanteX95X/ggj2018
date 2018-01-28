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

        private Vector3 velocity;

        private GameObject owner;

		private bool collisionEnabled = false;

		private Vector3 scale;

		private float lifetime;
		
        public Vector3 Velocity
        {
            set
            {
                //Debug.Log("value" + ": " + value);
                velocity = value * speed;
                //Debug.Log("velocity" + ": " + velocity);
                GetComponent<Rigidbody>().AddForce(velocity);
            }
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

			GetComponent<AudioSource>().playOnAwake = false;
		}

		private void Update()
		{
			lifetime += Time.deltaTime;
			Vector3 targetScale = scale *(1 + (maxScaleFraction-1) * Mathf.Clamp(lifetime/growthTime, 0, 1));
			transform.localScale = targetScale;
			GetComponentInChildren<ParticleSystem>().transform.localScale = targetScale * 0.1f;
		}

		private void OnCollisionEnter(Collision other)
		{
			UnitController unit = other.collider.gameObject.GetComponent<UnitController>();
			if (unit != null && !unit.HasBall)
			{
				Debug.Log("Destroyed" + unit.UnitName);
				if(owner != null)
					owner.GetComponent<UnitController>().IsIll = false;
				//if(!unit.IsIll)
				unit.HasBall = true;

				ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
				particles.transform.position = unit.transform.position + new Vector3(0, 0, 0.2f);
				particles.transform.parent = unit.transform;
				particles.transform.localScale = new Vector3(1, 1, 1);
				
				Destroy(gameObject);
				return;
			}
			else if(!collisionEnabled && owner != null)
			{
				Physics.IgnoreCollision(GetComponent<Collider>(), owner.GetComponent<Collider>(), false);
				collisionEnabled = true;
			}
			
			GetComponent<AudioSource>().Play();
		}
	}
}