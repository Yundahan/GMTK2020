using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour



{
	
	public AudioSource MainThemePiano;
	public AudioSource MainThemeOrchestra;
	public AudioSource DeathTheme;
	public AudioSource TutorialTheme;
	public AudioSource AltThemePiano;
	public AudioSource AltThemeOrchestra;
	
	private GameObject ControllerObject;
	private IEnumerator[] fader = new IEnumerator[2];
	private int volumeChangesPerSecond = 15;
	private AudioSource currentlyPlayingPiano;
	private AudioSource currentlyPlayingOrchestra; 
	

	
    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	 void Awake()
    {
		Scene scene = SceneManager.GetActiveScene();
		if (scene.name == "TutorialScene" || scene.name == "TutorialScene2")
		{
			if(!TutorialTheme.isPlaying)
			{
				TutorialTheme.Play();
				currentlyPlayingPiano = MainThemePiano;
				currentlyPlayingOrchestra = MainThemeOrchestra;
			}	
		}
		else 
		{
			if(TutorialTheme.isPlaying)
			{
				TutorialTheme.Stop();	
			}	
			if (!MainThemeOrchestra.isPlaying)
			{	
				MainThemeOrchestra.Play();
				AltThemeOrchestra.Play();
			}
			if (!MainThemePiano.isPlaying)
			{
				MainThemePiano.Play();
				AltThemePiano.volume = 0f;
				AltThemePiano.Play();
			}
		if (scene.name == "Level_4")
			{
				fader[0] = FadeAudioSource(currentlyPlayingOrchestra, 1f, 0.0f, () => { fader[0] = null; });
				StartCoroutine(fader[0]);
				currentlyPlayingOrchestra = AltThemeOrchestra;
				fader[0] = FadeAudioSource(currentlyPlayingPiano, 1f, 0.0f, () => { fader[0] = null; });
				StartCoroutine(fader[0]);
				currentlyPlayingPiano = AltThemePiano;
			}	
		/*MainThemeOrchestra.volume = 0f;
		MainThemePiano.volume = 1f;*/
		fader[0] = FadeAudioSource(currentlyPlayingOrchestra, 1f, 0.0f, () => { fader[0] = null; });
        StartCoroutine(fader[0]);
		fader[1] = FadeAudioSource(currentlyPlayingPiano, 1f, 1f, () => { fader[1] = null; });
        StartCoroutine(fader[1]);		
		}
			
        DontDestroyOnLoad(this.gameObject);
    }
		
	void Restart ()
	{
		//currentlyPlayingOrchestra.volume = 0;
		//currentlyPlayingPiano.volume = 1f;
		fader[0] = FadeAudioSource(currentlyPlayingOrchestra, 1f, 0.0f, () => { fader[0] = null; });
        StartCoroutine(fader[0]);
		fader[1] = FadeAudioSource(currentlyPlayingPiano, 1f, 1f, () => { fader[1] = null; });
        StartCoroutine(fader[1]);		
		
	}
	
	void StartWalking()
	{
		//currentlyPlayingPiano.volume = 0;
		//currentlyPlayingOrchestra.volume = 0.75f;
		fader[0] = FadeAudioSource(currentlyPlayingPiano, 1f, 0.0f, () => { fader[0] = null; });
        StartCoroutine(fader[0]);
		fader[1] = FadeAudioSource(currentlyPlayingOrchestra, 1f, 0.75f, () => { fader[1] = null; });
        StartCoroutine(fader[1]);
		
	}
	
	void WakeUp()
	{
		currentlyPlayingOrchestra.volume = 0f;
		currentlyPlayingPiano.volume = 0f;
		DeathTheme.volume = 0.3f;
		DeathTheme.Play();

		
	}
	IEnumerator FadeAudioSource(AudioSource player, float duration, float targetVolume, Action finishedCallback)
		{
			//Calculate the steps
			int Steps = (int)(volumeChangesPerSecond * duration);
			float StepTime = duration / Steps;
			float StepSize = (targetVolume - player.volume) / Steps;

			//Fade now
			for (int i = 1; i < Steps; i++)
			{
				player.volume += StepSize;
				yield return new WaitForSeconds(StepTime);
			}
			//Make sure the targetVolume is set
			player.volume = targetVolume;

			//Callback
			if (finishedCallback != null)
			{
				finishedCallback();
			}
		}
}
