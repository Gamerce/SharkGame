using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImpulseVibrations;


public class VibeMaster : MonoBehaviour
{
	[System.Serializable]
	public class Vibe{
		public float force;
		public float duration;
		public float atTime;
		public AnimationCurve uniqueCurve = null;

		public float Evaluate(AnimationCurve defaultCurve){
			float percentage = atTime / duration;
			if(uniqueCurve != null)
				return force * uniqueCurve.Evaluate(percentage);
			return force * defaultCurve.Evaluate(percentage);
		}
	}
	public AnimationCurve vibeCurve = new AnimationCurve(new Keyframe[]{new Keyframe(0, 1), new Keyframe(1,0)});
	public List<Vibe> vibes = new List<Vibe>();

	float lastVibe = 0;
	bool VibrateOn = true;

    // Start is called before the first frame update
    void Start()
    {
        VibrateOn = PlayerPrefs.GetInt("VibrateOn", 0) == 1;
    }

	private void OnDisable() {
		#if UNITY_ANDROID
		Vibrator.AndroidVibrate(0,1);
		#endif
	}

	// Update is called once per frame
	void Update()
    {
        float currentVibe = GetVibe();
		if(!VibrateOn)
			currentVibe = 0;
		if(currentVibe != lastVibe){
			lastVibe = currentVibe;
			#if UNITY_ANDROID
			Vibrator.AndroidVibrate(50,Mathf.Max(1,Mathf.Min((int)(lastVibe * 255.0f), 255)));
			#endif
			//Handheld.Vibrate();//lastVibe
			//Debug.Log("Vibrate!");
		}
		UpdateVibe();
    }

	public void SetActive(bool enabled){
		VibrateOn = enabled;
	}

	public void AddVibe(float force, float duration, AnimationCurve specialCurve = null){
		Vibe temp = new Vibe();
		temp.force = force/3;
		temp.duration = duration/50;
		temp.atTime = 0;
		temp.uniqueCurve = specialCurve;
		vibes.Add(temp);
	}





	void UpdateVibe(){
		for(int index = 0; index < vibes.Count; index++){
			vibes[index].atTime += Time.deltaTime;
			if(vibes[index].atTime > vibes[index].duration)
				vibes.RemoveAt(index--);
		}
	}

	float GetVibe(){
		float totalForce = 0;
		for(int index = 0; index < vibes.Count; index++){
			totalForce += vibes[index].Evaluate(vibeCurve);
		}
		return totalForce.Clamp01();
	}
}
