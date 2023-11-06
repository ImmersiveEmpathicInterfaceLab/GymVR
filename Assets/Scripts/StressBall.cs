using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bhaptics.SDK2;
using TMPro;
using Oculus.Interaction.HandGrab;
using Deform;
public class StressBall : MonoBehaviour
{
    [SerializeField] private TMP_Text leftHapticsText;
    [SerializeField] private TMP_Text rightHapticsText;

    [SerializeField] private HandGrabInteractor leftInteractor;
    [SerializeField] private HandGrabInteractor rightInteractor;

    public Elasticity elasticity;

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

    private SquashAndStretchDeformer deformer;
    private float highElasticityFactor = -0.32f;
    private float mediumElasticityFactor = -0.23f;
    private float lowElasticityFactor = -0.14f;

    private float currentElasticityIdentifier;

    // Start is called before the first frame update
    void Start()
    {
        rightHapticsText = Config.instance.rightHapticText;
        leftHapticsText = Config.instance.leftHapticText;

        rightInteractor = Config.instance.rightInteractor;
        leftInteractor = Config.instance.leftInteractor;

        leftHapticsText.text = "Left Haptics: Off";
        leftHapticsText.color = new Color(255, 0, 0);

        rightHapticsText.text = "Right Haptics: Off";
        rightHapticsText.color = new Color(255, 0, 0);

        switch (elasticity)
        {
            case Elasticity.High:
                {
                    currentWeightIdentifier = lowWeightIdentifier;
                    currentElasticityIdentifier = highElasticityFactor;
                    break;
                }

            case Elasticity.Medium:
                {
                    currentWeightIdentifier = mediumWeightIdentifier;
                    currentElasticityIdentifier = mediumElasticityFactor;
                    break;
                }

            case Elasticity.Low:
                {
                    currentWeightIdentifier = heavyWeightIdentifier;
                    currentElasticityIdentifier = lowElasticityFactor;
                    break;
                }
        }

        deformer = GetComponentInChildren<SquashAndStretchDeformer>();
    }

    public enum Elasticity
    {
        High,
        Medium,
        Low,
    }

    // Update is called once per frame
    void Update()
    {

        if (leftInteractor.Grabbed && gameObject.name.Equals(leftInteractor.GrabbedWeight))
        {
            if (!leftHapticsOn)
            {
                leftHapticsOn = true;
                StartCoroutine(LeftHandVibration());
            }
            leftHapticsText.text = "Left Haptics: On";
            leftHapticsText.color = new Color(0, 255, 0);

            //Deforms the ball when grabbed
            deformer.Factor = currentElasticityIdentifier;
        }
        else if (!leftInteractor.Grabbed)
        {
            leftHapticsOn = false;
            BhapticsLibrary.StopByEventId(leftHandIdentifier + currentWeightIdentifier);
            leftHapticsText.text = "Left Haptics: Off";
            leftHapticsText.color = new Color(255, 0, 0);

            //Resets the balls mesh
            deformer.Factor = 0;
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

            //Deforms the ball when grabbed
            deformer.Factor = currentElasticityIdentifier;
        }
        else if (!rightInteractor.Grabbed)
        {
            rightHapticsOn = false;
            BhapticsLibrary.StopByEventId(rightHandIdentifier + currentWeightIdentifier);
            rightHapticsText.text = "Right Haptics: Off";
            rightHapticsText.color = new Color(255, 0, 0);

            if (leftInteractor.GrabbedWeight != gameObject.name)
            {
                //Resets the ball mesh
                deformer.Factor = 0;
            }
        }
    }

    IEnumerator LeftHandVibration()
    {
        BhapticsLibrary.Play(leftHandIdentifier + currentWeightIdentifier);
        //BhapticsLibrary.PlayParam(leftHandIdentifier + currentWeightIdentifier, currentIntensity, 0.3f, 0, 0);

        yield return new WaitForSeconds(0.3f);

        leftHapticsOn = false;
    }

    IEnumerator RightHandVibration()
    {
        BhapticsLibrary.Play(rightHandIdentifier + currentWeightIdentifier);

        yield return new WaitForSeconds(1f);

        rightHapticsOn = false;
    }
}
