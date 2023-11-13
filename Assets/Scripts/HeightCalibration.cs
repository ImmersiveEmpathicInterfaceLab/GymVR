using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.HandGrab;
using TMPro;
public class HeightCalibration : MonoBehaviour
{
    private bool calibrating = false;
    public Vector3 currentHandPosition;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;

    [SerializeField] private GameObject calibrationUI;
    [SerializeField] private TMP_Text calibrationText;

    [SerializeField] private HandGrabInteractor leftInteractor;
    [SerializeField] private HandGrabInteractor rightInteractor;

    [SerializeField] private GameObject handGrabCollider1;
    [SerializeField] private GameObject handGrabCollider2;

    [SerializeField] private Calibration[] calibrations;

    private float timeToCalibrate = 3f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {

        rightInteractor = Config.instance.rightInteractor;
        leftInteractor = Config.instance.leftInteractor;

        timer = timeToCalibrate;
    }

    // Update is called once per frame
    void Update()
    {
        if (calibrating != true) return;

        timer -= Time.deltaTime;
        calibrationText.text = "Hold position for " + int.Parse(timer.ToString());

        if(timer <= 0)
        {
            calibrating = false;
            //calibrationUI.SetActive(false);
            currentHandPosition = rightHand.transform.position;
            calibrationText.text = "Please place both arms on your side and form fists to calibrate";
            timer = timeToCalibrate;

            foreach (Calibration c in calibrations)
            {
                c.CalibrateHeight(currentHandPosition);
            }
        }
    }

    public void StartCalibrating()
    {
        calibrating = true;
    }
}
