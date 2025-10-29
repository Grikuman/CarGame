using DG.Tweening;
using UnityEngine;

public class TitleItemAnimation : MonoBehaviour
{
    DG.Tweening.Sequence _currentSequence = null;

    // フェードフラグ
    public bool IsFade {  get; private set; }

    public void FadeIn()
    {
        //if (IsFade) return;

        // 新たなSequenceを作成
        this.SetSequence();

        // フェード中にする
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

        //Playで実行
        _currentSequence.Play();
    }

    public void FadeOut()
    {
        //if (IsFade) return;

        // 新たなSequenceを作成
        this.SetSequence();

        // フェード中にする
        IsFade = true;

        _currentSequence.Append(this.transform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), 0.5f, RotateMode.FastBeyond360));
        _currentSequence.Join(this.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            IsFade = false;
        })
            );

        //Playで実行
        _currentSequence.Play();
    }

    private void SetSequence()
    {
        // 現在のTweenを全て破棄する
        _currentSequence.Kill();
        // 念のためnull代入
        _currentSequence = null;

        // 新しいSequenceを作成
        _currentSequence = DOTween.Sequence();
    }
}
