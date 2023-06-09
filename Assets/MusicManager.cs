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
	
	List<AudioSource> activeSounds = new List<AudioSource>();
	List<float> activeSoundVolumes = new List<float>();

    public float MAX_VOLUME = 0.5f;
    public int currentMusicTrack = 0;

	public bool isSoundOn = true;
	public bool isMusicOn = true;

	bool forcePause = false;

	//public bool pingSuccessFull = true;
	bool exitPing = false;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }


        instance = this;
		isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
		isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
		//bool isVibrateOn = PlayerPrefs.GetInt("VibrateOn", 1) == 1;


        DontDestroyOnLoad(gameObject);
        PlayRandomMusic();
        //StartCoroutine(CheckPing());
    }
    float ScreamTimer = 0;
	List<bool> isPlaying = new List<bool>();
    // Update is called once per frame
    void Update()
    {
		if(forcePause){
			return;
		}
        if(Music1.isPlaying == false && Music2.volume == 0)
        {
            PlayRandomMusic();
        }
        ScreamTimer -= Time.deltaTime;
    }

	
    //System.Collections.IEnumerator CheckPing()
    //{
	//	while(!exitPing){
 	//
	//		if(pingSuccessFull)
	//			yield return new WaitForSeconds(25.0f);
	//		else
	//			yield return new WaitForSeconds(5.0f);
	//		pingSuccessFull = false;
	//		var ping = new Ping("8.8.8.8");
	//		while (!ping.isDone)
	//			yield return new WaitForSeconds(0.1f);
 	//
	//		pingSuccessFull = ping.time < 3;
	//		ping.DestroyPing();
	//	}
    //}

	public void PauseMusic(){
		if(!forcePause){
			isPlaying.Clear();
			isPlaying.Add(Music1.isPlaying);
			isPlaying.Add(Music2.isPlaying);
			if(Music1.isPlaying)
				Music1.Pause();
			if(Music2.isPlaying)
				Music2.Pause();
		}
		enabled = false;
		forcePause = true;
	}

	public void ResumeMusic(){
		if(forcePause){
			if(isPlaying.Count > 0 && isPlaying[0])
				Music1.UnPause();
			if(isPlaying.Count > 1 && isPlaying[1])
				Music2.UnPause();
		}
		enabled = true;
		forcePause = false;
	}

    public void PlayRandomMusic()
    {
        if(Music1.isPlaying)
        {
            Music2.volume = 0;
            Music1.volume = isMusicOn ? MAX_VOLUME : 0;
        }
        else
        {
            Music2.volume = 0;
            Music1.volume = isMusicOn ? MAX_VOLUME : 0;
            Music1.clip = bgMusic[currentMusicTrack];
            Music1.Play();
            currentMusicTrack++;
            if (currentMusicTrack > bgMusic.Count - 1)
                currentMusicTrack = 0;
        }
    }

    public void PlayCheseMusic()
	{
		Music2.volume = isMusicOn ? MAX_VOLUME : 0;
		Music1.volume = 0;
    }

	public void SetMusicOnOff(bool isOn){
		isMusicOn = isOn;
        Music2.volume = isMusicOn ? MAX_VOLUME : 0;
        Music1.volume = (isMusicOn && !Music2.isPlaying) ? MAX_VOLUME : 0;
		PlayerPrefs.SetInt("MusicOn", isMusicOn ? 1 : 0);
	}

	public void SetSoundOnOff(bool isOn){
		isSoundOn = isOn;
		PlayerPrefs.SetInt("SoundOn", isMusicOn ? 1 : 0);
		for(int index = 0; index < activeSounds.Count; index++){
			activeSounds[index].volume = isOn ? activeSoundVolumes[index] : 0;
		}
	}

    public void PlayAudioClip(int aIndex,float aDelay=0, float aVolume=1)
    {
		if(!isSoundOn)
			return;
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
		activeSounds.Add(audioS);
		activeSoundVolumes.Add(aVolume);
        audioS.clip = AudioEffectClips[aIndex];
        audioS.Play();
        audioS.volume = aVolume;
        StartCoroutine(DestroyAfterTime(2.0f, go));
    }
    IEnumerator DestroyAfterTime(float time,GameObject go)
    {
		for(int index = activeSounds.Count-1; index >= 0; index--){
			if(activeSounds[index] == null){
				activeSoundVolumes.RemoveAt(index);
				activeSounds.RemoveAt(index);
			}
		}
		if(go != null){
			int id = activeSounds.GetId(go.GetComponent<AudioSource>());
			activeSounds.RemoveAt(id);
			activeSoundVolumes.RemoveAt(id);
		}
        yield return new WaitForSeconds(time);
        Destroy(go);
    }
}
