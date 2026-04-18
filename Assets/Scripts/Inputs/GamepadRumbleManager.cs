using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public static class GamepadRumbleManager
{
    const int MS_PER_SEC = 1000;
    static List<RumblePulseData> onGoingRumbles;
    static float continuousLowFrequency01;
    static float continuousHighFrequency01;

    public struct RumblePulseData
    {
        public float lowFrequency01;
        public float highFrequency01;
        public float duration;
        public float startTime;
        public float EndTime => startTime + duration;

        public RumblePulseData(float lowFrequency01, float highFrequency01, float duration)
        {
            this.lowFrequency01 = lowFrequency01;
            this.highFrequency01 = highFrequency01;
            this.duration = duration;
            startTime = Time.time;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        onGoingRumbles = new();
        ControlSchemeManager.onControlSchemeChanged -= OnControlSchemeChanged;
        ControlSchemeManager.onControlSchemeChanged += OnControlSchemeChanged;
    }

    private static void OnControlSchemeChanged(ControlSchemeManager.ControlScheme controlScheme)
    {
        StopAll();
    }

    public static void StopAll()
    {
        onGoingRumbles.Clear();
        ClearContinuousRumble();
        InputSystem.ResetHaptics();
    }

    public static async Task RumblePulseDelayed(float lowFrequency01, float highFrequency01, float duration, float delay)
    {
        await Task.Delay(Mathf.RoundToInt(delay * MS_PER_SEC));

        await RumblePulse(lowFrequency01, highFrequency01, duration);
    }

    public static async Task RumblePulse(float lowFrequency01, float highFrequency01, float duration)
    {
        //Make sure we are using a pad
        if (ControlSchemeManager.currentScheme != ControlSchemeManager.ControlScheme.Gamepad)
            return;

        RumblePulseData rumbleData = new RumblePulseData(lowFrequency01, highFrequency01, duration);
        onGoingRumbles.Add(rumbleData);
        UpdateRumble();

        while(Time.time <= rumbleData.EndTime)
         await Task.Delay(100);

        UpdateRumble();
    }

    public static void SetContinuousRumble(float lowFrequency01, float highFrequency01)
    {
        continuousLowFrequency01 = lowFrequency01;
        continuousHighFrequency01 = highFrequency01;
        UpdateRumble();
    }

    public static bool IsContinuousRumble(float lowFrequency01, float highFrequency01)
    {
        return continuousLowFrequency01 == lowFrequency01 && continuousHighFrequency01 == highFrequency01;
    }

    public static void ClearContinuousRumble()
    {
        continuousLowFrequency01 = 0;
        continuousHighFrequency01 = 0;
        UpdateRumble();
    }

    static void UpdateRumble()
    {
        Gamepad pad = Gamepad.current;
        if (pad == null)
            return;

        if (ControlSchemeManager.currentScheme != ControlSchemeManager.ControlScheme.Gamepad)
            return;

        //Update rumbles data for current time
        RemoveFinishedRumbleData();

        //Set motor speed if has data
        if (TryGetHighestFrequencies(out float lowFrequency01, out float highFrequency01))
        {
            pad.SetMotorSpeeds(lowFrequency01, highFrequency01);
        }
        //Stops otherwise
        else
        {
            pad.ResetHaptics();
        }
    }

    static bool TryGetHighestFrequencies(out float lowFrequency01, out float highFrequency01)
    {
        //Start with continuous values (if none, should be 0)
        lowFrequency01 = continuousLowFrequency01;
        highFrequency01 = continuousHighFrequency01;

        //Get Max from rumble data
        for (int i = 0; i < onGoingRumbles.Count; i++)
        {
            if (onGoingRumbles[i].lowFrequency01 > lowFrequency01)
                lowFrequency01 = onGoingRumbles[i].lowFrequency01;

            if (onGoingRumbles[i].highFrequency01 > highFrequency01)
                highFrequency01 = onGoingRumbles[i].highFrequency01;
        }

        return lowFrequency01 > 0 || highFrequency01 > 0;
    }

    /// <summary>
    /// Remove all the rumble data that do not contribute to current rumble.
    /// </summary>
    static void RemoveFinishedRumbleData()
    {
        int id = 0;
        while (id < onGoingRumbles.Count)
        {
            //Remove if outdated
            if (Time.time > onGoingRumbles[id].EndTime)
                onGoingRumbles.RemoveAt(id);
            //Move next otherwise
            else
                id++;
        }
    }
}
