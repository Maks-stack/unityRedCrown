using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	[SerializeField] GameObject m_character;
	[SerializeField] GameObject m_hoverCar;
	[SerializeField] MoveCamera m_camera;

	private enum MovementMode
	{
		character,
		hovercar,
	}

	private bool m_blockedInput;

	private MovementMode m_currentMode = MovementMode.character;

	void Start()
	{
		m_currentMode = MovementMode.character;
		StartCoroutine(SetMode());
	}

	void Update()
	{
		if(InputManager.Instance.BButton)
		{	
			if(m_blockedInput)
				return;

			print("B");
			StartCoroutine(SetMode());
		}
	}

	private IEnumerator SetMode()
	{
		m_blockedInput = true;
		if(m_currentMode == MovementMode.character)
			m_currentMode = MovementMode.hovercar;
		else
			m_currentMode = MovementMode.character;

		m_hoverCar.SetActive(m_currentMode == MovementMode.hovercar);
		m_character.SetActive(m_currentMode == MovementMode.character);

		m_camera.Target = m_currentMode == MovementMode.hovercar ? m_hoverCar :  m_character;

		yield return new WaitForSeconds(1);
		m_blockedInput = false;
	}
}
