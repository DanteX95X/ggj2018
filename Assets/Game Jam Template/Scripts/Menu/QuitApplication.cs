using UnityEngine;
using System.Collections;

public class QuitApplication : MonoBehaviour {

    public AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Quit()
	{
        audioSource.Play();

		//If we are running in a standalone build of the game
	#if UNITY_STANDALONE
		//Quit the application
		Application.Quit();
	#endif

		//If we are running in the editor
	#if UNITY_EDITOR
		//Stop playing the scene
		UnityEditor.EditorApplication.isPlaying = false;
	#endif
	}
}
