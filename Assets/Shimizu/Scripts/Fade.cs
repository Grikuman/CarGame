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
        // ������
        _fadeImage.enabled = true;
        _fadeImage.color = Color.black;

        // Tween�����J�n
        _fadeImage.DOFade(0.0f, 1.0f).OnComplete(() =>
        {
            Debug.Log("OnComplete FadeIN");
            _fadeImage.enabled = false;
        });

    }

    public void FadeOut()
    {
        // ������
        _fadeImage.enabled = true;
        _fadeImage.color = new Color(0.0f ,0.0f ,0.0f ,0.0f);

        // Tween�����J�n
        _fadeImage.DOFade(1.0f, 1.0f).SetDelay(1.0f).OnComplete(() =>
        {
            Debug.Log("OnComplete FadeOUT");
            _fadeImage.enabled = false;
        });
    }


    public void FadeOut(string nextScene)
    {
        // �t�F�[�h�p�摜���������i�����ɂ��ėL�����j
        _fadeImage.enabled = true;
        _fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        // �񓯊��Ŏ��̃V�[����ǂݍ��ށi�����������Ő؂�ւ��Ȃ��j
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
        asyncLoad.allowSceneActivation = false;

        // �t�F�[�h�A�E�g�J�n�i1�b�����č�������A1�b�x���j
        _fadeImage.DOFade(1.0f, 1.0f).SetDelay(1.0f).OnComplete(() =>
        {
            Debug.Log("OnComplete FadeOUT");

            // �t�F�[�h�����������̂ŁA�V�[���J�ڂ�����
            asyncLoad.allowSceneActivation = true;
        });
    }
}
