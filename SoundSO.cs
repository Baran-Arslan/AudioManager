using UnityEngine;


[CreateAssetMenu(fileName = "New Sound", menuName = "Sounds/Sound")]
public class SoundSO : ScriptableObject
{
    [Header("UI")]
    public SoundData UIArrowSelect;
    public SoundData UIArrowRemove;

    [Header("Bow / Arrow"), Space(10)]
    public SoundData[] PullStrings;
    public SoundData[] ArrowPickup;

}
