using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBG : MonoBehaviour
{
    public float Backgroudspeed;
    public Renderer BackgroudRenderer;

    // Update is called once per frame
    void Update()
    {
        BackgroudRenderer.material.mainTextureOffset += new Vector2(Backgroudspeed*Time.deltaTime,0f);
    }
}
