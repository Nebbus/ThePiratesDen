﻿
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;


public static class EXT_GraphicalRayCast  {

    


    public static List<RaycastResult> GrapphicRayCast(GraphicRaycaster m_Raycaster, EventSystem m_EventSystem)
    {
        PointerEventData m_PointerEventData;

        //Set up the new Pointer event
        m_PointerEventData = new PointerEventData(m_EventSystem);

        //Set teh pointer event position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;
        
        //Create a list of raycast Result
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and move mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        return results;
    }


    public static List<RaycastResult> PhysicalRayCast(PhysicsRaycaster m_Raycaster, EventSystem m_EventSystem)
    {
        PointerEventData m_PointerEventData;
        
        //Set up the new Pointer event
        m_PointerEventData = new PointerEventData(m_EventSystem);

        //Set teh pointer event position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of raycast Result
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and move mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        return results;
    }


}