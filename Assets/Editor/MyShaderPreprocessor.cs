using UnityEditor.Rendering;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Build;

class MyShaderPreprocessor : IPreprocessShaders
{
    public int callbackOrder => 0;

    public void OnProcessShader(
        Shader shader,
        ShaderSnippetData snippet,
        IList<ShaderCompilerData> data)
    {
        if (shader.name == "Hidden/Screen Rain_URP_Soft Streaks")
        {
            return;
        }

        if (shader.name == "Knife/Muzzle Flash_URP")
        {
            return;
        }
    }
}