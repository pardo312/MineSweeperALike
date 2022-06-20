using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenPackages.ServiceLocator
{
    [CreateAssetMenu(fileName = "ServiceLocatorConfig.asset", menuName = "Jiufen/ServiceLocator/ServiceLocatorConfig")]
    public class ServiceLocatorConfig : ScriptableObject
    {
        public List<GameObject> listOfPrefabs;
    }
}