using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class MessageBox : MonoBehaviour
	{
		[SerializeField] private float messageTime = 2.0f;

		private List<String> messages = new List<string>();

		private float currentTime;

		public String Message
		{
			set 
			{ 
				messages.Add(value);
			}
		}

		void Start()
		{
			currentTime = messageTime;
		}

		void Update()
		{
			currentTime += Time.deltaTime;

			if (currentTime >= messageTime)
			{
				currentTime = 0;
				if (messages.Count > 0)
				{
					GetComponent<Text>().text = messages[0];
					messages.RemoveAt(0);
				}
				else
					GetComponent<Text>().text = "";
			}
		}
	}
}