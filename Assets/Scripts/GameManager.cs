﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public class Player
	{
		public int Id;
		public Color Color;
		public string ColorName;
		public GameObject Cannon;
		public bool destroyed;
	}

	private const int NUM_PLAYERS = 2;

	public GameObject CannonPrefab;
	public Renderer Ground;

	private List<Player> players = new List<Player>();
	private int currentPlayerIndex;

	// Use this for initialization
	void Start()
	{
		initGame();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OnTurnChange()
	{
		currentPlayerIndex = (currentPlayerIndex + 1) % NUM_PLAYERS;
		Debug.Log("changing turn, next: " + currentPlayerIndex);
	}

	public void OnCannonDestroyed(GameObject cannon)
	{
		foreach (Player p in players)
		{
			if (p.Cannon == cannon)
			{
				p.destroyed = true;
				break;
			}
		}

		// Check game over condition
		int numDestroyedPlayers = 0;
		Player undestroyedPlayer = null;
		foreach (Player p in players)
		{
			if (p.destroyed) numDestroyedPlayers++;
			else undestroyedPlayer = p;
		}

		if (numDestroyedPlayers == NUM_PLAYERS)
		{
			// draw
		}
		else if (numDestroyedPlayers == NUM_PLAYERS - 1)
		{
			// someone won
		}
	}

	private void initGame()
	{
		List<Color> colors = new List<Color> { Color.red, Color.blue, Color.green, Color.yellow };
		List<string> colorNames = new List<string> { "red", "blue", "green", "yellow" };

		var bounds = Ground.bounds;
		for (int i = 0; i < NUM_PLAYERS; i++)
		{
			// TODO better placement randomization
			float x = Random.Range(bounds.min.x, bounds.max.x);
			float z = Random.Range(bounds.min.z, bounds.max.z);
			float dir = Random.Range(0, 360);

			Player player = new Player();
			player.Id = i;
			player.Cannon = (GameObject)Instantiate(CannonPrefab, new Vector3(x, 0, z), Quaternion.Euler(0, dir, 0));

			int colorIndex = Random.Range(0, colors.Count);
			player.Color = colors[colorIndex];
			player.ColorName = colorNames[colorIndex];

			colors.RemoveAt(colorIndex);
			colorNames.RemoveAt(colorIndex);

			players.Add(player);

			Debug.Log("created player [" + player.Id + ", " + player.ColorName + "] cannon at " + player.Cannon.transform.position.ToString("f4") + ", rot: " + player.Cannon.transform.eulerAngles.ToString("f4"));
		}

		currentPlayerIndex = Random.Range(0, NUM_PLAYERS);
		Debug.Log("player [" + players[currentPlayerIndex].Id + ", " + players[currentPlayerIndex].ColorName + "] starts");
	}

}