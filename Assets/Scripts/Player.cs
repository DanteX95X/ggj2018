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
			foreach (Transform child in transform)
			{
				UnitController unit = child.GetComponent<UnitController>();
				unit.Owner = index;
				units.Add(unit);
			}

			currentUnitIndex = 0;
			units[0].IsActive = true;
			units[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

			GetComponent<AudioSource>().playOnAwake = false;

			messageBox = FindObjectOfType<MessageBox>();
		}

		void SwitchUnit()
		{
			if (units.Count == 0)
				return;
			
			units[currentUnitIndex].IsActive = false;
			units[currentUnitIndex].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			++currentUnitIndex;
			currentUnitIndex %= units.Count;
			units[currentUnitIndex].IsActive = true;
			units[currentUnitIndex].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
		}

		private static int ufo = 0;
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

			messageBox.Message = "Unitname has died!" + ufo++;
			Instantiate(tombstone, unit.transform.position, unit.transform.rotation);
			Destroy(unit.gameObject);

			if (units.Count == 0)
			{
				FindObjectOfType<Game>().RegisterPlayerDeath(index);
			}

			GetComponent<AudioSource>().Play();
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
