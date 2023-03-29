using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleThis : MonoBehaviour
{
	public AnimationCurve scaleCurve = Extensions.GetInOutFadeCurve(0.5f);
	public float scaleSpeed = 3;
	public float scaleMuliplier;

	float scaleTime;
	Vector3 startScale;

	// Start is called before the first frame update
	void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        scaleTime += Time.deltaTime * scaleSpeed;
		transform.localScale = startScale * (1 + scaleMuliplier * scaleCurve.Evaluate(scaleTime%1));
    }
}
