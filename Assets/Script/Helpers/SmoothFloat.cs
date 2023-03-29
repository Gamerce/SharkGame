using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SmoothFloat
{
	public enum SmoothType{
		FixedValue,
		Percentage,
		Time,
	}
	public float smoothSpeed = 1.0f;
	public SmoothType smoothTarget = SmoothType.FixedValue;
	float oldVal;
	float internalVal;
	float targetVal;
	float delay = -1;
	float targetTime;
	float currentTime;
	float timeValue;

	public float value{
		get{
			return internalVal;
		}
		set{
			oldVal = targetVal;
			targetVal = value;
			timeValue = internalVal;
		}
	}

	public SmoothFloat(float targetVal){
		internalVal = targetVal;
	}

    // Update is called once per frame
    public void Update(float deltaTime)
    {
		if(delay > 0)
			return;
		if(smoothTarget == SmoothType.FixedValue)
	        internalVal = internalVal.MoveTowards(targetVal, smoothSpeed*deltaTime);
		else if(smoothTarget == SmoothType.Percentage)
			internalVal = Mathf.Lerp(internalVal, targetVal, deltaTime * smoothSpeed);
		else if(smoothTarget == SmoothType.Time){
			currentTime += Time.deltaTime;
			internalVal = Mathf.Lerp(timeValue, targetVal, (currentTime/targetTime).Clamp01());
		}
    }

	public void SetValue(float newValue)
	{
		internalVal = newValue;
		targetVal = newValue;
		timeValue = newValue;
	}

	public void SetValueIn(float targetValue, float timeUntil, float delay = -1){
		this.delay = delay;
		value = targetValue;
		if(targetVal != internalVal && timeUntil > 0)
			smoothSpeed = (targetVal - internalVal).Abs() / timeUntil;
	}

	public void SetTimeType(float timeTarget){
		smoothTarget = SmoothType.Time;
		currentTime = 0;
		this.targetTime = timeTarget;
		timeValue = internalVal;
	}

	public float GetTarget(){
		return targetVal;
	}

	public bool IsDone(){
		return targetVal == internalVal;
	}

	public float GetPercentage(){
		return new Vector2(oldVal, targetVal).Evaluate(internalVal).Clamp01();
	}
}
