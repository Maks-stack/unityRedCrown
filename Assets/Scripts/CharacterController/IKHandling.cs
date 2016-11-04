using UnityEngine;
using System.Collections;

public class IKHandling : MonoBehaviour {

	public Animator anim;

	public float ikWeight = 1;

	public float offset;

	public Transform leftIKTarget;
	public Transform rightIKTarget;

	public Transform iKBoundLeft;
	public Transform iKBoundRight;

	public Transform hintLeft;
	public Transform hintRight;

	private float lWeight;
	private float rWeight;

	private void Start()
	{
		print ("start");
	}
	private void Update()
	{
		Transform leftPos = iKBoundLeft;
		Transform rightPos = iKBoundRight;

		Ray ray = new Ray(leftPos.position,-Vector3.up);

		//LEFT FOOT
		RaycastHit hit;
		Debug.DrawRay(ray.origin, ray.direction, Color.red,5);
		if(Physics.Raycast(ray,out hit))
		{
			leftIKTarget.transform.position = new Vector3(hit.point.x,hit.point.y,hit.point.z);
			leftIKTarget.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) *transform.rotation;
		}
	
		//RIGHT FOOT
		ray.origin = rightPos.position;

		Debug.DrawRay(ray.origin, ray.direction, Color.blue,5);
		if(Physics.Raycast(ray,out hit))
		{
			rightIKTarget.transform.position = new Vector3(hit.point.x,hit.point.y, hit.point.z);
			rightIKTarget.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) *transform.rotation;
		}

	}

	void OnAnimatorIK(int layerIndex)
	{
		float speed = anim.GetFloat("Speed");

		lWeight =  anim.GetFloat("LeftFoot");
		rWeight = anim.GetFloat("RightFoot");

		anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, lWeight);
		anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, rWeight);


		 if(speed > 0)
		 {
			anim.SetIKPosition(AvatarIKGoal.LeftFoot, new Vector3(
				anim.GetBoneTransform(HumanBodyBones.LeftFoot).position.x,
	          	leftIKTarget.position.y,
				anim.GetBoneTransform(HumanBodyBones.LeftFoot).position.z) + leftIKTarget.transform.up * offset);

			anim.SetIKPosition(AvatarIKGoal.RightFoot, new Vector3(
				anim.GetBoneTransform(HumanBodyBones.RightFoot).position.x,
	           	rightIKTarget.position.y,
	           	anim.GetBoneTransform(HumanBodyBones.RightFoot).position.z) + rightIKTarget.transform.up * offset);
		}else{

			anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftIKTarget.position + leftIKTarget.transform.up * offset);
			anim.SetIKPosition(AvatarIKGoal.RightFoot, rightIKTarget.position + rightIKTarget.transform.up * offset);
		}


		anim.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, ikWeight);
		anim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, ikWeight);

		anim.SetIKHintPosition(AvatarIKHint.LeftKnee, hintLeft.position);
		anim.SetIKHintPosition(AvatarIKHint.RightKnee, hintRight.position);

		anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot,lWeight);
		anim.SetIKRotationWeight(AvatarIKGoal.RightFoot,rWeight);

		anim.SetIKRotation(AvatarIKGoal.LeftFoot, leftIKTarget.rotation);
		anim.SetIKRotation(AvatarIKGoal.RightFoot, rightIKTarget.rotation);
	}
}
