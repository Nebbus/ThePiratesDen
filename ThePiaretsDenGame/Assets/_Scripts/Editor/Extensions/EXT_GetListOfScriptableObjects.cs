using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public static class EXT_GetListOfScriptableObjects
{
    /// <summary>
    /// generic funntkion building a list of a specific type
    /// </summary>
    /// <param name="genericType"> the type</param>
    /// <param name="genericTyps"> the aray of all typs that was found</param>
    /// <param name="genericTypeNames"> the list of said types name</param>
    public static void SetGenericNamesArray(Type genericType, out Type[] genericTyps, out string[] genericTypeNames)
    {

        /* Get all the types that are in the same 
         * Assembly (all the runtime scripts) as the genericType type.
         */
        Type[] allTypes = genericType.Assembly.GetTypes();

        /* Create an empty list to store all the types 
         * that are subtypes of genericType.
         */
        List<Type> reactionSubTypeList = new List<Type>();

        // Go through all the types in the Assembly...
        for (int i = 0; i < allTypes.Length; i++)
        {
            /* ... and if they are a non-abstract subclass of 
             * genericType then add them to the list.
             */
            if (allTypes[i].IsSubclassOf(genericType) && !allTypes[i].IsAbstract)
            {
                reactionSubTypeList.Add(allTypes[i]);
            }
        }

        // Convert the list to an array and store it.
        genericTyps = reactionSubTypeList.ToArray();

        /* Create an empty list of strings to store the names 
         * of the Reaction types.
         */
        List<string> reactionTypeNameList = new List<string>();

        // Go through all the genericType types and add their names to the list.
        for (int i = 0; i < genericTyps.Length; i++)
        {
            reactionTypeNameList.Add(genericTyps[i].Name);
        }

        // Convert the list to an array and store it.
        genericTypeNames = reactionTypeNameList.ToArray();
    }



}
