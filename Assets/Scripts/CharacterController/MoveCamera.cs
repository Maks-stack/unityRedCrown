using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

	[SerializeField] private GameObject m_target;
	[SerializeField] private float m_radius = 20;
		
	private float m_heightOffset = 3.7f;

	private float m_verticalInput;
	private float m_horizontalInput;

	private Vector3 m_cartPos;

	private float m_polar;
	public float m_elevation = 0.6f;

	private bool m_objectInView;
	private Vector3 m_correctedPosition;

	private bool m_hittingFloor;

	private float m_yHit;

	public GameObject Target
	{
		get 
		{
			return m_target;
		}
		set 
		{
			m_target = value;
		}
	}



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		GetInput();
		CheckEnvironment();


	}

	private void GetInput()
	{

		print (m_hittingFloor);

		m_horizontalInput = Input.GetAxis(InputManager.Instance.Get(InputManager.EInput.hRight));
		m_verticalInput = Input.GetAxis(InputManager.Instance.Get(InputManager.EInput.vRight));

		m_polar += m_horizontalInput * Time.deltaTime*2;
		m_elevation += m_verticalInput * Time.deltaTime*2;

		m_elevation = Mathf.Clamp(m_elevation, -1.2f, 1.2f);

		float a = m_radius * Mathf.Cos(m_elevation);
	
		m_cartPos.x = m_target.transform.position.x  + (a * Mathf.Cos(m_polar));

		if(!m_hittingFloor)
			m_cartPos.y = m_target.transform.position.y + (m_radius * Mathf.Sin(m_elevation));

		m_cartPos.z = m_target.transform.position.z + (a * Mathf.Sin(m_polar));

		/*
		m_posX = m_target.transform.position.x  +  Mathf.Cos(Mathf.Deg2Rad * m_theta) * m_distance;

		m_posY =  m_target.transform.position.y  +  Mathf.Asin(Mathf.Deg2Rad * m_phi) * m_distance;

		m_posZ = m_target.transform.position.z + Mathf.Sin(Mathf.Deg2Rad * m_theta) * m_distance;
		*/

	}

	private void CheckEnvironment()
	{
		float distance = Vector3.Distance(m_target.transform.position, transform.position);

  		Ray backRay = new Ray (transform.position, -transform.forward);

		RaycastHit hit = new RaycastHit();

		if(Physics.Raycast(backRay.origin, backRay.direction, out hit, 4))
		{
			//transform.position = hit.point;
		}

		Debug.DrawRay(backRay.origin, -Vector3.up, Color.red, 4f);

		if(Physics.Raycast(backRay.origin, -Vector3.up, out hit, 10))
	   	{		   
			m_yHit = hit.point.y;
		}

		m_hittingFloor = transform.position.y < m_yHit;

		if(m_hittingFloor)
			m_cartPos.y = m_yHit +0.1f;

		if(Physics.Linecast(m_target.transform.position, transform.position , out hit))
		{
			m_correctedPosition = hit.point;
			m_objectInView = true;
		}else{
			m_objectInView = false;
		}
	}

	public void ExecuteMovement()
	{
		if(transform.eulerAngles.x > 60)
		{
			transform.eulerAngles = new Vector3(60, transform.eulerAngles.y, transform.eulerAngles.z);
		}
	
		if(m_objectInView)
		{			

			//transform.position = m_correctedPosition + this.transform.forward;
		}else{
		
		}

		this.transform.position = Vector3.Lerp(this.transform.position, m_cartPos, Time.deltaTime * 5 );

		this.transform.LookAt(new Vector3(m_target.transform.position.x ,m_target.transform.position.y + m_heightOffset, m_target.transform.position.z));
	}

}



























