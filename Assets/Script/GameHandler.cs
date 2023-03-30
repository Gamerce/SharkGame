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
    public GameObject BossPrefab;
    public GameObject hitEffect;

    public TMPro.TMP_Text ScoreText;
    public GameObject ScoreCan;

    public GameObject sharkScoreObj;
    public TMPro.TMP_Text text;
    public GameObject bloodEffect;
    float CurrentScore = 0;

    public List<NPC> AllNPC= new List<NPC>();

    public List<NpcSpawner> AllNpcSpawners = new List<NpcSpawner>();
    public LevelManager _LevelManager;
    public SharkPlayer _SharkPlayer;
    public RewardBase _rewardBase;
    public bool GameHasEnded = false;
    public GameObject CheesePefab;
    public List<Transform> CheeseSpawnPoints;
    public GameObject WinEffect;

    public int CurrentCanScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        Init();


    }
    public void Init()
    {
        _SharkPlayer.Init();
        _LevelManager.Init();

    }
    float spawnTimer = 0;
    // Update is called once per frame
    void Update()
    {

        //spawnTimer -= Time.deltaTime;
        //if(spawnTimer<0)
        //{
        //    SpawnCheese();
        //    spawnTimer = Random.Range(5, 10);
        //}


        ScoreCan.transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * 100, Space.World);


        if( Input.GetKeyUp(KeyCode.D) )
        {
            AddScore();
        }
    }

    public float effectDuration = 0.3f;
    public float shakeStrength = 10f;
    public int shakeVibrato = 20;
    public float shakeRandomness = 0;

    //public void SpawnCheese()
    //{
    //    int randIndex = Random.Range(0, CheeseSpawnPoints.Count);
    //    Vector3 randPos = CheeseSpawnPoints[randIndex].position;
    //    GameObject go= GameObject.Instantiate(CheesePefab, randPos,Quaternion.identity);
    //    go.transform.parent = _SharkPlayer.transform;
    //    if(randIndex == 0 || randIndex == 1)
    //        go.transform.position = randPos + Camera.main.transform.right * 2;
    //    else
    //        go.transform.position = randPos - Camera.main.transform.right * 2;

    //    go.GetComponent<Pickup>().TargetTransform = CheeseSpawnPoints[randIndex].gameObject;

    //    go.transform.DOMove(randPos, 0.5f);
    //}
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

    public float d;
    public float s;
    public int v;
    public float r;
    public void AddScore()
    {

        CurrentCanScore += 1;
        ScoreText.text = CurrentCanScore.ToString();

        ScoreText.transform.DOShakeScale(d, s, v, r);
    }
}
