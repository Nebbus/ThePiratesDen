using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_CandleLightFlicker : MonoBehaviour {
    public Random randomRange;
    public Light lt;
    float duration = 1.0f;
    Color color0 = Color.red;
    Color color1 = Color.blue;
    void Start () {
        lt = GetComponent<Light>();
	}

    void Update() {
        float t = Mathf.PingPong(Time.time, duration) / duration;
        lt.color = Color.Lerp(color0, color1, t);
    }



}
