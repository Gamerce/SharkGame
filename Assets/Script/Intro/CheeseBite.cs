using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseBite : MonoBehaviour
{
	public List<GameObject> bites = new List<GameObject>();
	List<GameObject> availableBites = new List<GameObject>();
	public GameObject lastBite;
	
	public float scaleAmount = 1.1f;
	public AnimationCurve eatScaleCurve = Extensions.GetInOutFadeCurve(0.5f);
	public AnimationCurve moveCurve = Extensions.GetInOutFadeCurve(0.5f);

	Vector3 startPos;
	Vector3 startScale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	[ContextMenu("Init")]
	public void Init(){
		if(startPos == Vector3.zero){
			startPos = transform.position;
			startScale = transform.localScale;
		}
		availableBites.AddRange(bites);
		for(int index = 0; index < availableBites.Count; index++){
			availableBites[index].SetActive(true);
		}
		availableBites.Shuffle();
		lastBite.SetActive(true);
		//TakeBite();
	}

	public bool IsLastBite(GameObject target){
		return target == lastBite;
	}

	public void TakeBite(bool useLastBite = false){
		GameObject obj = null;
		if(useLastBite || availableBites.Count <= 0)
			obj = lastBite;
		else{
			obj = availableBites[availableBites.Count-1];
			availableBites.RemoveAt(availableBites.Count-1);
		}
		TakeBite(obj);
	}

	public GameObject PrepareBite(){
		if(availableBites.Count > 0){
			GameObject temp = availableBites[availableBites.Count-1];
			availableBites.RemoveAt(availableBites.Count-1);
			return temp;
		}
		return lastBite;
	}

	public void TakeBite(GameObject target, bool useLastBite = false){
		if(MusicManager.instance != null)
			MusicManager.instance.PlayAudioClip(0, 0.0f);

		UTween hideObj = UTween.Wait(gameObject, 0.05f, ()=>{
			target.SetActive(false);
		});
		UTween temp = UTween.MoveTo(transform, startPos - (target.transform.position - startPos)/2, 0.7f, true);
		temp.SetCurve(moveCurve);
		UTween.Scale(transform, startScale * scaleAmount, 0.5f, eatScaleCurve);
		//if(availableBites.Count > 0){
		//	temp = UTween.Wait(gameObject, 0.75f, ()=>{
		//		TakeBite();
		//	});
		//}
		//else if(!useLastBite){
		//	temp = UTween.Wait(gameObject, 0.75f, ()=>{
		//		TakeBite(true);
		//	});
		//}
	}
}
