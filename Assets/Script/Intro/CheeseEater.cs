using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheeseEater : MonoBehaviour
{
	public GameObject sharkPrefab;
	public Transform sharkRoot;
	public CheeseBite bite;
	public RewardBase reward;

	public float triggerBiteIn = 0.2f;
	public float biteDuration = 0.5f;
	public AnimationCurve biteCurve = Extensions.GetInOutFadeCurve(0.5f);
	public AnimationCurve UICurve = Extensions.GetDefaultCurve();

	public System.Action onDone;

	public CanvasGroup group;
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
	}

	// Update is called once per frame
	void Update()
    {
		canClickIn -= Time.deltaTime;
		playerDelay += Time.deltaTime;
		if(bite != null){
			if(Input.GetMouseButtonDown(0) && canClickIn < 0){
				playerDelay = 0;
				UTween temp = UTween.MoveTo(sharkRoot, bite.transform.position, biteDuration, true);
				temp.SetCurve(biteCurve);
				temp = UTween.Wait(gameObject, triggerBiteIn, ()=>{
					bite.TakeBite(nextTarget, bite.IsLastBite(nextTarget));
					if(bite.IsLastBite(nextTarget)){
						UTween temp2 = UTween.Wait(gameObject, 0.3f, ()=>{
							if(onDone != null)
								onDone.Invoke();
							reward.EatCheeseDone();
							gameObject.SetActive(false);
						});
						enabled = false;
					}
					nextTarget = bite.PrepareBite();
					canClickIn = 0.7f;
				});
			}
		}
		if(playerDelay > 5){
			group.alpha = UICurve.Evaluate((playerDelay-5).Clamp01());
		}
		else{
			group.alpha = (group.alpha - Time.deltaTime).Clamp01();
		}
    }
}
