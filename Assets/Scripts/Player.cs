﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private int index = -1;

		[SerializeField]  List<UnitController> units = new List<UnitController>();
		private int currentUnitIndex = -1;
		
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
			units[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		}

		void SwitchUnit()
		{
			units[currentUnitIndex].IsActive = false;
			units[currentUnitIndex].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			++currentUnitIndex;
			currentUnitIndex %= units.Count;
			units[currentUnitIndex].IsActive = true;
			units[currentUnitIndex].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		
		void Update()
		{
			if (Input.GetButtonUp("Switch" + index))
			{
				SwitchUnit();
			}
		}
	}
}