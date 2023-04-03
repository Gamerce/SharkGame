using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheeseEater : MonoBehaviour
{
	public GameObject sharkPrefab;
	public Transform sharkRoot;
	public CheeseBite bite;
	SharkHatFitter hatFitter;
	public RewardBase reward;
	public Camera cam;

	public float triggerBiteIn = 0.2f;
	public float biteDuration = 0.5f;
	public AnimationCurve biteCurve = Extensions.GetInOutFadeCurve(0.5f);
	public AnimationCurve UICurve = Extensions.GetDefaultCurve();

	public System.Action onDone;
	
	public Image fadeImg;
	public CanvasGroup group;
	public CanvasGroup textAlpha;
	float playerDelay = 0;
	GameObject nextTarget;
	bool isLast = false;
	float canClickIn = -1;


    // Start is called before the first frame update
    void Start()
    {
        
    }


	private void OnEnable() {
		bite.Init();
		nextTarget = bite.PrepareBite();
		GameObject temp = Instantiate(sharkPrefab, sharkRoot);
		temp.transform.localPosition = Vector3.zero;
		temp.transform.localScale = Vector3.one;
		hatFitter = temp.GetComponent<SharkHatFitter>();
		group.alpha = 1;
		textAlpha.alpha = 0;
		fadeImg.color = new Color(0,0,0,1);
		cam.enabled = false;
		UTween temp2 = UTween.Fade(group, new Vector2(0,1), 0.3f);
		temp2.onDone = ()=>{
			cam.enabled = true;
			UTween.Fade(fadeImg, new Vector2(1,0), 0.3f);
			canClickIn = 0.2f;	
		};
		canClickIn = 9999;
	}

	// Update is called once per frame
	void Update()
    {
		//if(group.alpha < 1)
		//	group.alpha += Time.deltaTime;
		//if(group.alpha >= 1)
		//	cam.enabled = true;
		canClickIn -= Time.deltaTime;
		playerDelay += Time.deltaTime;
		if(bite != null){
			if(Input.GetMouseButtonDown(0) && canClickIn < 0){
				playerDelay = 0;
				//UTween temp01 = UTween.Wait(sharkRoot.gameObject, 0.1f, ()=>{
				//});
				if(hatFitter != null){
					hatFitter.transform.localPosition = Vector3.zero;
					hatFitter.TriggerBite();
				}
				
				UTween.Clear(sharkRoot);
				UTween temp = UTween.MoveTo(sharkRoot, bite.transform.position, biteDuration, true);
				temp.SetCurve(biteCurve);
				temp = UTween.Wait(gameObject, triggerBiteIn, ()=>{
					bite.TakeBite(nextTarget, bite.IsLastBite(nextTarget));
					if(bite.IsLastBite(nextTarget)){
						UTween temp2 = UTween.Wait(gameObject, 0.3f, ()=>{
							UTween temp3 = UTween.Fade(fadeImg, new Vector2(0,1), 0.3f);
							temp3.onDone = ()=>{
								if(onDone != null)
									onDone.Invoke();
								reward.EatCheeseDone();
								gameObject.SetActive(false);
							};
						});
						enabled = false;
					}
					nextTarget = bite.PrepareBite();
					canClickIn = 0.1f;
				});
			}
		}
		if(playerDelay > 5){
			textAlpha.alpha = UICurve.Evaluate((playerDelay-5).Clamp01());
		}
		else{
			textAlpha.alpha = (textAlpha.alpha - Time.deltaTime).Clamp01();
		}
    }
}
