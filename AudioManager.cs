using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance;
    private static ObjectPool<AudioSource> sfxSourcePool;
    [SerializeField, Tooltip("You can add AudioMixer to your prefab and make prefab 3D")]
    private AudioSource sfxSourcePrefab;

    [SerializeField] private SoundSO soundSO;
    public static SoundSO Sounds;

    private static Transform mainCamera;


    private void Awake()
    {
        Instance = this;


        Sounds = soundSO;

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

        mainCamera = Camera.main.transform;
    }
    

    public static void PlaySFX(SoundData soundData, Vector3 playPosition = default(Vector3))
    {
        if (playPosition == default(Vector3))
            playPosition = mainCamera.position;


        AudioSource audioSource = sfxSourcePool.Get();
        audioSource.transform.position = playPosition;
        audioSource.clip = soundData.clip;
        audioSource.volume = soundData.volume;
        audioSource.Play();

        float clipLength = soundData.clip.length;

        Instance.StartCoroutine(ReturnToPool(audioSource, clipLength));
    }

    public static void PlaySFX(SoundData[] soundDataArray, Vector3 playPosition = default(Vector3))
    {
        SoundData randomSoundData = soundDataArray[Random.Range(0, soundDataArray.Length)];
        PlaySFX(randomSoundData, playPosition);
    }
    private static IEnumerator ReturnToPool(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        sfxSourcePool.Release(audioSource);
    }
}
