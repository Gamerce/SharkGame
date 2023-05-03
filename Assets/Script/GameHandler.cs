using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ImpulseVibrations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class GameHandler : MonoBehaviour
{
    public static GameHandler _instance=null;
    public static GameHandler instance   // property
    {
        get {
            if (_instance == null){
				GameObject handler = GameObject.Find("GameHandler");
				if(handler != null)
	                _instance = handler.GetComponent<GameHandler>();
			}
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

	public Image fadeImage;

    public int CurrentCanScore = 0;
	bool forceTimeStop = false;
	
	public int playerLives = 2;
	public Image damageTaken;

	public VibeMaster viber;
	public RateAppPopup rater;
    // Start is called before the first frame update
    void Start()
    {
        Init();
		if(viber == null)
			viber = gameObject.AddComponent<VibeMaster>();


        //Screen.SetResolution(Screen.width / 4*3, Screen.height / 4 *3, true);

        VoxelBusters.EssentialKit.NotificationServices.RequestPermission(NotificationPermissionOptions.Alert | NotificationPermissionOptions.Sound | NotificationPermissionOptions.Badge, callback: (result, error) =>
    {
        Debug.Log("Request for access finished.");
        Debug.Log("Notification access status: " + result.PermissionStatus);
    });


        SheduelNotifcations();
	}

    [Obsolete]
    public void SheduelNotifcations()
    {

        List<string> text = new List<string>();
        text.Add("Shark Puppet is hungry for cheese, come back and let’s smack some people around.");
        text.Add("Shark Puppet misses you :disappointed: please come back!");
        text.Add("Shark Puppet is Huuunnggggry, it's time to get some cheese.:cheese_wedge:");
        text.Add("I can hear Shark Puppets tummy grumbling.");
        text.Add("Time to collect a new hat :womans_hat:");
        text.Add("Are you ready to have fun and smack some people.");
        text.Add("Shark Puppet needs you.");
        text.Add(":cheese_wedge:Cheese is ready to be collected:cheese_wedge:");
        text.Add("Help Shark Puppet on the next mission.");
        text.Add("Shark Puppet is waiting for your help on a mission.");
        text.Add("Help Shark Puppet find some delicious cheese");
        text.Add("Time to get Shark Puppet to the next level.");
        text.Add("Cheese - loving Shark Puppet is waiting for you :clock1:.");
        text.Add("Your next outrageous adventure with Shark Puppet awaits.");
        text.Add("Shark Puppet is lonely without you :smiling_face_with_tear:");
		//#if UNITY_ANDROID
		for(int index = 0; index < text.Count; index++){
			text[index] = text[index].ReplaceAll(":cheese_wedge:", "\U0001F601");
			text[index] = text[index].ReplaceAll(":disappointed:", "\U0001F61E");
			text[index] = text[index].ReplaceAll(":womans_hat:", "\U0001F452");
			text[index] = text[index].ReplaceAll(":clock1:", "\U000023F3");
			text[index] = text[index].ReplaceAll(":smiling_face_with_tear:", "\U0001F972");
		}
		//#endif


        VoxelBusters.EssentialKit.NotificationServices.CancelAllScheduledNotifications();
		
		RequestNotificationAccess();

        SheduleNotification("24h", text[UnityEngine.Random.Range(0, text.Count - 1)], 60 * 60 * 24);
        SheduleNotification("7d", text[UnityEngine.Random.Range(0, text.Count - 1)], 60 * 60 * 24 * 7);
        SheduleNotification("14d", text[UnityEngine.Random.Range(0, text.Count - 1)], 60 * 60 * 24 * 14);
        SheduleNotification("28d", text[UnityEngine.Random.Range(0, text.Count - 1)], 60 * 60 * 24 * 28);


    }

	public void RequestNotificationAccess(){
		if(PlayerPrefs.GetInt("RequestNotifications", 0) == 0){
			VoxelBusters.EssentialKit.NotificationServices.RequestPermission(NotificationPermissionOptions.Provisional);
			PlayerPrefs.SetInt("RequestNotifications", 1);
		}
	}

    [Obsolete]
    public void SheduleNotification(string idN, string text, int timeSeconds)
    {

        INotification notification = NotificationBuilder.CreateNotification(idN)
           .SetTitle("Shark Puppet")
            .SetBody(text)
            .SetTimeIntervalNotificationTrigger(timeSeconds) //Setting the time interval to 10 seconds
            .Create();

        VoxelBusters.EssentialKit.NotificationServices.ScheduleNotification(notification, (error) =>
        {
            if (error == null)
            {
                Debug.Log("Request to schedule notification finished successfully.");
            }
            else
            {
                Debug.Log("Request to schedule notification failed with error. Error: " + error);
            }
        });
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
#if UNITY_IPHONE
        if (force>0.6)
            Vibrator.iOSVibrate(ImpactTypeFeedback.IMPACT_HEAVY);
        else if (force > 0.3f)
            Vibrator.iOSVibrate(ImpactTypeFeedback.IMPACT_MEDIUM);
        else if (force >= 0.0)
            Vibrator.iOSVibrate(ImpactTypeFeedback.IMPACT_MEDIUM);
#endif

#if UNITY_ANDROID
		if(viber != null)
			viber.AddVibe(force, duration);
		//Vibrator.AndroidVibrate(HapticFeedbackConstants.CONFIRM);
        //Vibrator.AndroidVibrate(Convert.ToInt64(duration/2), Convert.ToInt32(force));
#endif






    }
    public GameObject GameOverScreen;
    public void PressRetry()
    {
        _LevelManager.ResetLevel();

    }
	
	public void AttackPlayer(){
		playerLives--;
		if(playerLives <= 0){
			GameOverScreen.SetActive(true);
		}
		else{
			UTween.Fade(damageTaken, new Vector2(0,1), 0.3f, -1, new AnimationCurve(new Keyframe[]{new Keyframe(0,0), new Keyframe(0.2f, 1), new Keyframe(1,0)}));
		}
	}
}
