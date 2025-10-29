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


    // ���݂̑I��ԍ�
    private int _currentIndex = 0;
    // �O���b�h�Z���N�^�[
    private UIGridSelector _uiGridSelector = null;

    private void Awake()
    {
        _uiGridSelector = new UIGridSelector(3, 1);
   
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ���݂̔ԍ����擾
        _currentIndex = _uiGridSelector._currentIndex;

        for(int i = 0 ; i < _buttonBases.Length; i++)
        {
            // �I�𒆂̃{�^�����I���ɂ���
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

        // �{�^���ԍ����ύX���ꂽ�ꍇ
        if( _currentIndex != _uiGridSelector._currentIndex)
        {
            // �ԍ��̍X�V
            _currentIndex = _uiGridSelector._currentIndex;


            // �A�j���[�V�����̕ύX
            for (int i = 0; i < _buttonBases.Length; i++)
            {
                // �I�𒆂̃{�^�����I���ɂ���
                if (i == _currentIndex)
                {
                    _buttonBases[i].ChangeAnimationState(_buttonBases[i].GetAnimationState<OnTitleButtonAnimationState>());
                    // �Z���N�^�[�̈ʒu�ύX
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
