using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdWheel : MonoBehaviour
{
	public GameObject root;
	public RewardBase rBase;
	public Image fader;

	public List<float> sections = new List<float>();
	public Vector2 minMaxAngle;
	public RectTransform arrow;

	public List<int> sectionMultiplier = new List<int>();

	public int baseCoins;
	public List<TMPro.TextMeshProUGUI> coinAmount = new List<TMPro.TextMeshProUGUI>();

	public AnimationCurve curve = new AnimationCurve(new Keyframe[]{new Keyframe(0,0), new Keyframe(1,1)});

	System.Action<int> onDone = null;

	bool stopArrow = false;
	int lastValue = 0;
	float bouncePercent = 0.5f;
	bool goingUp = true;
    // Start is called before the first frame update
    void Start()
    {
        goingUp = Random.Range(0, 100) > 50;
    }

    // Update is called once per frame
    void Update()
    {
        int multiplier = GetMultiplier();
		int currentVal = multiplier * baseCoins;
		if(currentVal != lastValue){
			lastValue = currentVal;
			//coinAmount.SetText(lastValue.ToString());
			coinAmount.SetText((baseCoins*2).ToString() + "|" + (baseCoins*4));
		}
		if(!stopArrow){
			if(goingUp){
				bouncePercent += Time.deltaTime;
				if(bouncePercent >= 1){
					goingUp = false;
					bouncePercent = 1;
				}
			}
			else{
				bouncePercent -= Time.deltaTime;
				if(bouncePercent <= 0){
					goingUp = true;
					bouncePercent = 0;
				}
			}
		}
		arrow.localRotation = Quaternion.Euler(0,0,minMaxAngle.Lerp(curve.Evaluate(bouncePercent)));
    }

	public void Show(int amount, System.Action<int> onDone){
		AdMaster.instance.PrepareVideo();
		root.SetActive(true);
		this.onDone = onDone;
		baseCoins = amount;
		UTween.Fade(fader, new Vector2(1,0), 0.3f);
	}

	[ContextMenu("Wrapp All Angles")]
	public void WrappAllAngles(){
		for(int index = 0; index < sections.Count; index++){
			float angleVal = sections[index];
			if(angleVal > 180)
				angleVal -= 360;
			sections[index] = angleVal;
		}
	}

	[ContextMenu("Add Section")]
	public void AddSection(){
		float angleVal = arrow.localRotation.eulerAngles.z;
		if(angleVal > 180)
			angleVal -= 360;
		sections.Add(angleVal);
	}

	public int GetMultiplier(){
		float currentPos = arrow.localRotation.eulerAngles.z;
		if(currentPos > 180)
			currentPos -= 360;
		for(int index = 0; index < sections.Count-1; index++){
			if(currentPos <= sections[index] && currentPos > sections[index+1]){
				return sectionMultiplier[index];
			}
		}
		return 2;
	}

	public void ViewAdClicked(){
		if(enabled){
			enabled = false;
			stopArrow = true;
			//Debug.LogError("ViewAdClicked");
			AdMaster.instance.ShowRewardVideo(()=>{
				//Debug.LogError("Done showing reward video");
				UTween temp = UTween.Fade(fader, new Vector2(0,1), 0.3f);
				temp.onDone = ()=>{
					enabled = true;
					root.SetActive(false);
					if(onDone != null){
						onDone.Invoke(baseCoins * GetMultiplier());
						//rBase.AdWheelComplete(baseCoins * GetMultiplier());
					}
				};
			},()=>{
				//Debug.LogError("Failed showing reward video");
				UTween temp = UTween.Fade(fader, new Vector2(0,1), 0.3f);
				temp.onDone = ()=>{
					enabled = true;
					root.SetActive(false);
					if(onDone != null){
						onDone.Invoke(baseCoins * 1);
						//rBase.AdWheelComplete(baseCoins * GetMultiplier());
					}
				};
			});
			//Debug.LogError("View Ad Here");
		}
	}

	public void Skip(){
		if(enabled){
			enabled = false;
			UTween temp = UTween.Fade(fader, new Vector2(0,1), 0.3f);
			temp.onDone = ()=>{
				enabled = true;
				root.SetActive(false);
				if(onDone != null){
					onDone.Invoke(baseCoins);
					//rBase.AdWheelComplete(baseCoins * GetMultiplier());
				}
			};
		}
	}
}
