using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using SimpleJSON;

public class RatingTable : MonoBehaviour
{
	[SerializeField] private string ratEndpoint = "https://guarded-shelf-97453.herokuapp.com/rline";
	public Transform ratingTable;
	public Transform ratingline;
	private List<RatinglineEntry> ratinglineEntryList;
	private List<Transform> ratinglineEntryTransformList;
	public ScoreScript ScoreScr;

	private void Start()
	{
		StartCoroutine(TryAddScore());
	}

	private IEnumerator TryAddScore()
	{
		string username = MainManager.Instance.PlayerName;
		int score = MainManager.Instance.PlayerScore;

		UnityWebRequest request = UnityWebRequest.Get($"{ratEndpoint}?rusername={username}&rscore={score}");
		var handler = request.SendWebRequest();

		float startTime = 0.0f;
		while (!handler.isDone)
		{
			startTime += Time.deltaTime;
			if (startTime > 10.0f)
			{
				break;
			}
			yield return null;
		}

		if (request.result == UnityWebRequest.Result.Success)
		{
			ratinglineEntryList = new List<RatinglineEntry>();
			var reqResult = request.downloadHandler.text;

			ParseRequest(reqResult);
			CreateTable();
		}
		else
		{
			Debug.Log(username);
			Debug.Log("Error connecting to the server...");
		}

		yield return null;
	}

	public void ParseRequest(string reqResult)
	{
		string[] entries = reqResult.Split('}');
		Array.Resize(ref entries, 10);

		for (int i = 0; i < 10; i++)
		{
			var l = entries[i].Length - 10;
			entries[i] = "{" + entries[i].Substring(2, l) + "}";

			ratinglineEntryList.Add(JsonUtility.FromJson<RatinglineEntry>(entries[i]));
		}
	}
	public void CreateTable()
	{
		ratingline.gameObject.SetActive(false);

		ratinglineEntryTransformList = new List<Transform>();
		foreach (RatinglineEntry ratinglineEntry in ratinglineEntryList)
		{
			CreateRatinglineEntryTransform(ratinglineEntry, ratingTable, ratinglineEntryTransformList);
		}
	}

	private void CreateRatinglineEntryTransform(RatinglineEntry ratinglineEntry, Transform container, List<Transform> transformList)
	{
		float templateheight = 30f;
		Transform entryTransform = Instantiate(ratingline, container);
		RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
		entryRectTransform.anchoredPosition = new Vector2(0, -templateheight * transformList.Count);
		entryTransform.gameObject.SetActive(true);

		string name = ratinglineEntry.username;
		entryTransform.Find("name").GetComponent<Text>().text = name;

		string score = ratinglineEntry.score.ToString();
		entryTransform.Find("score").GetComponent<Text>().text = score;

		transformList.Add(entryTransform);
	}

	private class RatinglineEntry
	{
		public string username;
		public int score;
	}
}
