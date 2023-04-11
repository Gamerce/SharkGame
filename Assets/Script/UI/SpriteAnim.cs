using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnim : MonoBehaviour
{
	public Image target;
	public Vector2Int spriteXY;
	public float fps;
	public float speed = 1;
	public bool playForward = true;

	float atTime;
	Material tempMat;

	public Vector2 randomTime;
	public int debugID;
	public Vector2Int debugFrame;

    // Start is called before the first frame update
    void Start()
    {
        tempMat = Instantiate(target.material);
		target.material = tempMat;
		atTime = randomTime.GetRandom();
		//Debug.LogError("Spawned Mat:" + tempMat.name.ToString(), gameObject);
    }

	private void Reset() {
		target = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update()
    {
		if(playForward)
	        atTime += Time.deltaTime * speed;
		else
			atTime += Time.deltaTime * speed;
		int frame = (int)((atTime * fps)%(float)(spriteXY.x * spriteXY.y));
		SetFrame(frame);
    }

	public void SetFrame(int id){
		int xID = id % spriteXY.x;
		int yID = id / spriteXY.x;
		float xStep = 1.0f / (float)(spriteXY.x);
		float yStep = 1.0f / (float)(spriteXY.y);
		debugFrame.x = xID;
		debugFrame.y = yID;
		debugID = id;

		tempMat.mainTextureScale = new Vector2(xStep, yStep);
		tempMat.mainTextureOffset = new Vector2(xStep * (float)xID, (1.0f-yStep) - yStep * (float)yID);
	}
}
