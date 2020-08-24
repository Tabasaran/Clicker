using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Holoville.HOTween;

/// <summary>
///  This class is used to display or not a redirection button
/// Author : Pondomaniac Games
/// </summary>
public class SelectLevel : MonoBehaviour
{

		public GameObject _BestScore;//The best score text
		public GameObject _BestLevel;//The best level text
		public GameObject _TransitionRightButton;//The left button
		public GameObject _TransitionLeftButton;//The right button
		public GameObject[] _listOfPanels;//The list of panel that transit to the right and left

		public AudioClip MenuSound;//The sound of button clicks

		public float SpaceBetweenPanels;//The space to keep betwwen panels
		public EaseType AnimationTypeOfPanels;//The type of animation we want to use when panel transit
		public float AnimationDurationOfPanels;//The duration of animation between panels
		public  string  _FirstLevel;//The first level in the panel the help the system know where to start

		private int  _activePanelIndex;//The active panel index to know wich panel is active when transit


		//Called before init
		void Awake ()
		{
				Time.timeScale = 1;
				HOTween.Kill ();
				ButtonRedirect._FirstLevel = _FirstLevel; 
				if (_listOfPanels.Length > 0)
						_activePanelIndex = 0;
		ButtonRedirect.scenes = scenes;
		}

		
		//Initializing the scene
		void Start ()
		{
				(_BestScore.GetComponent (typeof(TextMesh))as TextMesh).text = "" + PlayerPrefs.GetInt ("HighScore");
				(_BestLevel.GetComponent (typeof(TextMesh))as TextMesh).text = "" + PlayerPrefs.GetInt ("HighLevel");


		}

		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.Escape)) {
						Application.Quit ();
				}
				//Detecting if the player clicked on the left mouse button and also if there is no animation playing
				if (Input.GetButtonDown ("Fire1")) {

						//The 3 following lines is to get the clicked GameObject and getting the RaycastHit2D that will help us know the clicked object
						RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
						if (hit.transform != null) {
								if ((hit.transform.gameObject.name == _TransitionRightButton.name && HOTween.GetAllTweens ().Count == 0)) {  
										GetComponent<AudioSource>().PlayOneShot (MenuSound);
										TransitToTheLeft (); 
										Util.ButtonPressAnimation (_TransitionRightButton);
								} else if ((hit.transform.gameObject.name == _TransitionLeftButton.name && HOTween.GetAllTweens ().Count == 0)) {  
										GetComponent<AudioSource>().PlayOneShot (MenuSound);
										TransitToTheRight ();
										Util.ButtonPressAnimation (_TransitionLeftButton);
								}
						}
				}
		}

		//Animate the panel and update panel index
		void TransitToTheRight ()
		{
				HOTween.Complete ();


				if (0 <= _activePanelIndex - 1)
						_activePanelIndex --;
				for (int i = 0; i<= _listOfPanels.Length-1; i++) {
						GameObject go = _listOfPanels [i] as GameObject;
						TweenParms parms = new TweenParms ().Prop ("position", new Vector3 (go.transform.position.x + SpaceBetweenPanels, go.transform.position.y, go.transform.position.z)).Ease (AnimationTypeOfPanels);
						HOTween.To (go.transform, AnimationDurationOfPanels, parms); }
	

		}

		//Animate the panel and update panel index
		void TransitToTheLeft ()
		{
				HOTween.Complete ();
				if (_listOfPanels.Length - 1 >= _activePanelIndex + 1)
						_activePanelIndex ++;
				for (int i = 0; i<= _listOfPanels.Length-1; i++) {
						GameObject go = _listOfPanels [i] as GameObject;
						TweenParms parms = new TweenParms ().Prop ("position", new Vector3 (go.transform.position.x - SpaceBetweenPanels, go.transform.position.y, go.transform.position.z)).Ease (AnimationTypeOfPanels);
						HOTween.To (go.transform, AnimationDurationOfPanels, parms);
				}

		}

	//display or not the right and left button
		void OnGUI ()
		{
				if (_listOfPanels.Length > 0) {
						if (_activePanelIndex <= 0) {
								_TransitionLeftButton.GetComponent<Renderer>().enabled = false;
								_TransitionLeftButton.GetComponent<Collider2D>().enabled = false;
						} else {
								_TransitionLeftButton.GetComponent<Renderer>().enabled = true;
								_TransitionLeftButton.GetComponent<Collider2D>().enabled = true;
						}
						if (_activePanelIndex >= _listOfPanels.Length - 1) {
								_TransitionRightButton.GetComponent<Renderer>().enabled = false;
								_TransitionRightButton.GetComponent<Collider2D>().enabled = false;
						} else {
								_TransitionRightButton.GetComponent<Renderer>().enabled = true;
								_TransitionRightButton.GetComponent<Collider2D>().enabled = true;
						}
				} 
		}

	public string[] scenes;
	#if UNITY_EDITOR
	private static string[] ReadNames()
	{
		List<string> temp = new List<string>();
		foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
		{
			if (S.enabled)
			{
				string name = S.path.Substring(S.path.LastIndexOf('/')+1);
				name = name.Substring(0,name.Length-6);
				temp.Add(name);
			}
		}
		return temp.ToArray();
	}
	[UnityEditor.MenuItem("CONTEXT/SelectLevel/Update Scene Names")]
	private static void UpdateNames(UnityEditor.MenuCommand command)
	{
		SelectLevel context = (SelectLevel)command.context;
		context.scenes = ReadNames();
		}
	
	private void Reset()
	{
		scenes = ReadNames();
		ButtonRedirect.scenes = scenes;
	}
	#endif

	}