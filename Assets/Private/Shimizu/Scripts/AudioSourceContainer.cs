// ============================================
// 
// �t�@�C����: AudioSourceContainer.cs
// �T�v: �I�[�f�B�I�\�[�X�̃R���e�i�i�V���O���g���j
// 
// ����� : �����x��
// 
// ============================================
using ShunLib.Utility;
using UnityEngine;
using UnityEngine.Pool;

public class AudioSourceContainer : MonoBehaviour
{
    // �V���O���g��
    public static AudioSourceContainer Instance => Singleton<AudioSourceContainer>.Instance;

    [SerializeField] private AudioSourceObject _audioSourceObjectPrefab; // �I�u�W�F�N�g�v�[���ŊǗ�����I�u�W�F�N�g
    [SerializeField] private int _defaultCapacity = 3;                   // ���炩���ߗp�ӂ��Ă����I�u�W�F�N�g��
    [SerializeField] private int _maxSize = 10;                          // �v�[���ɕێ��ł���ő�̃I�u�W�F�N�g��

    // �I�u�W�F�N�g�v�[���{��
    private ObjectPool<AudioSourceObject> _audioSourceObjectPool;

    // ����������
    private void Start()
    {
        // �I�u�W�F�N�g�v�[���̍쐬
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

    // �v�[������I�u�W�F�N�g���擾����
    public AudioSourceObject GetAudioSourceObject()
    {
        return _audioSourceObjectPool.Get();
    }

    // �v�[���̒��g����ɂ���
    public void ClearAudioSourceObject()
    {
        _audioSourceObjectPool.Clear();
    }

    // �v�[���ɓ����C���X�^���X��V������������ۂɍs������
    private AudioSourceObject OnCreateObject()
    {
        return Instantiate(_audioSourceObjectPrefab, transform);
    }

    // �v�[������C���X�^���X���擾�����ۂɍs������
    private void OnGetObject(AudioSourceObject audioSourceObject)
    {
        audioSourceObject.Initialize(() => _audioSourceObjectPool.Release(audioSourceObject));
        audioSourceObject.gameObject.SetActive(true);
        Debug.Log("Get");
    }

    // �v�[���ɃC���X�^���X��ԋp�����ۂɍs������
    private void OnReleaseObject(AudioSourceObject audioSourceObject)
    {
        audioSourceObject.gameObject.SetActive(false);
        Debug.Log("Release");  
    }

    // �v�[������폜�����ۂɍs������
    private void OnDestroyObject(AudioSourceObject audioSourceObject)
    {
        Destroy(audioSourceObject.gameObject);
    }

}
