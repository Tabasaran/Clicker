using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Holoville.HOTween;

/// <summary>
///  This class is the MainMenu class that contain the logic of the menu
/// Author : Pondomaniac Games
/// </summary>
public class MainMenu : MonoBehaviour {
	public GameObject _Logo;//The animated logos 
	public GameObject _PlayButton;//The play button
	public GameObject _PurchaseButton;//The purchase button
	public GameObject _BestScore;//The bestscore text
	public GameObject _BestLevel;//The level text
	public GameObject _FacebookButton;//The facebook button to connect
	public GameObject _AddCoinsButton;//The button used to add coins

	public GameObject _MoreTimeButton;//The more time button
	public GameObject _LuckButton;//The luck button
	public GameObject _Luckx2Button;//The luck x 2 button
	public GameObject _MoreTimeButtonText;//The more time text
	public GameObject _LuckButtonText;//The luck text
	public GameObject _Luckx2ButtonText;//The luck x 2 text
	private Boolean _IsMoreTime;//More time flag
	private Boolean _IsLuck;//Luck flag
	private Boolean _IsLuckx2;//Luck x 2 flag

	public GameObject _50CoinsButton;//50 Coins
	public GameObject _100CoinsButton;//100 Coins
	public GameObject _200CoinsButton;//200 Coins
	public GameObject _300CoinsButton;//300 Coins

	public GameObject _GoldValue;//Gold value text
	public GameObject _GoldValue2;//Gold value text 2

	public GameObject _TransitionPlayButton;//The small play button 
	public GameObject _TransitionReturnButton;//The return button 
	public GameObject _StoreReturnButton;//The return button to the store
	public GameObject _MainMenuPanel;//The main panel 
	public GameObject _PowerPanel;//The  panel that contains powers
	public GameObject _StorePanel;//The  panel that contains the store
	// Use this for initialization
	//private static List<object>                 friends         = null;//The  list of friends used by the facebook api
	private static Dictionary<string, string>   profile         = null;//The  list of profiles used by the facebook api
	public string _NextScene;//The  nextScene to navigate to 
	public string _Ladder;
	public AudioClip MenuSound;//The  menu sound when the user clicka buton
	public float SpaceBetweenPanels;//The space to keep between panels
	public EaseType AnimationTypeOfPanels;//The animation effect used on the panel
	public float AnimationDurationOfPanels;//The animation duration time
	public bool EnableFacebook;//Specify if we should display facebook buton or not

	//Called before init
void Awake()
	{ Time.timeScale=1; 
		GetGoldValues ();

	}

	void LoginCallback(FBResult result)                                                        
	{                                                                                          
		FbDebug.Log("LoginCallback");                                                          
		
		if (FB.IsLoggedIn)                                                                     
		{                                                                                      
			OnLoggedIn();                                                                      
		}                                                                                      
	}                                                                                          
	
