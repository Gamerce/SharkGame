using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UTween : MonoBehaviour
{
	public enum TweenType{
		None = -1,
		PositionTween,
		RotationTween,
		RotationShakeTween,
		ImageFadeTween,
		DestroyObject,
		SimpleRotTeween,
		Wait,
		Flash,
		Scale,
		ShakeTween,

		Count,
	}

	TweenType usedType;

	Transform targetTrans;
	RectTransform targetRectTrans;
	Image targetUIImg;
	Color targetCol, startCol;

	bool useWorldSpace = true;
	float duration = 1;
	float currentTime = 0;
	float delay = -1;

	Vector3 startPos, targetPos;
	Vector2 curveStartEnd = new Vector2(0,1);

	bool moveTo = true;

	AnimationCurve curve = Extensions.GetLinearCurve();
	
	MaskableGraphic targetImg;
	CanvasGroup targetGroup;

	public System.Action onDone;
	public bool useScaledTime = true;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void LateUpdate()
	{
		if(delay > 0){
			delay -= useScaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
			return;
		}
		currentTime += useScaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
		float percentage = (duration <= 0) ? 1 : currentTime / duration;
		if(SetPercentage(percentage > 1 ? 1 : percentage) && percentage >= 1){
			if(onDone != null){
				onDone.Invoke();
			}
			Destroy(this);
		}
	}

	public void SetCurve(AnimationCurve newCurve){
		curve = newCurve;
	}

	bool SetPercentage(float percent){
		percent = moveTo ? percent : 1.0f-percent;

		if(usedType == TweenType.PositionTween){
			if(targetTrans != null){
				if(useWorldSpace){
					targetTrans.position = Vector3.LerpUnclamped(startPos, targetPos, curve.Evaluate(percent));
				}
				else
					targetTrans.localPosition = Vector3.LerpUnclamped(startPos, targetPos, curve.Evaluate(percent));
			}
			if(targetRectTrans != null){
				if(useWorldSpace){
					targetRectTrans.position = Vector3.LerpUnclamped(startPos, targetPos, curve.Evaluate(percent));
				}
				else
					targetRectTrans.anchoredPosition = Vector2.LerpUnclamped(startPos.GetVector2Z(), targetPos.GetVector2Z(), curve.Evaluate(percent));
			}
		}
		else if(usedType == TweenType.RotationShakeTween){
			if(useWorldSpace){
				targetRectTrans.rotation = Quaternion.Euler(startPos + targetPos * curve.Evaluate(percent));
			}
			else
				targetRectTrans.localRotation = Quaternion.Euler(startPos + targetPos * curve.Evaluate(percent));
		}
		else if(usedType == TweenType.ImageFadeTween){
			if(targetImg != null){
				targetImg.color = targetImg.color.SetAlpha(curveStartEnd.Evaluate(curve.Evaluate(percent)));
			}
			if(targetGroup != null){
				targetGroup.alpha = curve.Evaluate(curveStartEnd.Evaluate(percent));
			}
		}
		else if(usedType == TweenType.DestroyObject){
			if(percent >= 1){
				Destroy(gameObject);
				return false;
			}
		}
		else if(usedType == TweenType.SimpleRotTeween){
			if(targetRectTrans != null){
				if(useWorldSpace)
					targetRectTrans.rotation = Quaternion.Euler(Vector3.Lerp(startPos, targetPos, curve.Evaluate(percent)));
				else
					targetRectTrans.localRotation = Quaternion.Euler(Vector3.Lerp(startPos, targetPos, curve.Evaluate(percent)));
			}
		}
		else if(usedType == TweenType.Wait){
			//Waiting do nothing.
		}
		else if(usedType == TweenType.Flash){
			targetUIImg.color = Color.Lerp(startCol, targetCol, curve.Evaluate(percent));
		}
		else if(usedType == TweenType.Scale){
			targetTrans.localScale = Vector3.Lerp(startPos, targetPos, curve.Evaluate(percent));
		}
		return true;
	}
	
	public static void Clear(Transform target){
		Clear(target.gameObject);
	}

	public static void Clear(GameObject target){
		UTween[] all = target.GetComponents<UTween>();
		if(all != null){
			for(int index = 0; index < all.Length; index++){
				Destroy(all[index]);
			}
		}
	}

	public static UTween DestroyAfter(GameObject targetObj, float duration){
		UTween tween = targetObj.AddComponent<UTween>();
		tween.duration = duration;
		tween.usedType = TweenType.DestroyObject;
		return tween;
	}

	public static UTween Fade(MaskableGraphic target, Vector2 startEndVal, float duration, float delay = -1, AnimationCurve usedCurve = null){
		UTween tween = target.gameObject.AddComponent<UTween>();
		tween.targetImg = target;
		tween.curveStartEnd = startEndVal;
		tween.delay = delay;
		tween.curve = (usedCurve == null) ? Extensions.GetDefaultCurve() : usedCurve;
		tween.duration = duration;
		tween.usedType = TweenType.ImageFadeTween;
		return tween;
	}

	public static UTween Fade(CanvasGroup target, Vector2 startEndVal, float duration, float delay = -1, AnimationCurve usedCurve = null){
		UTween tween = target.gameObject.AddComponent<UTween>();
		tween.targetGroup = target;
		tween.targetImg = null;
		tween.curveStartEnd = startEndVal;
		tween.delay = delay;
		tween.curve = (usedCurve == null) ? Extensions.GetDefaultCurve() : usedCurve;
		tween.duration = duration;
		tween.usedType = TweenType.ImageFadeTween;
		return tween;
	}
	
	public static UTween MoveTo(Transform targetTrans, Vector3 startPos, Vector3 targetPos, float time, bool useWorldSpace = true){
		UTween tween = MoveTo(targetTrans, targetPos, time, useWorldSpace);
		tween.startPos = startPos;
		return tween;
	}

	public static UTween MoveTo(Transform targetTrans, Vector3 targetPos, float time, bool useWorldSpace = true){
		if(!Application.isPlaying){
			Debug.LogError("Only use UTween when the game is active.");
			return null;
		}

		UTween tween = targetTrans.gameObject.AddComponent<UTween>();
		tween.targetTrans = targetTrans;
		tween.targetPos = targetPos;
		tween.duration = time;
		tween.useWorldSpace = useWorldSpace;
		tween.startPos = useWorldSpace ? targetTrans.position : targetTrans.localPosition;
		tween.moveTo = true;
		tween.SetPercentage(0);
		tween.usedType = TweenType.PositionTween;
		return tween;
	}
	
	public static UTween MoveTo(RectTransform targetTrans, Vector2 startPos, Vector2 targetPos, float time, bool useWorldSpace = false){
		UTween tween = MoveTo(targetTrans, targetPos, time, useWorldSpace);
		tween.startPos = startPos.GetVector3Z();
		return tween;
	}
	public static UTween MoveTo(RectTransform targetTrans, Vector2 targetPos, float time, bool useWorldSpace = false){
		if(!Application.isPlaying){
			Debug.LogError("Only use UTween when the game is active.");
			return null;
		}

		UTween tween = targetTrans.gameObject.AddComponent<UTween>();
		tween.targetRectTrans = targetTrans;
		tween.targetPos = targetPos.GetVector3Z();
		tween.duration = time;
		tween.useWorldSpace = useWorldSpace;
		tween.startPos = useWorldSpace ? targetTrans.position : targetTrans.anchoredPosition.GetVector3Z();
		tween.moveTo = true;
		tween.SetPercentage(0);
		tween.usedType = TweenType.PositionTween;
		return tween;
	}

	
	public static UTween MoveFrom(Transform targetTrans, Vector3 startPos, Vector3 targetPos, float time, bool useWorldSpace = true){
		UTween tween = MoveFrom(targetTrans, targetPos, time, useWorldSpace);
		tween.startPos = startPos;
		return tween;
	}

	public static UTween MoveFrom(Transform targetTrans, Vector3 targetPos, float time, bool useWorldSpace = true){
		UTween tween = MoveTo(targetTrans, targetPos, time, useWorldSpace);
		tween.moveTo = false;
		tween.SetPercentage(0);
		return tween;
	}

	
	public static UTween MoveFrom(RectTransform targetTrans, Vector2 startPos, Vector2 targetPos, float time, bool useWorldSpace = false){
		UTween tween = MoveFrom(targetTrans, targetPos, time, useWorldSpace);
		tween.startPos = startPos.GetVector3Z();
		return tween;
	}

	public static UTween MoveFrom(RectTransform targetTrans, Vector2 targetPos, float time, bool useWorldSpace = false){
		UTween tween = MoveTo(targetTrans, targetPos, time, useWorldSpace);
		tween.moveTo = false;
		tween.SetPercentage(0);
		return tween;
	}


	public static UTween ShakeRot(RectTransform targetTrans, Vector3 effectedAngles, float time, int shakes = 20, bool useWorldSpace = false){
		if(!Application.isPlaying){
			Debug.LogError("Only use UTween when the game is active.");
			return null;
		}
		UTween tween = targetTrans.gameObject.AddComponent<UTween>();
		
		tween.usedType = TweenType.RotationTween;
		tween.curve = Extensions.GetShakeCurve(shakes, 1);

		tween.targetPos = effectedAngles;
		tween.targetRectTrans = targetTrans;
		tween.duration = time;
		tween.startPos = useWorldSpace ? targetTrans.rotation.eulerAngles : targetTrans.localRotation.eulerAngles;
		tween.usedType = TweenType.RotationShakeTween;
		tween.SetPercentage(0);
		return tween;
	}

	//public static UTween Shake(Transform targetTrans, Vector3 effectedAngles, float force, float time, int shakes = 20, AnimationCurve shakeEvaluator){
	//	if(!Application.isPlaying){
	//		Debug.LogError("Only use UTween when the game is active.");
	//		return null;
	//	}
	//	UTween tween = targetTrans.gameObject.AddComponent<UTween>();
	//	
	//	tween.usedType = TweenType.ShakeTween;
	//	tween.curve = Extensions.GetShakeCurve(shakes, 1, shakeEvaluator);
	//
	//	tween.targetPos = effectedAngles;
	//	tween.targetRectTrans = targetTrans;
	//	tween.duration = time;
	//	tween.startPos = useWorldSpace ? targetTrans.rotation.eulerAngles : targetTrans.localRotation.eulerAngles;
	//	tween.usedType = TweenType.RotationShakeTween;
	//	tween.SetPercentage(0);
	//	return tween;
	//}
	
	public static UTween Rotate(RectTransform targetTrans, Vector3 startRotation, Vector3 targetRotation, float time, float delay = -1, bool useWorldSpace = false){
		if(!Application.isPlaying){
			Debug.LogError("Only use UTween when the game is active.");
			return null;
		}
		UTween tween = targetTrans.gameObject.AddComponent<UTween>();
		
		tween.usedType = TweenType.SimpleRotTeween;
		tween.curve = Extensions.GetDefaultCurve();
		
		tween.startPos = startRotation;
		tween.targetPos = targetRotation;
		tween.targetRectTrans = targetTrans;
		tween.duration = time;
		tween.delay = delay;
		tween.useWorldSpace = useWorldSpace;
		tween.SetPercentage(0);
		return tween;
	}

	public static UTween Wait(GameObject target, float duration, System.Action callback){
		if(!Application.isPlaying){
			Debug.LogError("Only use UTween when the game is active.");
			return null;
		}
		UTween tween = target.AddComponent<UTween>();
		
		tween.usedType = TweenType.Wait;
		tween.duration = -1;
		tween.delay = duration;
		tween.onDone = callback;
		return tween;
	}

	public static UTween Flash(Image target, Color targetCol, float duration, float delay = -1, System.Action callback = null, AnimationCurve curve = null){
		
		UTween tween = target.gameObject.AddComponent<UTween>();
		
		if(curve == null){
			curve = Extensions.GetInOutFadeCurve();
		}
		tween.usedType = TweenType.Flash;
		tween.delay = delay;
		tween.duration = duration;
		tween.targetUIImg = target;
		tween.targetCol = targetCol;
		tween.curve = curve;
		tween.startCol = target.color;
		tween.onDone = callback;
		return tween;
	}

	public static UTween Scale(Transform target, Vector3 targetScale, float duration, AnimationCurve curve = null, float delay = -1){
		
		UTween tween = target.gameObject.AddComponent<UTween>();
		
		if(curve == null){
			curve = Extensions.GetInOutFadeCurve();
		}
		tween.usedType = TweenType.Scale;
		tween.delay = delay;
		tween.duration = duration;
		tween.targetTrans = target;
		tween.startPos = target.localScale;
		tween.targetPos = targetScale;
		tween.curve = curve;
		return tween;
	}
}
