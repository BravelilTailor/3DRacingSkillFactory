using System;
using UnityEngine;

public class CarInputControl : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private AnimationCurve breakCurve;
    [SerializeField] private AnimationCurve steerCurve;

    [SerializeField] [Range(0.0f, 1.0f)] private float autoBrakeStrength = 0.2f;

    private float wheelSpeed;
    private float verticalAxis;
    private float horizontalAxis;
    private float handbrakeAxis;

    private void Update()
    {
        wheelSpeed = car.WheelSpeed;

        UpdateAxis();

        UpdateThrottleandBrake();
        UpdateSteer();

        car.SteerControl = Input.GetAxis("Horizontal");

        UpdateAutoBrake();

        // Debug
        if (Input.GetKeyDown(KeyCode.E))
            car.Upgear();

        if (Input.GetKeyDown(KeyCode.Q))
            car.DownGear();
    }

    private void UpdateThrottleandBrake()
    {
        if (Mathf.Sign(verticalAxis) == Mathf.Sign(wheelSpeed) || Mathf.Abs(wheelSpeed) < 0.5f)
        {
            car.ThrottleControl = Mathf.Abs(verticalAxis);
            car.BrakeControl = 0;
        }
        else
        {
            car.ThrottleControl = 0;
            car.BrakeControl = breakCurve.Evaluate(wheelSpeed / car.maxSpeed);
        }

        // Gears
        if (verticalAxis < 0 && wheelSpeed > -0.5f && wheelSpeed <= 0.5f)
        {
            car.ShiftToReverseGear();
        }

        if (verticalAxis > 0 && wheelSpeed > -0.5f && wheelSpeed < 0.5f)
        {
            car.ShiftToFirstGear();
        }
    }
    private void UpdateSteer()
    {
        car.SteerControl = steerCurve.Evaluate(wheelSpeed / car.maxSpeed) * horizontalAxis;
    }


    private void UpdateAutoBrake()
    {
        if (verticalAxis == 0)
        {
            car.BrakeControl = breakCurve.Evaluate(wheelSpeed / car.maxSpeed) * autoBrakeStrength;
        }
    }

    private void UpdateAxis()
    {
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");
        handbrakeAxis = Input.GetAxis("Jump");
    }

    public void Stop()
    {
        verticalAxis = 0;
        horizontalAxis = 0;
        handbrakeAxis = 0;
        car.ThrottleControl = 0;
        car.SteerControl = 0;
        car.BrakeControl = 1;
    }
}
