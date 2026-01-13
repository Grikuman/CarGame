// ============================================
// 
// ファイル名: AudioSourceContainer.cs
// 概要: オーディオソースのコンテナ（シングルトン）
// 
// 製作者 : 清水駿希
// 
// ============================================
using ShunLib.Utility;
using UnityEngine;
using UnityEngine.Pool;

public class AudioSourceContainer : MonoBehaviour
{
    // シングルトン
    public static AudioSourceContainer Instance => Singleton<AudioSourceContainer>.Instance;

    [SerializeField] private AudioSourceObject _audioSourceObjectPrefab; // オブジェクトプールで管理するオブジェクト
    [SerializeField] private int _defaultCapacity = 3;                   // あらかじめ用意しておくオブジェクト数
    [SerializeField] private int _maxSize = 10;                          // プールに保持できる最大のオブジェクト数

    // オブジェクトプール本体
    private ObjectPool<AudioSourceObject> _audioSourceObjectPool;

    // 初期化処理
    private void Start()
    {
        // オブジェクトプールの作成
        _audioSourceObjectPool = new ObjectPool<AudioSourceObject>(
            createFunc: () => OnCreateObject(),
            actionOnGet: (obj) => OnGetObject(obj),
            actionOnRelease: (obj) => OnReleaseObject(obj),
            actionOnDestroy: (obj) => OnDestroyObject(obj),
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize
        );

        DontDestroyOnLoad(gameObject);
    }

    // プールからオブジェクトを取得する
    public AudioSourceObject GetAudioSourceObject()
    {
        return _audioSourceObjectPool.Get();
    }

    // プールの中身を空にする
    public void ClearAudioSourceObject()
    {
        _audioSourceObjectPool.Clear();
    }

    // プールに入れるインスタンスを新しく生成する際に行う処理
    private AudioSourceObject OnCreateObject()
    {
        return Instantiate(_audioSourceObjectPrefab, transform);
    }

    // プールからインスタンスを取得した際に行う処理
    private void OnGetObject(AudioSourceObject audioSourceObject)
    {
        audioSourceObject.Initialize(() => _audioSourceObjectPool.Release(audioSourceObject));
        audioSourceObject.gameObject.SetActive(true);
        Debug.Log("Get");
    }

    // プールにインスタンスを返却した際に行う処理
    private void OnReleaseObject(AudioSourceObject audioSourceObject)
    {
        audioSourceObject.gameObject.SetActive(false);
        Debug.Log("Release");  
    }

    // プールから削除される際に行う処理
    private void OnDestroyObject(AudioSourceObject audioSourceObject)
    {
        Destroy(audioSourceObject.gameObject);
    }

}
