namespace Water2D {
	using UnityEngine;
    using UnityEngine.Events;
   	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine.UI;
	//using Apptouch;

#if UNITY_EDITOR
    using UnityEditor;
#endif


    public struct microSpawn{
		public Vector3 pos;
		public int amount;
		public Vector2 initVel;

		public microSpawn(Vector3 pos, int amount, Vector2 initVel)
		{
			this.pos = pos;
			this.amount = amount;
			this.initVel = initVel;
		}
	}

   

    [ExecuteInEditMode]
    [SelectionBase]
    public class Water2D_Spawner : MonoBehaviour
    {

        public enum EmissionType {
            ParticleSystem,
            FillerCollider
        }

        public enum FillerColliderType
        {
            Box,
            Circle,
            Polygon
        }

        public static Water2D_Spawner instance;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        /// <summary>
        /// The type of Water spawner (Regular/Toon/Refracting)).
        /// </summary>
        public string Water2DType = "";

        /// <summary>
        /// The emission type (ParticleSystem/FillerCollider)).
        /// </summary>
        public EmissionType Water2DEmissionType = EmissionType.ParticleSystem;


        /// <summary>
        /// The collider shape type (box, circle, polygon)
        /// </summary>
        public FillerColliderType Water2DFillerType = FillerColliderType.Box;

        /// <summary>
        /// The fill is inverse?
        /// </summary>
        public bool FillerColliderMasked = false;

        /// <summary>
        /// The type of pipeline spawner (Legacy or URP(LWRP)).
        /// </summary>
        public string Water2DRenderType = "";

        /// <summary>
        /// The version of the system.
        /// </summary>
        public string Water2DVersion = "1.2";


        /// <summary>
        /// DropObject
        /// </summary>
        public GameObject DropObject;

        /// <summary>
        /// All WaterDropsObjects.
        /// </summary>
        public GameObject[] WaterDropsObjects;

        /// <summary>
        /// This means that the spawner won't be update in every change of state and you can generate the amount of fluid in Editor for use in Play mode later.
        /// When this property is off, the spawner will be refresh at the end of the application runtime.
        /// </summary>
        public bool PersistentFluid = false;

        /// <summary>
        /// The size of each drop.
        /// </summary>
        //[Range (0f,2f)]
        public float size = .45f;


        /// <summary>
        /// If particle can down the scale over lifetime.
        /// </summary>
        public bool ScaleDown = false;


        /// <summary>
        /// The life time of each particle.
        /// </summary>
        //[Range (0f,100f)]
        public float LifeTime = 5f;


        /// <summary>
        /// The delay between particles emission.
        /// </summary>
        //[Range (0f,.3f)]
        public float DelayBetweenParticles = 0.05f;


        // [Header("Trail")]
        /// <summary>
        /// The Trail size on Start.
        /// </summary>
        //[Range(0f, 2f)]
        public float TrailStartSize = .4f;

        /// <summary>
		/// The Trail size on End.
		/// </summary>
        //[Range(0f, 2f)]
        public float TrailEndSize = .4f;

        /// <summary>
		/// The Trail time between Start - End.
		/// </summary>
        //[Range(0f, 2f)]
        public float TrailDelay = .1f;


        /// <summary>
        /// Actual water material.
        /// </summary>
        public Material WaterMaterial;

        /// <summary>
        /// The sorting order ID
        /// </summary>
        public int Sorting;

        /// <summary>
        /// The color category scheme
        /// </summary>
        public int ColorScheme = 1;

        /// <summary>
        /// Fill Color of the particle [Toon/CutoOut shader]
        /// </summary>
        public Color FillColor = new Color(0f, 112 / 255f, 1f);

        /// <summary>
        /// Stroke Color of the particle [Toon/CutoOut shader]
        /// </summary>
        public Color StrokeColor = new Color(4 / 255f, 156 / 255f, 1f);

        public Color _lastStrokeColor = new Color(4 / 255f, 156 / 255f, 1f);

        /// <summary>
        ///Allow blending colors with neighbor particles
        /// </summary>
        public bool Blending = false;

        public bool _lastBlending = false;


