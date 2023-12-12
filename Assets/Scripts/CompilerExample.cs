using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Immutable;

public class CompilerExample : MonoBehaviour
{
    void Start()
    {
        var assembly = Compile(@"
      using UnityEngine;

      public class Test
      {
      public static void Foo()
      {
      Debug.Log(""Hello, World!"");
      }
    }");

        var method = assembly.GetType("Test").GetMethod("Foo");
        var del = (Action)Delegate.CreateDelegate(typeof(Action), method);
        del.Invoke();
    }

    public static Assembly Compile(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);

        var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithOptimizationLevel(OptimizationLevel.Release);

        var compilation = CSharpCompilation.Create("DynamicAssembly")
            .WithOptions(compilationOptions)
            .AddReferences(GetAssemblyReferences())
            .AddSyntaxTrees(syntaxTree);

        using (var ms = new System.IO.MemoryStream())
        {
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var errors = GetErrors(result.Diagnostics);

                var msg = new StringBuilder();
                foreach (var error in errors)
                {
                    msg.AppendFormat("Error ({0}): {1}\n", error.Id, error.GetMessage());
                }
                throw new Exception(msg.ToString());
            }

            ms.Seek(0, System.IO.SeekOrigin.Begin);
            return Assembly.Load(ms.ToArray());
        }
    }

    private static IEnumerable<Diagnostic> GetErrors(ImmutableArray<Diagnostic> diagnostics)
    {
        var errors = new List<Diagnostic>();
        foreach (var diagnostic in diagnostics)
        {
            if (diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error)
            {
                errors.Add(diagnostic);
            }
        }
        return errors;
    }


    private static MetadataReference[] GetAssemblyReferences()
    {
        var references = new[]
        {
        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(UnityEngine.Debug).Assembly.Location),
        // Add more references as needed
    };

        return references;
    }
}