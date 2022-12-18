using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage Data", menuName = "Scriptable Object/StageData", order = int.MaxValue)]
public class Stage : ScriptableObject
{
    [SerializeField]
    public StageState[] stats;
}

[System.Serializable]
public struct StageState
{
    public int width;
    public int height;

    [Space(20)]
    public int[] UsingGems;
    public int[] UsingGemNumbers;

}
