using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextValue : MonoBehaviour
{
	public List<TMPro.TextMeshProUGUI> texts = new List<TMPro.TextMeshProUGUI>();
	int lastValue;
	int currentValue;
	int targetValue;
	int startValue;
	float atTime = 1;
	float duration = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentValue = (int)Mathf.Lerp((float)startValue, (float)targetValue, (atTime/duration).Clamp01());
		if(currentValue != lastValue){
			lastValue = currentValue;
			for(int index = 0; index < texts.Count; index++){
				texts[index].text = currentValue.ToString("0");
			}
		}
    }

	public int GetAmount(){
		return targetValue;
	}

	public void AddNow(int amount){
		targetValue += amount;
		currentValue = targetValue;
		atTime = 1;
		duration = 1;
		startValue = targetValue;
	}

	public void SetAmount(int amount){
		atTime = 1;
		duration = 1;
		currentValue = amount;
		lastValue = currentValue;
		targetValue = amount;
		startValue = amount;
		for(int index = 0; index < texts.Count; index++){
			texts[index].text = currentValue.ToString("0");
		}
	}

	public void AddAmount(int amount, float overTime = 0.5f){
		startValue = currentValue;
		targetValue = amount;

		atTime = 0;
		duration = overTime;
	}
}
