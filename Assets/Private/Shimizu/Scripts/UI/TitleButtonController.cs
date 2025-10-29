using UnityEngine;

public class TitleButtonController : MonoBehaviour
{


    public static readonly Vector3[] SelectorPositions = new Vector3[]
    {
        new Vector3(659.6f, -150.0f, 0),
        new Vector3(659.6f, -250.0f, 0),
        new Vector3(659.6f, -345.0f, 0)
    };

    
    [SerializeField] private ButtonBase[] _buttonBases;
    [SerializeField] private RectTransform _rectTransform;


    // 現在の選択番号
    private int _currentIndex = 0;
    // グリッドセレクター
    private UIGridSelector _uiGridSelector = null;

    private void Awake()
    {
        _uiGridSelector = new UIGridSelector(3, 1);
   
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 現在の番号を取得
        _currentIndex = _uiGridSelector._currentIndex;

        for(int i = 0 ; i < _buttonBases.Length; i++)
        {
            // 選択中のボタンをオンにする
            if(i == _currentIndex)
                _buttonBases[i].ChangeAnimationState(_buttonBases[i].GetAnimationState<OnTitleButtonAnimationState>());
            else
                _buttonBases[i].ChangeAnimationState(_buttonBases[i].GetAnimationState<OffTitleButtonAnimationState>());
        }

    }

    // Update is called once per frame
    void Update()
    {
        _uiGridSelector.Update();

        // ボタン番号が変更された場合
        if( _currentIndex != _uiGridSelector._currentIndex)
        {
            // 番号の更新
            _currentIndex = _uiGridSelector._currentIndex;


            // アニメーションの変更
            for (int i = 0; i < _buttonBases.Length; i++)
            {
                // 選択中のボタンをオンにする
                if (i == _currentIndex)
                {
                    _buttonBases[i].ChangeAnimationState(_buttonBases[i].GetAnimationState<OnTitleButtonAnimationState>());
                    // セレクターの位置変更
                    _rectTransform.localPosition = SelectorPositions[i];
                }
                else
                {
                    _buttonBases[i].ChangeAnimationState(_buttonBases[i].GetAnimationState<OffTitleButtonAnimationState>());
                }  
            }
        }
    }
}
