using Unity.VisualScripting;
using UnityEngine;

public class UIGridSelector
{
    
    private int _rows; // �c
    private int _cols; // ��

    public int _currentIndex { get;private set; }

    private InputManager _inputManager = null;


    public UIGridSelector(int row , int col)
    {
        _rows = row;
        _cols = col;

        // �C���X�^���X���擾����
        _inputManager = InputManager.Instance;
        _inputManager.Initialize();
    }

    public void Update()
    {
        // ���݂̍s�E����擾
        int currentRow = _currentIndex / _cols;
        int currentCol = _currentIndex % _cols;

        // ���̓`�F�b�N
        if (_inputManager.UI_WasPressedThisFrame(UiInputActionID.UP))
        {
            currentRow = Mathf.Max(0, currentRow - 1);
        }
        else if (_inputManager.UI_WasPressedThisFrame(UiInputActionID.DOWN))
        {
            currentRow = Mathf.Min(_rows - 1, currentRow + 1);
        }
        else if (_inputManager.UI_WasPressedThisFrame(UiInputActionID.LEFT))
        {
            currentCol = Mathf.Max(0, currentCol - 1);
        }
        else if (_inputManager.UI_WasPressedThisFrame(UiInputActionID.RIGHT))
        {
            currentCol = Mathf.Min(_cols - 1, currentCol + 1);
        }

        // �C���f�b�N�X�Čv�Z
        _currentIndex = currentRow * _cols + currentCol;

        Debug.Log($"Row:{currentRow}, Col:{currentCol}, Index:{_currentIndex}");
    }


}
