using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameManagerAudioSettings
{
    [System.Serializable]
    public class toAssign
    {
        public GameObject _audioQueues_Status;

    }
    public toAssign assignableFields;
    
    public AudioSource quoteAudioSource;

}


public class AudioManager : MonoBehaviour {
    public AudioSource masterSource;
    public AudioSource musicAudioSource;
    public AudioSource dialogAudioSource;
    public AudioSource voicesAudioSource;
    public AudioSource sfxAudioSource;



    /// <summary>
    /// Plays a random quote
    /// </summary>
    /// <param name="arr">Array of clips</param>
    /// <param name="currentPlayer">AudioSource</param>
    void playRandomQuote(UnityEngine.AudioClip[] arr, AudioSource currentPlayer)
    {
        int randquote;
        randquote = new System.Random().Next(0, arr.Length - 1);
        currentPlayer.clip = arr[randquote];
        currentPlayer.Play();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
