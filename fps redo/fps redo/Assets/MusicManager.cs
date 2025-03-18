using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    AudioSource source;
    private void Awake()
    {
        if(instance==null)
        {
            instance=this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        source=GetComponent<AudioSource>();
    }
    public void SwitchMusic(AudioClip music)
    {
        source.clip=music;
        source.Play();
    }
}
