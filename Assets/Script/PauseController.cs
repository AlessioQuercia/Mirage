using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {
    public bool pauseActive, settingsActive;
    bool moveSettingsArrow, movePauseArrow;
    bool moveScrollSound, moveScrollBack;

    public GameObject pauseMenu, settingsMenu, backgroundMenu;
    public GameObject arrowPause, arrowSettings;
    public GameObject scrollSound, scrollBackground;
    public GameObject posSetting1, posSetting2, posSetting3;
    public GameObject posStart1, posStart2, posStart3, posStart4, posStart5;
    public GameObject minBack, maxBack;
    public GameObject minSound, maxSound;

    int posArrowPause, posArrowSettings;
    int posScrollSound, posScrollBack;

    public static PauseController instance;
    
    public float[] volumes = {0, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1};

    private int backgroundVolumeIndex;

    private int soundVolumeIndex;
    
	// Use this for initialization
	void Start ()
    {
        pauseActive = false;
        settingsActive = false;
        pauseMenu.SetActive(pauseActive);
        settingsMenu.SetActive(settingsActive);
        backgroundMenu.SetActive(false);

        movePauseArrow = false;
        moveSettingsArrow = false;
        moveScrollSound = false;
        moveScrollBack = false;

        posArrowPause = 0;
        posArrowSettings = 0;
        posScrollBack = 10;
        posScrollSound = 10;

        backgroundVolumeIndex = volumes.Length - 1;
        
        soundVolumeIndex = volumes.Length - 1;
    }



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Menu");
    }




    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!settingsActive)
            {
                if (!pauseActive)
                {
                    pauseActive = true;
                    movePauseArrow = true;
                }
                else
                {
                    pauseActive = false;
                }

                backgroundMenu.SetActive(pauseActive);
                pauseMenu.SetActive(pauseActive);
            }
        }
        
        if (pauseActive || settingsActive)
        {
            if (settingsActive)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    settingsActive = false;
                    pauseActive = true;
                    pauseMenu.SetActive(pauseActive);
                    settingsMenu.SetActive(settingsActive);
                    posArrowPause = 0;
                    movePauseArrow = true;
                    moveSettingsArrow = false;
                }
            }
            
           if(movePauseArrow || moveSettingsArrow)
           {
               if (movePauseArrow)
               {
                    if(posArrowPause == 0)
                    {
                        arrowPause.transform.position = posStart1.transform.position;
                    }
                    else if (posArrowPause == 1)
                    {
                        arrowPause.transform.position = posStart2.transform.position;
                    }
                    else if (posArrowPause == 2)
                    {
                        arrowPause.transform.position = posStart3.transform.position;
                    }
                    else if (posArrowPause == 3)
                    {
                        arrowPause.transform.position = posStart5.transform.position;
                    }
    
    
                    movePauseArrow = false;
               }
               else
               {
                    if (posArrowSettings == 0)
                    {
                        arrowSettings.transform.position = posSetting1.transform.position;
                    }
                    else if (posArrowSettings == 1)
                    {
                        arrowSettings.transform.position = posSetting2.transform.position;
                    }
                    else if (posArrowSettings == 2)
                    {
                        arrowSettings.transform.position = posSetting3.transform.position;
                    }
    
                    moveSettingsArrow = false;
                }
           }
            else if(moveScrollBack || moveScrollSound)
            {
                if (moveScrollSound)
                {
                    float pos = maxSound.transform.position.x - minSound.transform.position.x;
                    pos = pos * posScrollSound * 0.1f + minSound.transform.position.x;
    
                    scrollSound.transform.position = new Vector3 (pos, scrollSound.transform.position.y, scrollSound.transform.position.z);
    
                    moveScrollSound = false;
                }
                else
                {
                    float pos = maxBack.transform.position.x - minBack.transform.position.x;
                    pos = pos * posScrollBack * 0.1f + minBack.transform.position.x;
    
                    scrollBackground.transform.position = new Vector3(pos, scrollBackground.transform.position.y, scrollBackground.transform.position.z);
    
                    moveScrollBack = false;
                }
            }

        
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (settingsActive)
                {
                    posArrowSettings++;
    
                    if(posArrowSettings == 3)
                    {
                        posArrowSettings = 0;
                    }
                    
    
                    moveSettingsArrow = true;
                }
                else if (pauseActive)
                {
                    posArrowPause++;
    
                    if (posArrowPause == 4)
                    {
                        posArrowPause = 0;
                    }
                    
                    movePauseArrow = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (settingsActive)
                {
                    posArrowSettings--;
    
                    if (posArrowSettings == -1)
                    {
                        posArrowSettings = 2;
                    }
    
                    moveSettingsArrow = true;
                }
                else if (pauseActive)
                {
                    posArrowPause--;
    
                    if (posArrowPause == -1)
                    {
                        posArrowPause = 3;
                    }
    
                    movePauseArrow = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (settingsActive)
                {
                    if(posArrowSettings == 0)
                    {
                        if(posScrollBack != 0)
                        {
                            posScrollBack--;
                            
                            backgroundVolumeIndex--;

                            SoundManager.instance.musicSource.volume = volumes[backgroundVolumeIndex];
    
                            moveScrollBack = true;
                        }
                    }
                    else if (posArrowSettings == 1)
                    {
                        if(posScrollSound != 0)
                        {
                            posScrollSound--;
                            
                            soundVolumeIndex--;
                            
                            SoundManager.instance.efxSource.volume = volumes[soundVolumeIndex];
    
                            moveScrollSound = true;
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (settingsActive)
                {
                    if (posArrowSettings == 0)
                    {
                        if (posScrollBack < 10)
                        {
                            posScrollBack++;

                            backgroundVolumeIndex++;
                            
                            SoundManager.instance.musicSource.volume = volumes[backgroundVolumeIndex];
    
                            moveScrollBack = true;
                            
                        }
                    }
                    else if (posArrowSettings == 1)
                    {
                        if (posScrollSound < 10)
                        {
                            posScrollSound++;
                            
                            soundVolumeIndex++;
                            
                            SoundManager.instance.efxSource.volume = volumes[soundVolumeIndex];
    
                            moveScrollSound = true;
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!settingsActive)
                {
                    if(posArrowPause == 0)
                    {
                        pauseActive = false;
                        settingsActive = false;
                        backgroundMenu.SetActive(pauseActive);
                        pauseMenu.SetActive(pauseActive);
                    }
                    else if (posArrowPause == 1)
                    {
                        // Restart Scene
                        pauseActive = false;
                        backgroundMenu.SetActive(pauseActive);
                        pauseMenu.SetActive(pauseActive);
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }
                    else if (posArrowPause == 2)
                    {
                        settingsActive = true;
                        pauseActive = false;
                        pauseMenu.SetActive(pauseActive);
                        settingsMenu.SetActive(settingsActive);
                        posArrowSettings = 0;
                        moveSettingsArrow = true;
                        movePauseArrow = false;
                    }
    
                    else if (posArrowPause == 3)
                    {
                        posArrowPause = 0;
                        movePauseArrow = true;
                        pauseActive = false;
    
                        backgroundMenu.SetActive(pauseActive);
                        pauseMenu.SetActive(pauseActive);
                        
                        SceneManager.LoadScene("Title");
                    }
                }
                else
                {
                    if(posArrowSettings == 2)
                    {
                        settingsActive = false;
                        pauseActive = true;
                        pauseMenu.SetActive(pauseActive);
                        settingsMenu.SetActive(settingsActive);
                        posArrowPause = 0;
                        movePauseArrow = true;
                    }
                }
            }
        } 
    }
        
}

