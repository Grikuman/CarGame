// ============================================
// 
// ファイル名: AudioManager.cs
// 概要: オーディオ全般を管理（シングルトン）
// 
// 製作者 : 清水駿希
// 
// ============================================
using System.Collections.Generic;
using ShunLib.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance => Singleton<AudioManager>.Instance;

    public static readonly string mainVolumeParam  = "Volume";
    public static readonly string sfxVolumeParam   = "SFXVol";
    public static readonly string uiVolumeParam    = "UIVol";
    public static readonly string musicVolumeParam = "MusicVol";

    public AudioSource musicSource;

    public AudioMixer masterMixer;
    public AudioMixerGroup sfxMixer;
    public AudioMixerGroup uiMixer;
    public AudioMixerGroup musicMixer;

    [SerializeField] private AudioBank soundBank;
    [SerializeField] private AudioBank musicBank;



    private void Awake()
    {
        InitBanks();
        musicSource.outputAudioMixerGroup = musicMixer;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitMixer();
    }

    // バンクの初期化処理
    private void InitBanks()
    {
        soundBank.Build();
        musicBank.Build();
    }

    private void InitMixer()
    {
        SetMixerFromPref(mainVolumeParam);
        SetMixerFromPref(musicVolumeParam);
        SetMixerFromPref(sfxVolumeParam);
        SetMixerFromPref(uiVolumeParam);
    }

    public static void Play(string clip, AudioMixerGroup mixerTarget, Vector3? position = null)
    {
        if (Instance.soundBank.TryGetAudio(clip, out AudioClip audioClip))
        {
            AudioSourceObject audioObj = AudioSourceContainer.Instance.GetAudioSourceObject();
            AudioSource src = audioObj._audioSource;
            if (position.HasValue)
            {
                audioObj.transform.position = position.Value;
                src.spatialBlend = 1;
                src.rolloffMode = AudioRolloffMode.Linear;
                src.maxDistance = 50;
                src.dopplerLevel = 0;
            }
            audioObj._audioSource.clip = audioClip;
            audioObj._audioSource.outputAudioMixerGroup = mixerTarget;
            audioObj._audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"AudioClip '{clip}' not present in audio bank");
        }
    }


    public static void PlayMusic(string music)
    {
        if(string.IsNullOrEmpty(music) == false)
        {
            if(Instance.musicBank.TryGetAudio(music,out AudioClip audio))
            {
                Instance.musicSource.clip = audio;
                Instance.musicSource.Play();
            }
            else
            {
                Debug.LogWarning($"AudioClip '{music}' not present in music bank");
            }
        }
    }

    public void PlaySFX()
    {
        Play("SelectSFX", sfxMixer);
    }

    public void PlayUI()
    {
        Play("WindowUI", uiMixer);
    }

    // Volume

    public static void SetVolumeMaster(float value)
    {
        Instance.masterMixer.SetFloat(mainVolumeParam, ToDecibels(value));
        SetPref(mainVolumeParam, value);
    }

    public static void SetVolumeSFX(float value)
    {
        Instance.masterMixer.SetFloat(sfxVolumeParam, ToDecibels(value));
        SetPref(sfxVolumeParam, value);
    }

    public static void SetVolumeUI(float value)
    {
        Instance.masterMixer.SetFloat(uiVolumeParam, ToDecibels(value));
        SetPref(uiVolumeParam, value);
    }

    public static void SetVolumeMusic(float value)
    {
        Instance.masterMixer.SetFloat(musicVolumeParam, ToDecibels(value));
        SetPref(musicVolumeParam, value);
    }

    // Prefs

    // returns a linear [0-1] volume value
    private static float GetPref(string pref)
    {
        float v = PlayerPrefs.GetFloat(pref, 0.75f);
        return v;
    }

    // sets a linear [0-1] volume value
    private static void SetPref(string pref, float val)
    {
        PlayerPrefs.SetFloat(pref, val);
    }

    public static float ToDecibels(float value)
    {
        if (value == 0) return -80;
        return Mathf.Log10(value) * 20;
    }

    private void SetMixerFromPref(string pref)
    {
        masterMixer.SetFloat(pref, ToDecibels(GetPref(pref)));
    }




    [System.Serializable]
    public class BankKVP
    {
        public string Key;      // サウンド識別用の名前
        public AudioClip Value; // オーディオデータ
    }

    [System.Serializable]
    public class AudioBank
    {
        // 複数のオーディオデータをキーとペアで格納
        [SerializeField] private BankKVP[] kvps;
        private readonly Dictionary<string, AudioClip> dictionary = new Dictionary<string, AudioClip>();


        public bool Validate()
        {
            // 配列が空なら無効
            if (kvps.Length == 0) return false;

            List<string> keys = new List<string>();
            foreach (var kvp in kvps)
            {
                // すでに存在するキーならfalse
                if (keys.Contains(kvp.Key)) return false;
                keys.Add(kvp.Key);
            }
            return true;
        }

        public void Build()
        {
            // 配列の確認処理を行う
            if (this.Validate())
            {
                // 配列から辞書に変換
                for (int i = 0; i < kvps.Length; i++)
                {
                    dictionary.Add(kvps[i].Key, kvps[i].Value);
                }
            }
        }

        public bool TryGetAudio(string key, out AudioClip audio)
        {
            return dictionary.TryGetValue(key, out audio);
        }




    }


#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AudioBank))]
    public class AudioBankDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("kvps"));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property.FindPropertyRelative("kvps"), label, true);
            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(BankKVP))]
    public class BankKVPDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.BeginProperty(position, label, property);

            Rect rect1 = new Rect(position.x, position.y, position.width / 2 - 4, position.height);
            Rect rect2 = new Rect(position.center.x + 2, position.y, position.width / 2 - 4, position.height);

            EditorGUI.PropertyField(rect1, property.FindPropertyRelative("Key"), GUIContent.none);
            EditorGUI.PropertyField(rect2, property.FindPropertyRelative("Value"), GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
#endif
}


