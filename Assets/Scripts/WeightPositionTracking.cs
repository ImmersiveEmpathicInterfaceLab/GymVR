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
        currentY = transform.position.y;

        if(currentY > lastY)
        {
            lifting = true;
            lowering = false;

            Config.instance.liftingText.text = "Lifting";

            positionDifference = Mathf.Abs(currentY) - Mathf.Abs(lastY);
        }
        else if(currentY < lastY)
        {
            lifting = false;
            lowering = true;

            Config.instance.liftingText.text = "Lowering";

            positionDifference = Mathf.Abs(currentY) - Mathf.Abs(lastY);
        }
        else
        {
            Config.instance.liftingText.text = "Still";

            lifting = false;
            lowering = false;
        }

        lastY = currentY;
    }
}
