using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateProjector : MonoBehaviour
{
    public float fps = 30.0f;
    public Texture2D[] frames;

    private int FrameIndex;
    private Projector projector;

    private void Start()
    {
        projector = GetComponent<Projector>();
        NextFrame();
        InvokeRepeating("NextFrame", 1 / fps, 1 / fps);
    }

    private void NextFrame()
    {
        projector.material.SetTexture("_ShadowTex", frames[FrameIndex]);
        FrameIndex = (FrameIndex + 1) % frames.Length;
    }
}
