using System.IO;
using UnityEditor;

public class AssetBundleBuildManager
{
    [MenuItem("Mytool/Build AssetBundles")]

    public static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        //AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("에셋 번들 빌드", "에셋 번들 빌드 완료","완료");
    }
}
