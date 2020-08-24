using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Holoville.HOTween;

/// <summary>
///  This class is the redirect class it should be attached to a button in the levels scene
/// Author : Pondomaniac Games
/// </summary>
public class ButtonRedirect : MonoBehaviour
{

		public string _redirectedScene;	//The name of the scene we want to redirect to 
		public AudioClip MenuSound; //The sound of the menu clicks
		private bool ShouldTransit = false;//A transition flag
		public static   string  _FirstLevel;//The sound of the menu clicks
	    public static string[] scenes;
		//Called before init
		void Awake ()
		{
				Time.timeScale = 1; 
				HOTween.Kill ();
	
				
		}

	//This method is called after the init 
		void Start ()
		{
		        //Loading the editor scenes orders to determine if the player can go the level or not
						
				int RedirectedSceneIndexInSettings = -1;
				int ReachedLevelIndexIndexInSettings = -1;
				int FirstLevel = -1;
		for (int i=0; i<=scenes.Length-1; i++) {
			if (_redirectedScene == scenes [i])
								RedirectedSceneIndexInSettings = i;
			if (PlayerPrefs.GetString ("ReachedLevel") == scenes [i])
								ReachedLevelIndexIndexInSettings = i;
			if (_FirstLevel == scenes [i])
								FirstLevel = i;
				}
				if (_redirectedScene != string.Empty && (RedirectedSceneIndexInSettings <= ReachedLevelIndexIndexInSettings || RedirectedSceneIndexInSettings == FirstLevel)) {
						this.GetComponent<Renderer>().enabled = true;
						this.GetComponent<Collider2D>().enabled = true;
			
			
			
				} else {
						this.GetComponent<Renderer>().enabled = false;
						this.GetComponent<Collider2D>().enabled = false;
						var allChildren = this.GetComponentsInChildren (typeof(Transform), true);
			
						foreach (Transform child  in allChildren) {
								GameObject Module = child.gameObject;
								Module.GetComponent<Renderer>().enabled = false;
								}
			
				}
		
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (HOTween.GetAllTweens ().Count == 0 && ShouldTransit) {
						if (_redirectedScene != string.Empty) {	
								Application.LoadLevel (_redirectedScene);
						}
				}
				if (Input.GetKeyDown (KeyCode.Escape)) {
						Application.Quit ();
				}
				//Detecting if the player clicked on the left mouse button and also if there is no animation playing
				if (Input.GetButtonDown ("Fire1")) {

						//The 3 following lines is to get the clicked GameObject and getting the RaycastHit2D that will help us know the clicked object
						RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
						if (hit.transform != null) {
								if ((hit.transform.gameObject.name == this.name)) {
										GetComponent<AudioSource>().PlayOneShot (MenuSound);
										Util.ButtonPressAnimation (hit.transform.gameObject);
										ShouldTransit = true;
										Time.timeScale = 1;
								}
		
						}
				}
		}

		}