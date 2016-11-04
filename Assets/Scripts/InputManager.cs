using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class InputManager : MonoBehaviour
{
	public int number;

	private static InputManager m_instance;

	public string HLeft
	{
		get {return InputMapping [EInput.hLeft];}
	}
	public string VLeft
	{
		get {return InputMapping [EInput.vLeft];}
	}
	public string HRight
	{
		get {return InputMapping [EInput.hRight];}
	}
	public string VRight
	{
		get {return InputMapping [EInput.vRight];}
	}
	public string TriggerLeft
	{
		get {return InputMapping [EInput.lTrigger];}
	}
	public string TriggerRight
	{
		get {return InputMapping [EInput.rTrigger];}
	}
	public bool AButton
	{
		get {return Input.GetButton(InputMapping [EInput.aButton]);}
	}
	public bool BButton
	{
		get {return Input.GetButton(InputMapping [EInput.bButton]);}
	}

	public enum EInput
	{
		hLeft,
		vLeft,

		hRight,
		vRight,

		aButton,
		bButton,

		rTrigger,
		lTrigger,
	}

	public enum AxisType
	{
		KeyOrMouseButton = 0,
		MouseMovement = 1,
		JoystickAxis = 2
	};
	
	public class InputAxis
	{
		public string name;
		public string descriptiveName;
		public string descriptiveNegativeName;
		public string negativeButton;
		public string positiveButton;
		public string altNegativeButton;
		public string altPositiveButton;
		
		public float gravity;
		public float dead;
		public float sensitivity;
		
		public bool snap = false;
		public bool invert = false;
		
		public AxisType type;
		
		public int axis;
		public int joyNum;
	}

	private Dictionary<EInput,string> InputMapping = new Dictionary<EInput,string>();
	 
	public static InputManager Instance
	{
		get 
		{ 
			if(m_instance == null)
			{
				m_instance = new GameObject("Singleton").AddComponent<InputManager>(); 
				Instance.Initialize();
			}
			return m_instance;
		}
	}

	private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
	{
		SerializedProperty child = parent.Copy();
		child.Next(true);
		do
		{
			if (child.name == name) return child;
		}
		while (child.Next(false));
		return null;
	}

	private static void AddAxis(InputAxis axis)
	{
		SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
		SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
		
		axesProperty.arraySize++;
		serializedObject.ApplyModifiedProperties();
		
		SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);
		
		GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
		GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
		GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
		GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
		GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
		GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
		GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
		GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
		GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
		GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
		GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
		GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
		GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
		GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
		GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;
		
		serializedObject.ApplyModifiedProperties();
	}
	private void SetUnityInputManager()
	{
		SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
		SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
		axesProperty.ClearArray();
		serializedObject.ApplyModifiedProperties();

		//Right
		AddAxis(new InputAxis() { name = "VerticalStickRightPC",  gravity = 0.5f, dead = 0.4f, sensitivity = 1f, type = AxisType.JoystickAxis, axis = 5,});
		AddAxis(new InputAxis() { name = "HorizontalStickRightPC", gravity = 0.5f, dead = 0.4f, sensitivity = 1f, type = AxisType.JoystickAxis, axis = 4,});
		//Left
		AddAxis(new InputAxis() { name = "VerticalStickLeftPC", gravity = 0.5f,  invert = true, dead = 0.4f, sensitivity = 1f, type = AxisType.JoystickAxis, axis = 2,});
		AddAxis(new InputAxis() { name = "HorizontalStickLeftPC", gravity = 0.5f, dead = 0.4f, sensitivity = 1f, type = AxisType.JoystickAxis, axis = 1,});

		//shoulder trigger
		AddAxis(new InputAxis() { name = "ShoulderRightPC", gravity = 0.5f, dead = 0.4f, sensitivity = 1f, type = AxisType.JoystickAxis, axis = 3,});
		AddAxis(new InputAxis() { name = "ShoulderLeftPC", gravity = 0.5f, dead = 0.4f, sensitivity = 1f,invert = true, type = AxisType.JoystickAxis, axis = 6,});

		//jump
		AddAxis(new InputAxis() { name = "A",descriptiveName = "A", positiveButton = "joystick 1 button 0",  type = AxisType.JoystickAxis, axis = 1,});

		//Change Modes
		AddAxis(new InputAxis() { name = "B",descriptiveName = "B", positiveButton = "joystick 1 button 1",  type = AxisType.JoystickAxis, axis = 1,});
	}
	private void Initialize()
	{
		SetUnityInputManager ();

		if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer) 
		{
			InputMapping.Add (EInput.hLeft, "HorizontalStickLeftPC");
			InputMapping.Add (EInput.vLeft, "VerticalStickLeftPC");
			InputMapping.Add (EInput.hRight, "HorizontalStickRightPC");
			InputMapping.Add (EInput.vRight, "VerticalStickRightPC");
			InputMapping.Add (EInput.aButton, "A");
			InputMapping.Add (EInput.bButton, "B");
			InputMapping.Add (EInput.rTrigger, "ShoulderRightPC");
			InputMapping.Add (EInput.lTrigger, "ShoulderLeftPC");
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			InputMapping.Add (EInput.hLeft, "HorizontalStickLeftPC");
			InputMapping.Add (EInput.vLeft, "VerticalStickLeftPC");
			InputMapping.Add (EInput.hRight, "HorizontalStickRightPC");
			InputMapping.Add (EInput.vRight, "VerticalStickRightPC");
			InputMapping.Add (EInput.aButton, "A");
			InputMapping.Add (EInput.bButton, "B");
			InputMapping.Add (EInput.rTrigger, "ShoulderRightPC");
			InputMapping.Add (EInput.lTrigger, "ShoulderLeftPC"); 
		}
	}

	public string Get(EInput _enum)
	{
		return InputMapping [_enum];
	}
}









