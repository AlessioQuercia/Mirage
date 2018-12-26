using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

	public static GameController instance;

	private PlayerControl32 player;
	
    private GameObject gameOverText;
    private GameObject leftDoor;
    private GameObject rightDoor;
	
    public bool gameOver;
	public bool load;

	public bool opening;
	public bool closing;

	private int sceneToLoad;

	private void Awake()
	{
//		if (instance == null)
//			instance = this;
//		else if (instance != this)
//			Destroy(gameObject);
		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (closing)
		{
			if (player.transform.position.y <= 10)
			{
				player.transform.position = new Vector3(player.transform.position.x,
					player.transform.position.y + 0.3f,
					player.transform.position.z);
			}
			
			if (leftDoor.gameObject.transform.position.x < Camera.main.gameObject.transform.position.x - 20)
			{
				leftDoor.gameObject.transform.position = new Vector3(leftDoor.gameObject.transform.position.x + 0.2f,
					leftDoor.gameObject.transform.position.y, -5);
			}
			else
			{
				leftDoor.GetComponent<StageDoor>().doorClosed = true;
//				print("left: " + leftDoorDelta);
//				print("leftClosed: " + (Camera.main.transform.position.x - leftDoor.transform.position.x));
			}

			if (rightDoor.gameObject.transform.position.x > Camera.main.gameObject.transform.position.x - 12.1f)
			{
				rightDoor.gameObject.transform.position = new Vector3(rightDoor.gameObject.transform.position.x - 0.2f,
					rightDoor.gameObject.transform.position.y, -5);
			}
			else
			{
				rightDoor.GetComponent<StageDoor>().doorClosed = true;
//				print("right: " + rightDoorDelta);
//				print("rightClosed: " + (Camera.main.transform.position.x - rightDoor.transform.position.x));
			}
			
			if (leftDoor.GetComponent<StageDoor>().doorClosed && rightDoor.GetComponent<StageDoor>().doorClosed)
			{
				if (gameOver)
				{
					gameOverText.SetActive(true);
					if (Input.GetKeyDown(KeyCode.Space))
					{
						gameOver = false;
						closing = false;
						ReloadScene();
					}
				}
				else if (load)
				{
					SceneManager.LoadScene(sceneToLoad);
					Save();
					load = false;
					closing = false;
					OpenDoors();
				}
			}
		}
		
		else if (opening)
		{ 
			if (player.GetComponent<PlayerControl32>().jumping)
			{
				player.transform.position = new Vector3(player.transform.position.x,
														player.transform.position.y - 0.3f,
														player.transform.position.z);
			}
			
			if (leftDoor.transform.position.x > Camera.main.gameObject.transform.position.x - 40)
			{
				leftDoor.transform.position = new Vector3(leftDoor.gameObject.transform.position.x - 0.2f,
					leftDoor.gameObject.transform.position.y, -5);
			}
			else
			{
				leftDoor.GetComponent<StageDoor>().doorClosed = false;
				leftDoor.SetActive(false);
			}
			
			if (rightDoor.transform.position.x < Camera.main.gameObject.transform.position.x + 8)
			{
				rightDoor.transform.position = new Vector3(rightDoor.transform.position.x + 0.2f,
					rightDoor.transform.position.y, -5);
			}
			else
			{
				rightDoor.GetComponent<StageDoor>().doorClosed = false;
				rightDoor.SetActive(false);
			}

			if (!leftDoor.GetComponent<StageDoor>().doorClosed && !rightDoor.GetComponent<StageDoor>().doorClosed)
			{
				opening = false;
				load = false;
				player.disabledMovements = false;
			}
		}
		
	}
	
	public void GameOver()
	{	
		CloseDoors();
	    gameOver = true;
	}

	public void LoadScene()
	{
		Camera.main.transform.position = new Vector3(player.transform.position.x, 
			player.transform.position.y + 2.7f, Camera.main.transform.position.z);
		leftDoor.gameObject.transform.position = new Vector3(Camera.main.gameObject.transform.position.x - 19.5f,
			Camera.main.gameObject.transform.position.y - 6.9f, -5);
		rightDoor.gameObject.transform.position = new Vector3(Camera.main.gameObject.transform.position.x - 12.5f,
			Camera.main.gameObject.transform.position.y - 6.9f, -5);
		gameOverText.SetActive(false);
		leftDoor.gameObject.SetActive(true);
		rightDoor.gameObject.SetActive(true);
		leftDoor.GetComponent<StageDoor>().doorClosed = true;
		rightDoor.GetComponent<StageDoor>().doorClosed = true;

		gameOver = false;
		load = true;
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		OpenDoors();
	}

	public void LoadScene(String sceneName)
	{
		CloseDoors();
		load = true;
		sceneToLoad = SceneManager.GetSceneByName(sceneName).buildIndex;
	}
	
	public void LoadScene(int sceneBuildIndex)
	{
		CloseDoors();
		load = true;
		sceneToLoad = sceneBuildIndex;
	}
	
	public void LoadNextScene()
	{
		CloseDoors();
		load = true;
		sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
	}

	public void OpenDoors()
	{
		if (player == null)
			player = FindObjectOfType<PlayerControl32>();
		
		if (gameOverText == null)
			gameOverText = Instantiate(Resources.Load("Prefabs/GameOverText")) as GameObject;
		
		if (leftDoor == null)
			leftDoor = Instantiate(Resources.Load("Prefabs/LeftStageDoor")) as GameObject;
		
		if (rightDoor == null)
			rightDoor = Instantiate(Resources.Load("Prefabs/RightStageDoor")) as GameObject;
		
		
		Camera.main.transform.position = new Vector3(player.transform.position.x, 
			Camera.main.transform.position.y, Camera.main.transform.position.z);
		leftDoor.transform.position = new Vector3(Camera.main.gameObject.transform.position.x - 20f,
			Camera.main.gameObject.transform.position.y - 6.9f, -5);
		rightDoor.transform.position = new Vector3(Camera.main.gameObject.transform.position.x - 12.2f,
			Camera.main.gameObject.transform.position.y - 6.9f, -5);
		gameOverText.SetActive(false);
		leftDoor.SetActive(true);
		rightDoor.SetActive(true);
		leftDoor.GetComponent<StageDoor>().doorClosed = true;
		rightDoor.GetComponent<StageDoor>().doorClosed = true;
		
		player.disabledMovements = true;
		player.transform.position = new Vector3(player.transform.position.x,
												player.transform.position.y + 10,
												player.transform.position.z);

		opening = true;
	}

	private void CloseDoors()
	{
		if (player == null)
			player = FindObjectOfType<PlayerControl32>();
		
		if (gameOverText == null)
			gameOverText = Instantiate(Resources.Load("Prefabs/GameOverText")) as GameObject;
		
		if (leftDoor == null)
			leftDoor = Instantiate(Resources.Load("Prefabs/LeftStageDoor")) as GameObject;
		
		if (rightDoor == null)
			rightDoor = Instantiate(Resources.Load("Prefabs/RightStageDoor")) as GameObject;
		
		leftDoor.transform.position = new Vector3(Camera.main.gameObject.transform.position.x - 40,
			Camera.main.gameObject.transform.position.y - 6.9f, -5);
		rightDoor.transform.position = new Vector3(Camera.main.gameObject.transform.position.x + 8,
			Camera.main.gameObject.transform.position.y - 6.9f, -5);
		gameOverText.transform.position = new Vector3(Camera.main.gameObject.transform.position.x - 13.5f, 
			Camera.main.gameObject.transform.position.y - 6, -6);
		leftDoor.SetActive(true);
		rightDoor.SetActive(true);

		player.disabledMovements = true;
		player.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0 , 0);
		
		closing = true;
	}


	public void Save()
	{
		if (!Directory.Exists(Application.dataPath + "/saves"))
		{
			Directory.CreateDirectory(Application.dataPath + "/saves");
		}

		if (!File.Exists(Application.dataPath + "/saves/autosave"))
		{
			File.Create(Application.dataPath + "/saves/autosave");
		}

		String toWrite = "CurrentScene = " + SceneManager.GetActiveScene().buildIndex;
		
		File.WriteAllText(Application.dataPath + "/saves/autosave", toWrite);
	}

	public void Load()
	{
		if (File.Exists(Application.dataPath + "/saves/autosave"))
		{
			String toLoad = File.ReadAllText(Application.dataPath + "/saves/autosave");

			sceneToLoad = Int32.Parse(toLoad.Split('=')[1].Replace(" ", ""));

			SceneManager.LoadScene(sceneToLoad);
			
			OpenDoors();
		}
	}
}