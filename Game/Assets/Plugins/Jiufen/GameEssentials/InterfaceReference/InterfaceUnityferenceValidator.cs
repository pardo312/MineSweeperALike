using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenModules.InterfaceReferenceValidator
{
    public static class InterfaceUnityRefereceValidator
    {
        public static T ValidateIfUnityObjectIsOfType<T>(UnityEngine.Object unityObject, string classNamespace = null)
        {
            if (unityObject == null)
                return default(T);

            if (unityObject is T)
            {
                return (T)(object)unityObject;
            }
            else if (unityObject is GameObject && (unityObject as GameObject).GetComponent<T>() != null)
            {
                return ((GameObject)unityObject).GetComponent<T>();
            }
            else
            {
                Type classType = Type.GetType(classNamespace + unityObject.name);
                if (classType != null)
                    return (T)Activator.CreateInstance(classType);

                Debug.LogError($"<color=red>ValidateInterface:</color>  The item: [{unityObject.name}] is not a {typeof(T).Name} subclass. Please check the reference");
                return default(T);
            }
        }

        public static List<T> ValidateIfUnityObjectArrayIsOfType<T>(UnityEngine.Object[] _unityObjects, string _classNamespace = null, string _classAssembly = null)
        {
            List<T> returnList = new List<T>();
            for (int i = 0; i < _unityObjects.Length; i++)
            {
                UnityEngine.Object item = _unityObjects[i];

                if (item == null)
                    continue;


                if ((object)item is T)
                {
                    returnList.Add((T)(object)item);
                }
                else if (item is GameObject && (item as GameObject).GetComponent<T>() != null)
                {
                    returnList.Add((item as GameObject).GetComponent<T>());
                }
                else
                {
                    string fullNameOfType = _classNamespace + item.name + (_classAssembly != null ? $", {_classAssembly}" : "");
                    Type classType = Type.GetType(fullNameOfType);

                    if (classType != null)
                    {
                        try
                        {
                            returnList.Add((T)Activator.CreateInstance(classType));
                        }
                        catch
                        {
                            Debug.LogError($"<color=red>ValidateInterface:</color>  The item: [{item.name}] is not a {typeof(T).Name} subclass. Please check the reference");
                        }
                        continue;
                    }
                    Debug.LogError($"<color=red>ValidateInterface:</color>  The item: [{item.name}] is not a {typeof(T).Name} subclass. Please check the reference");
                }

            }
            //Return List
            return returnList;
        }
    }
}
