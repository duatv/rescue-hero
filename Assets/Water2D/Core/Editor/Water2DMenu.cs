namespace Water2D
{
	using UnityEngine;
	using UnityEditor;
    using UnityEngine.Rendering;
    using System.Collections;
    using Apptouch;

    public class DynamicWSpawnerMenu : Editor
    {

        static internal Water2D.Water2D_Spawner WSpawner;
        const string menuPathMain = "GameObject/Water2D PRO";
        const string menuPath = menuPathMain + "/Spawner";
        const string menuPathFiller = menuPathMain + "/Filler";
       

        SerializedObject tmpAsset;

        private static string _path;

        internal void OnEnable()
        {
            WSpawner = target as Water2D.Water2D_Spawner;
            if (_path == null) _path = EditorUtils.getMainRelativepath();

        }



        [MenuItem(menuPath + "/Regular Water", false, 10)]
        static void AddRegular()
        {
            FindAndDeactiveMainCamera();
            AddRegularSpawnerLegacy();
           
        }


        [MenuItem(menuPath + "/Toon Water", false, 11)]
        static void AddToon()
        {
            FindAndDeactiveMainCamera();
            AddToonSpawnerLWRP();

        }

        [MenuItem(menuPath + "/Refracting Water", false, 12)]
        static void AddRefract()
        {
            FindAndDeactiveMainCamera();
            AddRefractingLegacy();

        }

        [MenuItem(menuPath + "/Slime", false, 24)]
        static void AddSlime()
        {
            FindAndDeactiveMainCamera();
            AddSlimeStyle();

        }

        [MenuItem(menuPath + "/Oil", false, 25)]
        static void AddOil()
        {
            FindAndDeactiveMainCamera();
            AddOilStyle();

        }

        [MenuItem(menuPath + "/Bouncy Water", false, 26)]
        static void AddBouncyWater()
        {
            FindAndDeactiveMainCamera();
            AddBouncyWaterStyle();

        }

        [MenuItem(menuPathMain + "/Extras/Add Cameras", false, 37)]
        static void AddCameras()
        {
            FindAndDeactiveMainCamera();
            addCameras();

        }

        [MenuItem(menuPathMain + "/Extras/Clean Scene", false, 38)]
        static void CleanScene()
        {
            if (EditorUtility.DisplayDialog("Water2D Pro cleaner", "Do you want to clean the scene of unused particles?", "CLEAN", "Cancel"))
            {
                GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
                foreach (GameObject go in allObjects)
                    if (go.name.Contains("Water2DParticlesID_"))
                    {
                        DestroyImmediate(go);
                    }

            }



        }


        [MenuItem(menuPathFiller + "/Shape as Regular", false, 12)]
        static void AddBoxFiller()
        {
            FindAndDeactiveMainCamera();
            AddRegularFiller();

        }
        [MenuItem(menuPathFiller + "/Shape as Refracting", false, 13)]
        static void AddBoxFillerRefract()
        {
            FindAndDeactiveMainCamera();
            AddRefractingFiller();

        }


        static void FindAndDeactiveMainCamera()
        {
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
            if (cam && cam.name != "2-DefaultCamera") cam.SetActive(false);
        }

        static void AddToonSpawnerLWRP()
        {
            if (_path == null) _path = EditorUtils.getMainRelativepath();

            Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterAlphaCutOut_URP.mat", typeof(Material)) as Material;
            Object dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDrop_URP.prefab", typeof(Object)) as Object;
            Object meshMask = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/MeshMask_URP.prefab", typeof(Object)) as Object;
            Texture effectTex2d = AssetDatabase.LoadAssetAtPath(_path + "Misc/Textures/CameraTexture - Effect.asset", typeof(Texture)) as Texture;
            // Texture backTex2d = AssetDatabase.LoadAssetAtPath(_path + "Misc/Textures/CameraTexture - Background.asset", typeof(Texture)) as Texture;


            Object pipeObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Pipe.prefab", typeof(Object)) as Object;
            GameObject waterGO = new GameObject("Water2D");
            waterGO.transform.position = getCameraPos();
            waterGO.transform.localEulerAngles = new Vector3(0, 0, 170f);

            //Pipe
            GameObject pipe = (GameObject)Instantiate(pipeObj);
            pipe.transform.SetParent(waterGO.transform);
            pipe.transform.localPosition = new Vector3(0, 1, 0);
            pipe.transform.localEulerAngles = new Vector3(0, 0, 0);
            pipe.transform.Find("ToonBadge").gameObject.SetActive(true);





            Water2D_Spawner _instanceW2D = waterGO.AddComponent<Water2D_Spawner>();
            _instanceW2D.Water2DVersion = SettingsManager.GetVersionString();
            _instanceW2D.Water2DType = "Toon";
            _instanceW2D.Water2DRenderType = "none";
            _instanceW2D.DropObject = (GameObject)dropObj;
            _instanceW2D.WaterMaterial = m;
            _instanceW2D.AlphaCutOff = 0.02f;
            _instanceW2D.AlphaStroke = 0.18f;
            _instanceW2D.Speed = 9f;
            _instanceW2D.size = .08f;
            _instanceW2D.DelayBetweenParticles = .016f;
            _instanceW2D.TrailStartSize = .8f;
            _instanceW2D.TrailEndSize = .3f;
            _instanceW2D.TrailDelay = .125f;


            SerializedObject m_CustomSettings = SettingsManager.GetSerializedSettings();
            int metaballLayer = AssetUtility.LoadPropertyAsInt("w2d_Metaball_layer", m_CustomSettings);
            int backgroundLayer = AssetUtility.LoadPropertyAsInt("w2d_Background_layer", m_CustomSettings);
            _instanceW2D.DropObject.layer = metaballLayer;

            // looking for Camera setup already in the scene
            ResizeQuadEffectController _camera = GameObject.FindObjectOfType<ResizeQuadEffectController>();

            GameObject cams = null;
            if (!_camera)
            {

                Object cameraSetup = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/CamerasForRegular.prefab", typeof(Object)) as Object;
                cams = (GameObject)Instantiate(cameraSetup);
                cams.name = "Cameras";

                GameObject effectCamera = cams.transform.Find("1-EffectCamera").gameObject;
                effectCamera.GetComponent<Camera>().cullingMask = 1 << metaballLayer;
                effectCamera.GetComponent<Camera>().backgroundColor = Color.black;


                GameObject DefaultCamera = cams.transform.Find("2-DefaultCamera").gameObject;
                //currentMask | (1 << newLayer); //'removes' newLayer layer.
                //currentMask & ~(1 << newLayer); //'removes' newLayer layer.
                int _mask = DefaultCamera.GetComponent<Camera>().cullingMask;
                DefaultCamera.GetComponent<Camera>().cullingMask = _mask & ~(1 << metaballLayer);


                _camera = effectCamera.transform.Find("EffectQuad").GetComponent<ResizeQuadEffectController>();

            }
            _camera.gameObject.GetComponent<MeshRenderer>().sharedMaterial = m;


        }





        static void AddRefractingLegacy()
        {
            if (_path == null) _path = EditorUtils.getMainRelativepath();

            //Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterGrabPass.mat", typeof(Material)) as Material;
            //Object dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDropRefractive.prefab", typeof(Object)) as Object;
            Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterGrabPass.mat", typeof(Material)) as Material;
            Object dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDrop.prefab", typeof(Object)) as Object;

            Object pipeObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Pipe.prefab", typeof(Object)) as Object;
            GameObject waterGO = new GameObject("Water2D");
            waterGO.transform.position = getCameraPos();
            waterGO.transform.localEulerAngles = new Vector3(0, 0, 170f);

            //Pipe
            GameObject pipe = (GameObject)Instantiate(pipeObj);
            pipe.transform.SetParent(waterGO.transform);
            pipe.transform.localPosition = new Vector3(0, 1, 0);
            pipe.transform.localEulerAngles = new Vector3(0, 0, 0);
            pipe.transform.Find("body").GetComponent<SpriteRenderer>().color = new Color(0f, 105 / 255f, 221 / 255f, 1f);
            pipe.transform.Find("body1").GetComponent<SpriteRenderer>().color = new Color(0f, 105 / 255f, 221 / 255f, 1f);




            Water2D_Spawner _instanceW2D = waterGO.AddComponent<Water2D_Spawner>();
            _instanceW2D.Water2DVersion = SettingsManager.GetVersionString();
            _instanceW2D.Water2DType = "Refracting";
            _instanceW2D.Water2DRenderType = "Legacy";
            _instanceW2D.DropObject = (GameObject)dropObj;
            _instanceW2D.WaterMaterial = m;
            //_instanceW2D.AlphaStroke = .4f;
            _instanceW2D.TintColor = new Color(0f, 112 / 255f, 1f);
            // _instanceW2D.StrokeColor = new Color(4 / 255f, 156 / 255f, 1f);
            _instanceW2D.Intensity = .6f;
            _instanceW2D.Distortion = .25f;
            _instanceW2D.Speed = 9f;
            _instanceW2D.size = .1f;
            _instanceW2D.TrailStartSize = 0f;
            _instanceW2D.TrailEndSize = .3f;
            _instanceW2D.TrailDelay = .125f;

            SerializedObject m_CustomSettings = SettingsManager.GetSerializedSettings();
            int metaballLayer = AssetUtility.LoadPropertyAsInt("w2d_Metaball_layer", m_CustomSettings);
            _instanceW2D.DropObject.layer = metaballLayer;



            // looking for Camera setup already in the scene
            ResizeQuadEffectController _camera = GameObject.FindObjectOfType<ResizeQuadEffectController>();

            GameObject cams = null;
            if (!_camera)
            {

                Object cameraSetup = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/CamerasForRefracting.prefab", typeof(Object)) as Object;
                cams = (GameObject)Instantiate(cameraSetup);
                cams.name = "Cameras";

                GameObject effectCamera = cams.transform.Find("1-EffectCamera").gameObject;
                effectCamera.GetComponent<Camera>().cullingMask = 1 << metaballLayer;
                //effectCamera.GetComponent<Camera>().backgroundColor = Color.black;


                GameObject DefaultCamera = cams.transform.Find("2-DefaultCamera").gameObject;
                //currentMask | (1 << newLayer); //'removes' newLayer layer.
                //currentMask & ~(1 << newLayer); //'removes' newLayer layer.
                int _mask = DefaultCamera.GetComponent<Camera>().cullingMask;
                DefaultCamera.GetComponent<Camera>().cullingMask = _mask & ~(1 << metaballLayer);


                _camera = effectCamera.transform.Find("EffectQuad").GetComponent<ResizeQuadEffectController>();

            }
            _camera.gameObject.GetComponent<MeshRenderer>().sharedMaterial = m;

        }

        static void AddSlimeStyle()
        {
            if (_path == null) _path = EditorUtils.getMainRelativepath();

            //Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterGrabPass.mat", typeof(Material)) as Material;
            //Object dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDropRefractive.prefab", typeof(Object)) as Object;
            Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterGrabPass.mat", typeof(Material)) as Material;
            Object dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDrop.prefab", typeof(Object)) as Object;

            Object pipeObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Pipe.prefab", typeof(Object)) as Object;
            GameObject waterGO = new GameObject("Water2D");
            waterGO.transform.position = getCameraPos();
            waterGO.transform.localEulerAngles = new Vector3(0, 0, 170f);

            //Pipe
            GameObject pipe = (GameObject)Instantiate(pipeObj);
            pipe.transform.SetParent(waterGO.transform);
            pipe.transform.localPosition = new Vector3(0, 1, 0);
            pipe.transform.localEulerAngles = new Vector3(0, 0, 0);
            pipe.transform.Find("body").GetComponent<SpriteRenderer>().color = new Color(0f, 105 / 255f, 221 / 255f, 1f);
            pipe.transform.Find("body1").GetComponent<SpriteRenderer>().color = new Color(0f, 105 / 255f, 221 / 255f, 1f);




            Water2D_Spawner _instanceW2D = waterGO.AddComponent<Water2D_Spawner>();
            _instanceW2D.Water2DVersion = SettingsManager.GetVersionString();
            _instanceW2D.Water2DType = "Refracting";
            _instanceW2D.Water2DRenderType = "Legacy";
            _instanceW2D.DropObject = (GameObject)dropObj;
            _instanceW2D.WaterMaterial = m;
            //_instanceW2D.AlphaStroke = .4f;
            _instanceW2D.TintColor = new Color(0f, 112 / 255f, 1f);
            // _instanceW2D.StrokeColor = new Color(4 / 255f, 156 / 255f, 1f);
            _instanceW2D.Intensity = .6f;
            _instanceW2D.Distortion = .25f;
            _instanceW2D.Speed = 9f;
            _instanceW2D.size = .1f;
            _instanceW2D.TrailStartSize = 0f;
            _instanceW2D.LinearDrag = 1.7f;
            _instanceW2D.AngularDrag = 3.6f;
            _instanceW2D.GravityScale = .7f;
            _instanceW2D.ColliderSize = 1.2f;

            _instanceW2D.ScaleDown = true;

            _instanceW2D.LifeTime = 20f;

            SerializedObject m_CustomSettings = SettingsManager.GetSerializedSettings();
            int metaballLayer = AssetUtility.LoadPropertyAsInt("w2d_Metaball_layer", m_CustomSettings);
            _instanceW2D.DropObject.layer = metaballLayer;



            // looking for Camera setup already in the scene
            ResizeQuadEffectController _camera = GameObject.FindObjectOfType<ResizeQuadEffectController>();

            GameObject cams = null;
            if (!_camera)
            {

                Object cameraSetup = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/CamerasForRefracting.prefab", typeof(Object)) as Object;
                cams = (GameObject)Instantiate(cameraSetup);
                cams.name = "Cameras";

                GameObject effectCamera = cams.transform.Find("1-EffectCamera").gameObject;
                effectCamera.GetComponent<Camera>().cullingMask = 1 << metaballLayer;
                //effectCamera.GetComponent<Camera>().backgroundColor = Color.black;


                GameObject DefaultCamera = cams.transform.Find("2-DefaultCamera").gameObject;
                //currentMask | (1 << newLayer); //'removes' newLayer layer.
                //currentMask & ~(1 << newLayer); //'removes' newLayer layer.
                int _mask = DefaultCamera.GetComponent<Camera>().cullingMask;
                DefaultCamera.GetComponent<Camera>().cullingMask = _mask & ~(1 << metaballLayer);


                _camera = effectCamera.transform.Find("EffectQuad").GetComponent<ResizeQuadEffectController>();

            }
            _camera.gameObject.GetComponent<MeshRenderer>().sharedMaterial = m;

        }

        static void AddRefractingSpawnerURP()
        {
            if (_path == null) _path = EditorUtils.getMainRelativepath();

            Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterGrabPass_URP.mat", typeof(Material)) as Material;
            Object dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDropRefractive_URP.prefab", typeof(Object)) as Object;


            Object pipeObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Pipe.prefab", typeof(Object)) as Object;
            GameObject waterGO = new GameObject("Water2D");
            waterGO.transform.position = getCameraPos();
            waterGO.transform.localEulerAngles = new Vector3(0, 0, 170f);

            //Pipe
            GameObject pipe = (GameObject)Instantiate(pipeObj);
            pipe.transform.SetParent(waterGO.transform);
            pipe.transform.localPosition = new Vector3(0, 1, 0);
            pipe.transform.localEulerAngles = new Vector3(0, 0, 0);
            pipe.transform.Find("body").GetComponent<SpriteRenderer>().color = new Color(0f, 105 / 255f, 221 / 255f, 1f);
            pipe.transform.Find("body1").GetComponent<SpriteRenderer>().color = new Color(0f, 105 / 255f, 221 / 255f, 1f);


            Water2D_Spawner _instanceW2D = waterGO.AddComponent<Water2D_Spawner>();
            _instanceW2D.Water2DVersion = SettingsManager.GetVersionString();
            _instanceW2D.Water2DType = "Refracting";
            _instanceW2D.Water2DRenderType = "URP(LWRP)";
            _instanceW2D.DropObject = (GameObject)dropObj;
            _instanceW2D.WaterMaterial = m;
            _instanceW2D.AlphaCutOff = 0.034f;
            _instanceW2D.AlphaStroke = 0.062f;
            _instanceW2D.Speed = 9f;
            _instanceW2D.size = .08f;
            _instanceW2D.DelayBetweenParticles = .016f;
            _instanceW2D.TrailStartSize = .8f;
            _instanceW2D.TrailEndSize = .3f;
            _instanceW2D.TrailDelay = .125f;

            SerializedObject m_CustomSettings = SettingsManager.GetSerializedSettings();
            int metaballLayer = AssetUtility.LoadPropertyAsInt("w2d_Metaball_layer", m_CustomSettings);
            _instanceW2D.DropObject.layer = metaballLayer;


            // NO SPECIALS CAMERA SETUP NEEDED  
            // JUST ORTHO type
            Camera.main.orthographic = true;

        }



        static void AddRegularSpawnerLegacy()
        {
            if (_path == null) _path = EditorUtils.getMainRelativepath();

            Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterRegularCutOut.mat", typeof(Material)) as Material;
            Object dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDrop.prefab", typeof(Object)) as Object;
            Object pipeObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Pipe.prefab", typeof(Object)) as Object;
            GameObject waterGO = new GameObject("Water2D");
            waterGO.transform.position = getCameraPos();
            waterGO.transform.localEulerAngles = new Vector3(0, 0, 170f);

            //Pipe
            GameObject pipe = (GameObject)Instantiate(pipeObj);
            pipe.transform.SetParent(waterGO.transform);
            pipe.transform.localPosition = new Vector3(0, 1, 0);
            pipe.transform.localEulerAngles = new Vector3(0, 0, 0);
            pipe.transform.Find("ToonBadge").gameObject.SetActive(false);


            Water2D_Spawner _instanceW2D = waterGO.AddComponent<Water2D_Spawner>();
            _instanceW2D.Water2DVersion = SettingsManager.GetVersionString();
            _instanceW2D.Water2DType = "Regular";
            _instanceW2D.Water2DRenderType = "Legacy";
            _instanceW2D.DropObject = (GameObject)dropObj;


            SerializedObject m_CustomSettings = SettingsManager.GetSerializedSettings();
            int metaballLayer = AssetUtility.LoadPropertyAsInt("w2d_Metaball_layer", m_CustomSettings);
            int backgroundLayer = AssetUtility.LoadPropertyAsInt("w2d_Background_layer", m_CustomSettings);
            bool flipTex = AssetUtility.LoadPropertyAsBool("w2d_FlipCameraTexture", m_CustomSettings);
            _instanceW2D.DropObject.layer = metaballLayer;

            _instanceW2D.DropCount = 180;
            _instanceW2D._lastDropCount = 180;
            _instanceW2D.WaterMaterial = m;
            _instanceW2D.AlphaCutOff = .4f;
            _instanceW2D.AlphaStroke = 4f;
            _instanceW2D.FillColor = new Color(0f, 112 / 255f, 1f, .5f);
            _instanceW2D.StrokeColor = new Color(175 / 255f, 224 / 255f, 1f);
            _instanceW2D.Speed = 9f;
            _instanceW2D.size = .095f;
            _instanceW2D.DelayBetweenParticles = 0.012f;
            _instanceW2D.TrailStartSize = 0f;



            // looking for Camera setup already in the scene
            ResizeQuadEffectController _quadResizer = GameObject.FindObjectOfType<ResizeQuadEffectController>();
            if (!_quadResizer)
            {
                Object cameraSetup = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/CamerasForRegular.prefab", typeof(Object)) as Object;
                GameObject cams = (GameObject)Instantiate(cameraSetup);
                cams.name = "Cameras";

                GameObject effectCamera = cams.transform.Find("1-EffectCamera").gameObject;
                effectCamera.GetComponent<Camera>().cullingMask = 1 << metaballLayer;
                effectCamera.GetComponent<Camera>().backgroundColor = new Color(1f, 1f, 1f, 0f);

                _quadResizer = effectCamera.transform.Find("EffectQuad").GetComponent<ResizeQuadEffectController>();

                GameObject DefaultCamera = cams.transform.Find("2-DefaultCamera").gameObject;
                //currentMask | (1 << newLayer); //'removes' newLayer layer.
                //currentMask & ~(1 << newLayer); //'removes' newLayer layer.
                int _mask = DefaultCamera.GetComponent<Camera>().cullingMask;
                DefaultCamera.GetComponent<Camera>().cullingMask = _mask & ~(1 << metaballLayer);

            }
            _quadResizer.FlipTexture = flipTex;
             

        }

        static void AddOilStyle()
        {
            if (_path == null) _path = EditorUtils.getMainRelativepath();

            Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterRegularCutOut.mat", typeof(Material)) as Material;
            Object dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDrop.prefab", typeof(Object)) as Object;
            Object pipeObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Pipe.prefab", typeof(Object)) as Object;
            GameObject waterGO = new GameObject("Oil2D");
            waterGO.transform.position = getCameraPos();
            waterGO.transform.localEulerAngles = new Vector3(0, 0, -128f);

            //Pipe
            GameObject pipe = (GameObject)Instantiate(pipeObj);
            pipe.transform.SetParent(waterGO.transform);
            pipe.transform.localPosition = new Vector3(0, 1, 0);
            pipe.transform.localEulerAngles = new Vector3(0, 0, 0);
            pipe.transform.Find("ToonBadge").gameObject.SetActive(false);


            Water2D_Spawner _instanceW2D = waterGO.AddComponent<Water2D_Spawner>();
            _instanceW2D.Water2DVersion = SettingsManager.GetVersionString();
            _instanceW2D.Water2DType = "Regular";
            _instanceW2D.Water2DRenderType = "Legacy";
            _instanceW2D.DropObject = (GameObject)dropObj;


            SerializedObject m_CustomSettings = SettingsManager.GetSerializedSettings();
            int metaballLayer = AssetUtility.LoadPropertyAsInt("w2d_Metaball_layer", m_CustomSettings);
            int backgroundLayer = AssetUtility.LoadPropertyAsInt("w2d_Background_layer", m_CustomSettings);
            bool flipTex = AssetUtility.LoadPropertyAsBool("w2d_FlipCameraTexture", m_CustomSettings);
            _instanceW2D.DropObject.layer = metaballLayer;

            _instanceW2D.DropCount = 180;
            _instanceW2D._lastDropCount = 180;
            _instanceW2D.WaterMaterial = m;
            _instanceW2D.AlphaCutOff = .4f;
            _instanceW2D.AlphaStroke = 4f;
            _instanceW2D.FillColor = new Color(41 / 255f, 41 / 255f, 41 / 255f, 128 / 255f);
            _instanceW2D.StrokeColor = new Color(0f, 0f, 0f, 1f);
            _instanceW2D.Speed = 15f;
            _instanceW2D.size = .095f;
            _instanceW2D.DelayBetweenParticles = 0.012f;
            _instanceW2D.TrailStartSize = 0f;
            _instanceW2D.LinearDrag = 2f;
            _instanceW2D.AngularDrag = 5f;
            _instanceW2D.GravityScale = .7f;


            // looking for Camera setup already in the scene
            ResizeQuadEffectController _quadResizer = GameObject.FindObjectOfType<ResizeQuadEffectController>();
            if (!_quadResizer)
            {
                Object cameraSetup = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/CamerasForRegular.prefab", typeof(Object)) as Object;
                GameObject cams = (GameObject)Instantiate(cameraSetup);
                cams.name = "Cameras";

                GameObject effectCamera = cams.transform.Find("1-EffectCamera").gameObject;
                effectCamera.GetComponent<Camera>().cullingMask = 1 << metaballLayer;
                effectCamera.GetComponent<Camera>().backgroundColor = new Color(1f, 1f, 1f, 0f);

                _quadResizer = effectCamera.transform.Find("EffectQuad").GetComponent<ResizeQuadEffectController>();

                GameObject DefaultCamera = cams.transform.Find("2-DefaultCamera").gameObject;
                //currentMask | (1 << newLayer); //'removes' newLayer layer.
                //currentMask & ~(1 << newLayer); //'removes' newLayer layer.
                int _mask = DefaultCamera.GetComponent<Camera>().cullingMask;
                DefaultCamera.GetComponent<Camera>().cullingMask = _mask & ~(1 << metaballLayer);

            }
            _quadResizer.FlipTexture = flipTex;


        }


        static void AddBouncyWaterStyle()
        {
            if (_path == null) _path = EditorUtils.getMainRelativepath();

            Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterRegularCutOut.mat", typeof(Material)) as Material;
            PhysicsMaterial2D pm = AssetDatabase.LoadAssetAtPath(_path + "Physics Materials/Bouncy Water.PhysicsMaterial2D", typeof(PhysicsMaterial2D)) as PhysicsMaterial2D;
            Object dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDrop.prefab", typeof(Object)) as Object;
            Object pipeObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Pipe.prefab", typeof(Object)) as Object;
            GameObject waterGO = new GameObject("Water2D");
            waterGO.transform.position = getCameraPos();
            waterGO.transform.localEulerAngles = new Vector3(0, 0, 170f);

            //Pipe
            GameObject pipe = (GameObject)Instantiate(pipeObj);
            pipe.transform.SetParent(waterGO.transform);
            pipe.transform.localPosition = new Vector3(0, 1, 0);
            pipe.transform.localEulerAngles = new Vector3(0, 0, 0);
            pipe.transform.Find("ToonBadge").gameObject.SetActive(false);


            Water2D_Spawner _instanceW2D = waterGO.AddComponent<Water2D_Spawner>();
            _instanceW2D.Water2DVersion = SettingsManager.GetVersionString();
            _instanceW2D.Water2DType = "Regular";
            _instanceW2D.Water2DRenderType = "Legacy";
            _instanceW2D.DropObject = (GameObject)dropObj;


            SerializedObject m_CustomSettings = SettingsManager.GetSerializedSettings();
            int metaballLayer = AssetUtility.LoadPropertyAsInt("w2d_Metaball_layer", m_CustomSettings);
            int backgroundLayer = AssetUtility.LoadPropertyAsInt("w2d_Background_layer", m_CustomSettings);
            bool flipTex = AssetUtility.LoadPropertyAsBool("w2d_FlipCameraTexture", m_CustomSettings);
            _instanceW2D.DropObject.layer = metaballLayer;


            _instanceW2D.DropCount = 180;
            _instanceW2D._lastDropCount = 180;
            _instanceW2D.WaterMaterial = m;
            _instanceW2D.AlphaCutOff = .4f;
            _instanceW2D.AlphaStroke = 4f;
            _instanceW2D.FillColor = new Color(0f, 112 / 255f, 1f, .5f);
            _instanceW2D.StrokeColor = new Color(175 / 255f, 224 / 255f, 1f);
            _instanceW2D.Speed = 9f;
            _instanceW2D.size = .095f;
            _instanceW2D.DelayBetweenParticles = 0.012f;
            _instanceW2D.TrailStartSize = 0f;
            _instanceW2D.PhysicMat = pm;



            // looking for Camera setup already in the scene
            ResizeQuadEffectController _quadResizer = GameObject.FindObjectOfType<ResizeQuadEffectController>();
            if (!_quadResizer)
            {
                Object cameraSetup = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/CamerasForRegular.prefab", typeof(Object)) as Object;
                GameObject cams = (GameObject)Instantiate(cameraSetup);
                cams.name = "Cameras";

                GameObject effectCamera = cams.transform.Find("1-EffectCamera").gameObject;
                effectCamera.GetComponent<Camera>().cullingMask = 1 << metaballLayer;
                effectCamera.GetComponent<Camera>().backgroundColor = new Color(1f, 1f, 1f, 0f);

                _quadResizer = effectCamera.transform.Find("EffectQuad").GetComponent<ResizeQuadEffectController>();

                GameObject DefaultCamera = cams.transform.Find("2-DefaultCamera").gameObject;
                //currentMask | (1 << newLayer); //'removes' newLayer layer.
                //currentMask & ~(1 << newLayer); //'removes' newLayer layer.
                int _mask = DefaultCamera.GetComponent<Camera>().cullingMask;
                DefaultCamera.GetComponent<Camera>().cullingMask = _mask & ~(1 << metaballLayer);

            }
            _quadResizer.FlipTexture = flipTex;


        }

        static void addCameras()
        {

            if (_path == null) _path = EditorUtils.getMainRelativepath();

            Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterRegularCutOut.mat", typeof(Material)) as Material;

            SerializedObject m_CustomSettings = SettingsManager.GetSerializedSettings();
            int metaballLayer = AssetUtility.LoadPropertyAsInt("w2d_Metaball_layer", m_CustomSettings);
            int backgroundLayer = AssetUtility.LoadPropertyAsInt("w2d_Background_layer", m_CustomSettings);
            bool flipTex = AssetUtility.LoadPropertyAsBool("w2d_FlipCameraTexture", m_CustomSettings);

            // looking for Camera setup already in the scene
            ResizeQuadEffectController _camera = GameObject.FindObjectOfType<ResizeQuadEffectController>();

            Water2D_Spawner _w2dPresentInScene = GameObject.FindObjectOfType<Water2D_Spawner>();

            GameObject cams = null;
            if (!_camera)
            {

                string CameraTypeName = "Prefabs/CamerasForRegular.prefab";

                if (_w2dPresentInScene)
                {
                    if (_w2dPresentInScene.Water2DType == "Refracting")
                    {
                        CameraTypeName = "Prefabs/CamerasForRefracting.prefab";
                        m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterGrabPass.mat", typeof(Material)) as Material;

                    }

                    if (_w2dPresentInScene.Water2DType == "Toon")
                    {
                        CameraTypeName = "Prefabs/CamerasForRegular.prefab";
                        m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterAlphaCutOut_URP.mat", typeof(Material)) as Material;
                    }
                }

                Object cameraSetup = AssetDatabase.LoadAssetAtPath(_path + CameraTypeName, typeof(Object)) as Object;
                cams = (GameObject)Instantiate(cameraSetup);
                cams.name = "Cameras";

                GameObject effectCamera = cams.transform.Find("1-EffectCamera").gameObject;
                effectCamera.GetComponent<Camera>().cullingMask = 1 << metaballLayer;
                if (_w2dPresentInScene.Water2DType == "Toon")
                {
                    effectCamera.GetComponent<Camera>().backgroundColor = Color.black;
                }

                GameObject DefaultCamera = cams.transform.Find("2-DefaultCamera").gameObject;
                //currentMask | (1 << newLayer); //'removes' newLayer layer.
                //currentMask & ~(1 << newLayer); //'removes' newLayer layer.
                int _mask = DefaultCamera.GetComponent<Camera>().cullingMask;
                DefaultCamera.GetComponent<Camera>().cullingMask = _mask & ~(1 << metaballLayer);


                _camera = effectCamera.transform.Find("EffectQuad").GetComponent<ResizeQuadEffectController>();

            }
            _camera.gameObject.GetComponent<MeshRenderer>().sharedMaterial = m;

        }


        static void AddRegularFiller(string colliderType = "Box")
        {
            if (_path == null) _path = EditorUtils.getMainRelativepath();

            Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterRegularCutOut.mat", typeof(Material)) as Material;
            Object dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDrop.prefab", typeof(Object)) as Object;
            //Object pipeObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Pipe.prefab", typeof(Object)) as Object;
            GameObject waterGO = new GameObject("Water2D_Filler");
            waterGO.transform.position = getCameraPos();
           // waterGO.transform.localEulerAngles = new Vector3(0, 0, 170f);


            Water2D_Spawner _instanceW2D = waterGO.AddComponent<Water2D_Spawner>();
            _instanceW2D.Water2DVersion = SettingsManager.GetVersionString();
            _instanceW2D.Water2DType = "Regular";
            _instanceW2D.Water2DRenderType = "Legacy";
            _instanceW2D.Water2DEmissionType = Water2D_Spawner.EmissionType.FillerCollider;
            _instanceW2D.DropObject = (GameObject)dropObj;


            SerializedObject m_CustomSettings = SettingsManager.GetSerializedSettings();
            int metaballLayer = AssetUtility.LoadPropertyAsInt("w2d_Metaball_layer", m_CustomSettings);
            int backgroundLayer = AssetUtility.LoadPropertyAsInt("w2d_Background_layer", m_CustomSettings);
            bool flipTex = AssetUtility.LoadPropertyAsBool("w2d_FlipCameraTexture", m_CustomSettings);
            _instanceW2D.DropObject.layer = metaballLayer;

            _instanceW2D.DropCount = 180;
            _instanceW2D._lastDropCount = 180;
            _instanceW2D.WaterMaterial = m;
            _instanceW2D.AlphaCutOff = .4f;
            _instanceW2D.AlphaStroke = 4f;
            _instanceW2D.FillColor = new Color(0f, 112 / 255f, 1f, .5f);
            _instanceW2D.StrokeColor = new Color(175 / 255f, 224 / 255f, 1f);
            _instanceW2D.Speed = 9f;
            _instanceW2D.size = .095f;
            _instanceW2D.DelayBetweenParticles = 0.012f;
            _instanceW2D.TrailStartSize = 0f;
            _instanceW2D.LifeTime = 0;
            _instanceW2D.PersistentFluid = true;

           

            // looking for Camera setup already in the scene
            ResizeQuadEffectController _quadResizer = GameObject.FindObjectOfType<ResizeQuadEffectController>();
            if (!_quadResizer)
            {
                Object cameraSetup = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/CamerasForRegular.prefab", typeof(Object)) as Object;
                GameObject cams = (GameObject)Instantiate(cameraSetup);
                cams.name = "Cameras";

                GameObject effectCamera = cams.transform.Find("1-EffectCamera").gameObject;
                effectCamera.GetComponent<Camera>().cullingMask = 1 << metaballLayer;
                effectCamera.GetComponent<Camera>().backgroundColor = new Color(1f, 1f, 1f, 0f);

                _quadResizer = effectCamera.transform.Find("EffectQuad").GetComponent<ResizeQuadEffectController>();

                GameObject DefaultCamera = cams.transform.Find("2-DefaultCamera").gameObject;
                //currentMask | (1 << newLayer); //'removes' newLayer layer.
                //currentMask & ~(1 << newLayer); //'removes' newLayer layer.
                int _mask = DefaultCamera.GetComponent<Camera>().cullingMask;
                DefaultCamera.GetComponent<Camera>().cullingMask = _mask & ~(1 << metaballLayer);

            }
            _quadResizer.FlipTexture = flipTex;



            ColliderFiller cf = _instanceW2D.gameObject.AddComponent<ColliderFiller>();
            cf.water2D_Spawner = _instanceW2D;
            cf.collider = _instanceW2D.gameObject.AddComponent<BoxCollider2D>();
            cf.collider.isTrigger = true;
            cf.Refresh();
            cf.Fill();


        }

        static void AddRefractingFiller() {
            if (_path == null) _path = EditorUtils.getMainRelativepath();

            //Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterGrabPass.mat", typeof(Material)) as Material;
            //Object dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDropRefractive.prefab", typeof(Object)) as Object;
            Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterGrabPass.mat", typeof(Material)) as Material;
            Object dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDrop.prefab", typeof(Object)) as Object;

           
            GameObject waterGO = new GameObject("Water2D_Filler");
            waterGO.transform.position = getCameraPos();
            waterGO.transform.localEulerAngles = new Vector3(0, 0, 0);

           
            Water2D_Spawner _instanceW2D = waterGO.AddComponent<Water2D_Spawner>();
            _instanceW2D.Water2DVersion = SettingsManager.GetVersionString();
            _instanceW2D.Water2DType = "Refracting";
            _instanceW2D.Water2DRenderType = "Legacy";
            _instanceW2D.Water2DEmissionType = Water2D_Spawner.EmissionType.FillerCollider;
            _instanceW2D.DropObject = (GameObject)dropObj;
            _instanceW2D.WaterMaterial = m;
            //_instanceW2D.AlphaStroke = .4f;
            _instanceW2D.TintColor = new Color(0f, 112 / 255f, 1f);
            // _instanceW2D.StrokeColor = new Color(4 / 255f, 156 / 255f, 1f);
            _instanceW2D.Intensity = .6f;
            _instanceW2D.Distortion = .25f;
            _instanceW2D.Speed = 9f;
            _instanceW2D.size = .1f;
            _instanceW2D.TrailStartSize = 0f;
            _instanceW2D.TrailEndSize = .3f;
            _instanceW2D.TrailDelay = .125f;

            SerializedObject m_CustomSettings = SettingsManager.GetSerializedSettings();
            int metaballLayer = AssetUtility.LoadPropertyAsInt("w2d_Metaball_layer", m_CustomSettings);
            _instanceW2D.DropObject.layer = metaballLayer;
            bool flipTex = AssetUtility.LoadPropertyAsBool("w2d_FlipCameraTexture", m_CustomSettings);


            // looking for Camera setup already in the scene
            ResizeQuadEffectController _camera = GameObject.FindObjectOfType<ResizeQuadEffectController>();

            GameObject cams = null;
            if (!_camera)
            {

                Object cameraSetup = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/CamerasForRefracting.prefab", typeof(Object)) as Object;
                cams = (GameObject)Instantiate(cameraSetup);
                cams.name = "Cameras";

                GameObject effectCamera = cams.transform.Find("1-EffectCamera").gameObject;
                effectCamera.GetComponent<Camera>().cullingMask = 1 << metaballLayer;
                //effectCamera.GetComponent<Camera>().backgroundColor = Color.black;


                GameObject DefaultCamera = cams.transform.Find("2-DefaultCamera").gameObject;
                //currentMask | (1 << newLayer); //'removes' newLayer layer.
                //currentMask & ~(1 << newLayer); //'removes' newLayer layer.
                int _mask = DefaultCamera.GetComponent<Camera>().cullingMask;
                DefaultCamera.GetComponent<Camera>().cullingMask = _mask & ~(1 << metaballLayer);


                _camera = effectCamera.transform.Find("EffectQuad").GetComponent<ResizeQuadEffectController>();

            }
            _camera.gameObject.GetComponent<MeshRenderer>().sharedMaterial = m;
            _camera.FlipTexture = flipTex;



            ColliderFiller cf = _instanceW2D.gameObject.AddComponent<ColliderFiller>();
            cf.water2D_Spawner = _instanceW2D;
            cf.collider = _instanceW2D.gameObject.AddComponent<BoxCollider2D>();
            cf.collider.isTrigger = true;
            cf.Refresh();
            cf.Fill();

        }


        static Vector3 getCameraPos()
        {
            float offset = 0f;


            if (Camera.current)
                return new Vector3(Camera.current.transform.position.x, Camera.current.transform.position.y + offset, 0);
            else
                return new Vector3(0, offset, 0);
        }


        [MenuItem(menuPathMain + "/Settings", false, 60)]
        static void createsettings()
        {
            SettingsService.OpenProjectSettings("Project/Water2D");
            //SettingsManager.GetOrCreateSettings();
        }

        [MenuItem(menuPathMain + "/Documentation", false, 80)]
        static void OpenDoc()
        {
            Application.OpenURL("https://docs.google.com/document/d/1fcFz19jsL9tIdfFB1U72S9r4D2c7PSd4s0E_vnySa88");
        }
        [MenuItem(menuPathMain + "/Support", false, 81)]
        static void OpenMail()
        {
            Application.OpenURL("mailto:info@2ddlpro.com?subject=Water2D PRO&body=");
        }

        
    }
}