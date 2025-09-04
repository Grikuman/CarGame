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

        // �t�F�[�h�C������
        _fade.FadeIn();
    }

    public void FadeOut(UnityEngine.UI.Button button = null)
    {
        // �{�^���ł̏����̏ꍇ������x������Ȃ��悤�ɃI�t�ɂ���
        if(button != null) 
        {
            button.interactable = false;
        }

        // �t�F�[�h�A�E�g����
        _fade.FadeOut(_nextSceneName);
    }

}
