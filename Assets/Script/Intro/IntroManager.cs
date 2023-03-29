using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
	public Image img;
	public bool testRewardOnClick = false;
	public GameObject testObj;
	public GameObject rewardBasePrefab;

	public List<GameObject> introObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0)){
			UTween temp = UTween.Fade(img, new Vector2(0,1), 0.5f);
			temp.onDone = ()=>{
				if(testRewardOnClick){
					TestReward();
				}
				else
					LoadNextScene();
			};
		}
    }

	public void LoadNextScene(){
		 SceneManager.LoadScene("CompleteTown 1", LoadSceneMode.Single);
	}

	public void TestReward(){
		img.gameObject.SetActive(false);
		GameObject temp = Instantiate(rewardBasePrefab);
		temp.SetActive(true);
		RewardBase baseVal = temp.GetComponent<RewardBase>();
		baseVal.Init((int)Random.Range(10,15));

		for(int index = 0; index < introObjects.Count; index++){
			introObjects[index].SetActive(false);
		}
	}
}
