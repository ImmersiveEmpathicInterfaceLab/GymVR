using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bhaptics.SDK2;
using TMPro;
using Oculus.Interaction.HandGrab;
public class BHapticsTest : MonoBehaviour
{
    [SerializeField] private TMP_Text leftHapticsText;
    [SerializeField] private TMP_Text rightHapticsText;
    [SerializeField] private TMP_Text vibrationCounter;

    private int vibrations = 0;

    [SerializeField] private HandGrabInteractor leftInteractor;
    [SerializeField] private HandGrabInteractor rightInteractor;

    public Weight bellWeight;

    private bool fireTest = false;

    //Used to find vibration in bHaptic
    private const string rightHandIdentifier = "righthand";
    private const string leftHandIdentifier = "lefthand";

    private const string lowWeightIdentifier = "lowweights";
    private const string mediumWeightIdentifier = "mediumweights";
    private const string heavyWeightIdentifier = "heavyweights";

    private string currentWeightIdentifier;

    private bool leftHapticsOn = false;
    private bool rightHapticsOn = false;
    // Start is called before the first frame update
    void Start()
    {
        rightHapticsText = Config.instance.rightHapticText;
        leftHapticsText = Config.instance.leftHapticText;
        vibrationCounter = Config.instance.vibrationText;

        rightInteractor = Config.instance.rightInteractor;
        leftInteractor = Config.instance.leftInteractor;

        leftHapticsText.text = "Left Haptics: Off";
        leftHapticsText.color = new Color(255, 0, 0);

        rightHapticsText.text = "Right Haptics: Off";
        rightHapticsText.color = new Color(255, 0, 0);

        switch (bellWeight)
        {
            case Weight.Light:
                {
                    currentWeightIdentifier = lowWeightIdentifier;
                    break;
                }

            case Weight.Medium:
                {
                    currentWeightIdentifier = mediumWeightIdentifier;
                    break;
                }

            case Weight.Heavy:
                {
                    currentWeightIdentifier = heavyWeightIdentifier;
                    break;
                }
        }

        Debug.Log(currentWeightIdentifier);
    }

    public enum Weight
    {
        Light,
        Medium,
        Heavy,
    }

    // Update is called once per frame
    void Update()
    {

        if (leftInteractor.Grabbed && gameObject.name.Equals(leftInteractor.GrabbedWeight))
        {
            if(!leftHapticsOn){
                leftHapticsOn = true;
                StartCoroutine(LeftHandVibration());
            }
            leftHapticsText.text = "Left Haptics: On";
            leftHapticsText.color = new Color(0, 255, 0);
        }
        else if(!leftInteractor.Grabbed)
        {
            leftHapticsOn = false;
            BhapticsLibrary.StopByEventId(leftHandIdentifier + currentWeightIdentifier);
            leftHapticsText.text = "Left Haptics: Off";
            leftHapticsText.color = new Color(255, 0, 0);
        }
        
        if (rightInteractor.Grabbed && gameObject.name.Equals(rightInteractor.GrabbedWeight))
        {
            if (!rightHapticsOn)
            {
                rightHapticsOn = true;

                StartCoroutine(RightHandVibration());
            }

            rightHapticsText.text = "Right Haptics: On";
            rightHapticsText.color = new Color(0, 255, 0);
        }
        else if(!rightInteractor.Grabbed)
        {
            rightHapticsOn = false;
            BhapticsLibrary.StopByEventId(rightHandIdentifier + currentWeightIdentifier);
            rightHapticsText.text = "Right Haptics: Off";
            rightHapticsText.color = new Color(255, 0, 0);
        }
    }

    IEnumerator LeftHandVibration() {
        BhapticsLibrary.Play(leftHandIdentifier + currentWeightIdentifier);

        yield return new WaitForSeconds(1f);

       leftHapticsOn = false;
    }

    IEnumerator RightHandVibration()
    {
        BhapticsLibrary.Play(rightHandIdentifier + currentWeightIdentifier);

        yield return new WaitForSeconds(1f);

        rightHapticsOn = false;
    }

    private void OnTriggerStay(Collider other)
    {
       if (other.gameObject.CompareTag("Player"))
        {
            if (leftHapticsOn == false)
            {
                Debug.Log(leftHandIdentifier + currentWeightIdentifier);
                leftHapticsOn = true;
                StartCoroutine(LeftHandVibration());
            }
        }
    }
}
