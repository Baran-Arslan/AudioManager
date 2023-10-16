using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class AudioManager : MonoBehaviour
{
    [SerializeField, Tooltip("You can add AudioMixer to your prefab")]
    private AudioSource sfxSourcePrefab;
    private static ObjectPool<AudioSource> sfxSourcePool;
    
    private static AudioManager instance;

    private void Awake()
    {
        if (instance != null)
            instance = this;
        else
            Destroy(gameObject);


        sfxSourcePool = new ObjectPool<AudioSource>
            (() =>
            {//Create
                AudioSource audioSource = Instantiate(sfxSourcePrefab);
                return audioSource;
            }
            , audioSource =>
            {//On Take
                audioSource.gameObject.SetActive(true); 
            }
            , audioSource =>
            {//On Return
                audioSource.gameObject.SetActive(false);
            }
            , audioSource =>
            {//On Destroy
               Destroy(audioSource.gameObject);
            }
            , true, 5, 100
            );
    }




    public static void PlaySFX(AudioClip clip, Vector3 playPosition, float volume = 1)
    {
        AudioSource audioSource = sfxSourcePool.Get();
        audioSource.transform.position = playPosition;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = clip.length;

        // Return the audio source to the pool after the clip has finished playing
        instance.StartCoroutine(ReturnToPool(audioSource, clipLength));
    }
    public static void PlaySFX(AudioClip[] clips, Vector3 playPosition, float volume = 1)
    {
        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        PlaySFX(randomClip, playPosition, volume);
    }
    private static IEnumerator ReturnToPool(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        sfxSourcePool.Release(audioSource);
    }
}
