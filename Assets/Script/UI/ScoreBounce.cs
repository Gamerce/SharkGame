using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBounce : MonoBehaviour
{
	public float targetValue;
	public float currentVal;
	public float velocity;

	float direction = 1;

	public float bounceSpeed = 1;
	public float decaySpeed = 1;

	public bool randDir = false;

	public Vector2 scaleScalerSubAdd = new Vector2(1,1);
	
	Vector3 randomDir = Vector3.one;
	Vector3 randomDirTarget = Vector3.one;
	Vector3 randomChanceIn = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		float oldVal = currentVal;
        currentVal += velocity * Time.deltaTime * bounceSpeed;
		direction = oldVal < currentVal ? -1 : 1;
		velocity += -currentVal;
		velocity = Mathf.Lerp(velocity, 0, Time.deltaTime * (decaySpeed+bounceSpeed.Abs()));
		if(randDir){
			UpdateRandom();
			float currentValMod = currentVal * (currentVal < 0 ? scaleScalerSubAdd.x : scaleScalerSubAdd.y);
			transform.localScale = new Vector3(targetValue + currentValMod*randomDir.x,targetValue + currentValMod * randomDir.y,targetValue + currentValMod * randomDir.z);
		}
		else
			transform.localScale = new Vector3(targetValue + currentVal,targetValue + currentVal,targetValue + currentVal);
    }

	void UpdateRandom(){
		randomDir.x = randomDir.x.MoveTowards(randomDirTarget.x, Time.deltaTime*2);
		randomDir.y = randomDir.y.MoveTowards(randomDirTarget.y, Time.deltaTime*2);
		randomDir.z = randomDir.z.MoveTowards(randomDirTarget.z, Time.deltaTime*2);
		randomChanceIn -= Vector3.one * Time.deltaTime;
		if(randomChanceIn.x <= 0){
			randomChanceIn.x = Random.Range(0.4f, 0.75f);
			randomDirTarget.x = Random.Range(0.5f, 1.5f);
		}
		if(randomChanceIn.y <= 0){
			randomChanceIn.y = Random.Range(0.4f, 0.75f);
			randomDirTarget.y = Random.Range(0.5f, 1.5f);
		}
		if(randomChanceIn.z <= 0){
			randomChanceIn.z = Random.Range(0.4f, 0.75f);
			randomDirTarget.z = Random.Range(0.5f, 1.5f);
		}
	}

	public void AddBounce(float amount){
		if(velocity >= 0)
			velocity += amount;// * direction;
		else
			velocity -= amount;
	}
}
