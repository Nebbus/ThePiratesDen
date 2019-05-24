using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_CandleLightFlicker : MonoBehaviour {
    public Light lt;
    float duration = 4.0f;
    float x  = 4;
    float y = 4;

    Color color0 = new Color(255, 189, 48);
    Color color1 = new Color(239, 218, 100);
    void Start () {
        lt = GetComponent<Light>();
	}

    void Update()
    {
        float t = Mathf.PingPong(Time.time, duration) / duration;
        lt.color = Color.Lerp(color0, color1, t);
        lt.range = 20 +Mathf.PerlinNoise(x * Time.time, y * Time.time) *100;
    }
}
