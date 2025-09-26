using UnityEngine;

namespace ShunLib.Utility
{
    /// <summary>
    /// 【日本語版説明】
    /// MonoBehaviour を継承するコンポーネント向けのシングルトン基底クラス。
    /// - シーン内に既に存在する同型コンポーネントを探して使います（Find）。
    /// - 見つからなければ自動生成し、プレイ中は DontDestroyOnLoad を付けてシーン遷移でも生き残らせます。
    /// - 終了処理中（アプリ終了）に新規生成しないためのガード付き。
    /// 
    /// 【使い方】
    /// public class AudioManager : Singleton<AudioManager> { ... }
    /// → どこからでも AudioManager.Instance で参照可能。
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // シングルトンの実体（静的に1つだけ保持）
        private static T _instance;

        // マルチスレッド環境（ジョブ・非同期）での同時実行を防ぐためのロック
        private static readonly object _lock = new object();

        // アプリ終了中フラグ：終了時に新しい「ゴースト」オブジェクトを作らないための安全装置
        private static bool _applicationIsQuitting = false;

        /// <summary>
        /// シングルトンのグローバルアクセサ
        /// </summary>
        public static T Instance
        {
            get
            {
                // すでにアプリ終了シーケンスに入っている場合は新規生成しない
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[Singleton] '{typeof(T)}' はアプリ終了中のため新規生成しません（null を返します）。");
                    return null;
                }

                // 同時アクセス保護
                lock (_lock)
                {
                    // 既に生きていればそのまま返す
                    if (_instance != null) return _instance;

                    // シーン内を検索：エディタ/ランタイムで手置きした場合はこちらが拾われる
                    var all = FindObjectsByType<T>(FindObjectsSortMode.None);
                    if (all != null && all.Length > 0)
                    {
                        _instance = all[0];

                        // 複数ある場合は警告（最初の1つを使用）
                        if (all.Length > 1)
                        {
                            Debug.LogWarning($"[Singleton] {typeof(T)} が {all.Length} 個見つかりました。最初の 1 つを使用します。重複配置を確認してください。");
                        }

                        // プレイ中であれば破棄されないように設定
                        if (Application.isPlaying)
                            DontDestroyOnLoad(_instance.gameObject);

                        Debug.Log($"[Singleton] 既存のインスタンスを使用: {_instance.gameObject.name}");
                        return _instance;
                    }

                    // 見つからなければ新規に GameObject を作ってアタッチ
                    var go = new GameObject($"(singleton) {typeof(T)}");
                    _instance = go.AddComponent<T>();

                    if (Application.isPlaying)
                        DontDestroyOnLoad(go);

                    Debug.Log($"[Singleton] インスタンスが見つからなかったため作成しました: {go.name}");
                    return _instance;
                }
            }
        }

        /// <summary>
        /// 【重要】終了時フラグを立てる。
        /// OnDestroy ではシーンアンロード時も発火するため、アプリ終了の意図に近い OnApplicationQuit を使う。
        /// </summary>
        private void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }

        /// <summary>
        /// 【任意改善】重複対策：
        /// シーンに手で複数置かれた時、最初の1つだけ残し他は破棄する。
        /// 派生クラス側で base.Awake() を呼ぶ前提にするとより安全。
        /// </summary>
        protected virtual void Awake()
        {
            // 既に別インスタンスが設定済みかつ自分ではない → 自分を破棄
            if (_instance != null && _instance != this as T)
            {
                // 同一型コンポーネントの二重生成を抑止
                Destroy(gameObject);
                return;
            }

            // 自分をインスタンスに登録
            _instance = this as T;

            // プレイ中であればシーン遷移で破棄されないようにする
            if (Application.isPlaying)
                DontDestroyOnLoad(gameObject);
        }
    }
}
