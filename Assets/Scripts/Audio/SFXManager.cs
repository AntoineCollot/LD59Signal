using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum GlobalSFX
{
    ButtonClick,
    ButtonHover,
    PowerSelect,
    MonsterAttack,
    PayerDamaged,
    PlayerDeath,
    XPPickUp,
    MonsterKill,
    MonsterDamaged,
    LevelUp,
}
public class SFXManager : MonoBehaviour
{
    public SFXBank bank;

    [HideInInspector] public GlobalSFX lastSFX;
    [HideInInspector] public float lastSFXTime;
    Dictionary<GlobalSFX, float> lastSFXTimes;
    public const float MIN_SFX_INTERVAL = 0.1f;

    AudioSource audioSource = null;

    public static SFXManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InitTimes();
        audioSource = GetComponent<AudioSource>();
    }

    void InitTimes()
    {
        lastSFXTimes = new();
        foreach (var sfx in EnumExtensions.GetValues<GlobalSFX>())
        {
            lastSFXTimes.Add(sfx, 0);
        }
    }

    public static void PlaySound(GlobalSFX sfx)
    {
        if (Instance == null || Time.time - Instance.lastSFXTimes[sfx] < MIN_SFX_INTERVAL)
            return;

        PlaySound((int)sfx);
        Instance.lastSFX = sfx;
        Instance.lastSFXTimes[sfx] = Time.time;
        Instance.lastSFXTime = Time.time;
    }

    public static void PlaySound(int id)
    {
        if (Instance == null || Instance.bank.clips[id] == null)
            return;
        Instance.audioSource.pitch = Random.Range(0.9f, 1.1f);
        Instance.audioSource.PlayOneShot(Instance.bank.clips[id], Instance.bank.volumes[id]);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SFXManager))]
public class SFXManagerEditor : Editor
{
    bool showSFXList = true;
    SerializedObject sfxBankObj;
    SerializedProperty clips;
    SerializedProperty volumes;

    private void OnEnable()
    {
        sfxBankObj = new SerializedObject(((SFXManager)target).bank);
        clips = sfxBankObj.FindProperty("clips");
        volumes = sfxBankObj.FindProperty("volumes");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SFXManager sfxManager = (SFXManager)target;

        showSFXList = EditorGUILayout.Foldout(showSFXList, "Global SFX");
        if(showSFXList && sfxManager.bank!=null)
        {
            SFXBankEditor.ShowClips(clips, volumes,sfxManager.bank);
        }
        if(sfxBankObj!=null)
            sfxBankObj.ApplyModifiedProperties();

        GUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(10);

        DrawDefaultInspector();
    }
}
#endif