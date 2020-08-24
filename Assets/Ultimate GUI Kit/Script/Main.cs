using UnityEngine;
using System.Collections;
using Holoville.HOTween;

/// <summary>
///  This class is the main entry point of the game it should be attached to a gameobject and be instanciate in the scene
/// Author : Pondomaniac Games
/// </summary>
public class Main : MonoBehaviour
{
    


		public GameObject _Tutorial;//Tutorial GameObject
		public int _scoreIncrement;//The amount of point to increment each time we score
		private  int _scoreTotal = 0;//The score 
		float progress = 0;//The progress bar progress 
		public float _timerCoef  ;//The progress bar speed 
		public GameObject _Time;//The timer
		public GameObject _PauseButton;//The pause button we use in the scene
		private bool isPaused = false ;//A flag indicating if the game is paused
		private bool isEnded = false ;//A flag indicating if the game has ended
		private bool isCountingDown = true ;//A flag indicating if the game is counting down
		public GameObject _ReloadButton;//The reload button we use in the scene
		public GameObject _PlayButton;//The play button we use in the scene
		public GameObject _MenuButton;//The menu button we use in the scene
		public GameObject _PausedBackground;//The pause background
		float timing = 0;//The local timer
		public GameObject _TimeIsUp;//The object indicating if the time is up
		public GameObject _MessageEffectWhenShown;//A particul effect we can use when a message is shown
		bool _BestScoreReached = false ;//A flag indicating if the bestscore has been reached
		bool _BestLevelReached = false ;//A flag indicating if the bestlevel has been reached
		public GameObject _CurrentScore;//The current score in the scene
		public GameObject _BestScore;//The best score in the scene
		public GameObject _CurrentLevel;//The current level in the scene
		public GameObject _BestLevel;//The best level in the scene
		public GameObject _ScoreBoard;//The score board when the game has ended or when the time is up
		public GameObject _FaceBookButton;//The facebook button to share the score with friends
		public GameObject _Level;//Level reached
		public GameObject _LevelTextValue;//Level reached text value
		public GameObject _CountDown;//The CountDown object that is not used directly that we use to replicate the style when displaying countdown
		public GameObject _TestScore;//The testscore button used to test the score and level increment 
		public GameObject _TestLevelEnd;//The testscore button used to test the rnd of the level
		public AudioClip PowerSound;//The sound heared when we level up
		public AudioClip MenuSound;//The sound heared when we click on a button
		int   level = 0;
		float maxProgress = 0;
		float ScoreByLevel = 0;
		public AudioClip LevelUpSound;
		public AudioClip TimeUpSound;
		public AudioClip BestScoreSound;
		public AudioClip EndSound;
		public AudioClip CountDownSound;

