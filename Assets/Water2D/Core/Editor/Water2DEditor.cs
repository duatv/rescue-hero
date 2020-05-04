namespace Water2D
{
	using UnityEngine;
	using UnityEditor;
    using UnityEngine.Rendering;
    using System.Reflection;
	using System.Collections;
	using System.IO;
	using System;
	using UnityEngine.UI;
    using Apptouch;
		
	
	public class CustomDragData{
		public int originalIndex;
		public IList originalList;
	}
	
	
	
	[CustomEditor (typeof (Water2D.Water2D_Spawner))] 
	[CanEditMultipleObjects]
	
	
	public class Water2DEditor : Editor {
		
		static internal Water2D.Water2D_Spawner WSpawner;

        SerializedProperty version;
        SerializedProperty PersistentFluid;
        SerializedProperty DropObject;
        SerializedProperty SizeDropObject;
        SerializedProperty CanScaleDown;
        SerializedProperty LifetimeDropObject;
        SerializedProperty TrailSizeStart;
        SerializedProperty TrailSizeEnd;
        SerializedProperty TrailDuration;
        SerializedProperty DelayBetweenParticles;
        SerializedProperty Material;
        SerializedProperty ColorScheme;
        SerializedProperty FillColor;
        SerializedProperty StrokeColor;
        SerializedProperty Blending;
        SerializedProperty _lastBlending;
        SerializedProperty Sorting;


        SerializedProperty FillerColliderType;
        SerializedProperty FillerColliderMasked;

        SerializedProperty AlphaCutOff;
        SerializedProperty AlphaStroke;
        
        SerializedProperty Intensity;
        SerializedProperty Distortion;
        SerializedProperty DistortionSpeed;



        SerializedProperty DropCount;
        SerializedProperty _lastDropCount;
        SerializedProperty CurrentlyDropsUsed;
        SerializedProperty Speed;
        SerializedProperty InitSpeed;
        SerializedProperty Loop;
        SerializedProperty SpeedLimiterX;
        SerializedProperty SpeedLimiterY;

        SerializedProperty SimulateInEditor;
        SerializedProperty SimulateOnAwake;


        SerializedProperty ColliderSize;
        SerializedProperty PhysicsMaterial;
        SerializedProperty LinearDrag;
        SerializedProperty AngularDrag;
        SerializedProperty GravityScale;
        SerializedProperty FreezeRotation;

        SerializedProperty OnValidateShapeFill;
        SerializedProperty ShapeFillCollider2D;
        SerializedProperty ShapeFillAccuracy;

        SerializedProperty OnCollisionEnterList;
        SerializedProperty OnSpawnerAboutToStart;
        SerializedProperty OnSpawnerAboutToEnd;
        SerializedProperty OnSpawnerEmittingParticle;




        private GUIStyle titleStyle, subTitleStyle, bgStyle, btnStyle;
		
		private Vector2 WSpawnerRectOrigin, WSpawnerRectSize;
		
		private string _path;
	
		bool _timeToReview;
		
		
		bool foldoutEmission = true;
        bool foldoutPhysics = false;
        bool foldoutEvents = false;
        bool foldoutEmissionFiller = true;



        private Texture2D headerTexture;
		private Font headerFont;

		

		private Tool _cTool;

       

       

        SCHEME _lastScheme;

        // MinMaxSlider
        float min = 0f;
        float max = 0f;
        float minLimit = -300f;
        float maxLimit = 300f;

        private enum SCHEME {
           
            SCHEME1 = 1,
            SCHEME2 = 2,
            SCHEME3 = 3,
            SCHEME4 = 4,
            SCHEME5 = 5,
            SCHEME6 = 6
        }

        private SCHEME CurrentColorScheme;

        internal void OnEnable(){

			_cTool = Tools.current;
			Tools.current = Tool.None;
            //serialtarget = new SerializedObject(target);
            WSpawner = target as Water2D.Water2D_Spawner;

            //foldoutPhysics = WSpawner.Water2DEmissionType == Water2D_Spawner.EmissionType.FillerCollider ? true : false;

            // GET ALL PROPERTIES 
			version = serializedObject.FindProperty("Version");
            PersistentFluid = serializedObject.FindProperty("PersistentFluid");
            DropObject = serializedObject.FindProperty("DropObject");
            SizeDropObject = serializedObject.FindProperty("size");
            CanScaleDown = serializedObject.FindProperty("ScaleDown");
            LifetimeDropObject = serializedObject.FindProperty("LifeTime");

            FillerColliderType = serializedObject.FindProperty("Water2DFillerType");
            FillerColliderMasked = serializedObject.FindProperty("FillerColliderMasked");


            TrailSizeStart = serializedObject.FindProperty("TrailStartSize");
            TrailSizeEnd = serializedObject.FindProperty("TrailEndSize");
            TrailDuration = serializedObject.FindProperty("TrailDelay");

           

            DropCount = serializedObject.FindProperty("DropCount");
            _lastDropCount = serializedObject.FindProperty("_lastDropCount");
            CurrentlyDropsUsed = serializedObject.FindProperty("DropsUsed");
            InitSpeed = serializedObject.FindProperty("initSpeed");
            Loop = serializedObject.FindProperty("Loop");
            Speed = serializedObject.FindProperty("Speed");
            SpeedLimiterX = serializedObject.FindProperty("SpeedLimiterX");
            SpeedLimiterY = serializedObject.FindProperty("SpeedLimiterY");

            DelayBetweenParticles = serializedObject.FindProperty("DelayBetweenParticles");

            Material = serializedObject.FindProperty("WaterMaterial");
            ColorScheme = serializedObject.FindProperty("ColorScheme");
            FillColor = serializedObject.FindProperty("FillColor");
            StrokeColor = serializedObject.FindProperty("StrokeColor");
            Blending = serializedObject.FindProperty("Blending");
            _lastBlending = serializedObject.FindProperty("_lastBlending");
            Sorting = serializedObject.FindProperty("Sorting");

            AlphaCutOff = serializedObject.FindProperty("AlphaCutOff");
            AlphaStroke = serializedObject.FindProperty("AlphaStroke");

            
            Intensity = serializedObject.FindProperty("Intensity");
            Distortion = serializedObject.FindProperty("Distortion");
            DistortionSpeed = serializedObject.FindProperty("DistortionSpeed");


            SimulateInEditor = serializedObject.FindProperty("SimulateInEditor");
            SimulateOnAwake = serializedObject.FindProperty("SimulateOnAwake"); // Only PlayMode!!



            PhysicsMaterial = serializedObject.FindProperty("PhysicMat");
            ColliderSize = serializedObject.FindProperty("ColliderSize");
            LinearDrag = serializedObject.FindProperty("LinearDrag");
            AngularDrag = serializedObject.FindProperty("AngularDrag");
            GravityScale = serializedObject.FindProperty("GravityScale");
            FreezeRotation = serializedObject.FindProperty("FreezeRotation");


            OnValidateShapeFill = serializedObject.FindProperty("OnValidateShapeFill");
            ShapeFillCollider2D = serializedObject.FindProperty("ShapeFillCollider2D");
            ShapeFillAccuracy = serializedObject.FindProperty("ShapeFillAccuracy");

            OnCollisionEnterList = serializedObject.FindProperty("OnCollisionEnterList");
            OnSpawnerAboutToStart = serializedObject.FindProperty("OnSpawnerAboutStart");
            OnSpawnerAboutToEnd = serializedObject.FindProperty("OnSpawnerAboutEnd");
            OnSpawnerEmittingParticle = serializedObject.FindProperty("OnSpawnerEmitingParticle");



            // ----------

            //Header stuffs
            headerTexture = EditorUtils.HeaderTexture(); 
			headerFont = EditorUtils.HeaderFont();
			
			//Undo.undoRedoPerformed += refreshWSpawnerObject;
			
			_path = EditorUtils.getMainRelativepath();


           
            // REVIEW System
            int _r = EditorPrefs.GetInt("Water2DReview", 0);
            if (_r >= 0)
                EditorPrefs.SetInt("Water2DReview", _r + 1);

            if (_r >= 100)
                _timeToReview = true;



            _msg_current = LoadMessageText();
           
        }



        internal void OnDisable(){
			//Undo.undoRedoPerformed -= refreshWSpawnerObject;
			Tools.current = _cTool;
            PhysicsSimulation.Stop();

            WSpawner.SimulateInEditor = false;
            

        }
        string[] _msgs;
        string _msg_current;
        string LoadMessageText() {

            if (_msgs == null) {
                _msgs = new string[6];

                _msgs[0] = "turn off the tap while brushing" ;
                _msgs[1] = "264 gallons of water requires to produce 1 pound of meat";
                _msgs[2] = "you need drink half gallon of water per day";
                _msgs[3] = "put some fresh water for stray animals";
                _msgs[4] = "take a shower instead of bath";
                _msgs[5] = "fix that faucet that is leaking";

            }


            return _msgs[UnityEngine.Random.Range(0, _msgs.Length)];
        }

        void focusGameObjectInSceneView()
        {
            Selection.activeGameObject = WSpawner.gameObject;
            SceneView.FrameLastActiveSceneView();
            EditorGUIUtility.PingObject(WSpawner.gameObject);
            Selection.activeGameObject = WSpawner.gameObject;
            EditorGUIUtility.ExitGUI();
        }

        bool _needRebuild = false;
        public override void OnInspectorGUI () {
			if (WSpawner == null){return;}

            // Check is pro skin
            if (!EditorGUIUtility.isProSkin)
                GUI.backgroundColor = new Color(.52f, .5f, .55f);


            serializedObject.Update();

            if (_needRebuild)
            {
                _needRebuild = false;
                RebuildAction();
            }
			//initStyles();
			
			GUILayoutOption miniButtonWidth = GUILayout.Width(50f);


           

            GUIStyle headerStyle = new GUIStyle( GUI.skin.box );
			headerStyle.normal.background = EditorUtils.MakeTex( 2, 2, new Color( 18f/255f, 18f / 255f, 18f / 255f, 1f ) );
			headerStyle.fixedHeight = headerStyle.fixedHeight * 1.2f;

            // MAIN TITLE STYLE
			GUIStyle textHeaderStyle = new GUIStyle(GUI.skin.label);
			textHeaderStyle.fontSize = 18;
			textHeaderStyle.fontStyle = FontStyle.Bold;
			textHeaderStyle.alignment = TextAnchor.MiddleCenter;

            if (headerTexture)
			    textHeaderStyle.fixedHeight = headerTexture.height;

            textHeaderStyle.normal.textColor = new Color(4/255f, 156/255f, 1f);
			textHeaderStyle.font = headerFont;

            GUIStyle primarySubHeaderStyle = new GUIStyle(textHeaderStyle);
            primarySubHeaderStyle.normal.textColor = Color.gray;
            primarySubHeaderStyle.fontSize = 11;
            //textSubHeaderStyle.fontStyle = FontStyle.Normal;


            //SUB HEADER STYLE
            GUIStyle textSubHeaderStyle = new GUIStyle(GUI.skin.label);
            textSubHeaderStyle.fontSize = 11;
            textSubHeaderStyle.fontStyle = FontStyle.Bold;
            //textSubHeaderStyle.alignment = TextAnchor.MiddleCenter;

            textSubHeaderStyle.normal.textColor = new Color(.1f, .1f , .1f);
            // textSubHeaderStyle.font = headerFont;


            //SUB HEADER STYLE 2
            GUIStyle textSubHeaderStyle2 = new GUIStyle(GUI.skin.label);
            textSubHeaderStyle2.fontSize = 10;
            textSubHeaderStyle2.fontStyle = FontStyle.Bold;
            //textSubHeaderStyle.alignment = TextAnchor.MiddleCenter;

            textSubHeaderStyle2.normal.textColor = new Color(.65f, .65f, .65f);
            // textSubHeaderStyle.font = headerFont;


            //SUB HEADER STYLE 3
            GUIStyle textSubHeaderStyle3 = new GUIStyle(GUI.skin.label);
            textSubHeaderStyle3.fontSize = 9;
            textSubHeaderStyle3.fontStyle = FontStyle.Normal;
            textSubHeaderStyle3.normal.textColor = new Color(.1f, .1f, .1f);
            textSubHeaderStyle3.alignment = TextAnchor.MiddleCenter;
            textSubHeaderStyle3.fixedHeight *= .5f;
           
            
            //SUB HEADER STYLE 4
            GUIStyle textSubHeaderStyle4 = new GUIStyle(GUI.skin.label);
            textSubHeaderStyle4.fontSize = 10;
            textSubHeaderStyle4.fontStyle = FontStyle.Normal;
            //textSubHeaderStyle3.normal.textColor = new Color(.1f, .1f, .1f);
            textSubHeaderStyle4.alignment = TextAnchor.MiddleCenter;
            textSubHeaderStyle4.fixedHeight *= .5f;


            // Header //
            EditorGUILayout.BeginHorizontal(headerStyle);


            GUILayoutOption[] reBtnStyle ={GUILayout.Width(35f), GUILayout.Height(35f)};

			//Focus btn
			if(GUILayout.Button(AssetDatabase.LoadAssetAtPath(EditorUtils.getMainRelativepath() + "Misc/hierarchyIcon.png", typeof(Texture2D)) as Texture2D , headerStyle, reBtnStyle))
			{
				focusGameObjectInSceneView();
			}
			GUILayout.FlexibleSpace();
			
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("WATER 2D  ", textHeaderStyle);
            string subText = WSpawner.Water2DType;
            EditorGUILayout.LabelField(subText + " style", primarySubHeaderStyle);
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();


            //TYPE &  VERSION //
            EditorGUILayout.BeginHorizontal("Box");
            
            WSpawner.Water2DRenderType = GraphicsSettings.renderPipelineAsset == null ? "Legacy" : "URP(LWRP)";
           
            EditorGUILayout.LabelField("Version:" + WSpawner.Water2DVersion + " | " + "Render pipeline:" + WSpawner.Water2DRenderType, textSubHeaderStyle3);
            EditorGUILayout.EndHorizontal();


            // message! //
            GUI.color = Color.cyan;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(_msg_current, textSubHeaderStyle4);
            EditorGUILayout.EndHorizontal();
            GUI.color = Color.white;

            #region review box
            if (_timeToReview)
            {
                Color c = GUI.color;
                GUI.color = new Color32(103, 188, 114, 255);
                EditorGUILayout.HelpBox("Enjoying Water2D? Please take a minute to write a responsable review. Thank you", MessageType.Info, true);
                EditorGUILayout.BeginHorizontal("box");
                if (GUILayout.Button("YES!"))
                {
                    _timeToReview = false;
                    EditorPrefs.SetInt("Water2DReview", -1);
                    //Application.OpenURL("http://u3d.as/asp");
                    UnityEditorInternal.AssetStore.Open("/content/25933");
                }
                if (GUILayout.Button("No"))
                {
                    _timeToReview = false;
                    EditorPrefs.SetInt("Water2DReview", -1);
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(10);
                GUI.color = c;
            }

            #endregion

            //  ---- BUTTONS  ------
            EditorGUILayout.BeginHorizontal("Box");
            // TOGGLE TO RUN IN EDITOR
            GUIStyle ToggleButtonStyleNormal = null;
            GUIStyle ToggleButtonStyleRun = null;
            GUIStyle ToggleButtonNormalFixed = null;

            ToggleButtonStyleNormal = "Button";
            //ToggleButtonStyleNormal.fixedWidth = 100f;
            //ToggleButtonStyleNormal.hover.background = ToggleButtonStyleNormal.active.background;

            ToggleButtonStyleRun = new GUIStyle(ToggleButtonStyleNormal);
            ToggleButtonStyleRun.normal.textColor = Color.red;
            ToggleButtonStyleRun.normal.background = ToggleButtonStyleRun.active.background;
            //ToggleButtonStyleRun.fixedWidth = 100f;
            //ToggleButtonStyleRun.onHover//

            ToggleButtonNormalFixed = new GUIStyle(ToggleButtonStyleNormal);
            ToggleButtonNormalFixed.fixedWidth = 100f;


            // MID MINI BUTTONS

            GUIStyle ToggleMiniStyleTrue = null;
            GUIStyle ToggleMiniStyleFalse = null;

            ToggleMiniStyleFalse = new GUIStyle(EditorStyles.miniButtonMid);

            ToggleMiniStyleTrue = new GUIStyle(EditorStyles.miniButtonMid);
            ToggleMiniStyleTrue.normal.textColor = new Color(.1f,.1f, .1f);
            ToggleMiniStyleTrue.normal.background = ToggleMiniStyleFalse.active.background;

            // LEFT MINI BUTTONS

            GUIStyle ToggleMiniLeftStyleTrue = null;
            GUIStyle ToggleMiniLeftStyleFalse = null;

            ToggleMiniLeftStyleFalse = new GUIStyle(EditorStyles.miniButtonLeft);

            ToggleMiniLeftStyleTrue = new GUIStyle(EditorStyles.miniButtonLeft);
            ToggleMiniLeftStyleTrue.normal.textColor = new Color(.1f, .1f, .1f);
            ToggleMiniLeftStyleTrue.normal.background = ToggleMiniLeftStyleFalse.active.background;

            // RIGHT MINI BUTTONS

            GUIStyle ToggleMiniRightStyleTrue = null;
            GUIStyle ToggleMiniRightStyleFalse = null;

            ToggleMiniRightStyleFalse = new GUIStyle(EditorStyles.miniButtonRight);

            ToggleMiniRightStyleTrue = new GUIStyle(EditorStyles.miniButtonRight);
            ToggleMiniRightStyleTrue.normal.textColor = new Color(.1f, .1f, .1f);
            ToggleMiniRightStyleTrue.normal.background = ToggleMiniRightStyleFalse.active.background;





            string btnCaption = "";

            Texture2D playIcon = AssetDatabase.LoadAssetAtPath(EditorUtils.getMainRelativepath() + "Misc/play.png", typeof(Texture2D)) as Texture2D;
            Texture2D stopIcon = AssetDatabase.LoadAssetAtPath(EditorUtils.getMainRelativepath() + "Misc/stop_2.png", typeof(Texture2D)) as Texture2D;
            Texture2D rebuildIcon = AssetDatabase.LoadAssetAtPath(EditorUtils.getMainRelativepath() + "Misc/reload-w.png", typeof(Texture2D)) as Texture2D;

           

            if (WSpawner.Water2DEmissionType == Water2D_Spawner.EmissionType.ParticleSystem)
            {
                if (Application.isPlaying)
                {
                    btnCaption = WSpawner.IsSpawning ? "Stop" : "Simulate";
                    Texture2D icon = WSpawner.IsSpawning ? stopIcon : playIcon;

                    if (GUILayout.Button(icon, WSpawner.IsSpawning ? ToggleButtonStyleRun : ToggleButtonStyleNormal))
                    {
                        if (!WSpawner.IsSpawning)
                            WSpawner.Spawn();
                        else
                            WSpawner.StopSpawning();

                    }
                }
                else
                {
                    btnCaption = WSpawner.SimulateInEditor ? "Stop" : "Simulate";
                    Texture2D icon = WSpawner.SimulateInEditor ? stopIcon : playIcon;
                    if (GUILayout.Button(icon, SimulateInEditor.boolValue ? ToggleButtonStyleRun : ToggleButtonStyleNormal))
                    {
                        //if(WSpawner.SimulateInEditor)


                        WSpawner.DropsUsed *= 0;
                        SimulateInEditor.boolValue = !SimulateInEditor.boolValue;


                        if (SimulateInEditor.boolValue)
                        {
                            WSpawner.InvokeOnSpawnerStart(WSpawner.gameObject);
                            PhysicsSimulation.Run(); // start physics simulation class
                        }
                        else
                        {
                            PhysicsSimulation.Stop();
                        }
                    }

                }
            }
            else {
                int _selected = FillerColliderType.intValue;
                int _lastSelected = _selected;
                bool _lastMaskedValue = FillerColliderMasked.boolValue;
                string[] _options = new string[3] { "Box", "Circle", "Polygon" };
                EditorGUILayout.BeginVertical();
                _selected = EditorGUILayout.Popup("Filler collider type", _selected, _options);

                FillerColliderMasked.boolValue =  EditorGUILayout.Toggle("Substract shape?",FillerColliderMasked.boolValue);
                EditorGUILayout.EndVertical();

                if (_selected != _lastSelected)
                {
                    Collider2D[] c = WSpawner.GetComponents<Collider2D>();
                    foreach (var col in c)
                    {
                        if(Application.isPlaying)
                            Destroy(col);
                        else
                            DestroyImmediate(col);
                    }

                    Collider2D _futureCol = null;
                    
                    if(_selected == 0)
                        _futureCol = WSpawner.gameObject.AddComponent<BoxCollider2D>();
                    if(_selected == 1)
                        _futureCol = WSpawner.gameObject.AddComponent<CircleCollider2D>();
                    if(_selected == 2)
                        _futureCol = WSpawner.gameObject.AddComponent<PolygonCollider2D>();

                    _futureCol.isTrigger = true;
                    ColliderFiller cf = WSpawner.gameObject.GetComponent<ColliderFiller>();
                    cf.collider = _futureCol;
                    cf.Masked = FillerColliderMasked.boolValue;
                    FillerColliderType.intValue = _selected;
                    _lastMaskedValue = FillerColliderMasked.boolValue;
                    RebuildAction();
                    
                    
                }
                if (FillerColliderMasked.boolValue != _lastMaskedValue)
                {
                    _lastMaskedValue = FillerColliderMasked.boolValue;
                    ColliderFiller cf = WSpawner.gameObject.GetComponent<ColliderFiller>();                    
                    cf.Masked = FillerColliderMasked.boolValue;
                    _needRebuild = true;
                    
                }


               
            }
           

            if (GUILayout.Button(rebuildIcon, ToggleButtonNormalFixed))
            {
                RebuildAction();
            }
            EditorGUILayout.EndHorizontal();

           

            // ----- END BUTTONS ----- 




           

            EditorGUILayout.BeginVertical("Box");

            /// ------- RENDERER --------///

            EditorGUILayout.LabelField("Renderer", textSubHeaderStyle2);
            EditorGUILayout.BeginVertical("Box");

            

            //EditorGUILayout.PropertyField(lmaterial, new GUIContent("Material", "Material Object used for render into light mesh"));

            //EditorGUILayout.PropertyField(DropObject, new GUIContent("Water drop object"));
            //EditorGUILayout.PropertyField(Material, new GUIContent("Material"));

            //------- MATERIAL PREFS -----///
            string _material = CheckMaterialSetup();
            if (_material != null)
            {
                //CurrentColorScheme = (SCHEME)ColorScheme.intValue;
                if (Selection.transforms.Length < 2 && WSpawner.Water2DType == "Toon"){
                    CurrentColorScheme = (SCHEME)EditorGUILayout.EnumPopup("Color Scheme", (SCHEME)ColorScheme.intValue);
                    ColorScheme.intValue = (ColorScheme.intValue != (int)CurrentColorScheme) ? (int)CurrentColorScheme : ColorScheme.intValue;
                }

                if (WSpawner.Water2DType == "Regular") // TOON SHADER
                {
                    Blending.boolValue = EditorGUILayout.Toggle("Blending Colors",Blending.boolValue);
                    EditorGUILayout.PropertyField(FillColor, new GUIContent("Fill Color"));
                    EditorGUILayout.PropertyField(StrokeColor, new GUIContent("Fresnel Color (shared)"));
                    EditorGUILayout.Slider(AlphaCutOff, 0f, 1f, new GUIContent("Alpha CutOff"));
                    EditorGUILayout.Slider(AlphaStroke, 0f, 6f, new GUIContent("Multiplier"));
                }
                if (WSpawner.Water2DType == "Toon") // TOON SHADER
                {
                    EditorGUILayout.PropertyField(FillColor, new GUIContent("Fill Color"));
                    EditorGUILayout.PropertyField(StrokeColor, new GUIContent("Stroke Color"));
                    EditorGUILayout.Slider(AlphaCutOff, 0f, 1f, new GUIContent("Alpha CutOff"));
                    EditorGUILayout.Slider(AlphaStroke, 0f, 1f, new GUIContent("Alpha Stroke"));
                }
                else if (WSpawner.Water2DType == "Refracting") // REFRACTING SHADER
                {
                    Blending.boolValue = EditorGUILayout.Toggle("Blending Colors", Blending.boolValue);
                    EditorGUILayout.PropertyField(FillColor, new GUIContent("Tint Color"));
                    EditorGUILayout.Slider(Intensity, 0f, 1f, new GUIContent("Intensity"));
                    EditorGUILayout.PropertyField(StrokeColor, new GUIContent("Fresnel Color (shared)"));
                    EditorGUILayout.Slider(Distortion, 0f, 3f, new GUIContent("Distortion"));
                    EditorGUILayout.Slider(DistortionSpeed, 0f, 5f, new GUIContent("Speed"));

                }

                if(_lastBlending.boolValue != Blending.boolValue)
                {
                    _lastBlending.boolValue = Blending.boolValue;
                    _needRebuild = true;
                }

                EditorGUILayout.PropertyField(Sorting, new GUIContent("Sorting Order"));
            }
           
            EditorGUILayout.EndVertical();

            /// ------- TRAIL --------///

            EditorGUILayout.LabelField("Trail", textSubHeaderStyle2);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.Slider(TrailSizeStart, 0f, 2f, new GUIContent("Start Size"));
            if (TrailSizeStart.floatValue > 0f)
            {
                EditorGUILayout.Slider(TrailSizeEnd, .1f, 2f, new GUIContent("End Size"));
                EditorGUILayout.Slider(TrailDuration, .001f, 1f, new GUIContent("Duration Size"));
            }


            EditorGUILayout.EndVertical();




            // TOGGLES TOGGLES TOGGLES TOGGLESTOGGLES TOGGLESTOGGLES TOGGLESTOGGLES TOGGLESTOGGLES TOGGLES
            //TOGGLES TOGGLESTOGGLES TOGGLESTOGGLES TOGGLESTOGGLES TOGGLESTOGGLES TOGGLESTOGGLES TOGGLES


            GUILayout.Space(20f);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();

            if (WSpawner.Water2DEmissionType == Water2D_Spawner.EmissionType.ParticleSystem)
            {
                // ------- EMISSION --------//
                if (GUILayout.Button("Emission", foldoutEmission ? ToggleMiniLeftStyleTrue : ToggleMiniLeftStyleFalse))
                {
                    foldoutEmission = true;
                    foldoutEvents = false;
                    foldoutPhysics = false;
                    foldoutEmissionFiller = false;
                }
            }

            if (WSpawner.Water2DEmissionType == Water2D_Spawner.EmissionType.FillerCollider)
            {
                // ------- EMISSION FILLER --------//
                if (GUILayout.Button("Particles", foldoutEmissionFiller ? ToggleMiniLeftStyleTrue : ToggleMiniLeftStyleFalse))
                {
                    foldoutEmission = false;
                    foldoutEvents = false;
                    foldoutPhysics = false;
                    foldoutEmissionFiller = true;
                }
            }


            // ---------------------------//


            // ------- PHYSICS --------//
            if (GUILayout.Button("Physics", foldoutPhysics ? ToggleMiniStyleTrue : ToggleMiniStyleFalse))
            {
                foldoutEmission = false;
                foldoutEvents = false;
                foldoutPhysics = true;
                foldoutEmissionFiller = false;

            }

            // ---------------------------//

            // ------- EVENTS --------//
            // string eventsCaption = foldoutEvents ? "Events" : "- Events";
            if (GUILayout.Button("Events",foldoutEvents ? ToggleMiniRightStyleTrue : ToggleMiniRightStyleFalse))
            {
                foldoutEmission = false;
                foldoutEvents = true;
                foldoutPhysics = false;
                foldoutEmissionFiller = false;
            }


            EditorGUILayout.EndHorizontal();


            //------------------------------//

            if (foldoutEmissionFiller && WSpawner.Water2DEmissionType == Water2D_Spawner.EmissionType.FillerCollider)
            {

                GUILayout.Space(10f);
                EditorGUILayout.LabelField("General", textSubHeaderStyle2);
                EditorGUILayout.BeginVertical("Box");


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(DropCount, new GUIContent("Particles count"));
                if (DropCount.intValue != _lastDropCount.intValue)
                {
                    // Button press
                    if (GUILayout.Button("rebuild", EditorStyles.miniButton))
                    {
                        RebuildAction();
                    }

                    // Return key press
                    if (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return)
                    {
                        GUIUtility.hotControl = 0;
                        RebuildAction();

                    }
                }
                EditorGUILayout.EndHorizontal();
                
                // Limiter 
                if (DropCount.intValue > 800)
                    DropCount.intValue = 800;


                float lastSize = SizeDropObject.floatValue;
                EditorGUILayout.Slider(SizeDropObject, .001f, .2f, new GUIContent("Start Size"));

                if (lastSize != SizeDropObject.floatValue)
                    _needRebuild = true;


                EditorGUILayout.Space();

               

                if (Speed.floatValue < 0)
                    Speed.floatValue = 0f;

                EditorGUILayout.EndVertical();


                GUILayout.Space(10f);
                EditorGUILayout.LabelField("Velocity", textSubHeaderStyle2);
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.PropertyField(Speed, new GUIContent("Speed"));
                EditorGUILayout.Space();

                // X LIMIT
                EditorGUILayout.LabelField("Range Limit X", textSubHeaderStyle2);
                min = SpeedLimiterX.vector2Value.x;
                max = SpeedLimiterX.vector2Value.y;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(min.ToString("##"));
                GUILayout.FlexibleSpace();
                EditorGUILayout.MinMaxSlider(ref min, ref max, minLimit, maxLimit);
                GUILayout.FlexibleSpace();
                GUILayout.Label(max.ToString("##"));
                GUILayout.FlexibleSpace();
                SpeedLimiterX.vector2Value = new Vector2(min, max);
                EditorGUILayout.EndHorizontal();

                // Y LIMIT
                EditorGUILayout.LabelField("Range Limit Y", textSubHeaderStyle2);
                min = SpeedLimiterY.vector2Value.x;
                max = SpeedLimiterY.vector2Value.y;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(min.ToString("##"));
                GUILayout.FlexibleSpace();
                EditorGUILayout.MinMaxSlider(ref min, ref max, minLimit, maxLimit);
                GUILayout.FlexibleSpace();
                GUILayout.Label(max.ToString("##"));
                GUILayout.FlexibleSpace();
                SpeedLimiterY.vector2Value = new Vector2(min, max);
                EditorGUILayout.EndHorizontal();
                //EditorGUILayout.EndVertical();




                EditorGUILayout.EndVertical();

            }



            if (foldoutEmission && WSpawner.Water2DEmissionType == Water2D_Spawner.EmissionType.ParticleSystem)
            {

                EditorGUILayout.Space();

                float living_part = (float)((float)CurrentlyDropsUsed.intValue / (float)DropCount.intValue);
                GUI.color = living_part >= .95f ? new Color(1f , 95 / 255f, 71/255f) : new Color(108/255f, 174/255f, 1f);
                var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                EditorGUI.ProgressBar(rect, living_part, "living particles: " + CurrentlyDropsUsed.intValue.ToString() + " / " + _lastDropCount.intValue.ToString());
                GUI.color = Color.white;


                GUILayout.Space(10f);
                EditorGUILayout.LabelField("General", textSubHeaderStyle2);
                EditorGUILayout.BeginVertical("Box");
               

                SimulateOnAwake.boolValue = EditorGUILayout.Toggle(new GUIContent("Play On Awake"), SimulateOnAwake.boolValue);

               
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(DropCount, new GUIContent("Particles count"));
                if (DropCount.intValue != _lastDropCount.intValue)
                {
                    // Button press
                    if (GUILayout.Button("rebuild", EditorStyles.miniButton))
                    {
                        RebuildAction();
                    }

                    // Return key press
                    if (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return)
                    {
                        GUIUtility.hotControl = 0;
                        RebuildAction();
                       
                    }
                }
                EditorGUILayout.EndHorizontal();
                // Limiter 
                if (DropCount.intValue > 800)
                    DropCount.intValue = 800;
               
         

                EditorGUILayout.Slider(SizeDropObject, .001f, .2f, new GUIContent("Start Size"));

                CanScaleDown.boolValue = EditorGUILayout.Toggle("Scale Down", CanScaleDown.boolValue);


                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Slider(LifetimeDropObject, 0f, 20f, new GUIContent("Lifespan"));

                if (LifetimeDropObject.floatValue <= 0f)
                {
                    GUI.color = Color.gray;
                    //EditorGUILayout.LabelField("*Particles will never die");
                    EditorGUILayout.HelpBox("Particles will never die", MessageType.Info);
                    GUI.color = Color.white;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Slider(DelayBetweenParticles, 0.00001f, 1f, new GUIContent("Delay in between"));

                EditorGUILayout.Space();


                EditorGUILayout.BeginHorizontal();
               Loop.boolValue = EditorGUILayout.Toggle(new GUIContent("Loop"),Loop.boolValue);
                
                if (Loop.boolValue)
                    EditorGUILayout.HelpBox("The spawner will spawn repeatedly", MessageType.Info);

                EditorGUILayout.EndHorizontal();
                


                EditorGUILayout.BeginHorizontal();
                PersistentFluid.boolValue = EditorGUILayout.Toggle(new GUIContent("Persistent Fluid"), PersistentFluid.boolValue);
                if(PersistentFluid.boolValue)
                    EditorGUILayout.HelpBox("Used to start playmode with already present fluild in scene", MessageType.Info);

                EditorGUILayout.EndHorizontal();

                /*
                autoCalcParticles = EditorGUILayout.Toggle(new GUIContent("Autocalculate amout"), autoCalcParticles);

                if (autoCalcParticles)
                    autoCalcParticles = false;

                if (autoCalcParticles)
                {
                    DropCount.intValue = (int)((LifetimeDropObject.floatValue / DelayBetweenParticles.floatValue) * 0.5f) + 1;
                    if (DropCount.intValue > 250)
                        DropCount.intValue = 250;
                }
                */

                if (Speed.floatValue < 0)
                    Speed.floatValue = 0f;

                EditorGUILayout.EndVertical();



                GUILayout.Space(10f);
                EditorGUILayout.LabelField("Velocity", textSubHeaderStyle2);
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.PropertyField(Speed, new GUIContent("Speed"));
                EditorGUILayout.Space();

                // X LIMIT
                EditorGUILayout.LabelField("Range Limit X", textSubHeaderStyle2);
                min = SpeedLimiterX.vector2Value.x;
                max = SpeedLimiterX.vector2Value.y;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(min.ToString("##"));
                GUILayout.FlexibleSpace();
                EditorGUILayout.MinMaxSlider(ref min, ref max, minLimit, maxLimit);
                GUILayout.FlexibleSpace();
                GUILayout.Label(max.ToString("##"));
                GUILayout.FlexibleSpace();
                SpeedLimiterX.vector2Value = new Vector2(min, max);
                EditorGUILayout.EndHorizontal();

                // Y LIMIT
                EditorGUILayout.LabelField("Range Limit Y", textSubHeaderStyle2);
                min = SpeedLimiterY.vector2Value.x;
                max = SpeedLimiterY.vector2Value.y;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(min.ToString("##"));
                GUILayout.FlexibleSpace();
                EditorGUILayout.MinMaxSlider(ref min, ref max, minLimit, maxLimit);
                GUILayout.FlexibleSpace();
                GUILayout.Label(max.ToString("##"));
                GUILayout.FlexibleSpace();
                SpeedLimiterY.vector2Value = new Vector2(min, max);
                EditorGUILayout.EndHorizontal();
                //EditorGUILayout.EndVertical();



               
                EditorGUILayout.EndVertical();

            }

            if (foldoutEvents)
            {
                GUILayout.Space(10f);

                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField("Events by Shape Fill Detection", textSubHeaderStyle2);
                EditorGUILayout.PropertyField(OnValidateShapeFill, new GUIContent("OnValidateShapeFill", "List of callback methods called when each drop collide."));
                EditorGUILayout.PropertyField(ShapeFillCollider2D, new GUIContent("Collider2D"));
                EditorGUILayout.Slider(ShapeFillAccuracy, 0f, 1f, new GUIContent("Accuracy Result"));
                if (WSpawner.Water2DEmissionType == Water2D_Spawner.EmissionType.FillerCollider)
                {
                    EditorGUILayout.LabelField("Fire events with " + (int)(WSpawner.DropsUsed*ShapeFillAccuracy.floatValue) + " inside the shape target", textSubHeaderStyle4);
                }
                else
                {
                    EditorGUILayout.LabelField("Fire events with " + (int)(WSpawner.DropCount * ShapeFillAccuracy.floatValue) + " inside the shape target", textSubHeaderStyle4);
                }
               
                EditorGUILayout.EndVertical();

                GUILayout.Space(20f);

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField("Events by each Particle", textSubHeaderStyle2);
                EditorGUILayout.PropertyField(OnCollisionEnterList, new GUIContent("OnParticleCollisionEnter", "List of callback methods called when each drop collide."));
                EditorGUILayout.PropertyField(OnSpawnerEmittingParticle, new GUIContent("OnSpawnerEmittingParticle", "List of callback methods called when spawner is creating a particle."));
                EditorGUILayout.EndVertical();

                GUILayout.Space(20f);

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField("Events by Spawner", textSubHeaderStyle2);
                EditorGUILayout.PropertyField(OnSpawnerAboutToStart, new GUIContent("OnSpawnerAboutToStart", "List of callback methods called when spawner is about to start."));
                EditorGUILayout.PropertyField(OnSpawnerAboutToEnd, new GUIContent("OnSpawnerAboutToEnd", "List of callback methods called when spawner is about to end."));
                EditorGUILayout.EndVertical();


               
                EditorGUILayout.EndVertical();
            }
            // ---------------------------//



            if (foldoutPhysics)
            {

                GUILayout.Space(10f);

                EditorGUILayout.BeginVertical("Box");


                EditorGUILayout.PropertyField(PhysicsMaterial, new GUIContent("Physics Material"));
                EditorGUILayout.Slider(ColliderSize, .001f, 3f, new GUIContent("Collider Size"));
                EditorGUILayout.Slider(LinearDrag, 0f, 30f, new GUIContent("Linear Drag"));
                EditorGUILayout.Slider(AngularDrag, 0f, 30f, new GUIContent("Angular Drag"));
                EditorGUILayout.PropertyField(GravityScale, new GUIContent("Gravity Scale"));
                FreezeRotation.boolValue = EditorGUILayout.Toggle(new GUIContent("Freeze rotation"), FreezeRotation.boolValue);

                EditorGUILayout.EndVertical();

                //EditorGUILayout.EndVertical();
            }
            // ---------------------------//




            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();



            // SAVE ALL
            serializedObject.ApplyModifiedProperties();
			
			
			
			
		}

        void RebuildAction() {

            bool wasRunning = false;

            if (WSpawner.Water2DEmissionType == Water2D_Spawner.EmissionType.ParticleSystem)
            {
                // Stop first
                if (Application.isPlaying)
                {
                    wasRunning = WSpawner.IsSpawning;
                    WSpawner.StopSpawning();
                    WSpawner.SetupParticles();
                    if (wasRunning)
                        WSpawner.Spawn();
                }
                else
                {
                    WSpawner.DropsUsed *= 0;
                    wasRunning = SimulateInEditor.boolValue;
                    SimulateInEditor.boolValue = false;
                    WSpawner.SetupParticles();
                    if (wasRunning)
                        SimulateInEditor.boolValue = true;

                    PhysicsSimulation.Run();
                }
                _lastDropCount.intValue = DropCount.intValue;
            }
            else
            {

                ColliderFiller cf = WSpawner.gameObject.GetComponent<ColliderFiller>();
                cf.collider = WSpawner.gameObject.GetComponent<Collider2D>();
                cf.collider.offset = Vector2.zero;
                cf.Refresh();
                cf.Fill();
               
                
            }
            



            SerializedObject m_CustomSettings = SettingsManager.GetSerializedSettings();
            bool flipTex = AssetUtility.LoadPropertyAsBool("w2d_FlipCameraTexture", m_CustomSettings);

            ResizeQuadEffectController.RebuildTextures(flipTex? 1:0);

        }



        void OnSceneGUI()
        {
           
            if (WSpawner)
            {
                Event _event = Event.current;
                Transform lTransform = WSpawner.transform;

                if (WSpawner.Water2DEmissionType == Water2D_Spawner.EmissionType.FillerCollider)
                {
                    if (_event.type == EventType.MouseUp && _event.button == 0)
                    {
                        RebuildAction();
                       // _event.Use();
                    }
                    return;

                }


               
               // EditorUtility.SetSelectedRenderState(WSpawner.GetComponent<MeshRenderer>(), EditorSelectedRenderState.Hidden);
                


                Vector3 oldPoint = lTransform.TransformPoint(new Vector3(WSpawner.Speed, 0, 0));
                float size = HandleUtility.GetHandleSize(oldPoint) * 1.4f;

                Undo.RecordObject(WSpawner, "Move Water Speed Point");


                Color color = (Color)WSpawner.FillColor; //((Color.gray - (Color)WSpawner.FillColor) * .3f);
                color.a = 1;
                Handles.color = color;

                //// Size & Rotation ----- ///
                //-----------------------
                float r = WSpawner.Speed;
                Vector3 newHandlePoint = Vector3.zero;
                bool dirtyRotation = false;
                Vector3 dirLookAt = Vector3.zero;
                Vector3 direction = (lTransform.localRotation * Vector2.down).normalized;
                newHandlePoint = (lTransform.position - Handles.FreeMoveHandle(
                        lTransform.position + direction * (-r),
                        Quaternion.identity,
                        size * 0.035f, Vector3.zero, Handles.CircleHandleCap));

                Handles.DrawSolidDisc(lTransform.position + direction * -r, -Vector3.forward, size * 0.030f);
                r = newHandlePoint.magnitude;
                WSpawner.Speed = r;

                Handles.DrawLine(lTransform.position, lTransform.position + direction * -r);
                Handles.DrawWireDisc(lTransform.position, -Vector3.forward, r - (size * 0.030f));
                //Handles.draw(012, lTransform.position, lTransform.rotation, (size * 0.030f));

                dirtyRotation = false;
                if (GUIUtility.hotControl == GetLastControlId())
                {
                    dirLookAt = (direction - newHandlePoint).normalized;
                    dirtyRotation = true;
                }

                //IF Ctrl is pressed , does not rotate
                if (dirtyRotation && (!_event.control))
                {
                    lTransform.up = dirLookAt;
                    Vector3 _r = lTransform.localEulerAngles;
                    if (_r.z > 360 || _r.z < -360) _r.z *= 0;
                    _r.x = 0;
                    _r.y = 0;

                    lTransform.localEulerAngles = _r;
                }

               



                #region FreeMove  //*******************************************************

                bool dirtyFreeMove = false;
                Vector3 dif = Vector3.zero;
                Handles.color = Color.black;
                Vector3 newFreeHandlePoint = (Handles.FreeMoveHandle(lTransform.position, Quaternion.identity,
                    size * 0.25f, Vector3.zero, Handles.CircleHandleCap));

                if (GUIUtility.hotControl == GetLastControlId() && !dirtyRotation)
                {
                    if (newFreeHandlePoint != lTransform.position)
                    {
                        dirtyFreeMove = true;
                    }
                }


                if (dirtyFreeMove)
                {
                    Event e = Event.current;
                    Vector3 _finalp = newFreeHandlePoint;
                    if (e.control)
                    {
                        _finalp.x = Mathf.Round(newFreeHandlePoint.x / EditorPrefs.GetFloat("MoveSnapX")) * EditorPrefs.GetFloat("MoveSnapX");
                        _finalp.y = Mathf.Round(newFreeHandlePoint.y / EditorPrefs.GetFloat("MoveSnapY")) * EditorPrefs.GetFloat("MoveSnapY");
                    }
                    lTransform.position = _finalp;
                }

                #endregion//Freemove  **********************************************************

                // SHOW SCENE VIEW AREA
                #region SCENE VIEW AREA
                GUILayout.BeginArea(new Rect(20, 20, 150, 100));

                var rect = EditorGUILayout.BeginVertical();
                GUI.color = Color.yellow;
                GUI.Box(rect, GUIContent.none);

                GUI.color = Color.white;

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(WSpawner.gameObject.name);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUI.backgroundColor = WSpawner.SimulateInEditor? Color.red : Color.green;

                // Buttons styles
                // TOGGLE TO RUN IN EDITOR
                GUIStyle ToggleButtonStyleNormal = null;
                GUIStyle ToggleButtonStyleRun = null;
                ToggleButtonStyleNormal = "Button";

                ToggleButtonStyleRun = new GUIStyle(ToggleButtonStyleNormal);
                //ToggleButtonStyleRun.normal.textColor = Color.gray;
                //ToggleButtonStyleRun.normal.background = ToggleButtonStyleRun.active.background;

                // -----------------


                //SIMULATE BUTTON ------------------------------
                string btnCaption = "";
                if (Application.isPlaying)
                {
                    btnCaption = WSpawner.IsSpawning ? "Stop" : "Simulate";
                    if (GUILayout.Button(btnCaption, WSpawner.IsSpawning ? ToggleButtonStyleRun : ToggleButtonStyleNormal))
                    {
                        if (!WSpawner.IsSpawning)
                            WSpawner.Spawn();
                        else
                            WSpawner.StopSpawning();

                    }
                }
                else
                {
                    btnCaption = WSpawner.SimulateInEditor ? "Stop" : "Simulate";
                    if (GUILayout.Button(btnCaption, SimulateInEditor.boolValue ? ToggleButtonStyleRun : ToggleButtonStyleNormal))
                    {
                        //if(WSpawner.SimulateInEditor)
                        PhysicsSimulation.Run(); // start physics simulation class

                        WSpawner.DropsUsed *= 0;
                        WSpawner.SimulateInEditor = !WSpawner.SimulateInEditor;
                    }

                }
                //END SIMULATE BUTTON ------------------------------

                GUI.backgroundColor = Color.green;

                if (GUILayout.Button("Rebuild"))
                {
                    bool wasRunning = false;
                    // Stop first
                    if (Application.isPlaying)
                    {
                        wasRunning = WSpawner.IsSpawning;
                        WSpawner.StopSpawning();
                        WSpawner.SetupParticles();
                        if (wasRunning)
                            WSpawner.Spawn();
                    }
                    else
                    {
                        WSpawner.DropsUsed *= 0;
                        wasRunning = SimulateInEditor.boolValue;
                        SimulateInEditor.boolValue = false;
                        WSpawner.SetupParticles();
                        if (wasRunning)
                            SimulateInEditor.boolValue = true;
                    }


                }

                GUILayout.EndHorizontal();
                GUILayout.Space(5f);
                EditorGUILayout.EndVertical();
                GUILayout.EndArea();

                #endregion // SCENE VIEW AREA


            }
        }

            void showSceneGUIHint(){
			Handles.BeginGUI ();
			GUI.color = Color.green;
			GUILayout.BeginArea(new Rect(Screen.width - 140, Screen.height - 120, 140,120));
				EditorGUILayout.HelpBox ("Rotation only: SHIFT + click", MessageType.None);
				EditorGUILayout.HelpBox ("Resize only: CTRL + click", MessageType.None);
			GUILayout.EndArea ();
			GUI.color = Color.white;
			Handles.EndGUI ();
		}
		
		
		
		
		
		
		private Texture2D MakeTex( int width, int height, Color col )
		{
			Color[] pix = new Color[width * height];
			for( int i = 0; i < pix.Length; ++i )
			{
				pix[ i ] = col;
			}
			Texture2D result = new Texture2D( width, height );
			result.SetPixels( pix );
			result.Apply();
			return result;
		}
		
		
		
		
		
		internal void initStyles(){
			if(_path == null)
			{_path = EditorUtils.getMainRelativepath();}
			
			titleStyle = new GUIStyle(GUI.skin.label);
			titleStyle.fontSize = 15;
			titleStyle.fontStyle = FontStyle.Bold;
			titleStyle.alignment = TextAnchor.MiddleCenter;
			titleStyle.margin = new RectOffset(4, 4, 10, 0);
			
			subTitleStyle = new GUIStyle(GUI.skin.label);
			subTitleStyle.fontSize = 13;
			subTitleStyle.fontStyle = FontStyle.Bold;
			subTitleStyle.alignment = TextAnchor.MiddleLeft;
			subTitleStyle.margin = new RectOffset(4, 4, 10, 0);
			
			bgStyle = new GUIStyle(GUI.skin.box);
			bgStyle.margin = new RectOffset(4, 4, 0, 4);
			bgStyle.padding = new RectOffset(1, 1, 1, 2);
			
			btnStyle = new GUIStyle(GUI.skin.button);
			Sprite bg = (Sprite)AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/Textures/box_sprite.png", typeof(Sprite));
			Sprite bgClicked = bg;//(Sprite)AssetDatabase.LoadAssetAtPath(_path + "Textures/box_sprite.png", typeof(Sprite));
			btnStyle.margin = new RectOffset(0, 0, 0, 0);
			btnStyle.padding = new RectOffset(0, 0, 4, 4);
			btnStyle.normal.background = bg.texture;
			btnStyle.active.background = bgClicked.texture;
			
		}
		
		//internal Material lastMat;
		//internal Material newMat;
		//internal bool previewingNewMat = false;
		
		
		
		public static FieldInfo LastControlIdField = typeof(EditorGUIUtility).GetField("s_LastControlID", BindingFlags.Static | BindingFlags.NonPublic);
		public static int GetLastControlId()
		{
			if (LastControlIdField == null)
			{
				Debug.LogError("Compatibility with Unity broke: can't find lastControlId field in EditorGUI");
				return 0;
			}
			return (int)LastControlIdField.GetValue(null);
		}
		
		void divider(){
			EditorGUILayout.Space();
			GUI.color = Color.black;
			GUILayout.Box("", new GUILayoutOption[]{ GUILayout.ExpandWidth(true), GUILayout.Height(1) });
			GUI.color = Color.white;
			GUILayout.Box("", new GUILayoutOption[]{ GUILayout.ExpandWidth(true), GUILayout.Height(1) });
			//GUI.color = Color.white;
		}

		
		#region SceneGUINotifications
		
		static bool allowNotificationScene = false;
		float _t = 0f;
		Color notifyColor = Color.cyan;
		static string _tmpText;
		static float _tmpTime;
		float _tmpDelay;
		public void ShowUpNotification(string text, float time = 100f, float delay = 0f){
			if (allowNotificationScene == true)
				return;
			
			_tmpText = text;
			_tmpTime = time;
			_tmpDelay = delay;
			allowNotificationScene = true;
		}
		private void showNotificationSceneGUI(){

			if (!allowNotificationScene)
				return;

			if (_tmpDelay > 0f) {
				_tmpDelay--;
				return;
			}


            notifyColor = Application.HasProLicense() ? Color.cyan : (Color.white * .8f);
            GUIStyle notifyStyle = new GUIStyle(GUI.skin.label);
			notifyStyle.fontSize = 20;
			notifyStyle.fontStyle = FontStyle.Bold;
			notifyStyle.alignment = TextAnchor.MiddleCenter;
			notifyStyle.margin = new RectOffset(4, 4, 10, 0);
			
			
			GUI.color = notifyColor;
			Handles.BeginGUI ();
			GUILayout.BeginArea(new Rect((Screen.width*.5f)-150f, (Screen.height*.5f)-150f, 300,300));
			GUILayout.BeginVertical("box");		
			GUILayout.Label (_tmpText, notifyStyle);
			GUILayout.EndVertical ();
			GUILayout.EndArea ();
			Handles.EndGUI ();
			GUI.color = Color.white;
			
			_t++;
			
			if (_t >= _tmpTime - 10f) {
				
				notifyColor.a -= .1f;
				
				if (_t >= _tmpTime) {
					allowNotificationScene = false;
					_t *= 0;
					notifyColor.a = 1f;
				}
			}
			
			
		}

        #endregion


        string CheckMaterialSetup()
        {
            Material _m = WSpawner.GetCurrentMaterial();
            if (_m == null)
                return null;

            if(WSpawner.WaterMaterial != _m)
                WSpawner.WaterMaterial = _m;


            if (!_m.shader.isSupported)
                EditorGUILayout.HelpBox("Shader does not work", MessageType.Error);

            


            if (_m.name.Contains("Refract") || WSpawner.DropObject.name.Contains("Refract"))
            {
                WSpawner.isRefractingMaterial = true;
            }
            else {
                WSpawner.isRefractingMaterial = false;
            }
           
            /*
            bool toFix = false;

                // USING LEGACY // BUILT-IN PIPELINE
           //if (GraphicsSettings.renderPipelineAsset != null && !LWRPMaterial)
             //    toFix = true;


           // if (GraphicsSettings.renderPipelineAsset == null && LWRPMaterial)
            //    toFix = true;

            if (WSpawner.Water2DType == "Regular")
                toFix = false;

            if (WSpawner.Water2DType == "Toon")
                toFix = false;

            // USING LWRP // UNIVERSAL PIPELINE
            if (toFix)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("You current Pipeline do not match with this Water2D material", MessageType.Error);

                EditorGUILayout.BeginVertical("Box");
                if (GUILayout.Button("Fix"))
                {
                    if (GraphicsSettings.renderPipelineAsset == null)
                    {
                        // Legacy repair
                        LegacyRepair();
                    }
                    else {
                        //URP repair
                        URPRepair();

                    }
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            */

            return _m.name;



        }
        /*
        void LegacyRepair()
        {
           
           string _path = EditorUtils.getMainRelativepath();
           UnityEngine.Object dropObj;
           Material m;
           if (WSpawner.isRefractingMaterial)
           {
               dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDropRefractive.prefab", typeof(UnityEngine.Object)) as UnityEngine.Object;
               m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterGrabPass.mat", typeof(Material)) as Material;
           }
           else
           {
               dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDrop.prefab", typeof(UnityEngine.Object)) as UnityEngine.Object;
               m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterAlphaCutOut.mat", typeof(Material)) as Material;
           }

            WSpawner.WaterMaterial = m;
            WSpawner.DropObject = (GameObject)dropObj;
            WSpawner.SetupParticles();
            WSpawner.Water2DRenderType = "Legacy";


           // ResizeMeshEffectController _MeshEffect = GameObject.FindObjectOfType<ResizeMeshEffectController>();

            // Fixing with toon mat
            if (!WSpawner.isRefractingMaterial)
            {
                GameObject cams = null;
                ResizeMeshEffectController _MeshEffect = GameObject.FindObjectOfType<ResizeMeshEffectController>();
                if (!_MeshEffect)
                {


                    UnityEngine.Object cameraSetup = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Cameras.prefab", typeof(UnityEngine.Object)) as UnityEngine.Object;
                    cams = (GameObject)Instantiate(cameraSetup);
                    cams.name = "Cameras";

                }
                else
                {
                    DestroyImmediate(_MeshEffect.gameObject);
                }
                GameObject effectCamera;
                if (cams != null)
                    effectCamera = cams.transform.Find("1-EffectCamera").gameObject;
                else
                {
                    effectCamera = GameObject.Find("1-EffectCamera");

                }

                effectCamera.GetComponent<MetaballCameraEffect>().enabled = true;
                effectCamera.GetComponent<Camera>().targetTexture = null;
               
                effectCamera.GetComponent<MetaballCameraEffect>().Restart();
               
            }
            else {
                // Fixing with refracting mat
                MetaballCameraEffect mce = GameObject.FindObjectOfType<MetaballCameraEffect>();
                if (mce != null)
                    mce.enabled = false;

                Camera.main.orthographic = true;


            }




        }
             

        void URPRepair()
        {
          
            string _path = EditorUtils.getMainRelativepath();
            UnityEngine.Object dropObj = null;
            Material m = null;

            if (WSpawner.Water2DType == "Refracting") { 
             
                    dropObj = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/WaterDropRefractive_URP.prefab", typeof(UnityEngine.Object)) as UnityEngine.Object;
                    m = AssetDatabase.LoadAssetAtPath(_path + "Materials/WaterGrabPass_URP.mat", typeof(Material)) as Material;

            }

            WSpawner.WaterMaterial = m;
            WSpawner.DropObject = (GameObject)dropObj;
            WSpawner.SetupParticles();
            WSpawner.Water2DRenderType = "URP (LWRP)";


            if (WSpawner.Water2DType == "Refracting")
            
            {
                // Fixing with refracting mat
                MetaballCameraEffect mce = GameObject.FindObjectOfType<MetaballCameraEffect>();
                if (mce != null)
                    mce.enabled = false;

                Camera.main.orthographic = true;


            }


        }
         */


    }

}