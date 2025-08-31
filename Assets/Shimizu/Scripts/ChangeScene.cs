using UnityEngine;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{

    [SerializeField] private Image _fadeImage;
    [SerializeField] private string _nextSceneName;

    private Fade _fade = null;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fade = new Fade(_fadeImage);

        // フェードイン処理
        _fade.FadeIn();
    }

    public void FadeOut(UnityEngine.UI.Button button = null)
    {
        // ボタンでの処理の場合もう一度押されないようにオフにする
        if(button != null) 
        {
            button.interactable = false;
        }

        // フェードアウト処理
        _fade.FadeOut(_nextSceneName);
    }

}
