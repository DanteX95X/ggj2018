using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
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
		
		private float lifetime;
		private Rigidbody2D body;
		private int owner;
		private bool isActive = false;
		
		private TextMesh text = null;

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
			body = GetComponent<Rigidbody2D>();
			text = GetComponentInChildren<TextMesh>();
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
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
					Debug.Log("Fired");
					ProjectileController projectile = (Instantiate(projectilePrefab, transform.position, transform.rotation) as GameObject).GetComponent<ProjectileController>();
					projectile.Velocity = lookingDirection;
					projectile.Owner = gameObject;
					Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
					isIll = false;
				}
				
				body.velocity = new Vector2(Input.GetAxis("Horizontal" + owner), Input.GetAxis("Vertical" + owner)) * speed * Time.deltaTime;
				
			}


			if (isIll)
			{
				lifetime -= Time.deltaTime;
				if (lifetime <= 0)
				{
					Debug.Log("Bang");
					Destroy(gameObject);
				}
			}

			text.text = "" + (int) lifetime;
		}
	}
}
