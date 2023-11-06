using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Oculus.Interaction.HandGrab;

public class Config : MonoBehaviour
{
    public static Config instance;

    [SerializeField]public TMP_Text rightHapticText;
    public TMP_Text leftHapticText;
    public TMP_Text vibrationText;

    public HandGrabInteractor leftInteractor;
    public HandGrabInteractor rightInteractor;

    void Awake()
    {
        instance = this;
    }
}
