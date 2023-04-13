using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIScale : MonoBehaviour
{
	public Image scaleTarget;

	[Range(0,3)]
	public float scalePercent = 1;
	float targetAspectRatio = 1;

	public bool runningUpdate = true;

	private void Reset() {
		scaleTarget = GetComponent<Image>();
	}

	// Start is called before the first frame update
	void Start()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(runningUpdate)
			ApplyScale();
    }

	[ContextMenu("ApplyScale")]
	public void ApplyScale(){
		Vector2 size = scaleTarget.rectTransform.sizeDelta;
		Vector2 nativeSize = new Vector2(scaleTarget.sprite.texture.width, scaleTarget.sprite.texture.height);

		scaleTarget.rectTransform.sizeDelta = nativeSize * scalePercent;
	}
}
