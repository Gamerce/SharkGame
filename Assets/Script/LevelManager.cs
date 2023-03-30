using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<LevelData> Levels;
    public int CurrentLevel = 0;
    public TMPro.TMP_Text levelText;

    // Start is called before the first frame update
    public void Init()
    {
        CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        //CurrentLevel = 1;
        LoadLevel(CurrentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(int aLevel)
    {
        levelText.text = "Level " + aLevel;
        Levels[aLevel].gameObject.SetActive(true);
        SharkPlayer.instance.transform.position = Levels[aLevel].PlayerPosition;
        SharkPlayer.instance.transform.rotation = Quaternion.Euler ( Levels[aLevel].PlayerRotation);
        SharkPlayer.instance.wpPath.Clear();
        SharkPlayer.instance.wpPath = Levels[aLevel].wpPath;
    }
    IEnumerator EndAfterTime(float time)
    {


        yield return new WaitForSeconds(0.95f);
        GameHandler.instance.WinEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GameHandler.instance._rewardBase.Init(10, LoadNextScene, true);
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
        SceneManager.LoadScene(0);
    }


}
