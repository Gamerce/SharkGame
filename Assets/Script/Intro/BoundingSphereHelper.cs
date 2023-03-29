using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingSphereHelper : MonoBehaviour
{
	public Camera displayCam;
	public RewardBase rewarder;
	public List<SkinnedMeshRenderer> renders = new List<SkinnedMeshRenderer>();

	public Vector3 center;
	public float radius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	[ContextMenu("Recalc Full")]
	public void RecalcFull(){
		renders.Clear();
		renders.AddRange(GetComponentsInChildren<SkinnedMeshRenderer>());
		CalculateBoundingSphere();
	}

	public void CalculateBoundingSphere(){
		Vector3 minPos = new Vector3( 999999999, 999999999, 999999999);
		Vector3 maxPos = new Vector3(-999999999,-999999999,-999999999);

		for(int index = 0; index < renders.Count; index++){
			Vector3[] verts = renders[index].sharedMesh.vertices;
			for(int i = 0; i < verts.Length; i++){
				minPos = Vector3.Min(minPos, verts[i] + renders[index].transform.position-transform.position);
				maxPos = Vector3.Max(maxPos, verts[i] + renders[index].transform.position-transform.position);
			}
		}

		center = minPos + (maxPos-minPos)/2;
		radius = (maxPos-minPos).magnitude/2;

		float dist = center.magnitude;
		Vector3[] corners = displayCam.GetWorldCorners(dist);
		
		Vector3 topPos = Intersection.GetClosestPoint(corners[1], corners[2], center);
		Vector3 botPos = Intersection.GetClosestPoint(corners[0], corners[3], center);
		
		float distToBot = (botPos - center).magnitude;
		float distToTop = (topPos - center).magnitude;
		float totalDist = distToTop + distToBot;

		rewarder.minMaxVal = new Vector2((distToBot-radius)/totalDist, 1-(distToTop-radius)/totalDist);
	}

	public bool debugShowGizmos = false;
	private void OnDrawGizmosSelected() {
		if(!debugShowGizmos)
			return;
		Gizmos.DrawSphere(transform.position + center, radius);

		float dist = center.magnitude;
		Vector3[] corners = displayCam.GetWorldCorners(dist);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(corners[0], 0.1f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(corners[1], 0.1f);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(corners[2], 0.1f);
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(corners[3], 0.1f);
	}
}
