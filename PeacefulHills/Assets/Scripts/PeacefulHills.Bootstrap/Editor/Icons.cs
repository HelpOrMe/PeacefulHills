using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Bootstrap.Editor
{
    public static class Icons
    {
        public static Texture2D Boot16X = AssetDatabase.LoadAssetAtPath<Texture2D>(
            "Assets/Scripts/PeacefulHills.Bootstrap/Editor Resources/boot-icon@16x.png");
        
        public static Texture2D Boot8X = AssetDatabase.LoadAssetAtPath<Texture2D>(
            "Assets/Scripts/PeacefulHills.Bootstrap/Editor Resources/boot-icon@8x.png"); 
        
        public static Texture2D Boot4X = AssetDatabase.LoadAssetAtPath<Texture2D>(
            "Assets/Scripts/PeacefulHills.Bootstrap/Editor Resources/boot-icon@4x.png"); 
        
        public static Texture2D Boot2X = AssetDatabase.LoadAssetAtPath<Texture2D>(
            "Assets/Scripts/PeacefulHills.Bootstrap/Editor Resources/boot-icon@2x.png"); 
    }
}