		// Use this for initialization
		void Start ()
		{

				UpdateLevel (0);
				progress = (float)(timing * _timerCoef);
				//_Timer new Rect(pos.x, pos.y, size.x, size.y), progressBarEmpty);
				_Time.transform.localScale = new Vector3 (Mathf.Clamp01 (progress), _Time.transform.localScale.y, 0);
	
				StartCoroutine (Init ());
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
								if (hit.transform.gameObject.name == _MenuButton.name) {
										GetComponent<AudioSource>().PlayOneShot (MenuSound);
										hit.transform.localScale = new Vector3 (1.1f, 1.1f, 0);
										Application.LoadLevel ("MainMenu");
								}
								if (hit.transform.gameObject.name == _FaceBookButton.name) {
										hit.transform.localScale = new Vector3 (1.5f, 1.5f, 0);
										onBragClicked ();
								}
								if (hit.transform.gameObject.name == _ReloadButton.name) {
										GetComponent<AudioSource>().PlayOneShot (MenuSound);
										Time.timeScale = 1;
										isPaused = false;
										HOTween.Play ();
										hit.transform.localScale = new Vector3 (1.1f, 1.1f, 0);
										Application.LoadLevel (Application.loadedLevelName);
								}
								if (hit.transform.gameObject.name == _PauseButton.name && !isPaused && !isCountingDown && !isEnded && HOTween.GetTweenersByTarget (_PlayButton.transform, false).Count == 0 && HOTween.GetTweenersByTarget (_MenuButton.transform, false).Count == 0) {
										GetComponent<AudioSource>().PlayOneShot (MenuSound);
										StartCoroutine (ShowMenu ());
										hit.transform.localScale = new Vector3 (1.1f, 1.1f, 0);
								} else if ((hit.transform.gameObject.name == _PauseButton.name || hit.transform.gameObject.name == _PlayButton.name) && !isEnded && !isCountingDown && isPaused && HOTween.GetTweenersByTarget (_PlayButton.transform, false).Count == 0 && HOTween.GetTweenersByTarget (_MenuButton.transform, false).Count == 0) {
										GetComponent<AudioSource>().PlayOneShot (MenuSound);
										StartCoroutine (HideMenu ());
										hit.transform.localScale = new Vector3 (1f, 1f, 0);
								} else if ((hit.transform.gameObject.name == _TestScore.name) && !isEnded && !isCountingDown && !isPaused) {
										GetComponent<AudioSource>().PlayOneShot (MenuSound);
										_scoreTotal += _scoreIncrement;
										UpdateLevel (20 * _scoreIncrement);
										Util.ButtonPressAnimation (hit.transform.gameObject);
								} else if ((hit.transform.gameObject.name == _TestLevelEnd.name) && !isEnded && !isCountingDown && !isPaused) {
										isEnded = true; 
										isPaused = true;
										StartCoroutine (ShowBoardScore ());
										Util.ButtonPressAnimation (hit.transform.gameObject);
					//Update the Level
										UpdateReachedLevel ();
								}
				

				
						}
				}
				if (isPaused)
						return;

				if (! isPaused) {
						timing += 0.001f;
						progress = (float)(timing * _timerCoef);
						_Time.transform.localScale = new Vector3 (Mathf.Clamp01 (progress), _Time.transform.localScale.y, 0);


				}
				if (Mathf.Clamp01 (progress) >= 1) {
						isEnded = true; 
						isPaused = true;
						TweenParms parms = new TweenParms ().Prop ("position", new Vector3 (_TimeIsUp.transform.position.x, -0.85f, -6)).Ease (EaseType.EaseOutQuart);
						HOTween.To (_TimeIsUp.transform, 0.5f, parms).WaitForCompletion ();
						StartCoroutine (ShowBoardScore ());

				}
				//Update the score
				(GetComponent (typeof(TextMesh))as TextMesh).text = _scoreTotal.ToString ();
				if (PlayerPrefs.GetInt ("HighScore") < _scoreTotal && !_BestScoreReached) {
						_BestScoreReached = true;
				}
				if (PlayerPrefs.GetInt ("HighLevel") < level && !_BestLevelReached) {
						_BestLevelReached = true;
				}
				


	

		}
	//Update the Level
		void UpdateLevel (int score)
		{ 
				ScoreByLevel += score;
				maxProgress = (float)Mathf.Floor (250 * (level + 1));
				_Level.transform.localScale = new Vector3 ((float)(ScoreByLevel / maxProgress), _Level.transform.localScale.y, 0);

				if (Mathf.Clamp01 (_Level.transform.localScale.x) >= 1) { 
						//_Level.transform.localScale= new Vector3 (0, _Level.transform.localScale.y, 0) ;
						//Order is important for calculus
						level += 1;
						ScoreByLevel = 0;
						timing = 0;
						GetComponent<AudioSource>().PlayOneShot (PowerSound);
						TweenParms parms = new TweenParms ().Prop ("localScale", new Vector3 (0, _Level.transform.localScale.y, -6)).Ease (EaseType.EaseOutQuart);
						HOTween.To (_Level.transform, 0.5f, parms).WaitForCompletion ();
						parms = new TweenParms ().Prop ("localScale", new Vector3 (0, _Time.transform.localScale.y, -6)).Ease (EaseType.EaseOutQuart);
						HOTween.To (_Time.transform, 0.5f, parms).WaitForCompletion ();

						(_LevelTextValue.GetComponent (typeof(TextMesh))as TextMesh).text = level.ToString ();

						var destroyingParticle = GameObject.Instantiate (_LevelTextValue as GameObject, new Vector3 (_LevelTextValue.transform.position.x, _LevelTextValue.transform.position.y, _LevelTextValue.transform.position.z - 1), transform.rotation) as GameObject;
						Color oldColor = destroyingParticle.GetComponent<Renderer>().material.color;
						parms = new TweenParms ().Prop ("color", new Color (oldColor.r, oldColor.b, oldColor.g, 0f)).Ease (EaseType.EaseOutQuart);
						HOTween.To ((destroyingParticle.GetComponent (typeof(TextMesh))as TextMesh), 4f, parms);
						parms = new TweenParms ().Prop ("fontSize", 150).Ease (EaseType.EaseOutQuart);
						HOTween.To ((destroyingParticle.GetComponent (typeof(TextMesh))as TextMesh), 2f, parms);
						Destroy (destroyingParticle, 5);

				} else {
						(_LevelTextValue.GetComponent (typeof(TextMesh))as TextMesh).text = level.ToString ();

				}

		}
	//Show the bestscore board
		IEnumerator ShowBoardScore ()
		{
				GetComponent<AudioSource>().Stop ();
				GetComponent<AudioSource>().PlayOneShot (TimeUpSound);
				yield return new WaitForSeconds (0.5f);
				GetComponent<AudioSource>().PlayOneShot (EndSound);
				(_BestScore.GetComponent (typeof(TextMesh))as TextMesh).text = "" + PlayerPrefs.GetInt ("HighScore");
				(_BestLevel.GetComponent (typeof(TextMesh))as TextMesh).text = "" + PlayerPrefs.GetInt ("HighLevel");
				(_CurrentScore.GetComponent (typeof(TextMesh))as TextMesh).text = "" + _scoreTotal;
				(_CurrentLevel.GetComponent (typeof(TextMesh))as TextMesh).text = "" + (_LevelTextValue.GetComponent (typeof(TextMesh))as TextMesh).text;
				SetScore (_scoreTotal);
				yield return new WaitForSeconds (1);
				TweenParms parms = new TweenParms ().Prop ("position", new Vector3 (_ScoreBoard.transform.position.x, 5f, _ScoreBoard.transform.position.z)).Ease (EaseType.EaseOutQuart);
				HOTween.To (_ScoreBoard.transform, 0.5f, parms);
				//_MenuButton.transform.position = new Vector3 (4, _MenuButton.transform.position.y, _MenuButton.transform.position.z);
				parms = new TweenParms ().Prop ("position", new Vector3 (_MenuButton.transform.position.x, 1.7f, -8)).Ease (EaseType.EaseOutQuart);
				HOTween.To (_MenuButton.transform, 0.7f, parms).WaitForCompletion ();
				parms = new TweenParms ().Prop ("position", new Vector3 (_ReloadButton.transform.position.x, 1.7f, -8)).Ease (EaseType.EaseOutQuart);
				HOTween.To (_ReloadButton.transform, 0.9f, parms).WaitForCompletion ();
		}
	//Update the pause menu
		IEnumerator ShowMenu ()
		{
				isPaused = true;
				HOTween.Pause ();
				GetComponent<AudioSource>().Pause ();

				TweenParms parms = new TweenParms ().Prop ("position", new Vector3 (_PausedBackground.transform.position.x, 4, -5)).Ease (EaseType.EaseOutQuart);
				HOTween.To (_PausedBackground.transform, 0.2f, parms).WaitForCompletion ();

				parms = new TweenParms ().Prop ("position", new Vector3 (_PlayButton.transform.position.x, 3.5f, -6)).Ease (EaseType.EaseOutQuart);
				HOTween.To (_PlayButton.transform, 0.4f, parms).WaitForCompletion ();

				parms = new TweenParms ().Prop ("position", new Vector3 (_ReloadButton.transform.position.x, 3.5f, -6)).Ease (EaseType.EaseOutQuart);
				HOTween.To (_ReloadButton.transform, 0.5f, parms).WaitForCompletion ();

				parms = new TweenParms ().Prop ("position", new Vector3 (_MenuButton.transform.position.x, 3.5f, -6)).Ease (EaseType.EaseOutQuart);
				yield return StartCoroutine (HOTween.To (_MenuButton.transform, 0.6f, parms).WaitForCompletion ());


				Time.timeScale = 0;


		}
	//Hide the pause menu
		IEnumerator HideMenu ()
		{
				Time.timeScale = 1;
				isPaused = false;
				HOTween.Play ();

				TweenParms parms = new TweenParms ().Prop ("position", new Vector3 (_PausedBackground.transform.position.x, 16, -5)).Ease (EaseType.EaseOutQuart);
				HOTween.To (_PausedBackground.transform, 0.6f, parms).WaitForCompletion ();

				parms = new TweenParms ().Prop ("position", new Vector3 (_PlayButton.transform.position.x, 16, -6)).Ease (EaseType.EaseOutQuart);
				HOTween.To (_PlayButton.transform, 0.4f, parms).WaitForCompletion ();
				GetComponent<AudioSource>().Play ();

				parms = new TweenParms ().Prop ("position", new Vector3 (_ReloadButton.transform.position.x, 16, -6)).Ease (EaseType.EaseOutQuart);
				HOTween.To (_ReloadButton.transform, 0.5f, parms).WaitForCompletion ();


				parms = new TweenParms ().Prop ("position", new Vector3 (_MenuButton.transform.position.x, 16, -6)).Ease (EaseType.EaseOutQuart);
				yield return StartCoroutine (HOTween.To (_MenuButton.transform, 0.2f, parms).WaitForCompletion ());

		}


	//Where facebook button is clicked
		private void onBragClicked ()
		{
				if (!FB.IsLoggedIn) {
						// Initialize FB SDK              
						enabled = false;                  
						FB.Init (SetInit, OnHideUnity);
						FB.Login ("email,publish_actions", LoginCallback);  
				}
	      

				FbDebug.Log ("onBragClicked");
				FB.Feed (
			linkCaption: "I just scored " + _scoreTotal + " ! Can you beat it?",
			picture: "http://static.wix.com/media/13f8cb_48245b5b162848f493d15a0f40e05b6b.png?dn=Icone_300.png",
			linkName: "Checkout my MatchMania greatness! Can you beat me?",
			link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
				);
		}

		void LoginCallback (FBResult result)
		{                                                                                          
				FbDebug.Log ("LoginCallback");                                                          
		
				if (FB.IsLoggedIn) {                                                                                      
						OnLoggedIn ();                                                                      
				}                                                                                      
		}
	
		void OnLoggedIn ()
		{                                                                                          
				FbDebug.Log ("Logged in. ID: " + FB.UserId);
				// Request player info and profile picture                                                                           
				onBragClicked ();
		
		}
	
		private void SetInit ()
		{                                                                                            
				FbDebug.Log ("SetInit");                                                                  
				enabled = true; // "enabled" is a property inherited from MonoBehaviour                  
				if (FB.IsLoggedIn) {                                                                                        
						FbDebug.Log ("Already logged in");                                                    
						OnLoggedIn ();                                                                        
				}                                                                                        
		}
	
		private void OnHideUnity (bool isGameShown)
		{                                                                                            
				FbDebug.Log ("OnHideUnity");                                                              
				if (!isGameShown) {                                                                                        
						// pause the game - we will need to hide                                             
						Time.timeScale = 0;                                                                  
				} else {                                                                                        
						// start the game back up - we're getting focus again                                
						Time.timeScale = 1;                                                                  
				}                                                                                        
		}

	//Setting the score in the player preferences
		void SetScore (int _scoreTotal)
		{
				PlayerPrefs.SetInt ("LastScore", _scoreTotal);
				if (PlayerPrefs.GetInt ("HighScore") < _scoreTotal) {

						PlayerPrefs.SetInt ("HighScore", _scoreTotal);
						GetComponent<AudioSource>().PlayOneShot (BestScoreSound);
				}
				if (PlayerPrefs.GetInt ("HighLevel") < _scoreTotal) {
						//PlayerPrefs.SetInt ("OldHighLevel",PlayerPrefs.GetInt ("HighLevel"));
						PlayerPrefs.SetInt ("HighLevel", int.Parse ((_LevelTextValue.GetComponent (typeof(TextMesh))as TextMesh).text));

				}
		}

	//Show a message in the screen 
			IEnumerator Init ()
		{
				isPaused = true;
				Vector3 center = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
				//PlayerPrefs.SetInt ("Tutorial", 0);
				if (PlayerPrefs.GetInt ("Tutorial") != 1) {
						var isOkay = false;

						while (isOkay==false) { 
								if (Input.GetButtonDown ("Fire1")) {
										isOkay = true; 
										TweenParms parms = new TweenParms ().Prop ("localPosition", new Vector3 (100, 0, -10)).Ease (EaseType.EaseOutQuart);
										HOTween.To (_Tutorial.transform, 3f, parms);
										PlayerPrefs.SetInt ("Tutorial", 1);
			     	
								}
								yield return 0;	
						}
		
				} else {
						_Tutorial.transform.localPosition = new Vector3 (100, 0, -10);

				}
	//Count down from 3,2,1 Go!
				AnimateBigSmall (_CountDown, new Vector3 (center.x, center.y, -5), "3");
				GetComponent<AudioSource>().PlayOneShot (CountDownSound);
				yield return new WaitForSeconds (0.7f);
				AnimateBigSmall (_CountDown, new Vector3 (center.x, center.y, -5), "2");
				GetComponent<AudioSource>().PlayOneShot (CountDownSound);
				yield return new WaitForSeconds (0.7f);
				AnimateBigSmall (_CountDown, new Vector3 (center.x, center.y, -5), "1");
				GetComponent<AudioSource>().PlayOneShot (CountDownSound);
				yield return new WaitForSeconds (0.7f);
				AnimateBigSmall (_CountDown, new Vector3 (center.x, center.y, -5), "Go!");
				GetComponent<AudioSource>().PlayOneShot (CountDownSound);
				yield return new WaitForSeconds (0.5f);
				isPaused = false;
				isCountingDown = false;
		}

	//Gameobject animation from big to small when showing a message
		void AnimateBigSmall (GameObject go, Vector3 position, string s)
		{ 
				var destroyingParticle = GameObject.Instantiate (go as GameObject, position, transform.rotation) as GameObject;
				(destroyingParticle.GetComponent (typeof(TextMesh))as TextMesh).text = s;
				Color oldColor2 = destroyingParticle.GetComponent<Renderer>().material.color;
				TweenParms parms2 = new TweenParms ().Prop ("color", new Color (oldColor2.r, oldColor2.b, oldColor2.g, 0f)).Ease (EaseType.EaseOutQuart);
				HOTween.To ((destroyingParticle.GetComponent (typeof(TextMesh))as TextMesh), 4f, parms2);
				parms2 = new TweenParms ().Prop ("fontSize", 200).Ease (EaseType.EaseOutQuart);
				HOTween.To ((destroyingParticle.GetComponent (typeof(TextMesh))as TextMesh), 3f, parms2);
				Destroy (destroyingParticle, 4);
		}

	//Update the reached level in the player preferences
		void UpdateReachedLevel ()
		{
				
		
				int LoadedSceneIndexInSettings = -1;
				int ReachedLevelIndexIndexInSettings = -1;
				for (int i=0; i<=ButtonRedirect.scenes.Length-1; i++) {
			if (Application.loadedLevelName == ButtonRedirect.scenes [i])
								LoadedSceneIndexInSettings = i;
			if (PlayerPrefs.GetString ("ReachedLevel") == ButtonRedirect.scenes [i])
								ReachedLevelIndexIndexInSettings = i;
				}
		if (LoadedSceneIndexInSettings >= ReachedLevelIndexIndexInSettings && (ButtonRedirect.scenes.Length - 1 >= LoadedSceneIndexInSettings + 1)) {
			PlayerPrefs.SetString ("ReachedLevel", ButtonRedirect.scenes [LoadedSceneIndexInSettings + 1]);	
			Debug.Log (ButtonRedirect.scenes [LoadedSceneIndexInSettings + 1]);
				}
		}
}

