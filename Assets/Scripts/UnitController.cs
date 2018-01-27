using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class UnitController : MonoBehaviour
	{
		[SerializeField] private float initialLifetime = 20;

		[SerializeField] private float speed = 10.0f;

		[SerializeField] private GameObject projectilePrefab = null;

		[SerializeField] private bool hasBall;

		[SerializeField] private float minScaleFraction = 0.3f;

		[SerializeField] private ParticleSystem sickness = null;
		
		private float lifetime;
		private Rigidbody body;
		private int owner;
		private bool isActive = false;
		
		private TextMesh text = null;
		private Vector3 scale;
		private bool isIll;

		public bool IsIll
		{
			get { return isIll; }
			set { isIll = value; }
		}

		public bool HasBall
		{
			get { return hasBall; }
			set 
			{ 
				hasBall = value;
				if (hasBall) 
					isIll = true;
			}
		}

		public int Owner
		{
			set { owner = value; }
		}
		
		public bool IsActive
		{
			set { isActive = value; }
		}

		public float Lifetime
		{
			get { return lifetime; }
		}
		
		void Start()
		{
			lifetime = initialLifetime;
			body = GetComponent<Rigidbody>();
			text = GetComponentInChildren<TextMesh>();
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			scale = transform.localScale;
			isIll = false;
			if (hasBall)
				isIll = true;
		}

		void Update()
		{

            //Debug.Log("Horizontal1:"+Input.GetAxis("Horizontal1"));
            //Debug.Log("Vertical1:"+Input.GetAxis("Vertical1"));
            //Debug.Log("Horizontal1R:" + Input.GetAxis("Horizontal1R"));
            //Debug.Log("Vertical1R:" + Input.GetAxis("Vertical1R"));


            if (isActive)
			{
                // aim and shoot
				if (Input.GetButtonUp("Fire" + owner) && hasBall)
				{

                    Vector2 mousePosition = Input.mousePosition;

                    Vector2 lookingDirection;

                    if (owner == 0)
                    {
                        lookingDirection = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane)) - transform.position;
                    }
                    else //(owner == 1)
                    {
                        lookingDirection = new Vector3(Input.GetAxis("Horizontal" + owner + "R"), Input.GetAxis("Vertical" + owner + "R"));
                    }

                    lookingDirection.Normalize();

                    SpawnProjectile(lookingDirection);
				}

                // player movement
                body.velocity = new Vector2(Input.GetAxis("Horizontal" + owner), Input.GetAxis("Vertical" + owner)) * speed * Time.deltaTime;
				
			}


			if (isIll)
			{
				lifetime -= Time.deltaTime;
				if (lifetime <= 0)
				{
					Debug.Log("Bang");
					
					if (hasBall)
					{
						Vector2 randomDirection = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)).normalized;
						SpawnProjectile(randomDirection);
					}

					transform.parent.GetComponent<Player>().DestroyUnit(this);
				}
			}

			text.text = "" + (int)Mathf.Ceil(lifetime);
			Vector3 minScale = minScaleFraction * scale;
			float degenerationRatio = lifetime / initialLifetime;
			transform.localScale = minScale + (scale-minScale)*degenerationRatio;
		}

		void SpawnProjectile(Vector2 direction)
		{
			Debug.Log("Fired");
			ProjectileController projectile = (Instantiate(projectilePrefab, transform.position, transform.rotation) as GameObject).GetComponent<ProjectileController>();
			projectile.Velocity = direction;
			projectile.Owner = gameObject;
			Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
			hasBall = false;
			ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
			particles.transform.position = projectile.transform.position + new Vector3(0, 0, 0.2f);
			particles.transform.parent = projectile.transform;
			particles.transform.localScale = new Vector3(1, 1, 1);

		}
	}
}
