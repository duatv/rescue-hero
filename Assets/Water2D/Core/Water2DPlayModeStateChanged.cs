#if UNITY_EDITOR
namespace Water2D
{
    using UnityEngine;
    using UnityEditor;


    // ensure class initializer is called whenever scripts recompile
    [InitializeOnLoadAttribute]
    public static class PlayModeStateChanged
    {

        static PlayModeStateChange _lastState;

        // register an event handler when the class is initialized
        static PlayModeStateChanged()
        {
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                /*
                GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
                for (int i = 0; i < gos.Length; i++)
                {
                    if (gos[i].name.Contains("Water2DParticlesID_"))
                    {
                        UnityEngine.GameObject.DestroyImmediate(gos[i]);
                    }

                }
                */
                Water2D_Spawner[] gos = (Water2D_Spawner[])GameObject.FindObjectsOfType(typeof(Water2D_Spawner));
                for (int i = 0; i < gos.Length; i++)
                {
                    if (!gos[i].PersistentFluid)
                    {
                        gos[i].SetupParticles();
                       
                    }

                }


            }

           
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                PhysicsSimulation.Stop();
            }

            
            _lastState = state;

        }
    }

    public class AssetModification : AssetModificationProcessor
    {

        static void OnWillSaveAssets(string[] paths)
        {
            ResizeQuadEffectController r_ = GameObject.FindObjectOfType<ResizeQuadEffectController>();
            if (r_) {
                //Debug.Log("saving");
                r_.AboutToRebuildAll();
            }

        }

        
    }

}
#endif