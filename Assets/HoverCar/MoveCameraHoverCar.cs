using UnityEngine;
using System.Collections;

public class MoveCameraHoverCar : MonoBehaviour {

	[SerializeField] private GameObject m_target;
	[SerializeField] private float m_radius = 20;
		
	private float m_heightOffset = 1f;

	private float m_verticalInput;
	private float m_horizontalInput;

	private Vector3 m_cartPos;

	private float m_polar;
	private float m_elevation = 0.6f;

	private bool m_objectInView;
	private Vector3 m_correctedPosition;

	





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
		m_horizontalInput = Input.GetAxis(InputManager.Instance.HLeft);
		m_verticalInput = Input.GetAxis(InputManager.Instance.VLeft);

		m_polar += m_horizontalInput * Time.deltaTime*2;
		m_elevation += m_verticalInput * Time.deltaTime*2;

		m_elevation = Mathf.Clamp(m_elevation, 0.01f, 1.2f);

		float a = m_radius * Mathf.Cos(m_elevation);

		m_cartPos.x = m_target.transform.position.x  + (a * Mathf.Cos(m_polar));
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

		if(Physics.Linecast(m_target.transform.position, transform.position , out hit))
		{
			m_correctedPosition = hit.point;
			m_objectInView = true;
		}else{
			m_objectInView = false;
		}
		/*
		Debug.DrawRay(transform.position, -transform.up, Color.red, 3);
		Debug.DrawRay(backRay.origin, backRay.direction, Color.red, 3);
		Debug.DrawLine(m_target.transform.position, transform.position, Color.yellow, distance);
		*/

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

		this.transform.position = Vector3.Lerp(this.transform.position, m_cartPos, Time.deltaTime * 10 );





		this.transform.LookAt(new Vector3(m_target.transform.position.x ,m_target.transform.position.y + m_heightOffset, m_target.transform.position.z));
	}

}



