        /// <summary>
        /// Threshold alpha value[Toon/CutoOut shader]
        /// </summary>
        public float AlphaCutOff = .2f;

        /// <summary>
        /// Threshold alpha stroke value [Toon/CutoOut shader]
        /// </summary>
        public float AlphaStroke = .2f;

        /// <summary>
        /// Tint Color of the particle [Refracting shader]
        /// </summary>
        public Color TintColor = new Color(0f, 112 / 255f, 1f);

        /// <summary>
        /// Intensity Color of the particle [Refracting shader]
        /// </summary>
        public float Intensity = .5f;

        /// <summary>
        /// Distortion of UV [Refracting shader]
        /// </summary>
        public float Distortion = .5f;

        /// <summary>
        /// Distortion of UV [Refracting shader]
        /// </summary>
        public float DistortionSpeed = .5f;

        //[Header("Speed & direction")]
        /// <summary>
        /// The initial speed of particles after spawn.
        /// </summary>
        public Vector2 initSpeed = new Vector2(1f, -1.8f);

        /// <summary>
        /// Amount of speed with which the particle is created
        /// </summary>
        public float Speed = 20f;

        /// <summary>
        /// The physic material
        /// </summary>
        public PhysicsMaterial2D PhysicMat;

        /// <summary>
        /// The size of radius / side of collider circle2D or box2D
        /// </summary>
        public float ColliderSize = 1.5f;

        /// <summary>
        /// The size of radius / side of collider circle2D or box2D
        /// </summary>
        public float LinearDrag = 0f;

        /// <summary>
        /// The size of radius / side of collider circle2D or box2D
        /// </summary>
        public float AngularDrag = 0f;

        /// <summary>
        /// The size of radius / side of collider circle2D or box2D
        /// </summary>
        public float GravityScale = 1f;

        /// <summary>
        /// Constrain rotation on Z axis.
        /// </summary>
        public bool FreezeRotation = false;

        /// <summary>
		/// The X speed limit .
		/// </summary>
		public Vector2 SpeedLimiterX = new Vector2(-300, 300);

        /// <summary>
        /// The Y speed limit .
        /// </summary>
        public Vector2 SpeedLimiterY = new Vector2(-300, 300);


        /// <summary>
        /// The simulation start in awake.
        /// </summary>
        public bool SimulateOnAwake = true;

        /// <summary>
        /// Water system can perform in editor mode.
        /// </summary>
        public bool SimulateInEditor = false;

        /// <summary>
        /// Water system can perform in play mode.
        /// </summary>
        public bool SimulateInPlayMode = false;

        /// <summary>
        /// How many particles in the spawner?
        /// </summary>
        public int DropCount = 100;

        public int _lastDropCount = 100;

        /// <summary>
        /// The responsible to spawn every particle once or repeat the spawn forever
        /// </summary>
        public bool Loop = true;


        /// <summary>
        /// Currently amount of particles useing for the simulation (debug purposes only!)
        /// </summary>
        public int DropsUsed;

        /// <summary>
        /// Apply setup changes over lifetime.
        /// </summary>
        public bool DynamicChanges = true;

        /// <summary>
        /// List of events to call when shape will filled with water particles.
        /// </summary>
        public Water2DEvents OnValidateShapeFill;

        /// <summary>
        /// The collider 2d used to check the fill level.
        /// </summary>
        public Collider2D ShapeFillCollider2D;

        /// <summary>
        /// The shape fill accuracy 1 = 100%
        /// </summary>
        public float ShapeFillAccuracy = 1f;
        /// <summary>
        /// List of gameobjects that have been collide with water particles .
        /// </summary>
        public Water2DEvents OnCollisionEnterList;

        /// <summary>
        ///  List of gameobjects that will be notified when spawner is about to start .
        /// </summary>
        public Water2DEvents OnSpawnerAboutStart;

        /// <summary>
        ///  List of gameobjects that will be notified when spawner is about to end .
        /// </summary>
        public Water2DEvents OnSpawnerAboutEnd;

