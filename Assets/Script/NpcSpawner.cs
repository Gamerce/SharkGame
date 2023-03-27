using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    public List<Transform> wpPath;
    // Start is called before the first frame update
    void Start()
    {

        GameObject np = GameObject.Instantiate(GameHandler.instance.NpcPrefab,transform.position,Quaternion.identity);
        np.gameObject.SetActive(true);
        GameHandler.instance.AllNPC.Add(np.GetComponent<NPC>());

        np.GetComponent<NPC>().wpPath = wpPath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
