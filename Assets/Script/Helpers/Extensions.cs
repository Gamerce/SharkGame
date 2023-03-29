using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class Extensions {	
	#region Curve

	static public AnimationCurve GetDefaultCurve(bool reversed = false){
		if(reversed)
			return new AnimationCurve(new Keyframe[]{new Keyframe(1,1), new Keyframe(0,0)});
		else
			return new AnimationCurve(new Keyframe[]{new Keyframe(0,0), new Keyframe(1,1)});
	}
	
	static public AnimationCurve GetInOutFadeCurve(float fadePercent = 0.1f, bool reversed = false){
		if(reversed)
			return new AnimationCurve(new Keyframe[]{new Keyframe(0,1), new Keyframe(fadePercent, 0), new Keyframe(1-fadePercent, 0), new Keyframe(1,1)});
		else
			return new AnimationCurve(new Keyframe[]{new Keyframe(0,0), new Keyframe(fadePercent, 1), new Keyframe(1-fadePercent, 1), new Keyframe(1,0)});
	}

	static public AnimationCurve GetLinearCurve(bool reversed = false){
		if(reversed)
			return AnimationCurve.Linear(1,1,0,0);
		else
			return AnimationCurve.Linear(0,0,1,1);
	}

	static public AnimationCurve GetShakeCurve(int shakes, float velocity, AnimationCurve evaluator = null){
		if(evaluator == null)
			evaluator = new AnimationCurve(new Keyframe[]{new Keyframe(0,0), new Keyframe(0.5f,1), new Keyframe(1,0)});
		List<Keyframe> frames = new List<Keyframe>();
		for(int index = 0; index < shakes*2-1+1; index+=2){
			float pos = ((float)(index)+0.5f) / (float)(shakes * 2);
			float pos2 = ((float)(index+1)+0.5f) / (float)(shakes * 2);
			frames.Add(new Keyframe(pos, velocity * evaluator.Evaluate(pos) * Mathf.Cos(((float)index) * Mathf.PI)));
			frames.Add(new Keyframe(pos2, velocity * evaluator.Evaluate(pos2) * Mathf.Cos(((float)index+1) * Mathf.PI)));
		}
		Keyframe temp = new Keyframe(0,0);
		frames.Insert(0, temp);
		temp = new Keyframe(1,0);
		frames.Add(temp);
		return new AnimationCurve(frames.ToArray());
	}
	
	static public AnimationCurve GetFlatCurve(float value){
		return new AnimationCurve(new Keyframe[]{new Keyframe(0,value), new Keyframe(1,value)});
	}
	

	static public AnimationCurve FlipX(this AnimationCurve curve){
		float maxTime = -99999999;
		float minTime = 99999999;

		for(int index = 0; index < curve.keys.Length; index++){
			maxTime = Mathf.Max(maxTime, curve.keys[index].time);
			minTime = Mathf.Min(minTime, curve.keys[index].time);
		}

		Vector2 minMaxVal = new Vector2(minTime, maxTime);
		Vector2 maxMinVal = new Vector2(maxTime, minTime);
		List<Keyframe> frames = new List<Keyframe>();
		for(int index = 0; index < curve.keys.Length; index++){
			Keyframe currentFrame = curve.keys[index];
			float currentTime = currentFrame.time;
			float percent = minMaxVal.CalculatePercentage(currentTime);
			
			Keyframe temp = new Keyframe(maxMinVal.Evaluate(percent), currentFrame.value, currentFrame.outTangent, currentFrame.inTangent, currentFrame.outWeight, currentFrame.inWeight);
			frames.Add(temp);
		}
		return new AnimationCurve(frames.ToArray());
	}

	#endregion


	#region Float

	

	static public float MoveTowards(this float value, float target, float distance){
		if(Mathf.Abs(target-value) <= distance){
			return target;
		}
		return value + (target-value).Normalize() * distance;
	}
	static public float Clamp(this float value, Vector2 clampWith)
	{
		return Mathf.Clamp(value, clampWith.x, clampWith.y);
	}
	static public float Clamp01(this float value)
	{
		return Mathf.Clamp(value, 0, 1);
	}
	static public float Clamp1(this float value)
	{
		return Mathf.Clamp(value, -1, 1);
	}
	static public float Abs(this float thisVal){
		return Mathf.Abs(thisVal);
	}

	static public float Normalize(this float thisVal){
		return thisVal < 0 ? -1 : 1;
	}

	static public float CalculatePercentage(this Vector2 pos, float value)
	{
		value = Mathf.Clamp(value, pos.x, pos.y);
		return Mathf.Clamp01((value-pos.x)/(pos.y-pos.x));
	}

	#endregion

	#region Vector3

	
	static public Vector3 GetVector3X(this Vector2 thisVal)
	{
		Vector3 tempVec = new Vector3(0, thisVal.x, thisVal.y);
		return tempVec;
	}

	static public Vector3 GetVector3Y(this Vector2 thisVal, float yVal = 0)
	{
		Vector3 tempVec = new Vector3(thisVal.x, yVal, thisVal.y);
		return tempVec;
	}

	static public Vector3 GetVector3Z(this Vector2 thisVal, float zVal = 0)
	{
		Vector3 tempVec = new Vector3(thisVal.x, thisVal.y, zVal);
		return tempVec;
	}

	#endregion

	#region Vector2
	
	public static float GetRandom(this Vector2 thisValue)
	{
		return UnityEngine.Random.Range(thisValue.x, thisValue.y);
	}
	static public Vector2 GetClosest(this Vector2 pos, Vector2 aPosition, Vector2 aSecondPosition)
	{
		bool segmentClamp = true;
		Vector2 AP = pos - aPosition;	
		Vector2 AB = aSecondPosition - aPosition;	
		float ab2 = AB.x*AB.x + AB.y*AB.y;	
		float ap_ab = AP.x*AB.x + AP.y*AB.y;	
		float t = ap_ab / ab2;	
		if (segmentClamp)	
		{		 
			if (t < 0.0f) 
			{
				t = 0.0f;
			}
			else if (t > 1.0f)
			{
				t = 1.0f;
			}
		}
		return aPosition + AB * t;
	}

	/// <summary>
	/// Without Y
	/// </summary>
	static public Vector2 GetVector2Y(this Vector3 thisVal)
	{
		return new Vector2(thisVal.x, thisVal.z);
	}
	
	/// <summary>
	/// Without Z
	/// </summary>
	static public Vector2 GetVector2Z(this Vector3 thisVal)
	{
		return new Vector2(thisVal.x, thisVal.y);
	}

	public static float Evaluate(this Vector2 thisVal, float percent){
		return Mathf.Lerp(thisVal.x, thisVal.y, percent);
	}

	#endregion


	#region Color
	
	static public Color SetAlpha(this Color thisVal, float targetAlpha){
		return new Color(thisVal.r, thisVal.g, thisVal.b, targetAlpha);
	}
	#endregion

	#region List
	
    public static void Shuffle<T>(this IList<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

	#endregion

	#region List<TMPro.TextMeshPro>

	static public void SetText(this List<TMPro.TextMeshPro> thisVal, string target){
		if(thisVal != null){
			for(int index = 0; index < thisVal.Count; index++){
				if(thisVal[index] != null){
					thisVal[index].text = target;
				}
			}
		}
	}
	static public void SetText(this List<TMPro.TextMeshProUGUI> thisVal, string target){
		if(thisVal != null){
			for(int index = 0; index < thisVal.Count; index++){
				if(thisVal[index] != null){
					thisVal[index].text = target;
				}
			}
		}
	}

	#endregion

	
	#region Camera
	
	static public Vector3[] GetWorldCorners (this Camera cam, float depth)
	{
		Vector3[] tempVals = new Vector3[4];
		cam.GetWorldCorners(ref tempVals, depth);
		return tempVals;
	}
	

	static public bool GetWorldCorners (this Camera cam, ref Vector3[] mSides, float depth)
	{
		return cam.GetWorldCorners(ref mSides, depth, null);
	}
	
	
	static public bool GetWorldCorners (this Camera cam, ref Vector3[] mSides, float depth, Transform relativeTo)
	{
		if (mSides.Length < 4)
			return false;
		#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
		if (cam.isOrthoGraphic)
			#else
			if (cam.orthographic)
				#endif
		{
			float os = cam.orthographicSize;
			float x0 = -os;
			float x1 = os;
			float y0 = -os;
			float y1 = os;
			
			Rect rect = cam.rect;
			
			Vector2 size = new Vector2(Screen.width*2, Screen.height);
			float aspect = cam.aspect;//size.x / size.y;
			aspect *= cam.pixelWidth / cam.pixelHeight;
			x0 *= aspect;
			x1 *= aspect;
			
			// We want to ignore the scale, as scale doesn't affect the camera's view region in Unity
			Transform t = cam.transform;
			Quaternion rot = t.rotation;
			Vector3 pos = t.position;
			
			mSides[0] = rot * (new Vector3(x0, y0, depth)) + pos;
			mSides[1] = rot * (new Vector3(x0, y1, depth)) + pos;
			mSides[2] = rot * (new Vector3(x1, y1, depth)) + pos;
			mSides[3] = rot * (new Vector3(x1, y0, depth)) + pos;
		}
		else
		{
			mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
			mSides[1] = cam.ViewportToWorldPoint(new Vector3(0f, 1f, depth));
			mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 1f, depth));
			mSides[3] = cam.ViewportToWorldPoint(new Vector3(1f, 0f, depth));
		}
		
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; ++i)
				mSides[i] = relativeTo.InverseTransformPoint(mSides[i]);
		}
		return true;
	}
	#endregion
}
