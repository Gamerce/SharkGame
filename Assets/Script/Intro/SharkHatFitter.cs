using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkHatFitter : MonoBehaviour
{
	public Animator sharkAnim;
	public List<GameObject> hatObjects = new List<GameObject>();
	public HatData hats;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHat();
    }

	private void Reset() {
		List<Transform> allObjs = new List<Transform>(GetComponentsInChildren<Transform>(true));
		for(int index = 0; index < ((int)HatData.HatName.Count); index++){
			string hatName = ((HatData.HatName)index).ToString();
			bool hasMatch = false;
			for(int i = 0; i < allObjs.Count; i++){
				if(allObjs[i].gameObject.name == hatName){
					hasMatch = true;
					hatObjects.Add(allObjs[i].gameObject);
					break;
				}
			}
			if(!hasMatch){
				hatObjects.Add(null);
			}
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public void TriggerBite(){
		if(sharkAnim != null)
			sharkAnim.Play("Bite");
	}

	[ContextMenu("Update Hat")]
	public void UpdateHat(){
		
		HatData.HatName selectedHat = (HatData.HatName)PlayerPrefs.GetInt("SelectedHat", (int)HatData.HatName.None);
		for(int index = 0; index < hatObjects.Count; index++){
			hatObjects[index].SetActive(false);
		}
		if(hats != null){
			if(selectedHat == HatData.HatName.None || !((int)selectedHat).IsInBound(hatObjects)){
				for(int index = hats.allHats.Count-1; index >= 0; index--){
						if(hats.allHats[index].IsUnlocked() && index < hatObjects.Count){
							hatObjects[index].SetActive(true);
							MeshRenderer tempRend = hatObjects[index].GetComponent<MeshRenderer>();
							if(tempRend != null)
								tempRend.enabled = true;
							SkinnedMeshRenderer skinnedRend = hatObjects[index].GetComponent<SkinnedMeshRenderer>();
							if(skinnedRend != null)
								skinnedRend.enabled = true;
							break;
						}
					}
				}
			else{
				for(int index = 0; index < hatObjects.Count; index++){
					if(index == ((int)selectedHat)){
						hatObjects[index].SetActive(true);
						MeshRenderer tempRend = hatObjects[index].GetComponent<MeshRenderer>();
						if(tempRend != null)
							tempRend.enabled = true;
						SkinnedMeshRenderer skinnedRend = hatObjects[index].GetComponent<SkinnedMeshRenderer>();
						if(skinnedRend != null)
							skinnedRend.enabled = true;
					}
					else
						hatObjects[index].SetActive(false);
				}
			}
		}
	}
}
