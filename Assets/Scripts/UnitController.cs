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

		[SerializeField] private bool isIll;

		[SerializeField] private float minScaleFraction = 0.3f;
		
		private float lifetime;
		private Rigidbody body;
		private int owner;
		private bool isActive = false;
		
		private TextMesh text = null;
		private Vector3 scale;

		public bool IsIll
		{
			get { return isIll; }
			set { isIll = value; }
		}

		public int Owner
		{
			set { owner = value; }
		}
		
		public bool IsActive
		{
			set { isActive = value; }
		}

		void Start()
		{
			lifetime = initialLifetime;
			body = GetComponent<Rigidbody>();
			text = GetComponentInChildren<TextMesh>();
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			scale = transform.localScale;
		}

		void Update()
		{
			if (isActive)
			{
				Vector2 mousePosition = Input.mousePosition;
				Vector2 lookingDirection =
					Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane)) -
					transform.position;
				lookingDirection.Normalize();

				if (Input.GetButtonUp("Fire" + owner) && isIll)
				{
					SpawnProjectile(lookingDirection);
				}
				
				body.velocity = new Vector2(Input.GetAxis("Horizontal" + owner), Input.GetAxis("Vertical" + owner)) * speed * Time.deltaTime;
				
			}


			if (isIll)
			{
				lifetime -= Time.deltaTime;
				if (lifetime <= 0)
				{
					Debug.Log("Bang");
					Vector2 randomDirection = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)).normalized;
					SpawnProjectile(randomDirection);
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
			isIll = false;
		}
	}
}
