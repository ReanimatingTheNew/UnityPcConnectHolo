﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieShader : MonoBehaviour {

	


    public Shader shader1;
    public Shader shader2;
    public Renderer rend;
    void Start()
    {
        //
        rend = GetComponent<Renderer>();
        shader1 = rend.material.shader;// Shader.Find("Diffuse");
        shader2 = Shader.Find("Transparent/Diffuse");
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            if (rend.material.shader == shader1)
                rend.material.shader = shader2;
            else
                rend.material.shader = shader1;

    }

}
