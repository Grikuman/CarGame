using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade
{
    private Image _fadeImage;

    public Fade(Image image)
    {
        _fadeImage = image;
    }

    public void FadeIn()
    {
        // 初期化
        _fadeImage.enabled = true;
        _fadeImage.color = Color.black;

        // Tween処理開始
        _fadeImage.DOFade(0.0f, 1.0f).OnComplete(() =>
        {
            Debug.Log("OnComplete FadeIN");
            _fadeImage.enabled = false;
        });

    }

    public void FadeOut()
    {
        // 初期化
        _fadeImage.enabled = true;
        _fadeImage.color = new Color(0.0f ,0.0f ,0.0f ,0.0f);

        // Tween処理開始
        _fadeImage.DOFade(1.0f, 1.0f).SetDelay(1.0f).OnComplete(() =>
        {
            Debug.Log("OnComplete FadeOUT");
            _fadeImage.enabled = false;
        });
    }


    public void FadeOut(string nextScene)
    {
        // フェード用画像を初期化（透明にして有効化）
        _fadeImage.enabled = true;
        _fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        // 非同期で次のシーンを読み込む（ただし自動で切り替えない）
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
        asyncLoad.allowSceneActivation = false;

        // フェードアウト開始（1秒かけて黒くする、1秒遅延）
        _fadeImage.DOFade(1.0f, 1.0f).SetDelay(1.0f).OnComplete(() =>
        {
            Debug.Log("OnComplete FadeOUT");

            // フェードが完了したので、シーン遷移を許可
            asyncLoad.allowSceneActivation = true;
        });
    }
}
