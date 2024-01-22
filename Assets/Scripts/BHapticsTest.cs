using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bhaptics.SDK2;
using TMPro;
using Oculus.Interaction.HandGrab;

// Test Change

[RequireComponent(typeof(WeightPositionTracking))]
public class BHapticsTest : MonoBehaviour
{
    [SerializeField] private TMP_Text leftHapticsText;
    [SerializeField] private TMP_Text rightHapticsText;

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


    //How much the intesity changes while raising the weights
    private float intensityChange = 2f;

    private float startingIntensity = 0.5f;
    private float maxIntensity = 1f;
    private float currentIntensity;

    private WeightPositionTracking positionTracking;

    // Start is called before the first frame update
    void Start()
    {
        //Used for intesity change for weight motion
        positionTracking = GetComponent<WeightPositionTracking>();

        //Debug Text to see if the haptics are on
        rightHapticsText = Config.instance.rightHapticText;
        leftHapticsText = Config.instance.leftHapticText;

        //Hand interactor components
        rightInteractor = Config.instance.rightInteractor;
        leftInteractor = Config.instance.leftInteractor;

        leftHapticsText.text = "Left Haptics: Off";
        leftHapticsText.color = new Color(255, 0, 0);

        rightHapticsText.text = "Right Haptics: Off";
        rightHapticsText.color = new Color(255, 0, 0);

        //Sets the current weight identifier based on the selected bellWeight
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

        currentIntensity = startingIntensity;
    }

    //Enum for the different weight values
    public enum Weight
    {
        Light,
        Medium,
        Heavy,
    }

    // Update is called once per frame
    void Update()
    {
        //Left hand is grabbing something and the gameobject this script is attached to is the one being grabbed (prevents all the weights from firing their vibrations at once when one is grabbed)
        if (leftInteractor.Grabbed && gameObject.name.Equals(leftInteractor.GrabbedWeight))
        {
            if(!leftHapticsOn){
                leftHapticsOn = true;
                StartCoroutine(LeftHandVibration());
            }
            leftHapticsText.text = "Left Haptics: On";
            leftHapticsText.color = new Color(0, 255, 0);

            //Handles lowering nad lifting of weight and intensity changes
            if (positionTracking.lifting)
            {
                //Sets the current intesnity based on the desired change times the differnce in the positions

                if (currentIntensity <= maxIntensity)
                {
                    currentIntensity += intensityChange * positionTracking.positionDifference;
                    Config.instance.intensityText.text = "Intensity: " + currentIntensity;
                }
            }
            else if (positionTracking.lowering)
            {
                //Sets the current intesnity based on the desired change times the differnce in the positions

                if (currentIntensity >= 0.1f)
                {
                    currentIntensity -= intensityChange * Mathf.Abs(positionTracking.positionDifference);
                    Config.instance.intensityText.text = "Intensity: " + currentIntensity;
                }
            }
        }
        //Player lets go of whats being grabbed, stop everything and turn off vibration
        else if(!leftInteractor.Grabbed)
        {
            leftHapticsOn = false;
            BhapticsLibrary.StopByEventId(leftHandIdentifier + currentWeightIdentifier);
            leftHapticsText.text = "Left Haptics: Off";
            leftHapticsText.color = new Color(255, 0, 0);
        }

        //Right hand is grabbing something and the gameobject this script is attached to is the one being grabbed (prevents all the weights from firing their vibrations at once when one is grabbed)
        if (rightInteractor.Grabbed && gameObject.name.Equals(rightInteractor.GrabbedWeight))
        {
            if (!rightHapticsOn)
            {
                rightHapticsOn = true;

                StartCoroutine(RightHandVibration());
            }

            rightHapticsText.text = "Right Haptics: On";
            rightHapticsText.color = new Color(0, 255, 0);

            //Handles lowering nad lifting of weight and intensity changes
            if (positionTracking.lifting)
            {
                //Sets the current intesnity based on the desired change times the differnce in the positions

                if (currentIntensity <= maxIntensity)
                {
                    currentIntensity += intensityChange * Mathf.Abs(positionTracking.positionDifference);
                    Config.instance.intensityText.text = "Intensity: " + currentIntensity;
                }
            }
            else if (positionTracking.lowering)
            {
                //Sets the current intesnity based on the desired change times the differnce in the positions

                if (currentIntensity >= 0.1f)
                {
                    currentIntensity -= intensityChange * Mathf.Abs(positionTracking.positionDifference);
                    Config.instance.intensityText.text = "Intensity: " + currentIntensity;
                }
            }
        }
        //Player lets go of whats being grabbed, stop everything and turn off vibration
        else if (!rightInteractor.Grabbed)
        {
            rightHapticsOn = false;
            BhapticsLibrary.StopByEventId(rightHandIdentifier + currentWeightIdentifier);
            rightHapticsText.text = "Right Haptics: Off";
            rightHapticsText.color = new Color(255, 0, 0);
        }
    }

    /// <summary>
    /// Coroutine for playing vibrations in the left hand
    /// </summary>
    /// <returns></returns>
    IEnumerator LeftHandVibration() {
        //BhapticsLibrary.Play(leftHandIdentifier + currentWeightIdentifier);
        BhapticsLibrary.PlayParam(leftHandIdentifier + currentWeightIdentifier, currentIntensity, 0.3f, 0, 0);

        yield return new WaitForSeconds(0.3f);

       leftHapticsOn = false;
    }

    /// <summary>
    /// Coroutine for playing vibrations in the right hand
    /// </summary>
    /// <returns></returns>
    IEnumerator RightHandVibration()
    {
        BhapticsLibrary.PlayParam(rightHandIdentifier + currentWeightIdentifier, currentIntensity, 0.3f, 0, 0);

        yield return new WaitForSeconds(0.3f);

        rightHapticsOn = false;
    }
}
