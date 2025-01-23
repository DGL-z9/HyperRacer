using UnityEngine;

public class MoveButton : MonoBehaviour
{
    public delegate void MoveButtonDelegate();
    public event MoveButtonDelegate OnMoveButtonDown;
    
    private bool isDown = false;

    private void Update()
    {
        if (isDown)
        {
            OnMoveButtonDown?.Invoke();
        }
    }
    
    public void ButtonDown()
    {
        isDown = true;
    }

    public void ButtonUp()
    {
        isDown = false;
    }
}