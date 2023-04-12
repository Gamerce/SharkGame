using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHat : MonoBehaviour
{
	public RewardBase rBase;
	public Camera renderCam;
	public Transform camDisplayRoot;

	public GameObject selectHatTemplate;
	public List<SelectHatButton> allButtons = new List<SelectHatButton>();

	HatData.HatName selectedHat = HatData.HatName.None;
	System.Action onClose;

    // Start is called before the first frame update
    void Start()
    {
		selectHatTemplate.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnDestroy() {
		for(int index = 0; index < allButtons.Count; index++){//Do clean up - Chj
			if(allButtons[index].hatImage != null){
				Destroy(allButtons[index].hatImage);
				Destroy(allButtons[index].gameObject);
			}
		}
	}

	public void Close(){
		gameObject.SetActive(false);
		if(onClose != null){
			System.Action temp = onClose;
			onClose = null;
			temp.Invoke();
		}
	}

	bool hasInited = false;
	[ContextMenu("Init")]
	public void Init(System.Action onClose){
		gameObject.SetActive(true);
		this.onClose = onClose;
		if(hasInited)
			return;
		hasInited = true;
		RenderTexture oldTex = renderCam.targetTexture;
		List<bool> oldActivity = new List<bool>();
		for(int index = 0; index < camDisplayRoot.childCount; index++){
			Transform targte = camDisplayRoot.GetChild(index);
			oldActivity.Add(targte.gameObject.activeSelf);
			targte.gameObject.SetActive(false);
		}

		for(int index = 0; index < (int)(HatData.HatName.Count); index++){
			CreateButton((HatData.HatName)index, rBase.allhats.allHats[index].UnlockPercent(), rBase.allhats.allHats[index].imageMinMax);
		}
		bool wasActive = renderCam.gameObject.activeSelf;
		renderCam.targetTexture = oldTex;
		
		for(int index = 0; index < oldActivity.Count; index++){
			Transform targte = camDisplayRoot.GetChild(index);
			targte.gameObject.SetActive(oldActivity[index]);
		}
		oldActivity.Clear();
        selectedHat = (HatData.HatName)PlayerPrefs.GetInt("SelectedHat", (int)HatData.HatName.None);
		SelectThisHat(selectedHat);
		renderCam.gameObject.SetActive(wasActive);
	}

	public void SelectThisHat(HatData.HatName target){
		selectedHat = target;
		PlayerPrefs.SetInt("SelectedHat", (int)selectedHat);
		for(int index = 0; index < allButtons.Count; index++){
			allButtons[index].SetSelected(target);
		}
	}

	void CreateButton(HatData.HatName targetHat, float fillAmount, Vector2 minMax){
		GameObject newHat = Instantiate(selectHatTemplate, selectHatTemplate.transform.parent);
		SelectHatButton button = newHat.GetComponent<SelectHatButton>();
		allButtons.Add(button);
		RenderTexture tempTex = new RenderTexture(128,128,8);
		Transform child = camDisplayRoot.FindChildTrans(targetHat.ToString(), false);
		if(child != null){
			child.gameObject.SetActive(true);
			renderCam.targetTexture = tempTex;
			renderCam.Render();


			child.gameObject.SetActive(false);
			button.Init(tempTex, targetHat, fillAmount, minMax);
		}
		newHat.SetActive(true);
	}

	public void UnlockRandomHat(){
		int randID = rBase.UnlockRandom();
		if(randID.IsInBound(allButtons)){
			allButtons[randID].SetFillAmount(1);

		}
		Debug.LogError("Add the video here.");
	}
}
