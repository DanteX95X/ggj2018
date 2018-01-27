using System.Collections;
using System.Collections.Generic;
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

		[SerializeField] private int owner;
		
		private float lifetime;
		private Rigidbody2D body;
		
		void Start()
		{
			lifetime = initialLifetime;
			body = GetComponent<Rigidbody2D>();
		}

		void Update()
		{
			Vector2 mousePosition = Input.mousePosition;
			Vector2 lookingDirection = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane)) - transform.position;
			lookingDirection.Normalize();
			
			body.velocity = new Vector2(Input.GetAxis("Horizontal" + owner), Input.GetAxis("Vertical" + owner)) * speed * Time.deltaTime;

			if (Input.GetButtonUp("Fire" + owner))
			{
				ProjectileController projectile = (Instantiate(projectilePrefab, transform.position, transform.rotation) as GameObject).GetComponent<ProjectileController>();
				projectile.Velocity = lookingDirection;
				projectile.Owner = gameObject;
				Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
			}
			
			lifetime -= Time.deltaTime;
			if (lifetime <= 0)
				Debug.Log("Bang");
			else
				Debug.Log(""+ (int)lifetime);
			
		}
	}
}
