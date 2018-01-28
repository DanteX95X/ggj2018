﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
	public class UnitController : MonoBehaviour
	{
		[SerializeField] private float initialLifetime = 20;

		[SerializeField] private float speed = 10.0f;

		[SerializeField] private GameObject projectilePrefab = null;

		[SerializeField] private bool hasBall;

		[SerializeField] private float minScaleFraction = 0.3f;

		[SerializeField] private String unitName = "";
		
		private float lifetime;
		private Rigidbody body;
		private int owner;
		[SerializeField] bool isActive = false;
		
		private TextMesh text = null;
		private Vector3 scale;
		private bool isIll;

		private Animator animator;
        
        private PlayerRotation rotator;

        void Awake()
        {
            // Set up references.
            rotator = GetComponent<PlayerRotation>();
        }

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

		public String UnitName
		{
			get { return unitName; }
		}
		
		void Start()
		{
			lifetime = initialLifetime;
			body = GetComponent<Rigidbody>();
			text = GetComponentInChildren<TextMesh>();
            scale = transform.localScale;
			isIll = false;
			if (hasBall)
				isIll = true;

			animator = GetComponent<Animator>();
		}

        // FixedUpdate is at fixed intervals - used for physics
        void FixedUpdate()
        {
            // turn active unity controlled by knm
            if (isActive)
            {
                if (owner == 0) // keyboard and mouse
                {
                    //turn
                    rotator.Turning();
                }
            }
        }

        void Update()
		{

            // debug
            //if (Input.GetAxis("Horizontal" + 1 + "R") != 0)
            //{
            //    Debug.Log("Horizontal" + 1 + "R" + ": " + Input.GetAxis("Horizontal" + 1 + "R"));
            //}
            //if (Input.GetAxis("Vertical" + 1 + "R") != 0)
            //{
            //    Debug.Log("Vertical" + 1 + "R" + ": " + Input.GetAxis("Vertical" + 1 + "R"));
            //}

            
            if (isActive)
			{
                // aim and shoot
				if (Input.GetButtonUp("Fire" + owner) && hasBall)
				{

                    Vector2 mousePosition = Input.mousePosition;

                    Vector3 lookingDirection;

                    if (owner == 0) // keyboard and mouse
                    {
                        lookingDirection = transform.forward;

                    }
                    else //(owner == 1, 2, 3)
                    {
                        lookingDirection = new Vector3(Input.GetAxis("Horizontal" + owner + "R"), 0, Input.GetAxis("Vertical" + owner + "R"));
                    }

                    lookingDirection.Normalize();

                    SpawnProjectile(lookingDirection);
				}

                // player movement
                Vector3 direction = new Vector3(Input.GetAxis("Horizontal" + owner), 0, Input.GetAxis("Vertical" + owner)).normalized;

                animator.SetFloat("Speed", direction.magnitude);

                Vector3 movement = direction * speed * Time.deltaTime;
                movement.y = body.velocity.y; // do not touch vertical movement, let gravity do its job

                body.velocity = movement;

            }


            if (isIll)
			{
				lifetime -= Time.deltaTime;
				if (lifetime <= 0)
				{
					Debug.Log("Bang");
					
					if (hasBall)
					{
                        Vector3 randomDirection = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)).normalized;
                        SpawnProjectile(randomDirection);
					}

					transform.parent.GetComponent<Player>().DestroyUnit(this);
				}
			}

			text.text = "" + unitName + " " + (int)Mathf.Ceil(lifetime);
			Vector3 minScale = minScaleFraction * scale;
			float degenerationRatio = lifetime / initialLifetime;
			transform.localScale = minScale + (scale-minScale)*degenerationRatio;
		}

        void SpawnProjectile(Vector3 direction)
        {
            
            //Debug.Log("Fired");
            //Debug.Log("direction" + ": " + direction);

            ProjectileController projectile = (Instantiate(projectilePrefab, transform.position, transform.rotation) as GameObject).GetComponent<ProjectileController>();
			projectile.Velocity = direction;
			projectile.Owner = gameObject;
			Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
			hasBall = false;

			ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
			particles.transform.position = projectile.transform.position + new Vector3(0, 0, 0.2f);
			particles.transform.parent = projectile.transform;
			particles.transform.localScale = new Vector3(1, 1, 1);

			animator.SetTrigger("Shoot");

		}
	}
}
