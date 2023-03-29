using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkArmFollowObject : MonoBehaviour
{
    public GameObject followTarget;
    public GameObject originalPoint;
    public GameObject targetRotation;
    public float Speed = 1;
    public float Speed2 = 1;

    public Rigidbody armBase;
    public float force = 1;
    public Animator _anim;
    public float crossfadeTime = 0.1f;
    public float whenIdlr = 0.1f;
    public static SharkArmFollowObject instace;
    // Start is called before the first frame update
    void Start()
    {
        instace = this;
    }
    float positionDelta = 0;
    Vector3 posLastFrame;
    public float treshholdANim;
    public float crossfadeTimeIdle = 0.1f;
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, followTarget.transform.position, Time.deltaTime * Speed);
        transform.forward = Vector3.Lerp(transform.forward, -targetRotation.transform.forward, Time.deltaTime * Speed2);

        Vector3 dif = transform.position - followTarget.transform.position;

        //armBase.AddForceAtPosition(dif* force, armBase.transform.position+ dif);



        float angle = AngleSigned(transform.forward, -targetRotation.transform.forward, -targetRotation.transform.up);



        //Debug.Log(angle);
        if (Vector3.Distance(transform.forward, -targetRotation.transform.forward) < whenIdlr)
            _anim.CrossFade("SharkArmIdle", crossfadeTimeIdle);
        else if (angle < treshholdANim)
            _anim.CrossFade("SharkArmLeft", crossfadeTime);
        else if (angle > -treshholdANim)
            _anim.CrossFade("SharkArmRight", crossfadeTime);
        else
            _anim.CrossFade("SharkArmIdle", crossfadeTimeIdle);


    }
    public float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }
    public int GetDirectionSwing()
    {
        float angle = AngleSigned(transform.forward, -targetRotation.transform.forward, -targetRotation.transform.up);
        if (Vector3.Distance(transform.forward, -targetRotation.transform.forward) < whenIdlr)
            return 0;
        else if (angle < treshholdANim)
            return 1;
        else if (angle > -treshholdANim)
            return -1;
        else
            return 0;
    }

}
