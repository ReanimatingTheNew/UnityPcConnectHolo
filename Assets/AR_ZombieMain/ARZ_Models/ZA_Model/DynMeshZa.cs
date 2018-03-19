
//this onlyworks on existing mats on a zombie, so not a good randzombie generator
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynMeshZa : MonoBehaviour {


    //shader variables must stay public 
    // DO NOT SET ANY OF THESE DOWN
    public Shader Shader_StandardFast;
    public Shader Shader_Standard;
    public Shader shader_MobileBumpedSpecular;
    public Shader shader_MobileBumpedSpecular1Light;
    public Shader shader_LegacyDecal;
    public Shader Shader_AdditiveSoft; //meant to be a secondary material's shader
    public Shader[] AvailableShaders;
    // DO NOT SET ANY OF THESE UP

    Renderer[] _renderersOnThisZombie;
    Material[] _uniquMaterials;
    TimerBehavior repeatTimer;

    int cnt_shader = 0;

    void GEtShaderRefs()
    {
        Shader_StandardFast = Shader.Find("HoloToolkit/StandardFast");
        //shader_hairmat_Cur = ObjectWithHairMesh.GetComponent<Renderer>().material.shader;
        Shader_Standard = Shader.Find("Standard");
        shader_MobileBumpedSpecular = Shader.Find("Mobile/Bumped Specular");
        shader_MobileBumpedSpecular1Light = Shader.Find("Mobile/Bumped Specular (1 Directional Light)");
        shader_LegacyDecal = Shader.Find("Legacy Shaders/Decal");
        //to be l=placed on a secondary mat only
        Shader_AdditiveSoft = Shader.Find("Particles/Priority Additive (Soft)");

    }
    void PopulateUniqueMAterials()
    {
        List<Material> FullMatList = new List<Material>();
        foreach (Renderer r in _renderersOnThisZombie)
        {
            foreach (Material m in r.materials)
            {
                FullMatList.Add(m);
            }

        }

        //_uniquMaterials = FullMatList
        //    .GroupBy(p => p.name)
        //    .Select(g => g.First())
        //    .ToArray();

        _uniquMaterials = FullMatList.ToArray();
    }
    void PopulateAvailableShaders()
    {
        AvailableShaders = new Shader[4] { Shader_StandardFast, Shader_Standard, shader_MobileBumpedSpecular, shader_LegacyDecal };
    }


    void Start()
    {
        repeatTimer = gameObject.AddComponent<TimerBehavior>();

        GEtShaderRefs();// m.shader = shader_MobileBumpedSpecular;
        PopulateAvailableShaders();
        _renderersOnThisZombie = GetComponentsInChildren<Renderer>();
        PopulateUniqueMAterials();

        StartCoroutine(AUTOLOOP());
    }

  
    void SetShaderForMyMats(Shader argShader) {
        foreach (Material m in _uniquMaterials) {
            m.shader = argShader;
        }
    }

   public void SetShaderByID(int ArgCntShader)
    {
        if (ArgCntShader >= AvailableShaders.Length || ArgCntShader < 0) ArgCntShader = 0;
        SetShaderForMyMats(AvailableShaders[ArgCntShader]);
    }
 

 
 
    IEnumerator AUTOLOOP()
    {
        
        while (true)
        {
           
            yield return new WaitForSeconds(3);
            cnt_shader++;
            if (cnt_shader >= AvailableShaders.Length || cnt_shader < 0) cnt_shader = 0;
            SetShaderByID(cnt_shader);
        }
    }
}
