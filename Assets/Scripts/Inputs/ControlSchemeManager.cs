using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public static class ControlSchemeManager
{
    public enum ControlScheme { Keyboard, Gamepad }
    public static ControlScheme currentScheme;

    static InputDevice currentDevice;

    public static event Action<ControlScheme> onControlSchemeChanged;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        //InputUser.onChange -= onInputDeviceChange;
        //InputUser.onChange += onInputDeviceChange;

       // InputSystem.onActionChange -= OnActionChange;
        //InputSystem.onActionChange += OnActionChange;

        InputSystem.onEvent -= OnInputSystemEvent;
        InputSystem.onEvent += OnInputSystemEvent;
    }

    //private static void OnActionChange(object obj, InputActionChange change)
    //{
    //    if (change == InputActionChange.ActionPerformed)
    //    {
    //        ControlScheme newScheme;

    //        InputAction inputAction = (InputAction)obj;
    //        InputControl lastControl = inputAction.activeControl;
    //        InputDevice lastDevice = lastControl.device;

    //        if (lastDevice is Gamepad)
    //            newScheme = ControlScheme.Gamepad;
    //        else
    //            newScheme = ControlScheme.KeyboardMouse;

    //        if (newScheme != currentScheme)
    //        {
    //            Debug.Log("Control Scheme Changed: " + newScheme);
    //            onControlSchemeChanged?.Invoke(currentScheme);
    //            currentScheme = newScheme;
    //        }
    //    }
    //}

    static void OnInputSystemEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (currentDevice == device)
            return;

        // Some devices like to spam events like crazy.
        // Example: PS4 controller on PC keeps triggering events without meaningful change.
        var eventType = eventPtr.type;
        if (eventType == StateEvent.Type)
        {
            // Go through the changed controls in the event and look for ones actuated
            // above a magnitude of a little above zero.
            if (!eventPtr.EnumerateChangedControls(device: device, magnitudeThreshold: 0.0001f).Any())
                return;
        }

        currentDevice = device;
        SetControlScheme(device);
        onControlSchemeChanged?.Invoke(0);
    }

    static void SetControlScheme(InputDevice device)
    {
        ControlScheme newScheme;
        if (device is Gamepad)
            newScheme = ControlScheme.Gamepad;
        else
            newScheme = ControlScheme.Keyboard;

        if (newScheme != currentScheme)
        {
            Debug.Log("Control Scheme Changed: " + newScheme);
            currentScheme = newScheme;
            onControlSchemeChanged?.Invoke(currentScheme);
        }
    }

    //static private void onInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
    //{
    //    if (change == InputUserChange.ControlSchemeChanged)
    //    {
    //        ControlScheme lastScheme = currentScheme;
    //        //Unselect object
    //        EventSystem.current.SetSelectedGameObject(null);
    //        // assuming we have only 2 schemes, gamepad and keyboard
    //        if (user.controlScheme.Value.name.Equals("Gamepad"))
    //        {
    //            currentScheme = ControlScheme.Gamepad;
    //        }
    //        else
    //        {
    //            currentScheme = ControlScheme.KeyboardMouse;
    //        }

    //        if (lastScheme != currentScheme)
    //            onControlSchemeChanged?.Invoke(currentScheme);

    //        lastScheme = currentScheme;
    //    }
    //}
}
