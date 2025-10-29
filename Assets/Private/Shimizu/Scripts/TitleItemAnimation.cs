using DG.Tweening;
using UnityEngine;

public class TitleItemAnimation : MonoBehaviour
{
    DG.Tweening.Sequence _currentSequence = null;

    // �t�F�[�h�t���O
    public bool IsFade {  get; private set; }

    public void FadeIn()
    {
        //if (IsFade) return;

        // �V����Sequence���쐬
        this.SetSequence();

        // �t�F�[�h���ɂ���
        IsFade = true;

        _currentSequence.Append(this.transform.DORotate(new Vector3(0.0f , 360.0f ,0.0f) , 1f, RotateMode.FastBeyond360));
        _currentSequence.Join(this.transform.DOScale(Vector3.one, 1.0f).OnComplete(() =>
        {
            IsFade = false;
        })
            );

        _currentSequence.Append(transform
            .DORotate(new Vector3(0.0f, 360.0f, 0.0f), 3.0f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental));

        //Play�Ŏ��s
        _currentSequence.Play();
    }

    public void FadeOut()
    {
        //if (IsFade) return;

        // �V����Sequence���쐬
        this.SetSequence();

        // �t�F�[�h���ɂ���
        IsFade = true;

        _currentSequence.Append(this.transform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), 0.5f, RotateMode.FastBeyond360));
        _currentSequence.Join(this.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            IsFade = false;
        })
            );

        //Play�Ŏ��s
        _currentSequence.Play();
    }

    private void SetSequence()
    {
        // ���݂�Tween��S�Ĕj������
        _currentSequence.Kill();
        // �O�̂���null���
        _currentSequence = null;

        // �V����Sequence���쐬
        _currentSequence = DOTween.Sequence();
    }
}
