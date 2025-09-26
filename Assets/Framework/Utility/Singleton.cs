using UnityEngine;

namespace ShunLib.Utility
{
    /// <summary>
    /// �y���{��Ő����z
    /// MonoBehaviour ���p������R���|�[�l���g�����̃V���O���g�����N���X�B
    /// - �V�[�����Ɋ��ɑ��݂��铯�^�R���|�[�l���g��T���Ďg���܂��iFind�j�B
    /// - ������Ȃ���Ύ����������A�v���C���� DontDestroyOnLoad ��t���ăV�[���J�ڂł������c�点�܂��B
    /// - �I���������i�A�v���I���j�ɐV�K�������Ȃ����߂̃K�[�h�t���B
    /// 
    /// �y�g�����z
    /// public class AudioManager : Singleton<AudioManager> { ... }
    /// �� �ǂ�����ł� AudioManager.Instance �ŎQ�Ɖ\�B
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // �V���O���g���̎��́i�ÓI��1�����ێ��j
        private static T _instance;

        // �}���`�X���b�h���i�W���u�E�񓯊��j�ł̓������s��h�����߂̃��b�N
        private static readonly object _lock = new object();

        // �A�v���I�����t���O�F�I�����ɐV�����u�S�[�X�g�v�I�u�W�F�N�g�����Ȃ����߂̈��S���u
        private static bool _applicationIsQuitting = false;

        /// <summary>
        /// �V���O���g���̃O���[�o���A�N�Z�T
        /// </summary>
        public static T Instance
        {
            get
            {
                // ���łɃA�v���I���V�[�P���X�ɓ����Ă���ꍇ�͐V�K�������Ȃ�
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[Singleton] '{typeof(T)}' �̓A�v���I�����̂��ߐV�K�������܂���inull ��Ԃ��܂��j�B");
                    return null;
                }

                // �����A�N�Z�X�ی�
                lock (_lock)
                {
                    // ���ɐ����Ă���΂��̂܂ܕԂ�
                    if (_instance != null) return _instance;

                    // �V�[�����������F�G�f�B�^/�����^�C���Ŏ�u�������ꍇ�͂����炪�E����
                    var all = FindObjectsByType<T>(FindObjectsSortMode.None);
                    if (all != null && all.Length > 0)
                    {
                        _instance = all[0];

                        // ��������ꍇ�͌x���i�ŏ���1���g�p�j
                        if (all.Length > 1)
                        {
                            Debug.LogWarning($"[Singleton] {typeof(T)} �� {all.Length} ������܂����B�ŏ��� 1 ���g�p���܂��B�d���z�u���m�F���Ă��������B");
                        }

                        // �v���C���ł���Δj������Ȃ��悤�ɐݒ�
                        if (Application.isPlaying)
                            DontDestroyOnLoad(_instance.gameObject);

                        Debug.Log($"[Singleton] �����̃C���X�^���X���g�p: {_instance.gameObject.name}");
                        return _instance;
                    }

                    // ������Ȃ���ΐV�K�� GameObject ������ăA�^�b�`
                    var go = new GameObject($"(singleton) {typeof(T)}");
                    _instance = go.AddComponent<T>();

                    if (Application.isPlaying)
                        DontDestroyOnLoad(go);

                    Debug.Log($"[Singleton] �C���X�^���X��������Ȃ��������ߍ쐬���܂���: {go.name}");
                    return _instance;
                }
            }
        }

        /// <summary>
        /// �y�d�v�z�I�����t���O�𗧂Ă�B
        /// OnDestroy �ł̓V�[���A�����[�h�������΂��邽�߁A�A�v���I���̈Ӑ}�ɋ߂� OnApplicationQuit ���g���B
        /// </summary>
        private void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }

        /// <summary>
        /// �y�C�Ӊ��P�z�d���΍�F
        /// �V�[���Ɏ�ŕ����u���ꂽ���A�ŏ���1�����c�����͔j������B
        /// �h���N���X���� base.Awake() ���ĂԑO��ɂ���Ƃ����S�B
        /// </summary>
        protected virtual void Awake()
        {
            // ���ɕʃC���X�^���X���ݒ�ς݂������ł͂Ȃ� �� ������j��
            if (_instance != null && _instance != this as T)
            {
                // ����^�R���|�[�l���g�̓�d������}�~
                Destroy(gameObject);
                return;
            }

            // �������C���X�^���X�ɓo�^
            _instance = this as T;

            // �v���C���ł���΃V�[���J�ڂŔj������Ȃ��悤�ɂ���
            if (Application.isPlaying)
                DontDestroyOnLoad(gameObject);
        }
    }
}
