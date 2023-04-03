using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThis : MonoBehaviour
{
	public Vector3 angleDir;
	public float speed = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(angleDir * speed * Time.deltaTime);
    }
}
