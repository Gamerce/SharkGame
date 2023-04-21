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

        if (_npc.animator.GetCurrentAnimatorStateInfo(0).IsName("landing"))
        {
            if( _npc.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <0.7f)
                return;
        }

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
					GameHandler.instance.AddForce(0.5f,0.2f);
					
                    _npc._hitDelay = 0.05f;
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
							else{
								GameHandler.instance.AddForce(0.5f,0.2f);
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
            if(collision.gameObject.name ==  "Train_Back" && _npc.isBoss)
            {

                 collision.transform.parent.GetChild(0).gameObject.GetComponent<MeshCollider>().enabled = false;
                collision.transform.parent.GetChild(1).gameObject.GetComponent<MeshCollider>().enabled = false;
                collision.transform.parent.GetChild(2).gameObject.GetComponent<MeshCollider>().enabled = false;


                for (int i = 0; i < _npc.allRG.Length; i++)
                {

                    _npc.allRG[i].AddForceAtPosition(new Vector3(0, 0.5f, -1) * 800, transform.position);
                }
                GameObject go= GameObject.Instantiate(GameHandler.instance.BoomEffect, _npc.allRG[0].transform.position+ new Vector3(0,0,0.5f), Quaternion.identity);
                go.SetActive(true);
                GameHandler.instance.SetSlowMotion();
            }
            if (collision.gameObject.name == "HotAirBalloon_BlueFLY" && _npc.isBoss)
            {
                collision.gameObject.GetComponent<Animator>().enabled = true;
                GameObject go = GameObject.Instantiate(GameHandler.instance.BoomEffect, _npc.allRG[0].transform.position + new Vector3(0, 0, 0.5f),Quaternion.Euler(180,0,0));
                go.SetActive(true);
                _npc.FollowObject(collision.gameObject);
            }
            if (collision.gameObject.name == "FIRE" && _npc.isBoss)
            {


                _npc.FireEffect.SetActive(true);
            }
            if (collision.gameObject.name == "ShreedCharacter" && _npc.isBoss)
            {
                //GameHandler.instance.SetSlowMotion();

                _npc.gameObject.SetActive(false);
                collision.transform.GetChild(0).gameObject.SetActive(true);
            }
            if (collision.gameObject.name == "Water" && _npc.isBoss)
            {
                //GameHandler.instance.SetSlowMotion();

                //for (int i = 0; i < _npc.allRG.Length; i++)
                //{

                //    _npc.allRG[i].isKinematic = true;
                //}
                collision.transform.GetComponent<BoxCollider>().enabled = false;
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).transform.position = new Vector3(_npc.spine.transform.position.x, collision.transform.GetChild(0).transform.position.y, _npc.spine.transform.position.z);
             }
            if (collision.gameObject.name == "Poison" && _npc.isBoss)
            {

                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetComponent<BoxCollider>().enabled = false;

            }
            if (collision.gameObject.name == "PopBallopns" && _npc.isBoss)
            {

                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetComponent<BoxCollider>().enabled = false;
                collision.transform.GetChild(1).gameObject.SetActive(false);

            }

			if(_npc.isBoss){
				CheckBossEffects(collision);
			}
			else
				CheckRegularNpcEffects(collision);




        }
    }

	void CheckRegularNpcEffects(Collision collision){
		if(collision.gameObject.name == "WaterTower"){
			if(collision.gameObject.GetComponent<UTween>() == null){
				UTween.Wait(collision.gameObject, 0.3f, null);
				Animation anim = collision.transform.parent.GetComponent<Animation>();
				if(anim.clip != null && anim.clip.name == "WaterTowerAnim")
					anim.Play("WaterTowerAnim");
				if(anim.clip != null && anim.clip.name == "WaterTowerAnim2")
					anim.Play("WaterTowerAnim2");
				List<Transform> kids = new List<Transform>();
				for(int index = 0; index < collision.transform.childCount; index++){
					Transform trans = collision.transform.GetChild(index);
					if(!trans.gameObject.activeSelf)
						kids.Add(trans);
				}
				if(kids.Count > 0){
					Transform target = kids[kids.Count == 1 ? 0 : Random.Range(0, kids.Count-1)];
					target.gameObject.SetActive(true);
				}
			}
		}
		
	}

	void CheckBossEffects(Collision collision){
		if(collision.gameObject.name == "ToiletCollider"){
			collision.transform.parent.GetComponent<Animation>().Play("ToiletAnim");
		}
		if(collision.gameObject.name == "ToiletCollider"){
			collision.transform.parent.GetComponent<Animation>().Play("ToiletAnim");
		}
		if(collision.gameObject.name == "InsideCar"){
			Animation anim = collision.transform.parent.GetComponent<Animation>();
			//anim.pla
			collision.transform.parent.GetComponent<Animation>().Play("CarDriving");
			collision.transform.GetChild(0).gameObject.SetActive(true);
			_npc.transform.SetParent(collision.transform, true);
		}
		if(collision.gameObject.name == "Pillar 02 (8)"){
			PillarFall faller = collision.transform.GetComponent<PillarFall>();
			faller.TriggerFall(_npc.transform.Find("Hips"));
			//anim.pla
			//collision.transform.parent.GetComponent<Animation>().Play("CarDriving");
			//collision.transform.GetChild(0).gameObject.SetActive(true);
			//_npc.transform.SetParent(collision.transform, true);
		}

		
	}
}
