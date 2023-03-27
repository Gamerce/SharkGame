using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public static GameHandler _instance=null;
    public static GameHandler instance   // property
    {
        get {
            if (_instance == null)
                _instance= GameObject.Find("GameHandler").GetComponent<GameHandler>();
            return _instance;
        }   // get method

    }


    public GameObject Shark;
    public GameObject NpcPrefab;

    public GameObject sharkScoreObj;
    public TMPro.TMP_Text text;
    public GameObject bloodEffect;
    float CurrentScore = 0;

    public List<NPC> AllNPC= new List<NPC>();
    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.S))
        {
            GiveScore();
        }
    }

    public float effectDuration = 0.3f;
    public float shakeStrength = 10f;
    public int shakeVibrato = 20;
    public float shakeRandomness = 0;


    public void GiveScore(float aTime = 0)
    {

        StartCoroutine(GiveScoreAfterTime(aTime));



    }
    IEnumerator GiveScoreAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        sharkScoreObj.SetActive(false);
        sharkScoreObj.SetActive(true);
        StartCoroutine(ExecuteAfterTime(0.2f));
    }
   IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        CurrentScore++;
        text.text = CurrentScore.ToString();
  


        bloodEffect.SetActive(false);
        bloodEffect.SetActive(true);

        //yield return new WaitForSeconds(0.05f);
        text.transform.DOShakePosition(effectDuration, new Vector3(shakeStrength, 0, 0), shakeVibrato, shakeRandomness);
    }
}
