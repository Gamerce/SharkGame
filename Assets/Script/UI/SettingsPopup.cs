using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
	public enum SettingsType{
		Sound,
		Music,
		Vibrate,
	}

	[System.Serializable]
	public class SimpleButtonInfo{
		public SettingsType buttonType;
		public GameObject upButton;
		public GameObject downButton;

		public bool isUp = true;
		public void Toggle(){
			isUp = !isUp;
			upButton.SetActive(isUp);
			downButton.SetActive(!isUp);
		}
		public void Set(bool isUp){
			this.isUp = isUp;
			upButton.SetActive(isUp);
			downButton.SetActive(!isUp);
		}
	}

	public List<SimpleButtonInfo> buttons = new List<SimpleButtonInfo>();

	public RectTransform popup;
	public Image bgFader;
	public Vector2 showPos;
	public Vector2 hidePos;

	bool blockClose = false;

    // Start is called before the first frame update
    void Start()
    {
		InitButton(SettingsType.Sound, PlayerPrefs.GetInt("SoundOn", 1) == 1);
		InitButton(SettingsType.Music, PlayerPrefs.GetInt("MusicOn", 1) == 1);
		InitButton(SettingsType.Vibrate, PlayerPrefs.GetInt("VibrateOn", 0) == 1);
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

	void InitButton(SettingsType target, bool isUp){
		for(int index = 0; index < buttons.Count; index++){
			if(buttons[index].buttonType == target){
				buttons[index].Set(isUp);
				break;
			}
		}
	}

	bool ToggleButton(SettingsType target){
		for(int index = 0; index < buttons.Count; index++){
			if(buttons[index].buttonType == target){
				buttons[index].Toggle();
				return buttons[index].isUp;
			}
		}
		return false;
	}

	public void SoundClicked(){
		bool isUp = ToggleButton(SettingsType.Sound);
		PlayerPrefs.SetInt("SoundOn", (isUp ? 1 : 0));
		if(MusicManager.instance != null)
			MusicManager.instance.SetSoundOnOff(isUp);
	}

	public void MusicClicked(){
		bool isUp = ToggleButton(SettingsType.Music);
		PlayerPrefs.SetInt("MusicOn", (isUp ? 1 : 0));
		if(MusicManager.instance != null)
			MusicManager.instance.SetMusicOnOff(isUp);
	}

	public void VibrateClicked(){
		bool isUp = ToggleButton(SettingsType.Vibrate);
		PlayerPrefs.SetInt("VibrateOn", (isUp ? 1 : 0));
		if(GameHandler.instance != null && GameHandler.instance.viber != null){
			GameHandler.instance.viber.SetActive(isUp);
		}
	}

	public void ResetClicked(){
		PlayerPrefs.DeleteAll();
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}

	public void PrivacyPolicyClicked(){
		Debug.LogError("Show privacy policy here or link to it.");
		Application.OpenURL("https://www.influencerapps.io/privacypolicy");
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

	[ContextMenu("Save Current To Show")]
	public void SaveCurrentToShow(){
		showPos = popup.anchoredPosition;
	}
	
	[ContextMenu("Save Current To Hide")]
	public void SaveCurrentToHide(){
		hidePos = popup.anchoredPosition;
	}
}
