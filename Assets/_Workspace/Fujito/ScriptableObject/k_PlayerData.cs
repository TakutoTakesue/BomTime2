using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CreatePlayerData", fileName = "PlayerData")]
[Serializable]
public class k_PlayerData : ScriptableObject
{
    [SerializeField]
    private int _sampleIntValue;

    public int SampleIntValue
    {
        get { return _sampleIntValue; }
#if UNITY_EDITOR
        set { _sampleIntValue = Mathf.Clamp(value, 0, int.MaxValue); }
#endif
    }
}