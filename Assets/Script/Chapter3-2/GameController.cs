using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameController : MonoBehaviour
{

	public static GameController instance;
	
	public CinemachineVirtualCamera vcam;
	
	public PlayerController player;

    private GameObject gameOverText;
    private GameObject leftDoor;
    private GameObject rightDoor;
	
	[Header("Player Spawn")]
	
	public Transform spawnPoint; 

	public float spawningHeight = 10;
	public float despawningHeight = 10;
	public float despawningDeltaY = 0.3f;
	public float spawningDeltaY = 0.3f;

	private float doorDeltaX = 0.2f;
	private float doorDepth = -5;
	private float doorHeight = 6.9f;
	private float leftDoorOpenX = 40f;
	private float leftDoorClosedX = 20.1f;
	private float rightDoorOpenX = 8f;
	private float rightDoorClosedX = 12.1f;

	private float gameOverX = 13.5f;
	private float gameOverY = 6f;
	private float gameOverZ = -6f;

	private Transform originalCameraFollow;
	private bool originalCameraFollowSaved;
	
	
	[Header("Game Info")]
    public bool gameOver;
	public bool load;

	public bool opening;
	public bool closing;

	public int gameOverTime = 100;

	private int gameOverCurrentTime = 0;

	private int sceneToLoad;

	private bool sceneLoaded;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		
//		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start ()
	{
		sceneLoaded = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (sceneLoaded)
		{
			OpenDoors();
			sceneLoaded = false;
			Save();
		}

		if (player.animator.GetBool("enterLadderFromAbove"))
		{
			if (!originalCameraFollowSaved)
			{
				originalCameraFollow = vcam.m_Follow;
				originalCameraFollowSaved = true;
			}
			vcam.m_Follow = null;
			    vcam.transform.position = new Vector3(vcam.transform.position.x,
				vcam.transform.position.y - 0.015f,
				vcam.transform.position.z);
		}
		
		else if (player.animator.GetBool("exitLadderFromAbove"))
		{
			if (!originalCameraFollowSaved)
			{
				originalCameraFollow = vcam.m_Follow;
				originalCameraFollowSaved = true;
			}
			vcam.Follow = null;
			vcam.transform.position = new Vector3(vcam.transform.position.x,
				vcam.transform.position.y + 0.015f,
				vcam.transform.position.z);
		}
		else
		{
			if (originalCameraFollowSaved)
			{
				vcam.m_Follow = originalCameraFollow;
				originalCameraFollow = null;
				originalCameraFollowSaved = false;
			}
		}
		
		if (closing)
		{
			if (player.transform.position.y <= despawningHeight)
			{
				player.transform.position = new Vector3(player.transform.position.x,
					player.transform.position.y + despawningDeltaY,
					player.transform.position.z);
			}
			
			if (leftDoor.gameObject.transform.position.x < vcam.gameObject.transform.position.x - leftDoorClosedX)
			{
				leftDoor.gameObject.transform.position = new Vector3(leftDoor.gameObject.transform.position.x + doorDeltaX,
					leftDoor.gameObject.transform.position.y, doorDepth);
			}
			else
			{
				leftDoor.GetComponent<StageDoor>().doorClosed = true;
//				print("left: " + leftDoorDelta);
//				print("leftClosed: " + (Camera.main.transform.position.x - leftDoor.transform.position.x));
			}

			if (rightDoor.gameObject.transform.position.x > vcam.gameObject.transform.position.x - rightDoorClosedX)
			{
				rightDoor.gameObject.transform.position = new Vector3(rightDoor.gameObject.transform.position.x - doorDeltaX,
					rightDoor.gameObject.transform.position.y, doorDepth);
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
					gameOverCurrentTime++;
					if (gameOverCurrentTime >= gameOverTime)
					{
						gameOverCurrentTime = 0;
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
					sceneLoaded = true;
					closing = false;
				}
			}
		}
		
		else if (opening)
		{ 
			if (player.animator.GetBool("jumping"))
			{
				player.transform.position = new Vector3(player.transform.position.x,
														player.transform.position.y - spawningDeltaY,
														player.transform.position.z);
			}
			
			if (leftDoor.transform.position.x >= vcam.gameObject.transform.position.x - leftDoorOpenX)
			{
				leftDoor.transform.position = new Vector3(leftDoor.gameObject.transform.position.x - doorDeltaX,
					leftDoor.gameObject.transform.position.y, doorDepth);
			}
			else
			{
				leftDoor.GetComponent<StageDoor>().doorClosed = false;
				leftDoor.SetActive(false);
			}
			
			if (rightDoor.transform.position.x <= vcam.gameObject.transform.position.x + rightDoorOpenX)
			{
				rightDoor.transform.position = new Vector3(rightDoor.transform.position.x + doorDeltaX,
					rightDoor.transform.position.y, doorDepth);
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
				player.isInControl = true;
				vcam.Follow = player.transform;
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
//		Camera.main.transform.position = new Vector3(player.transform.position.x, 
//			player.transform.position.y + 2.7f, Camera.main.transform.position.z);
		leftDoor.gameObject.transform.position = new Vector3(vcam.gameObject.transform.position.x - leftDoorClosedX,
			vcam.gameObject.transform.position.y - doorHeight, doorDepth);
		rightDoor.gameObject.transform.position = new Vector3(vcam.gameObject.transform.position.x - rightDoorClosedX,
			vcam.gameObject.transform.position.y - doorHeight, doorDepth);
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
		
		if (SceneManager.GetActiveScene().name == "Chapter3-4")
			sceneToLoad = 0;
		else 
			sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
			
	}

	public void OpenDoors()
	{
		if (gameOverText == null)
			gameOverText = Instantiate(Resources.Load("Prefabs/GameOverText")) as GameObject;
		
		if (leftDoor == null)
			leftDoor = Instantiate(Resources.Load("Prefabs/LeftStageDoor")) as GameObject;
		
		if (rightDoor == null)
			rightDoor = Instantiate(Resources.Load("Prefabs/RightStageDoor")) as GameObject;


		player.isInControl = false;
		player.transform.position = new Vector3(spawnPoint.position.x,
			spawnPoint.position.y + spawningHeight,
			spawnPoint.position.z);
		
		vcam.transform.position = new Vector3(player.transform.position.x, 
			vcam.transform.position.y, vcam.transform.position.z);
		
		leftDoor.transform.position = new Vector3(vcam.gameObject.transform.position.x - leftDoorClosedX,
            vcam.gameObject.transform.position.y - doorHeight, doorDepth);
		rightDoor.transform.position = new Vector3(vcam.gameObject.transform.position.x - rightDoorClosedX,
            vcam.gameObject.transform.position.y - doorHeight, doorDepth);
			
		gameOverText.SetActive(false);
		leftDoor.SetActive(true);
		rightDoor.SetActive(true);
		leftDoor.GetComponent<StageDoor>().doorClosed = true;
		rightDoor.GetComponent<StageDoor>().doorClosed = true;
		
		opening = true;
	}

	private void CloseDoors()
	{
		if (gameOverText == null)
			gameOverText = Instantiate(Resources.Load("Prefabs/GameOverText")) as GameObject;
		
		if (leftDoor == null)
			leftDoor = Instantiate(Resources.Load("Prefabs/LeftStageDoor")) as GameObject;
		
		if (rightDoor == null)
			rightDoor = Instantiate(Resources.Load("Prefabs/RightStageDoor")) as GameObject;
		
		leftDoor.transform.position = new Vector3(Camera.main.gameObject.transform.position.x - leftDoorOpenX,
			Camera.main.gameObject.transform.position.y - doorHeight, doorDepth);
		rightDoor.transform.position = new Vector3(Camera.main.gameObject.transform.position.x + rightDoorOpenX,
			Camera.main.gameObject.transform.position.y - doorHeight, doorDepth);
		gameOverText.transform.position = new Vector3(Camera.main.gameObject.transform.position.x - gameOverX, 
			Camera.main.gameObject.transform.position.y - gameOverY, gameOverZ);
			
		leftDoor.SetActive(true);
		rightDoor.SetActive(true);

		player.isInControl = false;
		player.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0 , 0);

		vcam.Follow = null;
		
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