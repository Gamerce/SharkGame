using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToFit : MonoBehaviour
{
	public Canvas rootCan;
	public RectTransform target;

	public float bonusPercent = 0.1f;

	private void Reset() {
		target = transform as RectTransform;
	}

	// Start is called before the first frame update
	void Start()
    {
		rootCan = GetComponentInParent<Canvas>();
    }

	private void OnEnable() {
		if(rootCan == null)
			rootCan = GetComponentInParent<Canvas>();
		Rect canvasRect = (rootCan.transform as RectTransform).rect;
		float overFlow = target.sizeDelta.x / canvasRect.width + bonusPercent;
		if(overFlow > 1){
			target.localScale = Vector3.one / overFlow;
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
