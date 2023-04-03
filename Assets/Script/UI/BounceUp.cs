using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceUp : MonoBehaviour
{
	public float targetValue;
	public float currentVal;
	public float velocity;

	float direction = 1;

	public float decaySpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		float oldVal = currentVal;
        currentVal += velocity * Time.deltaTime;
		direction = oldVal < currentVal ? -1 : 1;
		velocity += (targetValue - currentVal);
		velocity = Mathf.Lerp(velocity, 0, Time.deltaTime * decaySpeed);
		transform.localScale = new Vector3(currentVal,currentVal,currentVal);
    }

	public void AddBounce(float amount){
		if(velocity >= 0)
			velocity += amount;// * direction;
		else
			velocity -= amount;
	}
}
