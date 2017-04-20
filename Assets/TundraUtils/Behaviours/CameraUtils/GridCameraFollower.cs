//
//  GridCameraFollower.cs
//  TundraUtils
//
//  Created on 01/04/2017.
//  Copyright © 2017 Tundra Mobile. All rights reserved.
//
using UnityEngine;


namespace TundraUtils.Behaviours.CameraUtils
{
    public class GridCameraFollower : MonoBehaviour
    {
        public void LateUpdate()
        {
            var camPos = Camera.main.transform.position;
            camPos.x = 0.5f + (int)camPos.x; // 0.5f - is a offset, to center grid
            camPos.y = 0.5f + (int)camPos.y;
            camPos.z = transform.position.z;

            transform.position = camPos;
        }
    }
}
