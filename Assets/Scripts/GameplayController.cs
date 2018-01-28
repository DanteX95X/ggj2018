using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class GameplayController : MonoBehaviour
	{
		[SerializeField] private float deadline = 60;
		
		[SerializeField] private Text timer = null;

		[SerializeField] private float shakeStrenght = 5;

		private List<Player> players = new List<Player>();

		private List<bool> isPlayerDead = new List<bool>();

        public AudioClip gameMusic;
        public AudioClip gameOverMusic;

        private AudioSource audioSource;

		private bool gameOver = false;

		private Quaternion targetRotation;
		private Vector3 targetPosition;

		private GameObject survivor;

		private float shakeTime = 0;
		private Vector3 defaultPosition;
		
		private GameObject[] waypoints;
		private bool introDone = false;
		private int waypointIndex = 0;
		
		public bool GameOver
		{
			get { return gameOver; }
		}

		public float ShakeTime
		{
			set { shakeTime = value; }
		}

        void Start()
		{
			players = FindObjectsOfType<Player>().OfType<Player>().ToList();
			for (int i = 0; i < players.Count; ++i)
			{
				isPlayerDead.Add(false);
			}

            audioSource = GetComponent<AudioSource>();
            //audioSource.playOnAwake = true;
            audioSource.clip = gameMusic;
            audioSource.Play();
			targetRotation = Camera.main.transform.rotation;
			targetPosition = Camera.main.transform.position;
			defaultPosition = GameObject.FindGameObjectWithTag("Respawn").transform.position;//Camera.main.transform.position;
			//Time.timeScale = 0;
			waypoints = GameObject.FindGameObjectsWithTag("Respawn");
			introDone = false;
		}

		void Update()
		{
			Debug.Log("Update");
			if (!introDone)
			{
				Intro();
				return;
			}
			
			if (!gameOver)
			{
				deadline -= Time.deltaTime;
				timer.text = "" + (int) Mathf.Ceil(Mathf.Clamp(deadline, 0.0f, 10000.0f));
			}

			ShakeScreen();
			
			if (gameOver)
			{
				targetPosition = survivor.transform.position + survivor.transform.forward * 2 + survivor.transform.up * 2;
				Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, Time.deltaTime);
				targetRotation = Quaternion.LookRotation(survivor.transform.position - Camera.main.transform.position);
				Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, targetRotation, Time.deltaTime);	
			}

			int status = isGameOver();
			if (status != -2 && !gameOver)
			{
				Debug.Log("GameOver");
                
                audioSource.clip = gameOverMusic;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }

				foreach (UnitController unit in FindObjectsOfType<UnitController>())
				{
					if (unit.Owner == status)
					{
						survivor = unit.gameObject;
						unit.GetComponent<Animator>().SetBool("GameOver", true);
					}

					unit.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
				}
				
				gameOver = true;
				foreach (var projectile in FindObjectsOfType<ProjectileController>())
				{
					Destroy(projectile.gameObject);
				}

				foreach (var particles in FindObjectsOfType<ParticleSystem>())
				{
					Destroy(particles.gameObject);
				}
				
				foreach (var text in FindObjectsOfType<TextMesh>())
				{
					Destroy(text.gameObject);
				}
				
				Debug.Log(survivor.GetComponent<UnitController>().UnitName);
				Camera.main.projectionMatrix = Matrix4x4.Perspective(60, 16.0f/9.0f, 0.3f, 1000);
			}

        }

		public void RegisterPlayerDeath(int playerIndex)
		{
			isPlayerDead[playerIndex] = true;
			Debug.Log("Player " + playerIndex + " died!");
		}
		
		int isGameOver()
		{
			int index = -2;
			if (deadline <= 0)
			{
				float max = 0;
				foreach (Player player in players)
				{
					float timeLeft = 0;
					foreach (UnitController unit in player.Units)
					{
						timeLeft += unit.Lifetime;
					}

					if (timeLeft > max)
					{
						max = timeLeft;
						index = player.Index;
					}
				}
			}
			else
			{
				index = -1;
				for (int i = 0; i < isPlayerDead.Count; ++i)
				{
					if (!isPlayerDead[i])
					{
						if(index == -1)
							index = i;
						else
						{
							index = -2;
							break;
						}
					}
				}
			}
			
//			if(index >= 0)
//				Debug.Log("Player" + index + " won!");
//			else if (index == -1)
//				Debug.Log("Everybody died [*]");
			return index;
		}

		void ShakeScreen()
		{
			if (shakeTime > 0)
			{
				shakeTime -= Time.deltaTime;
				Camera.main.transform.position = defaultPosition;
				Camera.main.transform.position += (new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f))).normalized * shakeStrenght;

				if (shakeTime <= 0)
					Camera.main.transform.position = defaultPosition;
			}
		}

		void Intro()
		{
			Debug.Log("intro");
			targetPosition = waypoints[waypointIndex].transform.position;
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, Time.deltaTime);
			targetRotation = waypoints[waypointIndex].transform.rotation;
			Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, targetRotation, Time.deltaTime);

			if ((Camera.main.transform.position - waypoints[waypointIndex].transform.position).magnitude < 0.5 &&
			    (Camera.main.transform.rotation.eulerAngles.y - waypoints[waypointIndex].transform.rotation.eulerAngles.y) < 1)
			{
				Debug.Log("done");
				introDone = true;
				foreach (var player in players)
				{
					foreach (Transform child in player.transform)
						child.gameObject.SetActive(true);
				}
			}
		}


	}
}