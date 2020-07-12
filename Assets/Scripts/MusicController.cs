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
			TutorialTheme.Play();	
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
			}
			if (!MainThemePiano.isPlaying)
			{
				MainThemePiano.Play();
			}	
			/*MainThemeOrchestra.volume = 0f;
			MainThemePiano.volume = 1f;*/
		fader[0] = FadeAudioSource(MainThemeOrchestra, 1f, 0.0f, () => { fader[0] = null; });
        StartCoroutine(fader[0]);
		fader[1] = FadeAudioSource(MainThemePiano, 1f, 1f, () => { fader[1] = null; });
        StartCoroutine(fader[1]);		
		}
			
        DontDestroyOnLoad(this.gameObject);
    }
		
	void Restart ()
	{
		//MainThemeOrchestra.volume = 0;
		//MainThemePiano.volume = 1f;
		fader[0] = FadeAudioSource(MainThemeOrchestra, 1f, 0.0f, () => { fader[0] = null; });
        StartCoroutine(fader[0]);
		fader[1] = FadeAudioSource(MainThemePiano, 1f, 1f, () => { fader[1] = null; });
        StartCoroutine(fader[1]);		
		
	}
	
	void StartWalking()
	{
		//MainThemePiano.volume = 0;
		//MainThemeOrchestra.volume = 0.75f;
		fader[0] = FadeAudioSource(MainThemePiano, 1f, 0.0f, () => { fader[0] = null; });
        StartCoroutine(fader[0]);
		fader[1] = FadeAudioSource(MainThemeOrchestra, 1f, 0.75f, () => { fader[1] = null; });
        StartCoroutine(fader[1]);
		
	}
	
	void WakeUp()
	{
		MainThemeOrchestra.volume = 0f;
		MainThemePiano.volume = 0f;
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
