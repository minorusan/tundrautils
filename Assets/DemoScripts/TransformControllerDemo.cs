//
//  TransformControllerDemo.cs
//  TundraUtils
//
//  Created on 10/11/2016.
//  Copyright © 2016 Tundra Mobile. All rights reserved.
//
using TundraUtils.Behaviours.Transformations;
using TundraUtils.Actions;

using UnityEngine;

using System.Collections.Generic;

namespace DemoScripts
{
    public class TransformControllerDemo : MonoBehaviour
    {
        public TransformController TransformController;
        public Transform[] Targets;
        public Transform Hell;

        public void OnBananas()
        {
            TransformController.ForceStop();
            var actionsList = new List<ITAction>();
            for (int i = 0; i < Targets.Length; i++)
            {
                actionsList.Add(new MoveTo(TransformController.transform, Targets[i].transform.position, 2f));
            }

            TransformController.PlaySequence(actionsList);
        }

        public void OnHell()
        {
            TransformController.ForceStop();
            TransformController.PlayAction(new MoveTo(TransformController.transform, Hell.transform.position, 2f));
        }
    }
}


