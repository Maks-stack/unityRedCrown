using UnityEngine;
using System.Collections;

public class HoverCarEffectManager : MonoBehaviour {

	[SerializeField] private GameObject m_dustParticlePrefab; 

	private HoverCarControls m_controlsScript;
	private RaycastHit m_groundHitPoint;

	private GameObject m_dustParticle;

	private void Awake()
	{
		m_controlsScript = gameObject.GetComponent<HoverCarControls>();
		m_groundHitPoint = m_controlsScript.GroundHitPoint;
	}
	// Use this for initialization
	void Start () {

		Initialize ();
	}

	private void Initialize()
	{
		m_dustParticle = GameObject.Instantiate(m_dustParticlePrefab) as GameObject; 
		m_dustParticle.transform.parent = this.transform;
		m_dustParticle.transform.localPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		m_groundHitPoint = m_controlsScript.GroundHitPoint;
		m_dustParticle.transform.position = m_groundHitPoint.point;
		//m_dustParticle.transform.forward = m_groundHitPoint.normal; 
		Debug.DrawRay (m_groundHitPoint.point, m_groundHitPoint.normal, Color.red, Mathf.Infinity);
		Debug.DrawRay (m_groundHitPoint.point, m_dustParticle.transform.up, Color.blue, Mathf.Infinity);


	}
}
