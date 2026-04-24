// Place this file inside a folder named 'Editor' anywhere in your Assets folder.
// e.g. Assets/Editor/DrawingTextureFixerEditor.cs
//
// Usage:
//   1. Select all your drawing sprite textures in the Project window
//   2. Go to Tools > Fix Drawing Textures
//   3. All selected textures will have Read/Write enabled and be reimported

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class DrawingTextureFixerEditor : Editor
{
    [MenuItem("Tools/Fix Drawing Textures")]
    private static void FixSelectedTextures()
    {
        Object[] selected = Selection.objects;

        if (selected.Length == 0)
        {
            EditorUtility.DisplayDialog(
                "Fix Drawing Textures",
                "No assets selected. Select one or more textures in the Project window first.",
                "OK");
            return;
        }

        int fixedCount = 0;
        int skippedCount = 0;

        foreach (Object obj in selected)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer == null)
            {
                skippedCount++;
                continue;
            }

            bool changed = false;

            if (!importer.isReadable)
            {
                importer.isReadable = true;
                changed = true;
            }

            // Also disable Crunch Compression, which also blocks alphaHitTestMinimumThreshold
            TextureImporterPlatformSettings settings = importer.GetDefaultPlatformTextureSettings();
            if (settings.crunchedCompression)
            {
                settings.crunchedCompression = false;
                importer.SetPlatformTextureSettings(settings);
                changed = true;
            }

            if (changed)
            {
                importer.SaveAndReimport();
                fixedCount++;
            }
            else
            {
                skippedCount++;
            }
        }

        EditorUtility.DisplayDialog(
            "Fix Drawing Textures",
            $"Done.\n\n{fixedCount} texture(s) updated.\n{skippedCount} already correct or skipped.",
            "OK");
    }
}
#endif