	void OnLoggedIn()                                                                          
	{                                                                                          
		FbDebug.Log("Logged in. ID: " + FB.UserId);
		// Request player info and profile picture                                                                           
		FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);  
		FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, MyPictureCallback);    

	}           

	private void SetInit()                                                                       
	{                                                                                            
		FbDebug.Log("SetInit");                                                                  
	//	enabled = true; // "enabled" is a property inherited from MonoBehaviour                  
		if (FB.IsLoggedIn)                                                                       
		{                                                                                        
			FbDebug.Log("Already logged in");                                                    
			OnLoggedIn();                                                                        
		}                                                                                        
	}                                                                                            
	
	private void OnHideUnity(bool isGameShown)                                                   
	{                                                                                            
		FbDebug.Log("OnHideUnity");                                                              
		if (!isGameShown)                                                                        
		{                                                                                        
			// pause the game - we will need to hide                                             
			Time.timeScale = 0;                                                                  
		}                                                                                        
		else                                                                                     
		{                                                                                        
			// start the game back up - we're getting focus again                                
			Time.timeScale = 1;                                                                  
		}                                                                                        
	} 



	
	void APICallback(FBResult result)                                                                                              
	{                                                                                                                              
		FbDebug.Log("APICallback");                                                                                                
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			FbDebug.Error(result.Error);                                                                                           
			// Let's just try again                                                                                                
			FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);     
			return;                                                                                                                
		}                                                                                                                         
		
		profile = Util.DeserializeJSONProfile(result.Text);                                                                        
		GameStateManager.Username = profile["first_name"];                                                                         
		//friends = Util.DeserializeJSONFriends(result.Text);                                                                        
	}                                                                                                                              
	
	void MyPictureCallback(FBResult result)                                                                                        
	{                                                                                                                              
		FbDebug.Log("MyPictureCallback");                                                                                          
		
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			FbDebug.Error(result.Error);                                                                                           
			// Let's just try again                                                                                                
			FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, MyPictureCallback);                                
			return;                                                                                                                
		}                                                                                                                          
		GameStateManager.UserTexture = result.Texture;                                                                             
	}    

	//Display facebook button or not
	void OnGUI()
	{

		if (EnableFacebook) {
						if (!FB.IsLoggedIn) {
								_FacebookButton.GetComponent<Renderer>().enabled = true;
						} else {
								_FacebookButton.GetComponent<Renderer>().enabled = false;
						}
						
				}
		}

	//This method is called after the init 
	void Start () {

		PlayerPrefs.SetInt("MoreTime", 0);
		PlayerPrefs.SetInt("Luck", 0);
		PlayerPrefs.SetInt("Luckx2", 0);

		AnimateLogo ();

		(_BestScore.GetComponent (typeof(TextMesh))as TextMesh).text = "" + PlayerPrefs.GetInt ("HighScore");
		(_BestLevel.GetComponent (typeof(TextMesh))as TextMesh).text = "" + PlayerPrefs.GetInt ("HighLevel");
	
	
	}




	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
			//Detecting if the player clicked on the left mouse button and also if there is no animation playing
		if (Input.GetButtonDown ("Fire1") ) {

			//The 3 following lines is to get the clicked GameObject and getting the RaycastHit2D that will help us know the clicked object
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.transform!=null )
			{  if(( hit.transform.gameObject.name ==_FacebookButton.name )  ){  
					Util.ButtonPressAnimation(hit.transform.gameObject);
					  	FB.Login("email,publish_actions", LoginCallback);                                                                                                                                                                 
					  
				}
				else if(( hit.transform.gameObject.name ==_PlayButton.name )  ){GetComponent<AudioSource>().PlayOneShot(MenuSound);Util.ButtonPressAnimation(hit.transform.gameObject); Application.LoadLevel(_NextScene);Time.timeScale=1; }
				else if(( hit.transform.gameObject.name ==_PurchaseButton.name )  ){GetComponent<AudioSource>().PlayOneShot(MenuSound);Util.ButtonPressAnimation(hit.transform.gameObject);TransitToPowerMenu(); }
				else if(( hit.transform.gameObject.name ==_TransitionPlayButton.name )  ){GetComponent<AudioSource>().PlayOneShot(MenuSound); Util.ButtonPressAnimation(hit.transform.gameObject);Application.LoadLevel(_NextScene);Time.timeScale=1; }
				else if(( hit.transform.gameObject.name ==_TransitionReturnButton.name )  ){GetComponent<AudioSource>().PlayOneShot(MenuSound);Util.ButtonPressAnimation(hit.transform.gameObject);TransitMainMenu(); }
				else if(( hit.transform.gameObject.name ==_AddCoinsButton.name )  ){GetComponent<AudioSource>().PlayOneShot(MenuSound); Util.ButtonPressAnimation(hit.transform.gameObject);TransitToStoreMenu(); }
				else if(( hit.transform.gameObject.name ==_StoreReturnButton.name )  ){GetComponent<AudioSource>().PlayOneShot(MenuSound); Util.ButtonPressAnimation(hit.transform.gameObject);TransitToPowerMenu(); }
				else if(( hit.transform.gameObject.name ==_50CoinsButton.name )  ){GetComponent<AudioSource>().PlayOneShot(MenuSound);Util.ButtonPressAnimation(hit.transform.gameObject);IncreaseGoldValue(50); }
				else if(( hit.transform.gameObject.name ==_100CoinsButton.name )  ){GetComponent<AudioSource>().PlayOneShot(MenuSound); Util.ButtonPressAnimation(hit.transform.gameObject);IncreaseGoldValue(100); }
				else if(( hit.transform.gameObject.name ==_200CoinsButton.name )  ){GetComponent<AudioSource>().PlayOneShot(MenuSound); Util.ButtonPressAnimation(hit.transform.gameObject);IncreaseGoldValue(200); }
				else if(( hit.transform.gameObject.name ==_300CoinsButton.name )  ){GetComponent<AudioSource>().PlayOneShot(MenuSound); Util.ButtonPressAnimation(hit.transform.gameObject);IncreaseGoldValue(300); }
				else if(( hit.transform.gameObject.name ==_MoreTimeButton.name ) && DecreaseGoldValue(10) ){GetComponent<AudioSource>().PlayOneShot(MenuSound); Util.ButtonPressAnimation(hit.transform.gameObject);if(! _IsMoreTime  ){ _IsMoreTime = true ;PlayerPrefs.SetInt("MoreTime", 1);  DisableButton(hit.transform.gameObject);DisableButton(_MoreTimeButtonText);}}
				else if(( hit.transform.gameObject.name ==_LuckButton.name)&& DecreaseGoldValue(10)){GetComponent<AudioSource>().PlayOneShot(MenuSound); Util.ButtonPressAnimation(hit.transform.gameObject);if(! _IsLuck  ){_IsLuck = true ;PlayerPrefs.SetInt("Luck", 1);DisableButton(hit.transform.gameObject);DisableButton(_LuckButtonText);}}
				else if(( hit.transform.gameObject.name ==_Luckx2Button.name )&& DecreaseGoldValue(10)  ){GetComponent<AudioSource>().PlayOneShot(MenuSound); Util.ButtonPressAnimation(hit.transform.gameObject);if(! _IsLuckx2 ){_IsLuckx2 = true ;PlayerPrefs.SetInt("Luckx2", 1);DisableButton(hit.transform.gameObject);DisableButton(_Luckx2ButtonText);} }
			

			}}
}

	
	// Transition animation to power panel
	void TransitToPowerMenu() {
		TweenParms parms = new TweenParms().Prop("position", new Vector3(-SpaceBetweenPanels,_MainMenuPanel.transform.position.y,_MainMenuPanel.transform.position.z)).Ease(AnimationTypeOfPanels);
		HOTween.To (_MainMenuPanel.transform, AnimationDurationOfPanels, parms);
		parms = new TweenParms().Prop("position", new Vector3(0,_PowerPanel.transform.position.y,_PowerPanel.transform.position.z)).Ease(AnimationTypeOfPanels);
		HOTween.To(_PowerPanel.transform, AnimationDurationOfPanels, parms);
		parms = new TweenParms().Prop("position", new Vector3(SpaceBetweenPanels,_StorePanel.transform.position.y,_StorePanel.transform.position.z)).Ease(AnimationTypeOfPanels);
		HOTween.To(_StorePanel.transform, AnimationDurationOfPanels, parms);
	}
	// Transition animation to mainmenu panel
	void TransitMainMenu() {
		TweenParms parms = new TweenParms().Prop("position", new Vector3(SpaceBetweenPanels,_PowerPanel.transform.position.y,_PowerPanel.transform.position.z)).Ease(AnimationTypeOfPanels);
		HOTween.To(_PowerPanel.transform, AnimationDurationOfPanels, parms);
		parms = new TweenParms().Prop("position", new Vector3(0,_MainMenuPanel.transform.position.y,_MainMenuPanel.transform.position.z)).Ease(AnimationTypeOfPanels);
		HOTween.To(_MainMenuPanel.transform, AnimationDurationOfPanels, parms);
		parms = new TweenParms().Prop("position", new Vector3(SpaceBetweenPanels*2,_StorePanel.transform.position.y,_StorePanel.transform.position.z)).Ease(AnimationTypeOfPanels);
		HOTween.To(_StorePanel.transform, AnimationDurationOfPanels, parms);
	}
	// Transition animation to store panel
	void TransitToStoreMenu() {
		TweenParms parms = new TweenParms().Prop("position", new Vector3(-SpaceBetweenPanels*2,_StorePanel.transform.position.y,_StorePanel.transform.position.z)).Ease(AnimationTypeOfPanels);
		HOTween.To(_StorePanel.transform, AnimationDurationOfPanels, parms);
		parms = new TweenParms().Prop("position", new Vector3(-SpaceBetweenPanels,_PowerPanel.transform.position.y,_PowerPanel.transform.position.z)).Ease(AnimationTypeOfPanels);
		HOTween.To(_PowerPanel.transform, AnimationDurationOfPanels, parms);
		parms = new TweenParms().Prop("position", new Vector3(0,_StorePanel.transform.position.y,_StorePanel.transform.position.z)).Ease(AnimationTypeOfPanels);
		HOTween.To(_StorePanel.transform, AnimationDurationOfPanels, parms);

	}

	// Disable and disappear a button
	void DisableButton(GameObject go){
		Color oldColor = go.GetComponent<Renderer>().material.color;
			TweenParms parms = new TweenParms().Prop("color", new Color(oldColor.r, oldColor.b, oldColor.g, 0f)).Ease(EaseType.EaseOutQuart);
		HOTween.To(go.GetComponent<Renderer>().material, 1f,parms);

	}

	// Get the gold value to get displayed
	void GetGoldValues()
	{    if (	PlayerPrefs.GetInt ("NotFirstLaunch") !=1 ){
			PlayerPrefs.SetInt("Gold", 100);
			PlayerPrefs.SetInt("NotFirstLaunch", 1);
		}
		(_GoldValue.GetComponent(typeof( TextMesh))as TextMesh).text = PlayerPrefs.GetInt("Gold").ToString();
		(_GoldValue2.GetComponent(typeof( TextMesh))as TextMesh).text =PlayerPrefs.GetInt("Gold").ToString();
	}

	// Add an amount to the stored gold value 
	void IncreaseGoldValue(int val)
	{   
			PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold")+ val);
		GetGoldValues ();
	}

	// Decrease an amount to the stored gold value 
	bool DecreaseGoldValue(int val)
	{
			int GoldValue =  PlayerPrefs.GetInt ("Gold") - val;


		if (GoldValue < 0) {
						return false;
				}
		else {
			PlayerPrefs.SetInt ("Gold", GoldValue);
			GetGoldValues ();
			return true;
				}
	}

	// Animation of the logo 
	void AnimateLogo ()
	{   	
		Sequence	mySequence = new Sequence(new SequenceParms().Loops(-1));
		TweenParms parms;

		Color oldColor = _Logo.GetComponent<Renderer>().material.color;
			parms = new TweenParms().Prop("color", new Color(oldColor.r, oldColor.b, oldColor.g, 0.4f)).Ease(EaseType.EaseInQuart);

		parms = new TweenParms().Prop("localScale", new Vector3(1.1f,1.1f,-2)).Ease(EaseType.EaseOutElastic);
		mySequence.Append(HOTween.To(_Logo.transform, 6f, parms));


		parms = new TweenParms().Prop("localScale", new Vector3(0.9f,0.9f,-2)).Ease(EaseType.EaseOutElastic);
		mySequence.Append(HOTween.To(_Logo.transform, 5f, parms));

		mySequence.Play ();
	}

}