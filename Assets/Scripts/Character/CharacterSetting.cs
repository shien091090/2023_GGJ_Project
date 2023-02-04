using UnityEngine;

[CreateAssetMenu]
public class CharacterSetting : ScriptableObject
{
    public float acceleration;
    public float accelerationLimit;
    public float breakAcceleration;
    public float jumpForceConsume;
    public float jumpForce;
    public float strikeRiseForce;
    public AnimationCurve moveSpeedCurve;
}