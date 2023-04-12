using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHatButton : MonoBehaviour
{
	public RectTransform upCenter;
	public RectTransform downCenter;

	public HatData.HatName targetHat;

	public SelectHat owner;

	public RenderTexture hatImage;

	public RawImage hatBackground;
	public RawImage hatFillAmount;

	bool isDone = false;

	public Image buttonImg;
	public Sprite upButton;
	public Sprite downButton;

	public List<Sprite> AllSprites;
	public List<Sprite> AllSpritesUnlocked;
	public Image theHatImage;
	public GameObject SelectionOverlay;

	Vector2 usedMinMax;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Init(RenderTexture targetTex, HatData.HatName targetHat, float fillPercent, Vector2 minMax) {
		usedMinMax = minMax;
		isDone = fillPercent >= 1;
		this.targetHat = targetHat;
		hatImage = targetTex;
		hatBackground.texture = targetTex;
		hatFillAmount.texture = targetTex;
		hatFillAmount.material = Instantiate(hatFillAmount.material);
		hatFillAmount.material.SetFloat("_FillAmount", usedMinMax.Evaluate(fillPercent));

		if (isDone)
			theHatImage.sprite = AllSpritesUnlocked[(int)targetHat];
		else
		{
			theHatImage.sprite = AllSprites[(int)targetHat];
		}
	}

	public void SetFillAmount(float fillPercent){
		hatFillAmount.material.SetFloat("_FillAmount", usedMinMax.Evaluate(fillPercent));
		isDone = fillPercent >= 1.0f;
		if(fillPercent >= 0.99f){
			int targetHat = AllSprites.GetId(theHatImage.sprite);
			if(targetHat.IsInBound(AllSpritesUnlocked))
				theHatImage.sprite = AllSpritesUnlocked[targetHat];
		}
	}

	public void SetSelected(HatData.HatName selectedHat){
		if(targetHat == selectedHat){
			SetDownButton();
		}
		else
			SetUpButton();
	}

	public void SetDownButton(){
		RectTransform hatTrans = hatBackground.rectTransform;
		float rrw = hatTrans.rect.width;
		float rrh = hatTrans.rect.height;
		hatTrans.anchorMin = new Vector2(0.5f, 0.5f);
		hatTrans.anchorMax = new Vector2(0.5f, 0.5f);
		hatTrans.sizeDelta = new Vector2(rrw, rrh);


		//Vector2 size = new Vector2(hatTrans.rect.width, hatTrans.rect.height);
		//hatTrans.anchorMin = new Vector2(0.5f, 0.5f);
		//hatTrans.anchorMax = new Vector2(0.5f, 0.5f);
		//hatTrans.rect.Set(hatTrans.rect.x, hatTrans.rect.y, size.x,size.y);
		hatTrans.SetParent(downCenter, true);
		hatTrans.anchoredPosition = Vector2.zero;
		downCenter.gameObject.SetActive(true);
		upCenter.gameObject.SetActive(false);
		buttonImg.sprite = downButton;
		SelectionOverlay.SetActive(true);
	}

	public void SetUpButton(){
		RectTransform hatTrans = hatBackground.rectTransform;
		//Vector2 size = new Vector2(hatTrans.rect.width, hatTrans.rect.height);
		float rrw = hatTrans.rect.width;
		float rrh = hatTrans.rect.height;
		hatTrans.anchorMin = new Vector2(0.5f, 0.5f);
		hatTrans.anchorMax = new Vector2(0.5f, 0.5f);
		hatTrans.sizeDelta = new Vector2(rrw, rrh);

		//hatTrans.anchorMin = new Vector2(0f, hatTrans.anchorMin.y);
		//hatTrans.anchorMax = new Vector2(0f, hatTrans.anchorMax.y);
		//hatTrans.rect.Set(hatTrans.rect.x, hatTrans.rect.y, size.x,size.y);
		hatTrans.SetParent(upCenter, true);
		hatTrans.anchoredPosition = Vector2.zero;
		downCenter.gameObject.SetActive(false);
		upCenter.gameObject.SetActive(true);
		buttonImg.sprite = upButton;
		SelectionOverlay.SetActive(false);
	}

	public void OnClick(){
		if(isDone)
			owner.SelectThisHat(targetHat);
	}
}
