/*
------------------------------------------------------------------
  This file is part of UnitySharpNEAT 
  Copyright 2020, Florian Wolf
  https://github.com/flo-wolf/UnitySharpNEAT
------------------------------------------------------------------
*/
using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System;

namespace UnitySharpNEAT
{
    /// <summary>
    /// The SatelliteController controls a Satellite (Unit) that learns to find its way to a goal by avoiding obstacles.
    /// A 360 degree sensor range measures distances to the x nearest obstacles, which get input into its Neural Network. 
    /// The output corresponds to steering and thrust control. 
    /// The fitness of this Unit is calculated by how close to the goal it reached, how much time it took, and how (few) hits it has taken.
    /// When the Unit gets deactivated, it gets hidden and its values including its Transform get reset, to get a "fresh", reusable Unit on its next Activation.
    /// The resetting is important, since we use object pooling to resuse Units instead of instantiating/destroying them.
    /// </summary>
    public class SatelliteController : UnitController
    {
        public Vector3 _goalPosition;

        // general control variables
        public float Speed = 5f;
        public float TurnSpeedVertical = 180f;
        public float TurnSpeedHorizontal = 180f;
        public float TurnSpeedPerpendicular = 180f;
        public float SensorRange = 10;

        // track progress
        public float timeElapsed = 0;
        public int obstacleHits = 0;
        public int numObstaclesDetected = 0;
        public float distanceRemaining;

        private bool _boundsHit = false;
        private bool _movingForward = true;

        // cache the initial transform of this unit, to reset it on deactivation
        private float startTime = default;
        private Vector3 _initialPosition = default;
        private Quaternion _initialRotation = default;

        private void Start()
        {
            // cache the inital transform of this Unit, so that when the Unit gets reset, it gets put into its initial state
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            startTime = Time.time;
            distanceRemaining = Vector3.Distance(_initialPosition, _goalPosition);
        }

        private void Update() 
        {
            timeElapsed = Time.time - startTime;
            distanceRemaining = Vector3.Distance(transform.position, _goalPosition);
        }

        protected override void UpdateBlackBoxInputs(ISignalArray inputSignalArray)
        {
            float 
                top = 0,
                bottom = 0,
                left = 0,
                right = 0,
                front = 0,
                frontLeft = 0,
                frontRight = 0,
                frontTop = 0,
                frontBottom = 0,
                back = 0,
                backLeft = 0,
                backRight = 0,
                backTop = 0,
                backBottom = 0;

            // Nine raycasts into different directions each measure how far an obstacle is away
            RaycastHit hit;

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0, 1, 0).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    top = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0, -1, 0).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    bottom = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(-1, 0, 0).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    left = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(1, 0, 0).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    right = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0, 0, 1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    front = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(-0.5f, 0, 1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    frontLeft = 1 - hit.distance / SensorRange;
                }
            }
            
            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0.5f, 0, 1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    frontRight = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0, 0.5f, 1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    frontTop = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0, -0.5f, 1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    frontBottom = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0, 0, -1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    back = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(-0.5f, 0, -1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    backLeft = 1 - hit.distance / SensorRange;
                }
            }
            
            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0.5f, 0, -1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    backRight = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0, 0.5f, -1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    backTop = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0, -0.5f, -1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    numObstaclesDetected++;
                    backBottom = 1 - hit.distance / SensorRange;
                }
            }

            // modify the ISignalArray object of the blackbox that was passed into this function, by filling it with the sensor information.
            // Make sure that NeatSupervisor.NetworkInputCount fits the amount of sensors you have
            inputSignalArray[0] = top;
            inputSignalArray[1] = bottom;
            inputSignalArray[2] = left;
            inputSignalArray[3] = right;
            inputSignalArray[4] = front;
            inputSignalArray[5] = frontLeft;
            inputSignalArray[6] = frontRight;
            inputSignalArray[7] = frontTop;
            inputSignalArray[8] = frontBottom;
            inputSignalArray[9] = back;
            inputSignalArray[10] = backLeft;
            inputSignalArray[11] = backRight;
            inputSignalArray[12] = backTop;
            inputSignalArray[13] = backBottom;
        }

        protected override void UseBlackBoxOutpts(ISignalArray outputSignalArray)
        {
            var gas = (float) outputSignalArray[0] * 2 - 1;
            var steerVertical = (float) outputSignalArray[1] * 2 - 1;
            var steerHorizontal = (float) outputSignalArray[2] * 2 - 1;
            var steerPerpendicular = (float) outputSignalArray[3] * 2 - 1;

            var moveDist = gas * Speed * Time.deltaTime;
            var turnAngleVertical = steerVertical * TurnSpeedVertical * Time.deltaTime * gas;
            var turnAngleHorizontal = steerHorizontal * TurnSpeedHorizontal * Time.deltaTime * gas;
            var turnAnglePerpendicular = steerPerpendicular * TurnSpeedPerpendicular * Time.deltaTime * gas;

            transform.Translate(Vector3.forward * moveDist);
            transform.Rotate(new Vector3(turnAngleVertical, 0, 0));
            transform.Rotate(new Vector3(0, turnAngleHorizontal, 0));
            transform.Rotate(new Vector3(0, 0, turnAnglePerpendicular));
        }

        protected override void HandleIsActiveChanged(bool newIsActive)
        {
            if (newIsActive == false)
            {
                // the unit has been deactivated, IsActive was switched to false

                // reset transform
                transform.position = _initialPosition;
                transform.rotation = _initialRotation;

                // reset start time
                startTime = Time.time;

                // reset members
                timeElapsed = 0;
                obstacleHits = 0;
                numObstaclesDetected = 0;
                distanceRemaining = Vector3.Distance(_initialPosition, _goalPosition);
                _boundsHit = false;
                _movingForward = true;
            }

            // hide/show children 
            // the children happen to be the Satellite meshes => we hide this Unit when IsActive turns false and show it when it turns true
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(newIsActive);
            }
        }

        public override float GetFitness()
        {
            // calculate a fitness value based on how far goal was and how many obstacles collided.
            // return Mathf.Max(numObstaclesDetected - obstacleHits * 0.2f, 0);
            return Mathf.Max((400 / distanceRemaining) + (200 / timeElapsed) - obstacleHits * 0.2f, 0);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!IsActive)
                return;

            if (collision.collider.CompareTag("Obstacle"))
            {
                obstacleHits++;
                print("Hit");
                // if (obstacleHits == 10)
                // {
                //     _movingForward = false;
                // }
            }

            else if (collision.collider.CompareTag("Bounds"))
            {
                _boundsHit = true;
                _movingForward = false;
            }
            
            else if (collision.collider.CompareTag("Goal"))
            {
                _movingForward = false;
            }
        }
    }
}