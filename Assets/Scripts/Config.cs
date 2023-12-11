using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Oculus.Interaction.HandGrab;

/// <summary>
/// Singleton used for holding some common items that would need to be assigned in different scripts
/// </summary>
public class Config : MonoBehaviour
{
    public static Config instance;

    [SerializeField]public TMP_Text rightHapticText;
    public TMP_Text leftHapticText;
    public TMP_Text liftingText;
    public TMP_Text intensityText;

    public HandGrabInteractor leftInteractor;
    public HandGrabInteractor rightInteractor;

    void Awake()
    {
        instance = this;
    }
}
