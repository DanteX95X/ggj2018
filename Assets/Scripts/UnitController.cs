﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class UnitController : MonoBehaviour
	{
		[SerializeField] private float initialLifetime = 20;

		[SerializeField] private float speed = 10.0f;

		[SerializeField] private GameObject projectilePrefab = null;
		
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
			Vector2 lookingDirection = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
			
			body.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed * Time.deltaTime;

			if (Input.GetButtonUp("Jump"))
			{
				ProjectileController projectile = (Instantiate(projectilePrefab, transform.position, transform.rotation) as GameObject).GetComponent<ProjectileController>();
				projectile.Velocity = lookingDirection.normalized; //body.velocity.normalized;
			}
			
			lifetime -= Time.deltaTime;
			if (lifetime <= 0)
				Debug.Log("Bang");
			else
				Debug.Log(""+ (int)lifetime);
			
		}
	}
}
