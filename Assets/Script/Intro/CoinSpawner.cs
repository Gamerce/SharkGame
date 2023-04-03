using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
	public RewardBase rBase;
	public RectTransform fromLoc;
	public RectTransform toLoc;

	public GameObject template;
	public List<Vector2> velocitys = new List<Vector2>();
	public List<RectTransform> trans = new List<RectTransform>();
	public List<TMPro.TextMeshProUGUI> countTexts = new List<TMPro.TextMeshProUGUI>();

	public float pullAmount = 0.1f;
	int toSpawn = -1;
	float spawnIn;

	public Vector2 spawnDelay = new Vector2(0.3f, 0.7f);
	public Vector2 startVelocity = new Vector2(0.5f,1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(toSpawn > 0){
	        spawnIn -= Time.deltaTime;
			if(spawnIn < 0){
				Spawn();
			}
		}

		for(int index = 0; index < trans.Count; index++){
			trans[index].position += velocitys[index].GetVector3Z() * Time.deltaTime;
			velocitys[index] = Vector3.Lerp(velocitys[index], Vector3.zero, Time.deltaTime*3);
			velocitys[index] += (toLoc.position.GetVector2Z() - trans[index].position.GetVector2Z()) * pullAmount * Time.deltaTime;

			if((trans[index].position.GetVector2Z() - toLoc.position.GetVector2Z()).magnitude < 5){
				//AddCoin and fade;
				rBase.AddACoin();
				Destroy(trans[index].gameObject);
				velocitys.RemoveAt(index);
				trans.RemoveAt(index--);
			}
		}
    }

	public void StartSpawn(int amount){
		toSpawn = amount;
		spawnIn = spawnDelay.GetRandom();
		SetAmountText();
	}

	public void SetAmountText(){
		for(int index = 0; index < countTexts.Count; index++){
			countTexts[index].text = (toSpawn > 0 ? "+" + toSpawn.ToString() : "");
		}
	}

	public void Spawn(){
		toSpawn--;
		template.SetActive(toSpawn > 0);
		SetAmountText();
		velocitys.Add(new Vector2(Random.Range(-1,1), Random.Range(-1,1)).normalized * startVelocity.GetRandom());
		GameObject temp = Instantiate(template, template.transform.parent);
		temp.SetActive(true);
		trans.Add(temp.transform as RectTransform);
		trans[trans.Count-1].position = template.transform.position;
		spawnIn = spawnDelay.GetRandom();
		//toLoc.anchorMin = new Vector2(0.5f, 0.5f);
		//toLoc.anchorMax = new Vector2(0.5f, 0.5f);
		//fromLoc.anchorMin = new Vector2(0.5f, 0.5f);
		//fromLoc.anchorMax = new Vector2(0.5f, 0.5f);
	}
}
