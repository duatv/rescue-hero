namespace Water2D
{
    using UnityEngine;
    using UnityEditor;



    [ExecuteInEditMode]
    public class MultiColorManager : MonoBehaviour
    {
        // [InitializeOnLoadMethod]
        //public Texture2D Texs;

        static void Initialize(Color _c)
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("Water2D_MultiColorManager");
                if (go == null)
                {
                    MultiColorManager[] aux = FindObjectsOfType<MultiColorManager>();
                    for (int i = 0; i < aux.Length; i++)
                    {
                        DestroyImmediate(aux[i].gameObject);
                    }
                }



                if (go == null)
                {
                    go = new GameObject("Water2D_MultiColorManager");
                    instance = go.AddComponent<MultiColorManager>();
                    go.hideFlags = HideFlags.HideInHierarchy;
                }
                else
                {
                    if (instance == null)
                        instance = go.GetComponent<MultiColorManager>();
                }

                instance._arrayColors = new Color[12];
                instance._arrayCutOffStroke = new float[12];

                //instance._arrayTexture2DColors = new Texture2D[4];



                // Init Color array
                for (int i = 0; i < instance._arrayColors.Length; i++)
                {
                    instance._arrayColors[i] = _c;
                    instance._arrayCutOffStroke[i] = .3f;
                }

                /*
                // Init Texture 2D Color array
                Texture2D _t = new Texture2D(1,4);
                _t.SetPixel(0, 0, _c);
                _t.SetPixel(0, 1, _c);
                _t.SetPixel(0, 2, _c);
                _t.SetPixel(0, 3, _c);
                _t.Apply();
                instance._arrayTexture2DColors = _t;
                */
            }


        }


        public static MultiColorManager instance;
        //Legacy
        Color[] _arrayColors;
        float[] _arrayCutOffStroke;
        //URP
        Texture2D _arrayTexture2DColors;

        Camera fresnelCamera;
        ResizeQuadEffectController quadEffect;

        public static void SetFresnelColor(Color StrokeColor)
        {
            Initialize(StrokeColor);

            if (instance.fresnelCamera == null)
            {
                instance.fresnelCamera = GameObject.Find("1-EffectCamera").GetComponent<Camera>();

            }

            Water2D_Spawner[] otherSpawners = FindObjectsOfType<Water2D_Spawner>();
            for (int i = 0; i < otherSpawners.Length; i++)
            {

                otherSpawners[i].StrokeColor = StrokeColor;
            }

            if (instance.fresnelCamera)
            {
                StrokeColor.a = 0f;
                instance.fresnelCamera.backgroundColor = StrokeColor;
            }
        }

        public static void SetSorting(int sortingID)
        {
            Initialize(Color.white);


            if (instance.quadEffect == null)
            {
                instance.quadEffect = GameObject.Find("EffectQuad").GetComponent<ResizeQuadEffectController>();

            }

            instance.quadEffect.SetSorting(sortingID);
        }


        public static void SetColorScheme(int scheme, Material material, Color fillColor, Color strokeColor, float AlphaCutoff, float AlphaStroke) {

            Initialize(fillColor);
            instance._arrayColors[(scheme - 1) * 2] = fillColor;
            instance._arrayColors[((scheme - 1) * 2) + 1] = strokeColor;

            instance._arrayCutOffStroke[(scheme - 1) * 2] = AlphaCutoff;
            instance._arrayCutOffStroke[((scheme - 1) * 2) + 1] = AlphaStroke;


            material.SetColorArray("_colorArray", instance._arrayColors);
            material.SetFloatArray("_cutOffStrokeArray", instance._arrayCutOffStroke);

        }

        public static void SetColorSchemeURP(int scheme, Material material, Color fillColor, Color strokeColor, float AlphaCutoff, float AlphaStroke)
        {
            /*
            Initialize(fillColor);
            instance._arrayColors[(scheme - 1) * 2] = fillColor;
            instance._arrayColors[((scheme - 1) * 2) + 1] = strokeColor;

            instance._arrayCutOffStroke[(scheme - 1) * 2] = AlphaCutoff;
            instance._arrayCutOffStroke[((scheme - 1) * 2) + 1] = AlphaStroke;
            */


            // Need convert to texture2d before send to shader

            Initialize(fillColor);

            //instance._arrayColors[(scheme - 1) * 2] = fillColor;
            //instance._arrayColors[((scheme - 1) * 2) + 1] = strokeColor;

            //COLOR
            Texture2D _fill = instance._arrayTexture2DColors;
            _fill.filterMode = FilterMode.Point;
            _fill.SetPixel(0, (scheme - 1) * 2, fillColor);
            _fill.SetPixel(0, (scheme - 1) * 2 + 1, strokeColor);
            _fill.Apply();




            //instance.Texs = _fill;
            instance._arrayTexture2DColors = _fill;

            material.SetTexture("_Texture2DColor", instance._arrayTexture2DColors);


        }

        public static MetaballParticleClass[] GetAllParticles()
        {
            if (instance == null)
                Initialize(Color.white);

            instance.fetchAllParticles();
            return instance._allparticles;
        }

        public MetaballParticleClass[] _allparticles;
        public void fetchAllParticles()
        {
            Initialize(Color.white);
            _allparticles = GameObject.FindObjectsOfType<MetaballParticleClass>();

        }


    }
}