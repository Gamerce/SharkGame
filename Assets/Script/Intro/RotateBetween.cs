using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBetween : MonoBehaviour
{

	public Vector3 minRot;
	public Vector3 maxRot;

	public AnimationCurve curve = Extensions.GetInOutFadeCurve(0.5f);
	public float speed = 1;
	float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * speed;
		transform.localRotation = Quaternion.Euler(Vector3.Lerp(minRot, maxRot, curve.Evaluate(time%1)));
    }
}
