using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBase : MonoBehaviour
{
	public Material fillMat;
	public Transform displayParent;
	public Vector2 minMaxVal;
	
	public List<TMPro.TextMeshProUGUI> percentText = new List<TMPro.TextMeshProUGUI>();
	public List<TMPro.TextMeshProUGUI> goldGainAmount = new List<TMPro.TextMeshProUGUI>();
	SmoothFloat toSetVal = new SmoothFloat(0);

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

	public HatData allhats;
	System.Action onDone;

	public List<GameObject> hatObjects = new List<GameObject>();
	public int currentHat = -1;

    // Start is called before the first frame update
    void Start()
    {
		groupCanvas.alpha = 0;
		continueButton.alpha = 0;
    }

	public void Init(int rewardAmount, System.Action onDone, bool fadeOutWhenDone = true){
		gameObject.SetActive(true);
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
				toSetVal.SetValue(allhats.allHats[currentHat].atValue);
				allhats.allHats[currentHat].atValue = allhats.allHats[currentHat].atValue + rewardAmount;
				toSetVal.value = allhats.allHats[currentHat].atValue;
				toSetVal.smoothSpeed = 50.0f;//100.0f;

				if(allhats.allHats[currentHat].IsUnlocked()){
					Debug.LogError("Hat has been unlocked do fancy stuff.");
				}

		
				UTween.Fade(groupCanvas, new Vector2(0,1), 0.75f);
				continueButton.gameObject.SetActive(false);
				continueButton.alpha = 0;
				//toSetVal.SetValue(0);
				//toSetVal.value = 25;
				//toSetVal.value = toSetVal.value+25;
				//if(toSetVal.GetTarget() > 100){
				//	toSetVal.SetValue(0);
				//	toSetVal.value = 25;
				//}
				fillMat.SetFloat("_FillAmount", minMaxVal.Evaluate(toSetVal.value/(float)(allhats.allHats[currentHat].unlockPointsNeeded)));
				percentText.SetText(((toSetVal.value/(float)(allhats.allHats[currentHat].unlockPointsNeeded))*100).ToString("0") + "%");
				UTween.Wait(gameObject, 1.3f, ()=>{
					updateText = true;
					UTween.Wait(gameObject, 0.5f, ()=>{
						continueButton.alpha = 0;
						continueButton.gameObject.SetActive(true);
						UTween.Fade(continueButton, new Vector2(0,1), 0.5f);
					});
					coinSpawner.StartSpawn(10);
				});
			};
		}
	}

	[ContextMenu("Reset Curve")]
	public void ResetCurve(){
		pulseCurve = Extensions.GetShakeCurve(6, 1, Extensions.GetFlatCurve(1));
	}

	public void EatCheeseDone(){
		groupCanvas.gameObject.SetActive(true);
		eatCheese.SetActive(false);
		displayParent.gameObject.SetActive(true);
	}

	private void OnEnable() {
		groupCanvas.alpha = 0;
		continueButton.alpha = 0;
		//sphereHelper.RecalcFull();
	}

	// Update is called once per frame
	void Update()
    {
		if(updateText){
			toSetVal.Update(Time.deltaTime);
			percentText[0].transform.localScale = Vector3.one * (1+pulseVelocity * pulseCurve.Evaluate(toSetVal.GetPercentage()));
			float percent = (toSetVal.value/(float)(allhats.allHats[currentHat].unlockPointsNeeded));
			percent = percent.Clamp01();
			percentText.SetText((percent*100).ToString("0") + "%");
			fillMat.SetFloat("_FillAmount", minMaxVal.Evaluate(percent));
			if(toSetVal.IsDone()){
				updateText = false;
			}
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
						System.Action temp = onDone;
						onDone = null;
						temp.Invoke();
					}
				};
			}
			else{
				allowLevelClick = true;
				if(onDone != null){
					System.Action temp = onDone;
					onDone = null;
					temp.Invoke();
				}
			}
		}
	}
}
