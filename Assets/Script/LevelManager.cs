using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<LevelData> Levels;
    public int CurrentLevel = 0;
    public TMPro.TMP_Text levelText;

    public int ForceLevel = -1;

    public GameObject TrainGO;
    
    // Start is called before the first frame update
    public void Init()
    {

        CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        if (ForceLevel != -1)
        {
            CurrentLevel = ForceLevel;
        }
        //CurrentLevel = 1;
        LoadLevel(CurrentLevel);
		GameHandler.instance.fadeImage.enabled = true;
		UTween tweener = UTween.Fade(GameHandler.instance.fadeImage, new Vector2(1,0), 0.3f);
		tweener.onDone = ()=>{
			GameHandler.instance.fadeImage.enabled = false;
		};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(int aLevel)
    {
        int realLeave = aLevel;

        while(realLeave >= Levels.Count)
        {
            realLeave -= Levels.Count;
        }


        levelText.text = "Level " + aLevel;
        Levels[realLeave].gameObject.SetActive(true);

        SharkPlayer.instance.wpPath.Clear();
        SharkPlayer.instance.wpPath = Levels[realLeave].wpPath;

        if(Levels[realLeave].PlayerStartTransform != null)
        {
            SharkPlayer.instance.transform.position = Levels[realLeave].PlayerStartTransform.position;
            SharkPlayer.instance.transform.rotation = Levels[realLeave].PlayerStartTransform.rotation;

        }
        else
        {
            SharkPlayer.instance.transform.position = Levels[realLeave].PlayerPosition;
            SharkPlayer.instance.transform.rotation = Quaternion.Euler(Levels[realLeave].PlayerRotation);
        }

        for(int i = 0; i < Levels.Count;i++)
        {
            Levels[i].myWorld.SetActive(false);
        }

        Levels[realLeave].myWorld.SetActive(true);

        if (realLeave == 1)
        {
            TrainGO.GetComponent<Animator>().Play("trainIdle");
        }
    }
    IEnumerator EndAfterTime(float time)
    {
        int realLeave = CurrentLevel;
        while (realLeave >= Levels.Count)
        {
            realLeave -= Levels.Count;
        }
        if (realLeave == 1)
        {
            yield return new WaitForSeconds(0.2f);
            TrainGO.GetComponent<Animator>().Play("trainHit");
        }

        yield return new WaitForSeconds(0.95f);
        GameHandler.instance.WinEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
		GameHandler.instance.fadeImage.enabled = true;
		UTween.Fade(GameHandler.instance.fadeImage, new Vector2(0,1), 0.3f);
		yield return new WaitForSeconds(0.3f);
		GameHandler.instance.fadeImage.enabled = false;
		GameHandler.instance._rewardBase.Init((int)Random.Range(30,50) + GameHandler.instance.CurrentCanScore, LoadNextScene, true);
		Canvas parentCan = GameHandler.instance.tutorialHand.GetComponentInParent<Canvas>();
		parentCan.gameObject.SetActive(false);
    }

    public void GameFinished()
    {
        // Load new scene

        StartCoroutine(EndAfterTime(1f));

    }
    public void LoadNextScene()
    {
        CurrentLevel += 1;
        PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
        SceneManager.LoadScene(1);
    }
    public void ResetLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
        SceneManager.LoadScene(1);
    }

}
