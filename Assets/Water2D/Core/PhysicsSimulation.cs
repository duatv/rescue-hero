#if UNITY_EDITOR
namespace Water2D
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEditor.SceneManagement;
    using UnityEditor;
    using System.Collections.Generic;

    [ExecuteInEditMode]

    public class PhysicsSimulation : MonoBehaviour
    {
        static void Initialize()
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("physim_Water2d");
                if(go == null) {
                    PhysicsSimulation []aux = FindObjectsOfType<PhysicsSimulation>();
                    for (int i = 0; i < aux.Length; i++)
                    {
                        DestroyImmediate(aux[i].gameObject);
                    }
                }



                if (go == null)
                {
                    go = new GameObject("physim_Water2d");
                    instance = go.AddComponent<PhysicsSimulation>();
                }
                else
                {
                    if (instance == null)
                        instance = go.GetComponent<PhysicsSimulation>();
                }
            }
            else {
                instance.Simulate = true;
            }
               
        }

        
       
        private void Awake()
        {
           

        }



        public static void Run()
        {

            for (int i = 0; i < 500; i++)
            {
                //waiting time
            }

            if (Application.isPlaying)
            {
                print("exit because playing");
                Stop();
                return;
            }
                


            if (instance == null)
            {
                Initialize();
                instance.Create();
            }

            instance.OnRun();


        }

        public static void Stop()
        {
            if (instance != null)
            {

                instance.OnStop();
            }

        }


        public static PhysicsSimulation instance;

      

        public bool Simulate = false;
        bool _LastSimulateState = false;

        PhysicsScene2D CurrentPhysicsScene;
        float timer1 = 0;

        [HideInInspector]public List<Rigidbody2D> RBAltered;

        private void OnRun()
        {

            if (Simulate)
                return;

            //Unity Simulate OFF
            Physics2D.autoSimulation = false;

            ExcludeRB2D();


            Simulate = true;
            EditorApplication.update += UpdatePhysics;


        }
        private void OnStop()
        {

           
            Simulate = false;
            EditorApplication.update -= UpdatePhysics;
            BackToNormalRB2D();

            //Unity Simulate ON
            Physics2D.autoSimulation = true;
        }

       [HideInInspector]public bool alreadyCreated = false;
        void Create()
        {
            
            Scene scene = EditorSceneManager.GetActiveScene();//EditorSceneManager.CreateScene("MyScene1", csp);
            CurrentPhysicsScene = scene.GetPhysicsScene2D();

            RBAltered = new List<Rigidbody2D>(10);

            alreadyCreated = true;

        }

        //remove all rb2d from simulation but metaballs
        void ExcludeRB2D()
        {

           // if (RBAltered == null)
             //   RBAltered = new List<Rigidbody2D>(10);

            Rigidbody2D[] rb = FindObjectsOfType<Rigidbody2D>();
            for (int i = 0; i < rb.Length; i++)
            {
                if (rb[i].gameObject.tag == "Metaball_liquid")
                    continue;

                if (rb[i].bodyType == RigidbodyType2D.Static)
                    continue;

                if (rb[i].bodyType == RigidbodyType2D.Kinematic)
                    continue;

                rb[i].bodyType = RigidbodyType2D.Static;

                // trace object


                RBAltered.Add(rb[i]);
                //print("in: " + rb[i].gameObject.name);
            }


        }
        void BackToNormalRB2D()
        {
            if (RBAltered == null)
            {
                print("faile pop rb");
                return;
            }


            for (int i = 0; i < RBAltered.Count; i++)
            {
                if (RBAltered[i] != null)
                {
                    RBAltered[i].bodyType = RigidbodyType2D.Dynamic;
                    //print("out: " + RBAltered[i].gameObject.name);
                }
            }

            RBAltered.Clear();


        }


        void UpdatePhysics()
        {


            if (Application.isPlaying)
                return;



            if (_LastSimulateState != Simulate)
            {
                _LastSimulateState = Simulate;

                if (!alreadyCreated)
                {
                    Create();

                }

            }

            if (Simulate)
            {
               
                timer1 += Time.deltaTime;


                if (CurrentPhysicsScene != null && CurrentPhysicsScene.IsValid())
                {
                    while (timer1 >= Time.fixedDeltaTime)
                    {
                        timer1 -= Time.fixedDeltaTime;
                        if(!Physics2D.autoSimulation)
                            CurrentPhysicsScene.Simulate(Time.fixedDeltaTime);
                    }
                }

            }


        }

    }
}
#endif
