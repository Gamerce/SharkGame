using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarFall : MonoBehaviour
{
	public Vector3 tiltDirection;

	Transform target;
	Vector3 velocity = Vector3.zero;
	Vector3 collissionDir;
	public float fallSpeed = 5;
	Vector3 tilt;

	bool isFalling = false;
	[Range(0,10)]
	public float tiltStrength = 3;
	public GameObject splatter;

	public Transform testObj;
    // Start is called before the first frame update
    void Start()
    {
        angle = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(isFalling)
			UpdateFalling();
		//TestFalling();
    }
	public float angle = 0;
	public Vector3 forwardMod = new Vector3(90,0,0);
	void TestFalling(){
		tilt = testObj.position - transform.position;
		tilt.y = 0;

		Vector3 dir = tilt.normalized;
		Vector3 axis = Vector3.Cross(-dir, Vector3.up);
		float tiltAmount = tilt.magnitude;
		angle = ((Mathf.Clamp(tiltAmount, 0, tiltStrength) / tiltStrength)) * 90;
		if(angle > 80){
			enabled = false;
			Vector3 localEu = splatter.transform.localRotation.eulerAngles;
			splatter.transform.parent = transform.parent;
			splatter.transform.localRotation = Quaternion.Euler(localEu);
			splatter.transform.position = target.position;
			splatter.SetActive(true);
		}
		transform.rotation = Quaternion.AngleAxis(angle, axis);



		//transform.LookAt(transform.position + tilt, Vector3.right);
		////transform.localRotation = transform.localRotation * Quaternion.Euler(forwardMod);
		//float tiltAmount = tilt.magnitude;
		//transform.localRotation = Quaternion.Lerp(Quaternion.identity, transform.localRotation, (tiltAmount/tiltStrength).Clamp01());
	}

	void UpdateFalling(){
		tilt += velocity * Time.deltaTime;
		//if(Vector3.Dot(tilt.normalized, transform.position - target.position) < 0){
		//	velocity -= (transform.position - target.position).normalized * Time.deltaTime;
		//}
		//else
		//	velocity += (transform.position - target.position).normalized * Time.deltaTime;
		//velocity += (target.position - transform.position).normalized * Time.deltaTime * fallSpeed;
		velocity += collissionDir * Time.deltaTime * fallSpeed;
		velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime);
		
		Vector3 dir = tilt.normalized;
		Vector3 axis = Vector3.Cross(-dir, Vector3.up);
		float tiltAmount = tilt.magnitude;
		angle = ((Mathf.Clamp(tiltAmount, 0, tiltStrength) / tiltStrength)) * 90;
		if(angle > 90){
			enabled = false;
			//Vector3 localEu = splatter.transform.localRotation.eulerAngles;
			//splatter.transform.parent = transform.parent;
			//splatter.transform.localRotation = Quaternion.Euler(localEu);
			//splatter.transform.position = target.position;
			//splatter.SetActive(true);
		}
		transform.rotation = Quaternion.AngleAxis(angle, axis);

		//float tiltAmount = tilt.magnitude;
		//transform.LookAt(transform.position + tilt);
		//transform.localRotation = Quaternion.Lerp(Quaternion.identity, transform.localRotation, (tiltAmount/tiltStrength).Clamp01());
	}

	public void TriggerFall(Transform hitTarget, float hitSize = 0.5f){
		target = hitTarget;
		collissionDir = (transform.position - target.position).normalized;
		velocity = collissionDir;
		isFalling = true;
	}

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.black;
		Gizmos.DrawLine(transform.position + Vector3.up*1.5f, transform.position + Vector3.up*1.5f + tiltDirection);
	}
}
