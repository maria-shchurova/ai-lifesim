using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;
using UnityEditor;
using TMPro;

public class TempTest : MonoBehaviour
{
    public Button okButton;
    public TMP_InputField inputField;


    void Start()
    {
        okButton.onClick.AddListener(() => GenerateScript());
    }

    void GenerateScript()
    {
        //string scriptContent = "using UnityEngine;\n\npublic class Default : MonoBehaviour\n {\n private void Start()\n    {\n GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);\n cylinder.GetComponent<Renderer>().material.color = Color.red;\n }\n}";
        string scriptContent = "using UnityEngine;\n\npublic class Default : MonoBehaviour\n {\n public static void Foo()\n    {\n GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);\n cylinder.GetComponent<Renderer>().material.color = Color.red;\n\n }\n}";


        var assembly = CompilerExample.Compile(scriptContent);
        var method = assembly.GetType("Default").GetMethod("Foo");
        var del = (Action)Delegate.CreateDelegate(typeof(Action), method);
        del.Invoke();

    }

}
