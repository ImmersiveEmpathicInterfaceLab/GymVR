using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightPositionTracking : MonoBehaviour
{

    private Vector3 initialPosition;
    private float lastY;
    private float currentY;

    //Whether the player is lifting or lowering the weight
    public bool lifting = false;
    public bool lowering = false;
    public float positionDifference;



    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        lastY = initialPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the current y position of the weight
        currentY = transform.position.y;

        //If the current y position is greater than last y position the weight is being lifted
        if(currentY > lastY)
        {
            lifting = true;
            lowering = false;

            Config.instance.liftingText.text = "Lifting";

            //Get position difference between the two positions
            positionDifference = Mathf.Abs(currentY) - Mathf.Abs(lastY);
        }
        //If the current y position is greater than last y position the weight is being lowered
        else if (currentY < lastY)
        {
            lifting = false;
            lowering = true;

            Config.instance.liftingText.text = "Lowering";

            //Get position difference between the two positions
            positionDifference = Mathf.Abs(currentY) - Mathf.Abs(lastY);
        }
        //Else not moving at all and is still
        else
        {
            Config.instance.liftingText.text = "Still";

            lifting = false;
            lowering = false;
        }

        //Get the last y position after checking for the previous one
        lastY = currentY;
    }
}
