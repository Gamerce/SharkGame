using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaker : MonoBehaviour
{
	public Camera render;
	public GameObject target;
	public int frameTarget = 12;
	public Vector3 rotationDirection;

	public Vector2Int texSize = new Vector2Int(128,128);

	public string saveLocation;
	public string saveName;


    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void Reset() {
		render = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	[ContextMenu("CreateSprites")]
	public void CreateSprites(){
		RenderTexture tempTex = new RenderTexture(texSize.x, texSize.y, 8);
		RenderTexture old = render.targetTexture;
		render.targetTexture = tempTex;
		float rotationPerFrame = 360.0f/((float)frameTarget);
		rotationDirection = rotationDirection.normalized;
		for(int index = 0; index < frameTarget; index++){
			target.transform.Rotate(rotationDirection * rotationPerFrame);
			render.Render();
			tempTex.Save(saveLocation, saveName + "_" + index.ToString("000"));
		}
		render.targetTexture = old;
		DestroyImmediate(tempTex);
	}
}
