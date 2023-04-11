using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance=null;

    public AudioSource Music1;
    public AudioSource Music2;


    public List<AudioClip> bgMusic;
    public List<AudioClip> AudioEffectClips;

    public float MAX_VOLUME = 0.5f;
    public int currentMusicTrack = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }


        instance = this;
        DontDestroyOnLoad(gameObject);
        PlayRandomMusic();
    }
    float ScreamTimer = 0;
    // Update is called once per frame
    void Update()
    {
        if(Music1.isPlaying == false && Music2.volume == 0)
        {
            PlayRandomMusic();
        }
        ScreamTimer -= Time.deltaTime;
    }
    public void PlayRandomMusic()
    {
        if(Music1.isPlaying)
        {
            Music2.volume = 0;
            Music1.volume = MAX_VOLUME;
        }
        else
        {
            Music2.volume = 0;
            Music1.volume = MAX_VOLUME;
            Music1.clip = bgMusic[currentMusicTrack];
            Music1.Play();
            currentMusicTrack++;
            if (currentMusicTrack > bgMusic.Count - 1)
                currentMusicTrack = 0;
        }


    }
    public void PlayCheseMusic()
    {
        Music2.volume = MAX_VOLUME;
        Music1.volume = 0;
    }

    public void PlayAudioClip(int aIndex,float aDelay=0, float aVolume=1)
    {
        if(aIndex == 1 || aIndex == 2 )
        {
            if( ScreamTimer < 0 )
            {
                ScreamTimer = 5;
                StartCoroutine(PlayAfterTime(aIndex, aDelay, aVolume));
            }
        }
        else
        {
            StartCoroutine(PlayAfterTime(aIndex, aDelay, aVolume));

        }

    }
    IEnumerator PlayAfterTime(int aIndex, float aDelay, float aVolume)
    {
        yield return new WaitForSeconds(aDelay);

        GameObject go = new GameObject("AudioClip");
        go.transform.parent = transform;
        AudioSource audioS = go.AddComponent<AudioSource>();
        audioS.clip = AudioEffectClips[aIndex];
        audioS.Play();
        audioS.volume = aVolume;
        StartCoroutine(DestroyAfterTime(2.0f, go));
    }
    IEnumerator DestroyAfterTime(float time,GameObject go)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
    }

}
