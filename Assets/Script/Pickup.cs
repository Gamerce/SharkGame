using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float speed = 10;
    public GameObject pickupeffect;
    float collisionTimer = 0;
    public GameObject TargetTransform;
    public ProceduralMeshExploder.MeshExploder _meshExploder;
    // Start is called before the first frame update
    MeshRenderer _MeshRenderer;
    void Start()
    {
        _MeshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        _MeshRenderer.gameObject.SetActive(false);
    }

	private void OnEnable() {
		gameObject.SetLayer(LayerMask.NameToLayer("Cans"));
	}

	// Update is called once per frame
	void Update()
    {
        transform.Rotate(new Vector3(0,1,0),Time.deltaTime*speed,Space.World);
        //collisionTimer += Time.deltaTime;
        //if(collisionTimer>1)
        //{
        //    GetComponent<BoxCollider>().enabled = true;
        //}

        //transform.position = Vector3.Lerp(transform.position, TargetTransform.transform.position, Time.deltaTime * 3);



        float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(SharkPlayer.instance.transform.position.x, SharkPlayer.instance.transform.position.z));


        if(dist<8 && _MeshRenderer.gameObject.activeSelf == false)
        {
            _MeshRenderer.gameObject.SetActive( true);
            Vector3 endPos = transform.position;
            transform.position = endPos + new Vector3(0,100,0);
            transform.DOMove(endPos, 1f);
        }
  

    }
    void OnCollisionEnter(Collision collision)
    {
 
        if(collision.gameObject.name.Contains("HitBox"))
        {
            //GameObject go = GameObject.Instantiate(pickupeffect, transform.position, Quaternion.identity);
            //go.transform.parent = transform.parent;
            _meshExploder.Explode();
            Destroy(gameObject);

            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 position = contact.point;


            GameObject go = GameObject.Instantiate(GameHandler.instance.hitEffect);
            go.transform.position = transform.position;
            go.SetActive(true);

            GameHandler.instance.AddScore();

            MusicManager.instance.PlayAudioClip(0, 0.0f, 0.5f);

        }

    }
}
