using UnityEngine;
using System.Collections;
using System;
using Holoville.HOTween;

/// <summary>
///  This class is used to animate the splatch screen 
/// Author : Pondomaniac Games
/// </summary>
public class SplatchScreen : MonoBehaviour
{
		public GameObject _Logo;//The logo to animate
		public float _FadeInTime;//The fadeIn animation time
		public float _WaitingTime;//The stay time of the logo
		public float _FadeOutTime;//The fadeOut animation time
		public string _nextScene;//The next scene to load

		// Used before initialization
		void Awake ()
		{
				Time.timeScale = 1; 
		}
		                                                                                        
		// Used for initialization
		void Start ()
		{
				StartCoroutine (Init ());
		}

	    // Animate the Logos with fadeIn and fadeOut effect
		IEnumerator Init ()
		{    	
				Sequence mySequence = new Sequence (new SequenceParms ());
				TweenParms parms;
					
				Color oldColor = _Logo.GetComponent<Renderer>().material.color;

				_Logo.GetComponent<Renderer>().material.color = new Color (oldColor.r, oldColor.b, oldColor.g, 0f);

				parms = new TweenParms ().Prop ("color", new Color (oldColor.r, oldColor.b, oldColor.g, 1f)).Ease (EaseType.EaseInQuart);

				mySequence.Append (HOTween.To (_Logo.GetComponent<Renderer>().material, _FadeInTime, parms));
				mySequence.Append (HOTween.To (_Logo.GetComponent<Renderer>().material, _WaitingTime, parms));

				parms = new TweenParms ().Prop ("color", new Color (oldColor.r, oldColor.b, oldColor.g, 0f));

				mySequence.Append (HOTween.To (_Logo.GetComponent<Renderer>().material, _FadeOutTime, parms));

				mySequence.Play ();
		
				yield return new WaitForSeconds (_FadeInTime + _WaitingTime + _FadeOutTime);
				Application.LoadLevel (_nextScene); 
		
		
		}




	


}