        /// <summary>
        ///  List of gameobjects that will be notified when spawner is emitting each particle .
        /// </summary>
        public Water2DEvents OnSpawnerEmitingParticle;


        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.5f);
            if (Application.isPlaying && SimulateOnAwake && Water2DEmissionType == EmissionType.ParticleSystem)
            {
                Restore();
                yield return new WaitForEndOfFrame();
                Spawn();
            }
            else {
                yield return new WaitForEndOfFrame();
                StartCoroutine(UpdateQuietParticleProperties());
            }
            yield return null;
        }

        static void RunSpawner()
		{
            instance.Spawn();
        }
       
        static void StopSpawner()
        {
            instance.Restore();
        }

		public int AllBallsCount{ get; private set;}
		public bool IsSpawning{ get; private set;}

        public bool isRefractingMaterial = false;

        int usableDropsCount;
		int DefaultCount;


		bool _breakLoop = false;

		GameObject _parent;
        string _parentNameID = "Water2DParticlesID_";
       


        public void SetupParticles()
        {
            if (_parent == null && WaterDropsObjects != null)
            {
                if (WaterDropsObjects.Length > 0 && WaterDropsObjects[0] != null)
                    _parent = WaterDropsObjects[0].transform.parent.gameObject;
            }
            if(_parent != null){
               
                DestroyImmediate(_parent);
            }
            _parent = new GameObject(_parentNameID + gameObject.GetInstanceID());
            _parent.transform.hideFlags = HideFlags.HideInHierarchy;

            WaterDropsObjects = new GameObject[DropCount];

            for (int i = 0; i < WaterDropsObjects.Length; i++)
            {
                WaterDropsObjects[i] = Instantiate(DropObject, gameObject.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
                
                WaterDropsObjects[i].GetComponent<MetaballParticleClass>().Active = false;
                WaterDropsObjects[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                WaterDropsObjects[i].transform.SetParent(_parent.transform);
                WaterDropsObjects[i].transform.localScale = new Vector3(size, size, 1f);
                WaterDropsObjects[i].layer = WaterDropsObjects[0].layer;
               
                //Set tex color for scheme selection
                Color ColorTex = Color.white;

                if (ColorScheme == 1)
                    ColorTex = new Color(1f,0f,0f);
                if (ColorScheme == 2)
                    ColorTex = new Color(0f, 1f, 0f);
                if (ColorScheme == 3)
                    ColorTex = new Color(0f, 0f, 1f);
               /* if (ColorScheme == 4)
                    ColorTex = new Color(1f, 1f, 0f);
                if (ColorScheme == 5)
                    ColorTex = new Color(0f, 1f, 1f);
                if (ColorScheme == 6)
                    ColorTex = new Color(1f, 1f, 1f);
                */

                if(Water2DType == "Regular" || Water2DType == "Refracting") 
                    ColorTex = FillColor;

                WaterDropsObjects[i].GetComponent<SpriteRenderer>().color = ColorTex;
                WaterDropsObjects[i].GetComponent<TrailRenderer>().startColor = ColorTex;
                WaterDropsObjects[i].GetComponent<TrailRenderer>().endColor = ColorTex;

                WaterDropsObjects[i].GetComponent<MetaballParticleClass>().BlendingColor = Blending;

                TrailRenderer tr = WaterDropsObjects[i].GetComponent<TrailRenderer>();
                if (TrailStartSize <= 0f) {
                    tr.enabled = false;
                }
                else {
                    tr.enabled = true;
                    tr.startWidth = TrailStartSize;
                    tr.endWidth = TrailEndSize;
                    tr.time = TrailDelay;
                }
                WaterDropsObjects[i].GetComponent<MetaballParticleClass>().SpawnerParent = this;
            }

            AllBallsCount = WaterDropsObjects.Length;

            if(Water2DEmissionType == EmissionType.ParticleSystem)
            {
                DropsUsed *= 0;
                _spawnedDrops *= 0;
            }

            RestoreCheckingFillShape(); // restore events
        }

#if UNITY_EDITOR

        private void OnEnable()
        {
            EditorApplication.update += OnEditorUpdate;
            auxTime = Time.realtimeSinceStartup;
            auxTime += DelayBetweenParticles;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        float auxTime;
        float NextTick;
        private void OnEditorUpdate()
        {
            if (DropObject == null)
                return;

            if (WaterDropsObjects == null || WaterDropsObjects.Length < 1)
            {
                SetupParticles();
            }

            if (WaterDropsObjects[0] == null)
            {
                SetupParticles();
            }

            if (EditorApplication.timeSinceStartup >= NextTick)
            {
                //tick
                NextTick = (float)EditorApplication.timeSinceStartup + DelayBetweenParticles;
                loop_editor(gameObject.transform.position, initSpeed);

                if (Selection.activeGameObject == gameObject)
                {
                    //print("change color");
                    //CHANGE COLOR
                    if (Water2DType == "Refracting")
                    {
                        SetRefractingWaterparams(Intensity, Distortion, DistortionSpeed);
                    }
                    else if(Water2DType == "Toon")
                    {
                        SetToonWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                    }
                    else if (Water2DType == "Regular")
                    {
                        SetRegularWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                    }
                }
            }
            //else
            //{
               
            //}

           
            //Check ShapeFill events
            if(OnValidateShapeFill?.GetPersistentEventCount() > 0 && ShapeFillCollider2D != null)
            {
                StartCheckingFillShape();
            }
        }
#endif


		public void Spawn(){
			Spawn (DefaultCount);
		}

		public void Spawn(int count){
            if (DelayBetweenParticles == 0f)
            {
                DropsUsed *= 0;
                SpawnAll();
            }
            else {
                DropsUsed *= 0;
                StartCoroutine(loop(gameObject.transform.position, initSpeed, count));
            }
		}

        public void SpawnAll() {
            SpawnAllParticles(gameObject.transform.position, initSpeed, DefaultCount);
        }

		public void Spawn(int count, Vector3 pos){
		
			StartCoroutine (loop(pos, initSpeed, count));
		}

		public void Spawn(int count, Vector3 pos, Vector2 InitVelocity, float delay = 0f){
			
			StartCoroutine (loop(pos, InitVelocity, count, delay));
		}

		
        public void StopSpawning()
        {
            _breakLoop = true;
            IsSpawning = false;
        }

		public void Restore()
		{
			IsSpawning = false;
			_breakLoop = true;
            DropsUsed *= 0;
            
			for (int i = 0; i < WaterDropsObjects.Length; i++) {

                if (WaterDropsObjects[i] != null) {
                    if (WaterDropsObjects[i].GetComponent<MetaballParticleClass>().Active == true)
                    {
                        WaterDropsObjects[i].GetComponent<MetaballParticleClass>().Active = false;
                    }
                    WaterDropsObjects[i].GetComponent<MetaballParticleClass>().witinTarget = false;
                }
			}

			//gameObject.transform.localEulerAngles = Vector3.zero;
			//initSpeed = new Vector2 (0, -2f);

			DefaultCount = AllBallsCount;
			usableDropsCount = DefaultCount;
			//Dynamic = false;
		}

        int _spawnedDrops = 0;
        Color _lastFillColor;
        void loop_editor(Vector3 _pos, Vector2 _initSpeed, int count = -1, float delay = 0f, bool waitBetweenDropSpawn = true)
        {
            if (Application.isPlaying)
                return;

            if (Water2DEmissionType == EmissionType.FillerCollider)
            {
                //Debug.LogError("You're trying spawn particles in a Filler type. You should create a water spawner instead");
                return;
            }

            if (!SimulateInEditor)
                return;

            if (WaterDropsObjects == null || WaterDropsObjects.Length < 1) {
                SetupParticles();
                return;
            }


            for (int i = 0; i < WaterDropsObjects.Length; i++)
            {
                if (WaterDropsObjects[i] == null)
                    return;

                MetaballParticleClass MetaBall = WaterDropsObjects[i].GetComponent<MetaballParticleClass>();

                //CHANGE COLOR
                if (Water2DType == "Refracting")
                {
                    if (MetaBall.Active == false)
                        MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;

                    SetRefractingWaterparams(Intensity, Distortion, DistortionSpeed);
                }
                else if (Water2DType == "Toon")
                {
                    SetToonWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                }
                else if (Water2DType == "Regular")
                {   
                    if (MetaBall.Active == false)
                        MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;

                    SetRegularWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                }

                if (MetaBall.Active == true)
                    continue;

                _canInvokeAttheEnd = true;

                if (LifeTime <= 0) {
                    MetaBall.LifeTime = -1f;
                }
                else {
                    MetaBall.LifeTime = LifeTime;
                }
               
                WaterDropsObjects[i].transform.position = transform.position;
                MetaBall.Active = true;
                MetaBall.witinTarget = false;

                if (_initSpeed == Vector2.zero)
                    _initSpeed = initSpeed;

                if (true)
                {
                    _initSpeed = initSpeed;
                    MetaBall.transform.localScale = new Vector3(size, size, 1f);

                    TrailRenderer tr = WaterDropsObjects[i].GetComponent<TrailRenderer>();
                    if (TrailStartSize <= 0f)
                    {
                        tr.enabled = false;
                    }
                    else
                    {
                        tr.enabled = true;
                        tr.startWidth = TrailStartSize;
                        tr.endWidth = TrailEndSize;
                        tr.time = TrailDelay;
                    }

                    MetaBall.Velocity_Limiter_X = SpeedLimiterX;
                    MetaBall.Velocity_Limiter_Y = SpeedLimiterY;

                    Rigidbody2D rb = MetaBall.GetComponent<Rigidbody2D>();
                    rb.sharedMaterial = PhysicMat;
                    rb.drag = LinearDrag;
                    rb.angularDrag = AngularDrag;
                    rb.gravityScale = GravityScale;

                    if (FreezeRotation)
                        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                    MetaBall.GetComponent<CircleCollider2D>().sharedMaterial = PhysicMat;
                    MetaBall.GetComponent<CircleCollider2D>().radius = ColliderSize;

                    MetaBall.ScaleDown = ScaleDown;

                    //CHANGE COLOR
                    if (Water2DType == "Refracting")
                    {
                        if (MetaBall.Active == false)
                            MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;

                        SetRefractingWaterparams(Intensity, Distortion, DistortionSpeed);
                    }
                    else if (Water2DType == "Toon")
                    {
                        SetToonWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                    }
                    else if (Water2DType == "Regular")
                    {
                        _lastFillColor = FillColor;
                        if (MetaBall.Active == false)
                            MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;

                        SetRegularWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                    }
                }

                Vector2 dir = transform.localRotation * Vector2.down;
                MetaBall.GetComponent<Rigidbody2D>().velocity = dir * Speed;
               
                DropsUsed++;
                _spawnedDrops++;

                //Invoke event
                InvokeOnSpawnerEmittinEachParticle(gameObject);

                if (_spawnedDrops >= DropCount)
                {
                    if(!Loop)
                        SimulateInEditor = false;

                    //Invoke event End
                    if (_canInvokeAttheEnd)
                    {
                        InvokeOnSpawnerEnd(gameObject);
                        _canInvokeAttheEnd = false;
                    }
                    _spawnedDrops *= 0;
                }
                return;
            }
        }

        bool _canInvokeAttheEnd = true;
        IEnumerator loop(Vector3 _pos, Vector2 _initSpeed, int count = -1, float delay = 0f, bool waitBetweenDropSpawn = true){

            if (IsSpawning)
                yield break;

            if (Water2DEmissionType == EmissionType.FillerCollider)
            { Debug.LogError("You're trying spawn particles in a Filler type. You should create a water spawner instead"); yield break; }

            Physics2D.autoSimulation = true;

            IsSpawning = true;

            yield return new WaitForSeconds (delay);

			_breakLoop = false;

			
			//int auxCount = 0;

            //Invoke event Start
            InvokeOnSpawnerStart(gameObject);

            while (true) {
				for (int i = 0; i < WaterDropsObjects.Length; i++) {
					if (_breakLoop)
						yield break;

					MetaballParticleClass MetaBall = WaterDropsObjects [i].GetComponent<MetaballParticleClass> ();

					if (MetaBall.Active == true)
						continue;

                    _canInvokeAttheEnd = true; 

                    if (LifeTime <= 0)
                    {
                        MetaBall.LifeTime = -1f;
                    }
                    else
                    {
                        MetaBall.LifeTime = LifeTime;
                    }

                    WaterDropsObjects [i].transform.position = transform.position;
					
					if (_initSpeed == Vector2.zero)
						_initSpeed = initSpeed;

					if (DynamicChanges) {
						_initSpeed = initSpeed;
						MetaBall.transform.localScale = new Vector3 (size, size, 1f);

                        //CHANGE COLOR
                        if (Water2DType == "Refracting")
                        {
                            SetRefractingWaterparams(Intensity, Distortion, DistortionSpeed);
                           
                            if (MetaBall.Active == false)
                                MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;
                        }
                        else if (Water2DType == "Toon")
                        {
                            SetToonWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                        }
                        else if (Water2DType == "Regular")
                        {
                            _lastFillColor = FillColor;
                            if (MetaBall.Active == false)
                                MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;

                            SetRegularWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                        }

                        TrailRenderer tr = WaterDropsObjects[i].GetComponent<TrailRenderer>();
                        if (TrailStartSize <= 0f)
                        {
                            tr.enabled = false;
                        }
                        else
                        {
                            tr.enabled = true;
                            tr.startWidth = TrailStartSize;
                            tr.endWidth = TrailEndSize;
                            tr.time = TrailDelay;
                        }
                        
                        MetaBall.Velocity_Limiter_X = SpeedLimiterX;
                        MetaBall.Velocity_Limiter_Y = SpeedLimiterY;

                        Rigidbody2D rb = MetaBall.GetComponent<Rigidbody2D>();
                        rb.sharedMaterial = PhysicMat;
                        rb.drag = LinearDrag;
                        rb.angularDrag = AngularDrag;
                        rb.gravityScale = GravityScale;

                        if (FreezeRotation)
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                        MetaBall.GetComponent<CircleCollider2D>().sharedMaterial = PhysicMat;
                        MetaBall.GetComponent<CircleCollider2D>().radius = ColliderSize;
                        MetaBall.ScaleDown = ScaleDown;
                    }

                    MetaBall.Active = true;
                    MetaBall.witinTarget = false; 
                    
                    //WaterDropsObjects [i].GetComponent<Rigidbody2D> ().velocity = _initSpeed;
                    Vector2 dir = transform.localRotation * Vector2.down;
                    MetaBall.GetComponent<Rigidbody2D>().velocity = dir * Speed;

                    DropsUsed++;
                    _spawnedDrops++;


                    /*
                    // Count limiter
                    if (count > -1) {
						auxCount++;
						if (auxCount >= count && !Loop) {
							yield break;
						} 
					}

                    */

                    //print(Time.fixedDeltaTime / DelayBetweenParticles);
					if(waitBetweenDropSpawn)
						yield return new WaitForSeconds (DelayBetweenParticles);

                    //Invoke event
                    InvokeOnSpawnerEmittinEachParticle(gameObject);
                }
				yield return new WaitForEndOfFrame ();

                if (_spawnedDrops >= DropCount)
                {
                    //Invoke event End
                    if (_canInvokeAttheEnd)
                    {
                        InvokeOnSpawnerEnd(gameObject);
                        _canInvokeAttheEnd = false;
                    }

                    _spawnedDrops *= 0;

                    if (!Loop)
                        yield break;
                }
			}
		}

        /// <summary>
        /// Spawn all particles together at the same time
        /// </summary>
        /// <param name="_pos"></param>
        /// <param name="_initSpeed"></param>
        /// <param name="count"></param>
        /// <param name="delay"></param>
        void SpawnAllParticles(Vector3 _pos, Vector2 _initSpeed, int count = -1, float delay = 0f)
        {
            IsSpawning = true;

            int auxCount = 0;
           // while (true)
            //{
                for (int i = 0; i < WaterDropsObjects.Length; i++)
                {
                    MetaballParticleClass MetaBall = WaterDropsObjects[i].GetComponent<MetaballParticleClass>();

                    if (MetaBall.Active == true)
                        continue;

                    MetaBall.LifeTime = LifeTime;
                    WaterDropsObjects[i].transform.position = transform.position;
                    MetaBall.Active = true;
                    MetaBall.witinTarget = false;

                    if (_initSpeed == Vector2.zero)
                        _initSpeed = initSpeed;

                    if (DynamicChanges)
                    {
                        _initSpeed = initSpeed;
                        MetaBall.transform.localScale = new Vector3(size, size, 1f);
                    //CHANGE COLOR
                    if (Water2DType == "Refracting")
                    {
                        SetRefractingWaterparams(Intensity, Distortion, DistortionSpeed);
                      
                        if (MetaBall.Active == false)
                            MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;
                    }
                    else if (Water2DType == "Toon")
                    {
                        SetToonWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                    }
                    else if (Water2DType == "Regular")
                    {
                        //_lastFillColor = FillColor;
                        if (MetaBall.Active == false)
                            MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;

                        SetRegularWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                    }
                }

                WaterDropsObjects[i].GetComponent<Rigidbody2D>().velocity = _initSpeed;
                
                    // Count limiter
                    if (count > -1)
                    {
                        auxCount++;
                        if (auxCount >= count && !Loop)
                        {
                            break;
                        }
                    }
                }
                //alreadySpawned = true;
           // }
        }

        public void InvokeOnShapeFill(GameObject obj, GameObject results)
        {
            OnValidateShapeFill?.Invoke(obj, null);
        }

        public void InvokeOnCollisionEnter2D(GameObject obj, GameObject other)
        {
            OnCollisionEnterList.Invoke(obj, other);
        }

        public void InvokeOnSpawnerStart(GameObject obj)
        {
            if(OnSpawnerAboutStart != null)
                OnSpawnerAboutStart.Invoke(obj, null);
        }

        public void InvokeOnSpawnerEnd(GameObject obj)
        {
            OnSpawnerAboutEnd.Invoke(obj, null);
        }

        public void InvokeOnSpawnerEmittinEachParticle(GameObject obj)
        {
            OnSpawnerEmitingParticle.Invoke(obj, null);
        }



        Camera fresnelCamera;
        int _lastSorting;
        public void SetRegularWaterparams(Color fill, Color fresnel, float alphaCutoff, float multiplier)
        {
            //WaterMaterial.SetColor("_StrokeColor", stroke);
            WaterMaterial.SetFloat("_constant", multiplier);
            WaterMaterial.SetFloat("_botmcut", alphaCutoff);
           
            // I do use Stroke as Fresnel
            // Fresnel is a shared property since I've use camera to reach that effect
            if(_lastStrokeColor != StrokeColor) {
                _lastStrokeColor = StrokeColor;
                MultiColorManager.SetFresnelColor(StrokeColor);
            }

            if (_lastSorting != Sorting)
            {
                _lastSorting = Sorting;
                MultiColorManager.SetSorting(Sorting);
            }
        }

        public void SetToonWaterparams(Color fill, Color stroke, float alphaCutoff, float alphaStroke)
		{
			WaterMaterial.SetColor ("_Color", fill);
            WaterMaterial.SetColor ("_StrokeColor", stroke);
            WaterMaterial.SetFloat("_Cutoff", alphaCutoff);
            WaterMaterial.SetFloat("__Cutoff", alphaCutoff);
            WaterMaterial.SetFloat("_Stroke", alphaStroke);
            
            MultiColorManager.SetColorScheme(ColorScheme, WaterMaterial, fill,stroke, alphaCutoff, alphaStroke);

            if (_lastSorting != Sorting)
            {
                _lastSorting = Sorting;
                MultiColorManager.SetSorting(Sorting);
            }
        }

        public void SetRefractingWaterparams(float intensity, float mag, float speed)
        {
            //WaterMaterial.SetColor("_TintColor", tint);
            WaterMaterial.SetFloat("_AmountOfTintColor", intensity);
            WaterMaterial.SetFloat("_Mag", mag*10f);
            WaterMaterial.SetFloat("_Speed", speed*10f);

            // I do use Stroke as Fresnel
            // Fresnel is a shared property since I've use camera to reach that effect
            if (_lastStrokeColor != StrokeColor)
            {
                _lastStrokeColor = StrokeColor;
                MultiColorManager.SetFresnelColor(StrokeColor);
            }

            if (_lastSorting != Sorting)
            {
                _lastSorting = Sorting;
                MultiColorManager.SetSorting(Sorting);
            }
        }

        IEnumerator UpdateQuietParticleProperties()
        {
            while (true)
            {
                for (int i = 0; i < WaterDropsObjects.Length; i++)
                {
                    MetaballParticleClass MetaBall = WaterDropsObjects[i].GetComponent<MetaballParticleClass>();
                    //CHANGE COLOR
                    if (Water2DType == "Refracting")
                    {
                        SetRefractingWaterparams(Intensity, Distortion, DistortionSpeed);

                        if (MetaBall.Active == false)
                            MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;
                    }
                    else if (Water2DType == "Toon")
                    {
                        SetToonWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                    }
                    else if (Water2DType == "Regular")
                    {
                        _lastFillColor = FillColor;
                        if (MetaBall.Active == false)
                            MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;

                        SetRegularWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                    }

                    MetaBall.Velocity_Limiter_X = SpeedLimiterX;
                    MetaBall.Velocity_Limiter_Y = SpeedLimiterY;
                    
                    Rigidbody2D rb = MetaBall.GetComponent<Rigidbody2D>();
                    rb.sharedMaterial = PhysicMat;
                    rb.drag = LinearDrag;
                    rb.angularDrag = AngularDrag;
                    rb.gravityScale = GravityScale;

                    if (FreezeRotation)
                        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                    MetaBall.GetComponent<CircleCollider2D>().sharedMaterial = PhysicMat;
                    MetaBall.GetComponent<CircleCollider2D>().radius = ColliderSize;
                }
                yield return null;
            }
        }


        public Material GetCurrentMaterial()
        {
            /*
            if (DropObject == null)
                return null;

            if (DropObject.GetComponent<SpriteRenderer>() == null)
                return null;

            Material mat = DropObject.GetComponent<SpriteRenderer>().sharedMaterial;
            if (mat)
                return mat;
            else
                return null;
                */

            return WaterMaterial;
        }

        void StartCheckingFillShape()
        { 
            if(!_checkOnFillRunning && !_checkOnFillComplete)
            {
                StartCoroutine(CheckOnFill(ShapeFillCollider2D, ShapeFillAccuracy));
            }
        }

        void RestoreCheckingFillShape()
        {
            StartCoroutine(_restoreCheckingFillShapeEnum());   
        }
        IEnumerator _restoreCheckingFillShapeEnum()
        {
            yield return new WaitForSeconds(.2f);
            _breakCheckOnFill = true;
            _checkOnFillComplete = false;
        }


        bool _checkOnFillRunning = false;
        bool _breakCheckOnFill = false;
        bool _checkOnFillComplete = false;
        IEnumerator CheckOnFill(Collider2D shapeCollider, float accuracy = .8f)
        {
            _checkOnFillRunning = true;

            ContactFilter2D cf = new ContactFilter2D();
            cf.useTriggers = true;
            cf.SetLayerMask(Physics2D.GetLayerCollisionMask(DropObject.layer));
            cf.useLayerMask = true;

            Collider2D[] allOverlappingColliders = new Collider2D[DropCount];
            
            int result = 0;
            
            while (true)
            {
                if(_breakCheckOnFill)
                {
                    _checkOnFillRunning = false;
                    _breakCheckOnFill = false;
                    yield break;
                }

                yield return new WaitForFixedUpdate();
                result = shapeCollider.OverlapCollider(cf, allOverlappingColliders);

                //Debug.Log("currently: " + result);

                bool _trigged = false;

                if (Water2DEmissionType == EmissionType.FillerCollider)
                {
                    _trigged = (result >= DropsUsed * accuracy);
                }
                else
                {
                    _trigged = (result >= DropCount * accuracy);
                }

                if (_trigged) {

                    InvokeOnShapeFill(instance.gameObject, null);
                    Debug.Log("Fill Event sucessful Complete! : droplives:" + DropsUsed +"   // "  + ((int)(DropsUsed*accuracy)).ToString() + "within the target");
                    _checkOnFillComplete = true;
                    _breakCheckOnFill = true;
                }
            }
        }


        private void OnDestroy()
        {
            DestroyImmediate(_parent);
        }
    }
}