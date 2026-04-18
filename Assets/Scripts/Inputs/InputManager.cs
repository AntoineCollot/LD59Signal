using UnityEngine;
using UnityEngine.InputSystem;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class InputManager
{
    public static InputMap inputs;

    public static Vector2 MovementInput
    {
        get
        {
            if (inputs == null)
                return Vector2.zero;
            return inputs.Main.Move.ReadValue<Vector2>();
        }
    }

    static InputManager()
    {
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void OnBeforeSceneLoad()
    {
        Init();
        EnableInputs();
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= OnEditorPlayModeChanged;
        EditorApplication.playModeStateChanged += OnEditorPlayModeChanged;
#endif
    }

#if UNITY_EDITOR
    private static void OnEditorPlayModeChanged(PlayModeStateChange change)
    {
        switch (change)
        {
            case PlayModeStateChange.EnteredPlayMode:
                EnableInputs();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                ClearInputs();
                break;
        }
    }
#endif

    static void Init()
    {
        if (inputs != null)
            return;
        inputs = new InputMap();
    }

    public static void EnableInputs()
    {
        inputs.Enable();
    }

    public static void DisableInputs()
    {
        inputs.Disable();
    }

    public static void ClearInputs()
    {
        DisableInputs();
        inputs.Dispose();
        inputs = null;
    }

    public static int GetBindingIndexOfActiveGroup(InputAction action)
    {
        string controlGroup = ControlSchemeManager.currentScheme.ToString();
        return action.GetBindingIndex(InputBinding.MaskByGroup(controlGroup));
    }
}
