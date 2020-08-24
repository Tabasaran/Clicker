using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.MiniJSON;
using Holoville.HOTween;

/// <summary>
///  This class contains all static method used in different part of the project or that may be usefull for you
/// Author : Pondomaniac Games
/// </summary>
public class Util : ScriptableObject
{
	//Getting the picture using the facebook GRAPH api
    public static string GetPictureURL(string facebookID, int? width = null, int? height = null, string type = null)
    {
        string url = string.Format("/{0}/picture", facebookID);
        string query = width != null ? "&width=" + width.ToString() : "";
        query += height != null ? "&height=" + height.ToString() : "";
        query += type != null ? "&type=" + type : "";
        if (query != "") url += ("?g" + query);
        return url;
    }

	//Getting the picture texture
    public static void FriendPictureCallback(FBResult result)
    {
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            return;
        }

        GameStateManager.FriendTexture = result.Texture;
    }

	//Getting a random friend list
    public static Dictionary<string, string> RandomFriend(List<object> friends)
    {
        var fd = ((Dictionary<string, object>)(friends[Random.Range(0, friends.Count - 1)]));
        var friend = new Dictionary<string, string>();
        friend["id"] = (string)fd["id"];
        friend["first_name"] = (string)fd["first_name"];
        return friend;
    }

	//Getting the profile infos
    public static Dictionary<string, string> DeserializeJSONProfile(string response)
    {
        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object nameH;
        var profile = new Dictionary<string, string>();
        if (responseObject.TryGetValue("first_name", out nameH))
        {
            profile["first_name"] = (string)nameH;
        }
        return profile;
    }
    
	//Getting the score
    public static List<object> DeserializeScores(string response) 
    {

        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object scoresh;
        var scores = new List<object>();
        if (responseObject.TryGetValue ("data", out scoresh)) 
        {
            scores = (List<object>) scoresh;
        }

        return scores;
    }

	//Getting the friends
    public static List<object> DeserializeJSONFriends(string response)
    {
        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object friendsH;
        var friends = new List<object>();
        if (responseObject.TryGetValue("friends", out friendsH))
        {
            friends = (List<object>)(((Dictionary<string, object>)friendsH)["data"]);
        }
        return friends;
    }
    
	//Draw the texture picture  
    public static void DrawActualSizeTexture (Vector2 pos, Texture texture, float scale = 1.0f)
    {
        Rect rect = new Rect (pos.x, pos.y, texture.width * scale , texture.height * scale);
        GUI.DrawTexture(rect, texture);
    }
	//Draw a text
    public static void DrawSimpleText (Vector2 pos, GUIStyle style, string text)
    {
        Rect rect = new Rect (pos.x, pos.y, Screen.width, Screen.height);
        GUI.Label (rect, text, style);
    }
	//The animation when you press a button
	public static void ButtonPressAnimation(GameObject go) {
				Sequence mySequence = new Sequence (new SequenceParms ());
				TweenParms parms;
		
		
				parms = new TweenParms ().Prop ("localScale", new Vector3 (0.7f, 0.7f, go.transform.position.z)).Ease (EaseType.EaseInQuad);
				mySequence.Append (HOTween.To (go.transform, 0.12f, parms));
		
		
				parms = new TweenParms ().Prop ("localScale", new Vector3 (1f, 1f, go.transform.position.z)).Ease (EaseType.EaseInQuad);
				mySequence.Append (HOTween.To (go.transform, 0.12f, parms));


				mySequence.Play ();


				mySequence = new Sequence (new SequenceParms ());


	}


}
