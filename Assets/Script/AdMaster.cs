using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class AdMaster : MonoBehaviour, IInterstitialAdListener, IRewardedVideoAdListener, IAppodealInitializationListener
{
	public class AdInfo{
		public float startLoadingIn = 1;
		public bool hasStartedLoading = false;
		public bool canShow = false;
		public System.Action onDone;
		public System.Action onFail;
	}

	public enum CallBackID{
		onInitializationFinished,


		onInterstitialLoaded,
		onInterstitialFailedToLoad,
		onInterstitialShowFailed,
		onInterstitialShown,
		onInterstitialClosed,
		onInterstitialClicked,
		onInterstitialExpired,

		
		onRewardedVideoLoaded,
		onRewardedVideoFailedToLoad,
		onRewardedVideoShowFailed,
		onRewardedVideoShown,
		onRewardedVideoFinished,
		onRewardedVideoClosed,
		onRewardedVideoExpired,
		onRewardedVideoClicked,

		Count,
	}

	AdInfo interstitialAd = new AdInfo();
	AdInfo rewardVideoAd = new AdInfo();

	public List<CallBackID> debbugerString = new List<CallBackID>();

	static AdMaster ourInstance;
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
		Debug.LogError("Init of Appodeal started.");
		Appodeal.setInterstitialCallbacks(this);
		Appodeal.setRewardedVideoCallbacks(this);
		Appodeal.initialize(appKey, adTypes, this as IAppodealInitializationListener); 
		
		
		interstitialAd.startLoadingIn = 9999999.0f;
		rewardVideoAd.startLoadingIn = 9999999.0f;

		ourInstance = this;
		DontDestroyOnLoad(gameObject);
    }
	List<string> internalErrors = null;
	
	public void onInitializationFinished(List<string> errors){
		internalErrors = errors;
		debbugerString.Add(CallBackID.onInitializationFinished);
	}

	public void InternalOnInitializationFinished(){
		//	enabled = true;
		Debug.LogError("onInitializationFinished");
		if(internalErrors == null){
		
			Appodeal.setAutoCache(Appodeal.INTERSTITIAL, true);
			Appodeal.setAutoCache(Appodeal.REWARDED_VIDEO, true);
		}
	}

    // Update is called once per frame
    void Update()
    {
		//for(int index = 0; index < debbugerString.Count; index++){
		//	Debug.LogError(debbugerString[index]);
		//}
		//if(debbugerString.Count != 0)
		//	debbugerString.Clear();
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

		while(debbugerString.Count > 0){
			CallBackID id = debbugerString[0];
			if(id == CallBackID.onInitializationFinished)
				InternalOnInitializationFinished();
			if(id == CallBackID.onInterstitialLoaded)
				InternalOnInterstitialLoaded();
			if(id == CallBackID.onInterstitialFailedToLoad)
				InternalOnInterstitialFailedToLoad();
			if(id == CallBackID.onInterstitialShowFailed)
				InternalOnInterstitialShowFailed();
			if(id == CallBackID.onInterstitialShown)
				InternalOnInterstitialShown();
			if(id == CallBackID.onInterstitialClosed)
				InternalOnInterstitialClosed();
			if(id == CallBackID.onInterstitialClicked)
				InternalOnInterstitialClicked();
			if(id == CallBackID.onInterstitialExpired)
				InternalOnInterstitialExpired();

		
			if(id == CallBackID.onRewardedVideoLoaded)
				InternalOnRewardedVideoLoaded();
			if(id == CallBackID.onRewardedVideoFailedToLoad)
				InternalOnRewardedVideoFailedToLoad();
			if(id == CallBackID.onRewardedVideoShowFailed)
				InternalOnRewardedVideoShowFailed();
			if(id == CallBackID.onRewardedVideoShown)
				InternalOnRewardedVideoShown();
			if(id == CallBackID.onRewardedVideoFinished)
				InternalOnRewardedVideoFinished();
			if(id == CallBackID.onRewardedVideoClosed)
				InternalOnRewardedVideoClosed();
			if(id == CallBackID.onRewardedVideoExpired)
				InternalOnRewardedVideoExpired();
			if(id == CallBackID.onRewardedVideoClicked)
				InternalOnRewardedVideoClicked();
			debbugerString.RemoveAt(0);
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
				//Appodeal.setInterstitialCallbacks(null);
				//Appodeal.setInterstitialCallbacks(this);
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
		Debug.LogError("onSuccess | onFail");
		rewardVideoAd.onDone = onSuccess;
		rewardVideoAd.onFail = onFail;
		//if(rewardVideoAd.canShow){
			if(Appodeal.isLoaded(Appodeal.REWARDED_VIDEO) && !Application.isEditor){
				Appodeal.show(Appodeal.REWARDED_VIDEO);
				//Appodeal.setRewardedVideoCallbacks(null);
				//Appodeal.setRewardedVideoCallbacks(this);
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
		debbugerString.Add(CallBackID.onInterstitialLoaded);
	}

	public void InternalOnInterstitialLoaded(){
		interstitialAd.hasStartedLoading = false;
		interstitialAd.canShow = true;
	}
	public void onInterstitialFailedToLoad(){
		debbugerString.Add(CallBackID.onInterstitialFailedToLoad);
	}

	public void InternalOnInterstitialFailedToLoad(){
		interstitialAd.hasStartedLoading = false;
		interstitialAd.canShow = false;
	}
	public void onInterstitialShowFailed(){
		debbugerString.Add(CallBackID.onInterstitialShowFailed);
	}

	public void InternalOnInterstitialShowFailed(){
		if(interstitialAd.onFail != null)
			interstitialAd.onFail.Invoke();
	}
	public void onInterstitialShown(){
		debbugerString.Add(CallBackID.onInterstitialShown);
	}

	public void InternalOnInterstitialShown(){
		if(interstitialAd.onDone != null){
			System.Action temp = interstitialAd.onDone;
			interstitialAd.onDone = null;
			interstitialAd.onFail = null;
			temp.Invoke();
		}
	}
	public void onInterstitialClosed(){
		debbugerString.Add(CallBackID.onInterstitialClosed);
	}

	public void InternalOnInterstitialClosed(){
		if(interstitialAd.onDone != null){
			System.Action temp = interstitialAd.onDone;
			interstitialAd.onDone = null;
			interstitialAd.onFail = null;
			temp.Invoke();
		}
	}
	public void onInterstitialClicked(){
		debbugerString.Add(CallBackID.onInterstitialClicked);
	}

	public void InternalOnInterstitialClicked(){
		if(interstitialAd.onDone != null){
			System.Action temp = interstitialAd.onDone;
			interstitialAd.onDone = null;
			interstitialAd.onFail = null;
			temp.Invoke();
		}
	}
	public void onInterstitialExpired(){
		debbugerString.Add(CallBackID.onInterstitialExpired);
	}

	public void InternalOnInterstitialExpired(){
		interstitialAd.canShow = false;
		interstitialAd.hasStartedLoading = false;
	}
	#endregion

	#region RewardVideo
	
	public void onRewardedVideoLoaded(bool precache){
		debbugerString.Add(CallBackID.onRewardedVideoLoaded);
	}

	public void InternalOnRewardedVideoLoaded(){
		Debug.LogError("OnRewardedVideoLoaded");
		rewardVideoAd.hasStartedLoading = false;
		rewardVideoAd.canShow = true;
	}

	public void onRewardedVideoFailedToLoad(){
		debbugerString.Add(CallBackID.onRewardedVideoFailedToLoad);
	}

	public void InternalOnRewardedVideoFailedToLoad(){
		Debug.LogError("OnRewardedVideoFailedToLoad");
		rewardVideoAd.hasStartedLoading = false;
		rewardVideoAd.canShow = false;
	}

	public void onRewardedVideoShowFailed(){
		debbugerString.Add(CallBackID.onRewardedVideoShowFailed);
	}

	public void InternalOnRewardedVideoShowFailed(){
		Debug.LogError("OnRewardedVideoShowFailed");
		if(rewardVideoAd.onFail != null){
			System.Action temp = rewardVideoAd.onFail;
			rewardVideoAd.onFail = null;
			rewardVideoAd.onDone = null;
			temp.Invoke();
		}
	}

	public void onRewardedVideoShown(){
		debbugerString.Add(CallBackID.onRewardedVideoShown);
	}

	public void InternalOnRewardedVideoShown(){
		Debug.LogError("OnRewardedVideoShown");
		if(rewardVideoAd.onDone != null){
			System.Action temp = rewardVideoAd.onDone;
			rewardVideoAd.onDone = null;
			rewardVideoAd.onFail = null;
			temp.Invoke();
		}
	}

	public void onRewardedVideoFinished(double amount, string name){
		debbugerString.Add(CallBackID.onRewardedVideoFinished);
	}

	public void InternalOnRewardedVideoFinished(){
		Debug.LogError("OnRewardedVideoFinished");
		if(rewardVideoAd.onDone != null){
			System.Action temp = rewardVideoAd.onDone;
			rewardVideoAd.onDone = null;
			rewardVideoAd.onFail = null;
			temp.Invoke();
		}
	}

	bool InternalOnRewardedVideoClosedFinished = false;
	public void onRewardedVideoClosed(bool finished){
		InternalOnRewardedVideoClosedFinished = finished;
		debbugerString.Add(CallBackID.onRewardedVideoClosed);
	}

	public void InternalOnRewardedVideoClosed(){
		Debug.LogError("OnRewardedVideoClosed");
		bool finished = InternalOnRewardedVideoClosedFinished;
		if(finished && rewardVideoAd.onDone != null){
			System.Action temp = rewardVideoAd.onDone;
			rewardVideoAd.onDone = null;
			rewardVideoAd.onFail = null;
			temp.Invoke();
		}
		else if(!finished && rewardVideoAd.onFail != null){
			System.Action temp = rewardVideoAd.onFail;
			rewardVideoAd.onFail = null;
			rewardVideoAd.onDone = null;
			temp.Invoke();
		}
	}

	public void onRewardedVideoExpired(){
		debbugerString.Add(CallBackID.onRewardedVideoExpired);
	}

	public void InternalOnRewardedVideoExpired(){
		Debug.LogError("OnRewardedVideoExpired");
		rewardVideoAd.hasStartedLoading = false;
		rewardVideoAd.canShow = false;
	}

	public void onRewardedVideoClicked(){
		debbugerString.Add(CallBackID.onRewardedVideoClicked);
	}

	public void InternalOnRewardedVideoClicked(){
		Debug.LogError("OnRewardedVideoClicked");
		if(rewardVideoAd.onDone != null){
			System.Action temp = rewardVideoAd.onDone;
			rewardVideoAd.onDone = null;
			rewardVideoAd.onFail = null;
			temp.Invoke();
		}
	}

	#endregion
}