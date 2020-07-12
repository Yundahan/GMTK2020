using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour



{
	
	public AudioSource MainThemePiano;
	public AudioSource MainThemeOrchestra;
	public AudioSource DeathTheme;
	
	private GameObject ControllerObject;

	
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
		if (!MainThemeOrchestra.isPlaying)
		{	
			MainThemeOrchestra.Play();
		}
		if (!MainThemePiano.isPlaying)
		{
			MainThemePiano.Play();
		}	
		MainThemeOrchestra.volume = 0f;
		MainThemePiano.volume = 1f;

        DontDestroyOnLoad(this.gameObject);
    }
		
	void Restart ()
	{
		MainThemeOrchestra.volume = 0;
		MainThemePiano.volume = 1f;	
		
	}
	
	void StartWalking()
	{
		MainThemePiano.volume = 0;
		MainThemeOrchestra.volume = 0.75f;
		
	}
	
	void WakeUp()
	{
		MainThemeOrchestra.volume = 0f;
		MainThemePiano.volume = 0f;
		DeathTheme.volume = 0.3f;
		DeathTheme.Play();

		
	}
}
