using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class MetaballParticleClass : MonoBehaviour
{
    public GameObject MObject;
    public float LifeTime;
    public Water2D.Water2D_Spawner SpawnerParent;
    public bool Active
    {
        get { return _active; }
        set
        {
            _active = value;
            if (MObject)
            {
                MObject.SetActive(value);

                if (tr)
                    tr.Clear();
            }

            if (value)
            {
                delta *= 0;
                wakeUpTime = Time.time;
            }

            if (!value)
            {
                //substrac 1 unit of particle using.
                if(SpawnerParent)
                    SpawnerParent.DropsUsed--;

                if (rb != null) // reset speed simulation in Editor
                    rb.velocity *= 0f;

                delta *= 0;
            }
            ScaleDownIsPerforming = false;
        }
    }
    public bool witinTarget;

    public Vector2 Editor_Velocity; // velocity used within editor simulation
    public Vector2 Velocity_Limiter_X; // Limiter of speed in X in simulation
    public Vector2 Velocity_Limiter_Y; // Limiter of speed in X in simulation

    public bool ScaleDown = false;
    public float endSize = 0f;
    public bool BlendingColor;


    bool _active;
    float delta;
    Rigidbody2D rb;
    CircleCollider2D cc;
    TrailRenderer tr;
    SpriteRenderer sr;
    Collider2D[] Contacts;
    float deltaSimul;
    float fixedDeltaSimul = 0f;
    float wakeUpTime;



    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.update += Update;
#endif
        if (SpawnerParent)
        {
            gameObject.name = SpawnerParent.name + "_" + SpawnerParent.DropsUsed;
        }
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= Update;
#endif
    }

    void Start()
    {
        //MObject = gameObject;
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        cc = GetComponent<CircleCollider2D>();
        sr = GetComponent <SpriteRenderer>();

        Contacts = new Collider2D[4];
    }

    void Update()
    {
        if (Active == true)
        {
            if(ScaleDown)
                ScaleItDown();

            VelocityLimiter();

            if (BlendingColor) {
               
                Blend();
            }

            if(SpawnerParent.Water2DEmissionType == Water2D.Water2D_Spawner.EmissionType.FillerCollider)
                return;

            if (LifeTime < 0)
                return;

            if (delta > LifeTime)
            {
                delta *= 0;
                Active = false;
            }
            else
            {
                delta += Time.deltaTime;
            }

            // Handle collisions in fixedtime only editor
            if (deltaSimul > fixedDeltaSimul)
            {
                deltaSimul *= 0;
                OnCollisionEnter2DEditor();
            }
            else
            {
                deltaSimul += Time.deltaTime;
            }
        }
    }

    Vector3 pos_aux;
    void AnimInEditor()
    {
        Editor_Velocity += Physics2D.gravity * .0001f;
       // VelocityLimiterEditor();
        pos_aux = transform.position;
        pos_aux = (Vector2)pos_aux + Editor_Velocity;
        transform.position = pos_aux;
        //print(Time.deltaTime);
    }

    void VelocityLimiter()
    {
        if (rb == null)
            return;

        Vector2 _vel = rb.velocity;
        _vel = rb.velocity;

        if (_vel.x < Velocity_Limiter_X.x)
        {
            _vel.x = Velocity_Limiter_X.x;
        }

        if (_vel.x > Velocity_Limiter_X.y)
        {
            _vel.x = Velocity_Limiter_X.y;
        }

        if (_vel.y < Velocity_Limiter_Y.x)
        {
            _vel.y = Velocity_Limiter_Y.x;
        }

        if (_vel.y > Velocity_Limiter_Y.y)
        {
            _vel.y = Velocity_Limiter_Y.y;
        }

        rb.velocity = _vel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SpawnerParent.InvokeOnCollisionEnter2D(gameObject, collision.contacts[0].collider.gameObject);
    }

   // private void OnCollisionExit2D(Collision2D collision)
   //{
       // SpawnerParent.InvokeOnCollisionEnter2D(gameObject);
   // }

    private void OnCollisionEnter2DEditor()
    {
        if (Application.isPlaying)
            return;

        if (cc == null)
            return;

        if (Contacts == null)
            return;

        int i = Physics2D.OverlapCircleNonAlloc(rb.position, cc.radius * .9f, Contacts);

        if (i > 0)
        {
            for (int j = 0; j < Contacts.Length; j++)
            {
                if (Contacts[j] == null)
                    continue;

                if (Contacts[j].GetInstanceID() == gameObject.GetInstanceID())
                    continue;

                SpawnerParent.InvokeOnCollisionEnter2D(gameObject, Contacts[j].gameObject);
            }
        }
    }

    bool ScaleDownIsPerforming = false;
    Vector2 deltaScale;
    Vector2 initScale;
    Vector2 tmpScale;
    void ScaleItDown()
    {
        // Initializes
        if (!ScaleDownIsPerforming) {
            ScaleDownIsPerforming = true;
        }

        if(ScaleDownIsPerforming)
            ScaleDownPerform();
    }
    void ScaleDownPerform()
    {
         transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(endSize, endSize), (delta /LifeTime) * Time.deltaTime * .8f );
        //print(Time.deltaTime);
        //elapsedTime += Time.deltaTime;
    }


    int breathFrames = 6;
    int framesCount = 0;


    void Blend()
    {
        if (true)
        {
            if (cc == null)
                return;

            if (Contacts == null)
            {
                Contacts = new Collider2D[2];
            }

            if(framesCount < breathFrames)
            {
                framesCount++;
                return;
            }

            framesCount *= 0;
            //print("try blending");
            //mix only in movement

            if (rb.velocity.sqrMagnitude < 0.000001f)
              return;

            int i = Physics2D.OverlapCircleNonAlloc(rb.position, cc.radius * .3f, Contacts, 1 << gameObject.layer);

            if (i > 0)
            {
                for (int j = 0; j < Contacts.Length; j++)
                {
                    if (Contacts[j] == null)
                        continue;

                    if (!Contacts[j].gameObject.activeSelf)
                        continue;

                    if (Contacts[j].GetInstanceID() == gameObject.GetInstanceID())
                        continue;

                    if (Contacts[j].tag != "Metaball_liquid")
                        continue;

                    //print("blending");
                    Color c2 = Contacts[j].GetComponent<SpriteRenderer>().color;

                    if (c2 == sr.color)
                        return;

                    //print("mixing");

                    sr.color = Color.Lerp(sr.color, c2, .025f);
                    if (Contacts[j].GetComponent<MetaballParticleClass>().SpawnerParent.Blending) {
                        Contacts[j].GetComponent<SpriteRenderer>().color = Color.Lerp(c2, sr.color, .025f);
                    }   
                }
            }
        }
    }

    void Blend2()
    {
        if (true)
        {

            if (cc == null)
                return;

            if (framesCount < breathFrames)
            {
                framesCount++;
                return;
            }

            framesCount *= 0;

            //mix only in movement

            if (rb.velocity.sqrMagnitude < 0.00000001f)
                return;

            int c = 0;
            int ContactsMax = 60;
            MetaballParticleClass []_contacts = new MetaballParticleClass[ContactsMax];

            if (Water2D.MultiColorManager.instance == null)
                Water2D.MultiColorManager.GetAllParticles();
            

            if (Water2D.MultiColorManager.instance._allparticles == null)
                return;

            for (int i = 0; i < Water2D.MultiColorManager.instance._allparticles.Length; i++)
            {
                if (c >= ContactsMax)
                    break;

                if (Water2D.MultiColorManager.instance._allparticles[i].gameObject.GetHashCode() == gameObject.GetHashCode())
                    continue;

                if (!Water2D.MultiColorManager.instance._allparticles[i].gameObject.activeSelf)
                    continue;

                if (Water2D.MultiColorManager.instance._allparticles[i].gameObject.tag != "Metaball_liquid")
                    continue;

                if ((Water2D.MultiColorManager.instance._allparticles[i].transform.position - gameObject.transform.position).sqrMagnitude < (cc.radius +.1f) * (cc.radius + .1f))
                {
                    // Contact!
                    _contacts[c] = Water2D.MultiColorManager.instance._allparticles[i];
                    c++;
                }
            }
            

            //int i = Physics2D.OverlapCircleNonAlloc(rb.position, cc.radius * .9f, Contacts, 1 << gameObject.layer);

            if (c > 0)
            {
                for (int j = 0; j < _contacts.Length; j++)
                {
                    if (_contacts[j] == null)
                        continue;
                    
                    Color c2 = _contacts[j].gameObject.GetComponent<SpriteRenderer>().color;

                    if (c2 == sr.color)
                        return;

                    sr.color = Color.LerpUnclamped(sr.color, c2, .02f);
                   // print(sr.color + " " + c2);
                }
            }
        }
    }
}
