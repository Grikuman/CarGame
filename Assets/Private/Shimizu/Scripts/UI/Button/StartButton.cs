using System.Collections.Generic;
using UnityEngine;

public class StartButton : ButtonBase
{
    [SerializeField] private GameObject _itemObject;

    private bool _isActive;

    private readonly List<IButtonAnimationState> _states = new();

    private IButtonAnimationState _currentState = null;

    /// <summary> �X�e�[�g���擾���� </summary>
    public override T GetAnimationState<T>()
    {
        foreach (var state in _states)
        {
            if (state is T target)
                return target;
        }

        Debug.LogWarning($"State of type {typeof(T).Name} not found in {name}");
        return default;
    }

    private void Awake()
    {
        // �X�e�[�g�쐬��������
        AddState(new OnTitleButtonAnimationState(_itemObject));
        AddState(new OffTitleButtonAnimationState(_itemObject));

        // �����X�e�[�g�ݒ�
        _currentState = GetAnimationState<OffTitleButtonAnimationState>();
        _currentState.OnShow();
    }

    private void Update()
    {
        // ���݂̃X�e�[�g�̍X�V����
        _currentState.OnUpdate();
    }

    /// <summary> �A�N�e�B�u��Ԃ�ݒ� </summary>
    public override void SetActive(bool value) => _isActive = value;
    /// <summary> �A�N�e�B�u��Ԃ��擾 </summary>
    public override bool GetIsActive() => _isActive;

    /// <summary> �X�e�[�g��؂�ւ��� </summary>
    /// <param name="state">�؂�ւ���X�e�[�g</param>
    public override void ChangeAnimationState(IButtonAnimationState state)
    {
        // ���݂̃X�e�[�g�̏I������
        _currentState.OnHide();

        // �X�e�[�g�̐؂�ւ�
        _currentState = state;

        // ���̃X�e�[�g�̊J�n����
        _currentState.OnShow();
    }

    /// <summary> �C�x���g�𔭍s���� </summary>
    public override void OnEvent()
    {

    }


    /// <summary> �X�e�[�g�ǉ��A������ </summary>
    private void AddState(IButtonAnimationState state)
    {
        state.Initialize(gameObject);
        _states.Add(state);
    }
}
