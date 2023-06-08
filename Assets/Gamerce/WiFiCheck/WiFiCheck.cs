using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiFiCheck : MonoBehaviour
{
	public List<GameObject> wifiImges = new List<GameObject>();

	public int atID = 1;
	bool goingUp = true;
	float nextChangeIn = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	public void Show(){
		//gameObject.SetActive(!MusicManager.instance.pingSuccessFull);
		gameObject.SetActive(Application.internetReachability == NetworkReachability.NotReachable);
	}

    // Update is called once per frame
    void Update()
    {
		//if(MusicManager.instance.pingSuccessFull)
		//	gameObject.SetActive(false);
		if(Application.internetReachability != NetworkReachability.NotReachable)
			gameObject.SetActive(false);
        nextChangeIn -= Time.deltaTime;
		if(nextChangeIn <= 0){
			nextChangeIn = Random.Range(0.3f,0.5f);
			int randVal = Random.Range(0,100);
			if(randVal > 33){
				if(goingUp)
					atID++;
				else
					atID--;
			}
			else{
				if(!goingUp)
					atID++;
				else
					atID--;
			}

			if(atID > wifiImges.Count){
				atID = wifiImges.Count;
				goingUp = false;
			}
			else if(atID <= 0){
				atID = 0;
				goingUp = true;
			}

			EnabaleImages();
		}
    }

	void EnabaleImages(){
		for(int index = 0; index < wifiImges.Count; index++){
			wifiImges[index].SetActive(index < atID);
		}
	}
}
