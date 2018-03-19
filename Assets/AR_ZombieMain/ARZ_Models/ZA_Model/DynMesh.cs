// @Author Nabil Lamriben ©2017
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynMesh : MonoBehaviour {
    public GameObject ObjectWithBodyMesh;
    //todonabil also adapt the haitr if not null. get the propper mat as well , not just load all, but alsos filter for hair
    public GameObject ObjectWithHairMesh;
    //shader variables must stay public 
    // DO NOT SET ANY OF THESE DOWN
    public Shader Shader_Standard;
    public Shader shader_MobileBumpedSpecular;
    public Shader shader_MobileBumpedSpecular1Light;
    public Shader shader_LegacyDecal;
    public Shader Shader_AdditiveSoft; //meant to be a secondary material's shader
    public Shader shader_bodymat_Cur;
    public Shader shader_hairmat_Cur;
    public Material[] AvailableMats;
    public Shader[] AvailableShaders;
    // DO NOT SET ANY OF THESE UP
    string ZombieBaseName;
  

    void Start() {
        ZombieBaseName = this.gameObject.name.Substring(0, 2);
        shader_bodymat_Cur = ObjectWithBodyMesh.GetComponent<Renderer>().material.shader;
        //shader_hairmat_Cur = ObjectWithHairMesh.GetComponent<Renderer>().material.shader;
        Shader_Standard = Shader.Find("Standard");
        shader_MobileBumpedSpecular = Shader.Find("Mobile/Bumped Specular");
        shader_MobileBumpedSpecular1Light = Shader.Find("Mobile/Bumped Specular (1 Directional Light)");
        shader_LegacyDecal = Shader.Find("Legacy Shaders/Decal");
        //to be l=placed on a secondary mat only
        Shader_AdditiveSoft = Shader.Find("Particles/Priority Additive (Soft)");
        LoadResourcesMaterials();
        LoadShaders();
        totalSingleMatShader =AvailableShaders.Length; //standard bumpspec decal
        totalZAmaterials= AvailableMats.Length;
    }

    void LoadResourcesMaterials()
    {
        string MatsPath = ZombieBaseName+ "_Mats";
        //ZA_AvailableMats = Resources.LoadAll("ZA_Mats", typeof(Material)).Cast<Material>().ToArray();
        AvailableMats = Resources.LoadAll(MatsPath, typeof(Material)).Cast<Material>().ToArray();      
    }
    void LoadShaders() {
        AvailableShaders = new Shader[3] { Shader_Standard, shader_MobileBumpedSpecular, shader_LegacyDecal };
    }

    void Update()
    {
        Cycle_MATS_single();
        Cycle_SHADER_single();
      //  switchShader();
      //  switchMaterial();
    }

    #region cyclethrough
    int totalSingleMatShader;
    int totalZAmaterials;

    int cnt_shader = 0;
    int cnt_material = 0;

    void Cycle_MATS_single() {
        if (Input.GetKeyDown(KeyCode.M))
        {
            cnt_material++;
            if (cnt_material >= totalZAmaterials) cnt_material = 0;
            SetSingleMat(cnt_material);
        }
    }

    void SetSingleMat(int argMatIndex) {
            ObjectWithBodyMesh.GetComponent<Renderer>().materials = new Material[1] { AvailableMats[argMatIndex] };
    }
    /// <summary>
    ///  in case i wanna trry dual material setup of mobile bumpdifuse sigle lightshader on one material , and ADDitiv shader on copy of  the mat
    /// </summary>
    /// <param name="argMatIndex"></param>
    //void SetDoubleMat(int argMatIndex)
    //{
    //    ObjectWithBodyMesh.GetComponent<Renderer>().materials = new Material[2] { ZA_AvailableMats[argMatIndex], ZA_AvailableMats[argMatIndex] };
    //}

    void Cycle_SHADER_single()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            cnt_shader++;
            if (cnt_shader >= totalSingleMatShader) cnt_shader = 0;
            SetShaderSingleMat(cnt_shader);

        }
    }

    void SetShaderSingleMat(int arg) {
        ObjectWithBodyMesh.GetComponent<Renderer>().materials[0].shader = AvailableShaders[arg];
    }
    #endregion


}
