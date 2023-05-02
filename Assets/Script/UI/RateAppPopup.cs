using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateAppPopup : MonoBehaviour
{
	public RectTransform popup;
	public Image bgFader;
	public Vector2 showPos;
	public Vector2 hidePos;
	bool blockClose;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	private void OnEnable() {
		UTween temp = UTween.Fade(bgFader, new Vector2(0,0.65f), 0.3f);
		temp.useScaledTime = false;
		popup.anchoredPosition = hidePos;
		temp = UTween.MoveTo(popup, showPos, 0.3f);
		temp.useScaledTime = false;
		if(GameHandler.instance != null){
			GameHandler.instance.ForceTimeStop(true);
		}
	}
	
	public void CloseClicked(){
		if(blockClose)
			return;
		blockClose = true;

		UTween temp = UTween.Fade(bgFader, new Vector2(0.65f,0), 0.3f);
		temp.useScaledTime = false;
		temp = UTween.MoveTo(popup, hidePos, 0.3f);
		temp.useScaledTime = false;
		temp.onDone += ()=>{
			gameObject.SetActive(false);
			blockClose = false;
		};
		if(GameHandler.instance != null){
			GameHandler.instance.ForceTimeStop(false);
		}
	}

	public void ClickedYes(){
		PlayerPrefs.SetInt("HasRatedApp", 1);
		VoxelBusters.EssentialKit.RateMyApp.AskForReviewNow();
		CloseClicked();
	}

	public void ClickedNo(){
		CloseClicked();
	}

	public void ShowApp(){
		gameObject.SetActive(true);
	}

	public void TryRateApp(){
		if(PlayerPrefs.GetInt("HasRatedApp", 0) == 1)
			return;
		int rateIn = PlayerPrefs.GetInt("RateAppIn", 5);
		rateIn--;
		if(rateIn <= 0){
			ShowApp();
			rateIn = Random.Range(10,15);
		}
		PlayerPrefs.SetInt("RateAppIn", rateIn);
	}
}
