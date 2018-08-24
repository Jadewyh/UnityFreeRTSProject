/*
Class(es): MenuAudioSettings
Short description: Menu Handler for audio settings

Initial author: Christian Kessner <SpiegelEiXXL>
Initial creation date: 20th AUG 2018
Initial name: MenuAudioSettings.cs
Written for: Unity Free 2 Play RTS project
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioSettings : MonoBehaviour {
	public GameObject MasterSlider;
	public GameObject DialogSlider;
	public GameObject VoiceSlider;
	public GameObject EffectSlider;
	public GameObject MusicSlider;

	[ReadOnly] public bool ready;
	
	
	// Use this for initialization
	void Start () {
		if (!MasterSlider || !DialogSlider || !VoiceSlider || !EffectSlider || !MusicSlider)
		{
			Debug.Log("Please assign the sliders in the UnityEditor!");
			return;
		}		
	}

	public void applyAudioChanges(){
		// yep. apply stuff. do stuff.
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
