using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionObject : MonoBehaviour
{
    public NPC _npc;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
  
    }
    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;

        //        Debug.Log("Collision:" + collision.gameObject.name);

        //float impactforce = collision.relativeVelocity.x + collision.relativeVelocity.y + collision.relativeVelocity.z;
        //Debug.Log(impactforce);
        //if (impactforce > 1.5 || impactforce < -1.5)
        //{
        //if (_npc.isRagdool == false)
        //{
        //    //GetComponent<BoxCollider>().enabled = false;
        //    //_npc.Explode(collision);
        //}
        //else
        //{
        //    _npc.LandEffect(collision);
        //}
        //}



        if (_npc.isRagdool == false)
        {

   
        Vector3 pos1 = SharkPlayer.instance._sharkarm.SharkHead.transform.position;
        Vector3 pos2 = transform.position;
        float dist = Vector2.Distance(new Vector2(pos1.x, pos1.z), new Vector2(pos2.x, pos2.z));

        dist = Mathf.Abs(pos1.x - pos2.x);
        Rigidbody rb = SharkPlayer.instance._sharkarm.SharkHead.GetComponent<Rigidbody>();
        if (Vector3.Distance(SharkPlayer.instance.avrDir[0], SharkPlayer.instance.avrDir[4]) > 1.5f)
        {

            if ( _npc._hitDelay <= 0)
            {
                if (_npc.isBoss && _npc.ArmourParts.Count > 0)
                {
                        _npc.HitEffect(collision);
                        int dirSwing = SharkArmFollowObject.instace.GetDirectionSwing();

                    if (dirSwing == -1)
                    {
                         _npc.GetComponent<Animator>().Play("headhit", 0, 0);
                        _npc.hitAnimation = 1;
                    }
                    else
                    {
                            _npc.GetComponent<Animator>().Play("headhit2", 0, 0);
                        _npc.hitAnimation = 0;

                    }

                    if (_npc.hitCount >= 3)
                    {
                        _npc.ArmourParts[0].transform.parent = null;
                        _npc.ExplodeObject(_npc.ArmourParts[0]);
                        _npc.ArmourParts.RemoveAt(0);
                        _npc.hitCount = 0;
                    }
                    _npc._hitDelay = 0.4f;
                    _npc.hitCount++;
                }
                else
                {
                        _npc.HitEffect(collision);
                        if(_npc.isBoss)
                        {
                            int dirSwing = SharkArmFollowObject.instace.GetDirectionSwing();
                            if (dirSwing == -1)
                            {
                                _npc.GetComponent<Animator>().Play("headhit", 0, 0);
                                _npc.hitAnimation = 1;
                            }
                            else
                            {
                                _npc.GetComponent<Animator>().Play("headhit2", 0, 0);
                                _npc.hitAnimation = 0;

                            }

                            if (_npc.hitCount >= 3)
                            {
                                _npc.Explode();
                                _npc.hitCount = 0;
                            }
                            _npc._hitDelay = 0.4f;
                            _npc.hitCount++;
                        }
                        else
                        {
                            _npc.Explode();
                        }
        

      
                }


            }
        }
        }
        else
        {
           _npc.LandEffect(collision);
        }
    }
}
