using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SharkHeadFill : MonoBehaviour
{
	public Image fillImg;
	public RawImage rawFillImag;
	public Material fillMat;

	Material usedMat;
	[Range(0,1)]
	public float fillAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(rawFillImag != null && fillMat != null){
			usedMat = Instantiate(fillMat);
			rawFillImag.material = usedMat;
		}
    }

    // Update is called once per frame
    void Update()
    {
		if(fillImg != null)
	        fillImg.fillAmount = fillAmount;
		if(rawFillImag != null)
			usedMat.SetFloat("_fillAmount", fillAmount);
    }
}
