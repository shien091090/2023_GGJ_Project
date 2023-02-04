using UnityEngine;

[CreateAssetMenu]
public class CharacterSetting : ScriptableObject
{
    public float breakAcceleration;
    public float jumpForceConsume;
    public float jumpForce;
    public float strikeRiseForce;
    public float footRadius;
    public AnimationCurve moveSpeedCurve;
    public AnimationCurve horizontalStrikeCurve;
    public AnimationCurve verticalStrikeCurve;
}