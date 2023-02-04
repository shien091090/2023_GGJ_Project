using UnityEngine;

[CreateAssetMenu]
public class CharacterSetting : ScriptableObject
{
    public float acceleration;
    public float breakAcceleration;
    public float speedLimit;
    public float jumpForceConsume;
    public float jumpForce;
}