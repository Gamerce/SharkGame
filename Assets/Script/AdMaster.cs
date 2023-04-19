using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class AdMaster : MonoBehaviour, IInterstitialAdListener, IRewardedVideoAdListener
{
	public class AdInfo{
		public float startLoadingIn = 1;
		public bool hasStartedLoading = false;
		public bool canShow = false;
		public System.Action onDone;
		public System.Action onFail;
	}

	AdInfo interstitialAd = new AdInfo();
	AdInfo rewardVideoAd = new AdInfo();

	static AdMaster ourInstance=null;
	public static AdMaster instance { // This is a property.
		get {
			if(ourInstance == null){
				GameObject temp = new GameObject("AdMaster");
				ourInstance = temp.AddComponent<AdMaster>();
			}
		  return ourInstance;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		if(instance != null && instance != this){
			Destroy(gameObject);
			return;
		}


		int adTypes = Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO;// | Appodeal.BANNER | Appodeal.MREC
		string appKey = "YOUR_APPODEAL_APP_KEY";
		#if UNITY_ANDROID//ca-app-pub-
			appKey = "f9f2b89235e1be4f3a190353f3200dd2e0598d0462a7a566";
#elif UNITY_IOS
			appKey = "3643ba340b0cacc468bad0eec1e9c76c2394e3d9b40974f0";
#endif
		Appodeal.setInterstitialCallbacks(this);
		Appodeal.setRewardedVideoCallbacks(this);

		Appodeal.initialize(appKey, adTypes, this); 
		

		
		Appodeal.setAutoCache(Appodeal.INTERSTITIAL, true);
		Appodeal.setAutoCache(Appodeal.REWARDED_VIDEO, true);
		
		interstitialAd.startLoadingIn = 9999999.0f;
		rewardVideoAd.startLoadingIn = 9999999.0f;

		ourInstance = this;
		DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(!interstitialAd.hasStartedLoading && !interstitialAd.canShow){
			if(interstitialAd.startLoadingIn > 0){
				interstitialAd.startLoadingIn -= Time.deltaTime;
			}
			else{
				interstitialAd.hasStartedLoading = true;
				Appodeal.cache(Appodeal.INTERSTITIAL);
			}
		}
		if(!rewardVideoAd.hasStartedLoading && !rewardVideoAd.canShow){
			if(rewardVideoAd.startLoadingIn > 0){
				rewardVideoAd.startLoadingIn -= Time.deltaTime;
			}
			else{
				rewardVideoAd.hasStartedLoading = true;
				Appodeal.cache(Appodeal.REWARDED_VIDEO);
			}
		}
    }
	
	//public void onInitializationFinished(List<string> errors){
	//	if(errors.Count > 0)
	//		Debug.LogError("Error loading ads");
	//	for(int index = 0; index < errors.Count; index++){
	//		Debug.LogError(errors[index]);
	//	}
	//}

	public void ShowInterstitial(System.Action onSuccess, System.Action onFail){
		interstitialAd.onDone = onSuccess;
		interstitialAd.onFail = onFail;
		//if(interstitialAd.canShow){
			if(Appodeal.isLoaded(Appodeal.INTERSTITIAL) && !Application.isEditor){
				Appodeal.show(Appodeal.INTERSTITIAL);
			}
			else{
				interstitialAd.canShow = false;
				interstitialAd.hasStartedLoading = true;
				if(onFail != null)
					onFail.Invoke();
				Appodeal.cache(Appodeal.INTERSTITIAL);
			}
		//}
		//else if(onFail != null)
		//	onFail.Invoke();
	}

	public void ShowRewardVideo(System.Action onSuccess, System.Action onFail){
		rewardVideoAd.onDone = onSuccess;
		rewardVideoAd.onFail = onFail;
		//if(rewardVideoAd.canShow){
			if(Appodeal.isLoaded(Appodeal.REWARDED_VIDEO) && !Application.isEditor){
				Appodeal.show(Appodeal.REWARDED_VIDEO);
			}
			else{
				rewardVideoAd.canShow = false;
				rewardVideoAd.hasStartedLoading = true;
				if(onFail != null)
					onFail.Invoke();
				Appodeal.cache(Appodeal.REWARDED_VIDEO);
			}
		//}
		//else if(onFail != null)
		//	onFail.Invoke();
	}

	public void PrepareVideo(){
		if(rewardVideoAd.canShow){
			if(Appodeal.canShow(Appodeal.REWARDED_VIDEO)){
			}
			else{
				Appodeal.cache(Appodeal.REWARDED_VIDEO);
				rewardVideoAd.canShow = false;
				rewardVideoAd.hasStartedLoading = true;
			}
		}
	}


	#region Interstitial
	
	public void onInterstitialLoaded(bool isPrecache){
		interstitialAd.hasStartedLoading = false;
		interstitialAd.canShow = true;
	}
	public void onInterstitialFailedToLoad(){
		interstitialAd.hasStartedLoading = false;
		interstitialAd.canShow = false;
	}
	public void onInterstitialShowFailed(){
		if(interstitialAd.onFail != null)
			interstitialAd.onFail.Invoke();
	}
	public void onInterstitialShown(){
		if(interstitialAd.onDone != null){
			System.Action temp = interstitialAd.onDone;
			interstitialAd.onDone = null;
			temp.Invoke();
		}
	}
	public void onInterstitialClosed(){
		if(interstitialAd.onDone != null){
			System.Action temp = interstitialAd.onDone;
			interstitialAd.onDone = null;
			temp.Invoke();
		}
	}
	public void onInterstitialClicked(){
		if(interstitialAd.onDone != null){
			System.Action temp = interstitialAd.onDone;
			interstitialAd.onDone = null;
			temp.Invoke();
		}
	}
	public void onInterstitialExpired(){
		interstitialAd.canShow = false;
		interstitialAd.hasStartedLoading = false;
	}
	#endregion

	#region RewardVideo
	
	public void onRewardedVideoLoaded(bool precache){
		rewardVideoAd.hasStartedLoading = false;
		rewardVideoAd.canShow = true;
	}

	public void onRewardedVideoFailedToLoad(){
		rewardVideoAd.hasStartedLoading = false;
		rewardVideoAd.canShow = false;
	}

	public void onRewardedVideoShowFailed(){
		if(rewardVideoAd.onFail != null)
			rewardVideoAd.onFail.Invoke();
	}

	public void onRewardedVideoShown(){
		if(rewardVideoAd.onDone != null){
			System.Action temp = rewardVideoAd.onDone;
			rewardVideoAd.onDone = null;
			temp.Invoke();
		}
	}

	public void onRewardedVideoFinished(double amount, string name){
		if(rewardVideoAd.onDone != null){
			System.Action temp = rewardVideoAd.onDone;
			rewardVideoAd.onDone = null;
			temp.Invoke();
		}
	}

	public void onRewardedVideoClosed(bool finished){
		if(finished && rewardVideoAd.onDone != null){
			System.Action temp = rewardVideoAd.onDone;
			rewardVideoAd.onDone = null;
			temp.Invoke();
		}
		else if(!finished && rewardVideoAd.onFail != null){
			System.Action temp = rewardVideoAd.onFail;
			rewardVideoAd.onFail = null;
			temp.Invoke();
		}
	}

	public void onRewardedVideoExpired(){
		rewardVideoAd.hasStartedLoading = false;
		rewardVideoAd.canShow = false;
	}

	public void onRewardedVideoClicked(){
		if(rewardVideoAd.onDone != null){
			System.Action temp = rewardVideoAd.onDone;
			rewardVideoAd.onDone = null;
			temp.Invoke();
		}
	}

	#endregion
}
