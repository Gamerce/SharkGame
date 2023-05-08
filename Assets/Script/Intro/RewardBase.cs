using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBase : MonoBehaviour
{
	public Material fillMat;
	public Transform displayParent;
	public Vector2 minMaxVal;
	
	public List<TMPro.TextMeshProUGUI> percentText = new List<TMPro.TextMeshProUGUI>();
	public UnityEngine.UI.Text percentTextUi;
	public TextValue totalCoinAmount;
	public BounceUp canBouncer;
	public List<TMPro.TextMeshProUGUI> goldGainAmount = new List<TMPro.TextMeshProUGUI>();
	public UnityEngine.UI.Text goldGainAmounttext;
	public List<TMPro.TextMeshProUGUI> goldTotalAmount = new List<TMPro.TextMeshProUGUI>();
	float fillValue = 0;

	[Range(0,1)]
	public float debugFillAmount = 0;
	public bool debugFill = false;
	[Range(0,0.1f)]
	public float pulseVelocity = 0.1f;
	public AnimationCurve pulseCurve = Extensions.GetShakeCurve(6, 1, Extensions.GetFlatCurve(1));
	bool updateText = false;
	bool fadeOutWhenDone = false;

	public BoundingSphereHelper sphereHelper;
	public CanvasGroup groupCanvas;
	public CanvasGroup continueButton;
	public CoinSpawner coinSpawner;
	public GameObject eatCheese;

	public UnityEngine.UI.Image fadeFrom;
	public HatData allhats;
	System.Action onDone;

	public List<GameObject> hatObjects = new List<GameObject>();
	public int currentHat = -1;

	#region HatSelector
	public SelectHat hatSelector;
	#endregion

	#region AdWheel
	public GameObject adWheelRoot;
	public AdWheel adWheel;
	#endregion

	// Start is called before the first frame update
	void Start()
    {
		//groupCanvas.alpha = 0;
		//continueButton.alpha = 0;
		totalCoinAmount.SetAmount(PlayerPrefs.GetInt("CoinAmount", 0));
    }

	public void Init(int rewardAmount, System.Action onDone, bool fadeOutWhenDone = true, bool blockAd = false){
		
		//GameObject[] roots = UnityEngine.SceneManagement.Scene.GetRootGameObjects();
		Camera[] cameras = Camera.allCameras;
		for(int index = 0; index < cameras.Length; index++){
			if(!cameras[index].transform.IsChildOf(transform)){
				cameras[index].enabled = false;
			}
		}
		PlayerPrefs.SetInt("CoinAmount", PlayerPrefs.GetInt("CoinAmount", 0) + rewardAmount);
		//totalCoinAmount.AddAmount(rewardAmount);
		hatSelector.camDisplayRoot.gameObject.SetActive(true);
		hatSelector.renderCam.gameObject.SetActive(true);
		gameObject.SetActive(true);

		goldGainAmount.SetText(rewardAmount.ToString());
		goldGainAmounttext.text = rewardAmount.ToString();

		this.onDone = onDone;
		this.fadeOutWhenDone = fadeOutWhenDone;
		for(int index = 0; index < hatObjects.Count; index++)
			hatObjects[index].SetActive(false);
		for(int index = 0; index < allhats.allHats.Count; index++){
			if(!allhats.allHats[index].IsUnlocked()){
				hatObjects[index].SetActive(true);
				minMaxVal = allhats.allHats[index].imageMinMax;
				currentHat = index;
				break;
			}
		}
		if(currentHat < 0)
			currentHat = 0;

		displayParent.gameObject.SetActive(false);
		//if(targetObj != null){
		//	targetObj.SetActive(true);
		//	targetObj.transform.SetParent(displayParent);
		//	targetObj.transform.localPosition = Vector3.zero;
		//	//targetObj.transform.localScale = Vector3.one;
		//}
		groupCanvas.gameObject.SetActive(false);
		eatCheese.SetActive(true);
		updateText = false;
		CheeseEater eater = eatCheese.GetComponent<CheeseEater>();
		if(eater != null){
	

			eater.onDone = ()=>{
				
				adWheel.Show(rewardAmount, (int amount)=>{
					enabled = true;
					goldGainAmount.SetText(amount.ToString());
					goldGainAmounttext.text = amount.ToString(); ;


					groupCanvas.gameObject.SetActive(true);
					groupCanvas.alpha = 1;
					displayParent.gameObject.SetActive(true);

					PlayerPrefs.SetInt("CoinAmount", PlayerPrefs.GetInt("CoinAmount", 0) - rewardAmount + amount);
					fillValue = allhats.allHats[currentHat].atValue;
					fillMat.SetFloat("_FillAmount", minMaxVal.Evaluate(fillValue / (float)(allhats.allHats[currentHat].unlockPointsNeeded)));
					percentText.SetText(((fillValue / (float)(allhats.allHats[currentHat].unlockPointsNeeded)) * 100).ToString("0") + "%");
					percentTextUi.text = percentText[0].text;

					continueButton.gameObject.SetActive(false);
					continueButton.alpha = 0;

					UTween.Wait(gameObject, 0.1f, () =>
					{
			
						UTween.Wait(gameObject, 0.5f, () => {

							UTween.Fade(continueButton, new Vector2(0, 1), 0.5f);
						});
						coinSpawner.StartSpawn(amount);

						StartCoroutine(ExecuteAfterTime(1.3f, amount));
					});
				});
			};
		}
		
		if(AdMaster.instance != null && !blockAd){
			AdMaster.instance.ShowInterstitial(()=>{
				if(MusicManager.instance != null)
					MusicManager.instance.PlayCheseMusic();
			}, ()=>{
				if(MusicManager.instance != null)
					MusicManager.instance.PlayCheseMusic();
			});
		}
	}

	IEnumerator ExecuteAfterTime(float time,int rewardAmount)
	{
		yield return new WaitForSeconds(time);
		continueButton.alpha = 0;
		continueButton.gameObject.SetActive(true);

		updateText = true;
		fillValue = allhats.allHats[currentHat].atValue;
		allhats.allHats[currentHat].atValue = allhats.allHats[currentHat].atValue + rewardAmount;
		//fillValue = allhats.allHats[currentHat].atValue;

		if (allhats.allHats[currentHat].IsUnlocked())
		{
			Debug.LogError("Hat has been unlocked do fancy stuff.");
		}
		

		fillMat.SetFloat("_FillAmount", minMaxVal.Evaluate(fillValue / (float)(allhats.allHats[currentHat].unlockPointsNeeded)));
		percentText.SetText(((fillValue / (float)(allhats.allHats[currentHat].unlockPointsNeeded)) * 100).ToString("0") + "%");
		percentTextUi.text = percentText[0].text;
	}
	public void AddACoin(int coinValue = 1){
		totalCoinAmount.AddNow(coinValue);
		canBouncer.AddBounce(1.5f);
		fillValue += coinValue;
		if(GameHandler.instance != null){
			GameHandler.instance.AddForce(0.1f, 0.1f);
		}
	}

	[ContextMenu("Reset Curve")]
	public void ResetCurve(){
		pulseCurve = Extensions.GetShakeCurve(6, 1, Extensions.GetFlatCurve(1));
	}

	public void EatCheeseDone(){
		eatCheese.SetActive(false);
		fadeFrom.color = Color.black;
		UTween.Fade(fadeFrom, new Vector2(1,0), 0.3f);
	}

	private void OnEnable() {
		//groupCanvas.alpha = 0;
		//continueButton.alpha = 0;
		//sphereHelper.RecalcFull();
	}

	// Update is called once per frame
	void Update()
    {
		if(updateText){
			float percent = (fillValue/(float)(allhats.allHats[currentHat].unlockPointsNeeded));
			percent = percent.Clamp01();
			percentText[0].transform.localScale = Vector3.one * (1+pulseVelocity * pulseCurve.Evaluate(percent));
			percentTextUi.transform.localScale = Vector3.one * (1 + pulseVelocity * pulseCurve.Evaluate(percent));
			percentText.SetText((percent*100).ToString("0") + "%");
			percentTextUi.text = percentText[0].text;
			fillMat.SetFloat("_FillAmount", minMaxVal.Evaluate(percent));
			//if(toSetVal.IsDone()){
			//	updateText = false;
			//}
		}
		if(debugFill){
			fillMat.SetFloat("_FillAmount", minMaxVal.Evaluate(debugFillAmount));
		}
    }

	[System.NonSerialized]
	bool allowLevelClick = true;
	public void NextLevelClicked(){
		if(allowLevelClick) {
			allowLevelClick = false;
			if(fadeOutWhenDone){
				UTween temp = UTween.Fade(groupCanvas, new Vector2(groupCanvas.alpha, 0), 0.3f);
				temp.onDone = ()=>{
					allowLevelClick = true;
					if(onDone != null){
						System.Action temp2 = onDone;
						onDone = null;
						temp2.Invoke();
					}
				};
			}
			else{
				allowLevelClick = true;
				if(onDone != null){
					System.Action temp2 = onDone;
					onDone = null;
					temp2.Invoke();
				}
			}
		}
	}

	public int UnlockRandom(){
		List<HatData.HatInfo> availables = new List<HatData.HatInfo>();
		for(int index = 0; index < allhats.allHats.Count; index++){
			if(!allhats.allHats[index].IsUnlocked()){
				availables.Add(allhats.allHats[index]);
			}
		}
		if(availables.Count == 1){
			availables[0].Unlock();
			return allhats.allHats.GetId(availables[0]);
		}
		else if(availables.Count > 1){
			int randID = Random.Range(0, availables.Count-1);
			availables[randID].Unlock();
			return allhats.allHats.GetId(availables[randID]);
		}
		Debug.LogError("Show case the randomly unlocked hat.");
		return -1;
	}

	#region HatSelector
	
	public void ShowHatSelector(System.Action callback){
		hatSelector.Init(callback);
	}
	public void ShowHatSelector(){
		hatSelector.Init(null);
	}

	#endregion

	#region AdWheel

	public void AdWheelComplete(int multiplier){

	}

	public void AdWheelSkipped(){
	}

	#endregion
}
