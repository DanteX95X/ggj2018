using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class Game : MonoBehaviour
	{
		[SerializeField] private float deadline = 60;
		
		[SerializeField] private Text timer = null;

		private List<Player> players = new List<Player>();

		private List<bool> isPlayerDead = new List<bool>();
		
		void Start()
		{
			players = FindObjectsOfType<Player>().OfType<Player>().ToList();
			for (int i = 0; i < players.Count; ++i)
			{
				isPlayerDead.Add(false);
			}

			GetComponent<AudioSource>().playOnAwake = false;
		}

		void Update()
		{
			deadline -= Time.deltaTime;
			timer.text = "" + (int)Mathf.Ceil(Mathf.Clamp(deadline, 0.0f, 10000.0f));

			if (isGameOver() != -2)
			{
				Debug.Log("GameOver");
				if(!GetComponent<AudioSource>().isPlaying)
					GetComponent<AudioSource>().Play();
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
			
			if(index >= 0)
				Debug.Log("Player" + index + " won!");
			else if (index == -1)
				Debug.Log("Everybody died [*]");
			return index;
		}
		
	}
}