namespace Water2D
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using System.IO;

    public class PostProcessMethods
    {

        static bool chk = false;
        public static void checkIconTexture()
        {

            if (!chk && !File.Exists("Assets/Gizmos/w2d_gizmos.png"))
            {

                chk = true;

                string folderPath = "Assets/Gizmos";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    //Debug.Log("folder does not exist");
                }

                string oldPath = Water2D.CoreUtils.MainPath() + "Misc/w2d_gizmos.png";

                string newPath = folderPath + "/w2d_gizmos.png";

                FileUtil.CopyFileOrDirectory(oldPath, newPath); //
            }
        }

        public static void SyncWithPhysics2D()
        {
            Physics2D.autoSyncTransforms = true;
        }

    }
}



