using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ImpulseVibrations;
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

    public CanvasGroup tutorialHand;
    public float touchTimer = 0;

    public UnityEngine.UI.Text ScoreText;
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
    public GameObject BoomEffect;
    public GameObject SharkHeadEat;

    public int CurrentCanScore = 0;
	bool forceTimeStop = false;

	public VibeMaster viber;
    // Start is called before the first frame update
    void Start()
    {
        Init();
		if(viber == null)
			viber = gameObject.AddComponent<VibeMaster>();


        //Screen.SetResolution(Screen.width / 4*3, Screen.height / 4 *3, true);
        

    }
    public void Init()
    {
        Application.targetFrameRate = 60;
        _SharkPlayer.Init();
        _LevelManager.Init();

    }
    public float isSlowmotion = 0;
    public void SetSlowMotion()
    {
        isSlowmotion = 0.5f;
    }
    float spawnTimer = 0;
    // Update is called once per frame
    void Update()
    {
        if (GameHandler.instance.GameOverScreen.activeSelf)
            return;

        if (GameHandler.instance._rewardBase.gameObject.activeSelf)
			touchTimer = 0;
        if(touchTimer>2)
        {
            tutorialHand.alpha = Mathf.Lerp(tutorialHand.alpha, 1, Time.deltaTime * 3);
        }
        if (Input.GetMouseButton(0))
        {
            touchTimer = 0;
            tutorialHand.alpha = 0;
        }
        else
            touchTimer += Time.deltaTime;


        if (Input.GetKeyUp(KeyCode.R))
        {
            GameHandler.instance._rewardBase.Init(10, null, true);
        }

		if(forceTimeStop){
			Time.timeScale = 0;
		}
		else{
			if(isSlowmotion<=0)
			{
				Time.timeScale = 1f;
			}else
			{
				Time.timeScale = 0.02f;
				isSlowmotion -= Time.unscaledDeltaTime;
			}
		}
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

        if (Input.GetKeyUp(KeyCode.H))
        {
            PunshText();
        }
        
    }
    public Text ComboAwesomeTextLabel;
    public void PunshText()
    {
        ComboAwesomeTextLabel.enabled = true;
        iTween.Stop(ComboAwesomeTextLabel.gameObject);
        int rand = UnityEngine.Random.Range(0, 100);
            if(rand <30)
            ComboAwesomeTextLabel.text = "Perfect!";
            else if (rand < 60)
                ComboAwesomeTextLabel.text = "Awesome!";
            else if (rand < 100)
                ComboAwesomeTextLabel.text = "Nice!";
		AddScore(1);
        iTween.PunchScale(ComboAwesomeTextLabel.gameObject, new Vector3(1, 1, 0) * 0.3f, 1.0f);

        StartCoroutine(DisableAfterTime(0.9f));
    }


    IEnumerator DisableAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ComboAwesomeTextLabel.enabled = false;
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
        //sharkScoreObj.SetActive(false);
        //sharkScoreObj.SetActive(true);
        StartCoroutine(ExecuteAfterTime(0.2f));
    }
   IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        CurrentScore++;
        text.text = CurrentScore.ToString();
  


        //bloodEffect.SetActive(false);
        //bloodEffect.SetActive(true);

        //yield return new WaitForSeconds(0.05f);
        text.transform.DOShakePosition(effectDuration, new Vector3(shakeStrength, 0, 0), shakeVibrato, shakeRandomness);
    }

    public float d;
    public float s;
    public int v;
    public float r;
    public void AddScore(int amount = 10)
    {

        CurrentCanScore += amount;
        ScoreText.text = CurrentCanScore.ToString();

        //ScoreText.transform.DOShakeScale(d, s, v, r);
		ScoreBounce bouncer = ScoreText.GetComponent<ScoreBounce>();
		if(bouncer != null){
			bouncer.AddBounce(amount/10 + 3);
		}
    }

	public void ForceTimeStop(bool toStop){
		forceTimeStop = toStop;
	}

	public void AddForce(float force, float duration){
        //if(viber != null)
        //	viber.AddVibe(force, duration);
#if UNITY_IPHONE
        if (force>0.6)
            Vibrator.iOSVibrate(ImpactTypeFeedback.IMPACT_HEAVY);
        else if (force > 0.3f)
            Vibrator.iOSVibrate(ImpactTypeFeedback.IMPACT_MEDIUM);
        else if (force >= 0.0)
            Vibrator.iOSVibrate(ImpactTypeFeedback.IMPACT_MEDIUM);
#endif

#if UNITY_ANDROID
        Vibrator.AndroidVibrate(HapticFeedbackConstants.CONFIRM);
        //Vibrator.AndroidVibrate(Convert.ToInt64(duration/2), Convert.ToInt32(force));
#endif






    }
    public GameObject GameOverScreen;
    public void PressRetry()
    {
        _LevelManager.ResetLevel();

    }

}
