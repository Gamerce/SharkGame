using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public AnimationCurve vibeCurve = new AnimationCurve(new Keyframe[]{new Keyframe(0,0), new Keyframe(0.05f, 1), new Keyframe(0,0)});
	public List<Vibe> vibes = new List<Vibe>();

	float lastVibe = 0;
	bool VibrateOn = true;

    // Start is called before the first frame update
    void Start()
    {
        VibrateOn = PlayerPrefs.GetInt("VibrateOn", 0) == 1;
    }

    // Update is called once per frame
    void Update()
    {
		UpdateVibe();
        float currentVibe = GetVibe();
		if(!VibrateOn)
			currentVibe = 0;
		if(currentVibe != lastVibe){
			lastVibe = currentVibe;
			Handheld.Vibrate();//lastVibe
		}
    }

	public void SetActive(bool enabled){
		VibrateOn = enabled;
	}

	public void AddVibe(float force, float duration, AnimationCurve specialCurve = null){
		Vibe temp = new Vibe();
		temp.force = force;
		temp.duration = duration;
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
