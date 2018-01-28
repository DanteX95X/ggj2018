using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private int index = -1;

		[SerializeField]  List<UnitController> units = new List<UnitController>();

		[SerializeField] private GameObject tombstone = null;

		[SerializeField] private MessageBox messageBox = null;
		
		private int currentUnitIndex = -1;

        [SerializeField] private List<AudioClip> deathSounds = new List<AudioClip>();
        
        public Color color = Color.black;
        
        public List<UnitController> Units
		{
			get { return units; }
		}

		public int Index
		{
			get { return index; }
		}
		
		void Start()
		{
            // gain ownership over all child units
			foreach (Transform child in transform)
			{
				UnitController unit = child.GetComponent<UnitController>();
				unit.Owner = index;

                unit.fillBackgroundColor.GetComponent<Image>().color = color;
                unit.fillFillColor.GetComponent<Image>().color = color;

                units.Add(unit);
			}

			currentUnitIndex = 0;
			units[0].IsActive = true;

			GetComponent<AudioSource>().playOnAwake = false;

			messageBox = FindObjectOfType<MessageBox>();
		}

		void SwitchUnit()
		{
			if (units.Count == 0)
				return;

            units[currentUnitIndex].healthBar.SetActive(false);
            units[currentUnitIndex].IsActive = false;

			++currentUnitIndex;
			currentUnitIndex %= units.Count;
			units[currentUnitIndex].IsActive = true;
            units[currentUnitIndex].healthBar.SetActive(true);

        }

        public void DestroyUnit(UnitController unit)
		{	
			if (unit == units[currentUnitIndex])
			{
				SwitchUnit();
			}


			for (int i = 0; i < units.Count; ++i)
			{
				if (units[i] == unit)
				{
					units.RemoveAt(i);
					if (i < currentUnitIndex)
						--currentUnitIndex;
				}
			}

			messageBox.Message = "" + unit.UnitName + " ciągnie druta w zaświatach.";
			Instantiate(tombstone, unit.transform.position, tombstone.transform.rotation);
			Destroy(unit.gameObject);

			if (units.Count == 0)
			{
				FindObjectOfType<GameplayController>().RegisterPlayerDeath(index);
			}

            int soundsIndex = Random.Range(0, deathSounds.Count);
            GetComponent<AudioSource>().PlayOneShot(deathSounds[soundsIndex]);

		}
		
		void Update()
		{
			if (Input.GetButtonUp("Fire1"))
			{
				Debug.Log("git");
			}
			if (Input.GetButtonUp("Switch" + index))
			{
				SwitchUnit();
			}
		}
	}
}
