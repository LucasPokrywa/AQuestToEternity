using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class Player : MonoBehaviour
{
    // camel case -> translationSpeed
    // upper camel case = Pascal case -> TranslationSpeed

    // classe field
    [Tooltip("en m.s-1")]
    [SerializeField] private float m_TranslationSpeed = 1.0f; // m.s-1
    [Tooltip("en °.s-1")]
    [SerializeField] float m_RotationSpeed = 1.0f; // °.s-1

    Rigidbody m_Rb;

    [Header("Ball Shooting")]
    [SerializeField] GameObject m_BallPrefab;
    [SerializeField] Transform m_BallSpawnPos;
    [SerializeField] float m_BallShootSpeed = 20.0f;
    [SerializeField] float m_BallLifeTime = 3.0f;
    [SerializeField] float m_ShootingPeriod = .25f;

    float m_NextShootingTime;

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_NextShootingTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");
        
        // kinematic behaviour
        // position sans gérer les collisions. Exemple : les oiseaux purement esthétique dans le ciel. 
        // transform, Update(), Time.deltaTime
        // L'objet change de position et d'orientation immédiatement après l'appel aux méthodes Translate et Rotate

        Vector3 moveWorldVect = vInput * transform.forward * m_TranslationSpeed * Time.deltaTime;

        //transform.Translate(transform.forward * m_TranslationSpeed * Time.deltaTime, Space.World);        // référentiel du monde
        //transform.Translate(vInput * Vector3.forward * m_TranslationSpeed * Time.deltaTime, Space.Self);    // référentiel du Player | Vector3.forward == new Vector3(0,0,1)
        
        //transform.translate(moveWorldVect, Space.Self);
        transform.position += moveWorldVect;

        transform.Rotate(Vector3.up * hInput*m_RotationSpeed * Time.deltaTime, Space.Self);
        */

        if (Input.GetButton("Fire1") && Time.time > m_NextShootingTime)
        {
            m_NextShootingTime = Time.time + m_ShootingPeriod;

            GameObject newBallGO = Instantiate(m_BallPrefab, m_BallSpawnPos.position, Quaternion.identity);
            Rigidbody ballRb = newBallGO.GetComponent<Rigidbody>();
            ballRb.linearVelocity = m_BallSpawnPos.forward * m_BallShootSpeed;
            Destroy(newBallGO, m_BallLifeTime);
        }
    }

    private void FixedUpdate()
    {
        // dynamic (physic) behaviour
        // PhysX (Nvidia) -> équation de la mécanique de Newton
        // rigibody, FixedUpdate(), Time.fixedDeltaTime
        // L'objet n'aura changé de position et/ou d'orientation qu'à la frame suivante

        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        Vector3 moveWorldVect = vInput * transform.forward * m_TranslationSpeed * Time.fixedDeltaTime;

        // POSITION MODE -> téléportation
        // ce mode n'annule pas l'inertie physique de l'objet
        // Les frotements entre objets ne sont pas pris en compte
        /*
        m_Rb.MovePosition(m_Rb.position + moveWorldVect);

        Quaternion qUprightRot = Quaternion.FromToRotation(transform.up, Vector3.up);
        Quaternion qUprightOrient = qUprightRot * m_Rb.rotation;

        Quaternion qUprightLerpedOrient = Quaternion.Lerp(m_Rb.rotation, qUprightOrient, Time.fixedDeltaTime * 6);

        Quaternion qRot = Quaternion.AngleAxis(hInput * m_RotationSpeed * Time.fixedDeltaTime, transform.up);
        Quaternion newOrient = qRot * qUprightLerpedOrient;
        m_Rb.MoveRotation(newOrient);

        m_Rb.linearVelocity = Vector3.zero;
        m_Rb.angularVelocity = Vector3.zero;
        */

        // VELOCITY MODE
        // Prise en compte des frotements entre objets
        Vector3 targetVelocity = vInput * transform.forward * m_TranslationSpeed;
        m_Rb.AddForce(targetVelocity - m_Rb.linearVelocity, ForceMode.VelocityChange);

        Vector3 targetAngularVelocity = hInput * transform.up * m_RotationSpeed * Mathf.Deg2Rad;
        // Torque == couple mécanique
        m_Rb.AddTorque(targetAngularVelocity - m_Rb.angularVelocity, ForceMode.VelocityChange);

        Quaternion qUprightRot = Quaternion.FromToRotation(transform.up, Vector3.up);
        Quaternion qUprightOrient = qUprightRot * m_Rb.rotation;
        Quaternion qUprightLerpedOrient = Quaternion.Lerp(m_Rb.rotation, qUprightOrient, Time.fixedDeltaTime * 6);
        m_Rb.MoveRotation(qUprightLerpedOrient);

        // ACCELERATION MODE    -> rarement utilisé
        // FORCE MODE           -> rarement utilisé
    }
}