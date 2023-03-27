using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handmovment : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float horizontalSpeed = 2.0F;
    public float verticalSpeed = 2.0F;

    public float Idletimer = 0;

    public Animator anim;

    public List<BoxCollider> bx;


    public float minRotation = -45;
    public float maxRotation = 45;
    public Vector3 currentEulerAngles;
    public float rotationSpeed = 1;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");




            //    transform.Rotate(-h, 0, -v);

            Debug.Log(currentEulerAngles);
            //modifying the Vector3, based on input multiplied by speed and time
            currentEulerAngles += new Vector3(0, v, 0) * Time.deltaTime * rotationSpeed;


            if (currentEulerAngles.y > 10)
                currentEulerAngles.y = 10;

            if (currentEulerAngles.y < -10)
                currentEulerAngles.y = -10;



            //apply the change to the gameObject
            transform.localEulerAngles = currentEulerAngles;



            //anim.enabled = false;
            //Idletimer = 0;
            //for(int i =0; i < bx.Count;i++)
            //{
            //    bx[i].isTrigger = false;
            //}



        }
        else
        {
            //Idletimer += Time.deltaTime;
            //for (int i = 0; i < bx.Count; i++)
            //{
            //    bx[i].isTrigger = true;
            //}
        }
    

        //if(Idletimer>1)
        //{
        //    if(anim.enabled == false)
        //    {
        //        anim.enabled = true;
        //        anim.CrossFade("idleShark",0.3f);
        //    }
            
        //}


    }


}
