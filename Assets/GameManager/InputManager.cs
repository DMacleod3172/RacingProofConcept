using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public RacingProofConcept inputActions;
    public static InputManager Instance { get; private set; }
    public event Action<InputActionMap> actionMapChange;

    private void Awake()
    {
        inputActions = new RacingProofConcept();
    }

    void Start()
    {
        ToggleActionMap(inputActions.Player);
    }

    public void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled)
        {
            return;
        }

        inputActions.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    }
}