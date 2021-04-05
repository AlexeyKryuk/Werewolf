using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private Transform sensor;
	[SerializeField] private Transform ownModel;
	[SerializeField] private Laser laser;
	[SerializeField] private float damage;
	
	public Animator _animator;
	public int rays = 6;
	public int distance = 15;
	public float angle = 20;
	public Vector3 offset;

	private float _repeatTime = 2f;
	private float _currTime;

    private void Start()
    {
		_currTime = _repeatTime;
	}

    bool GetRaycast(Vector3 dir)
	{
		bool result = false;
		RaycastHit hit = new RaycastHit();
		Vector3 pos = sensor.position + offset;
		if (Physics.Raycast(pos, dir, out hit, distance))
		{
			if (hit.transform == target)
			{
				result = true;
				Debug.DrawLine(pos, hit.point, Color.green);
			}
			else
			{
				Debug.DrawLine(pos, hit.point, Color.blue);
			}
		}
		else
		{
			Debug.DrawRay(pos, dir * distance, Color.red);
		}
		return result;
	}

	bool RayToScan()
	{
		bool result = false;
		bool a = false;
		bool b = false;
		float j = 0;
		for (int i = 0; i < rays; i++)
		{
			var x = Mathf.Sin(j);
			var y = Mathf.Cos(j);

			j += angle * Mathf.Deg2Rad / rays;

			Vector3 dir = sensor.TransformDirection(new Vector3(x, 0, y));
			if (GetRaycast(dir)) a = true;

			if (x != 0)
			{
				dir = sensor.TransformDirection(new Vector3(-x, 0, y));
				if (GetRaycast(dir)) b = true;
			}
		}

		if (a || b) result = true;
		return result;
	}

	void Update()
	{
		if (Vector3.Distance(sensor.position, target.position) < distance 
			&& target.GetComponent<Transformation>().IsTransformed)
		{
			if (RayToScan())
			{
				LookAtTarget();
				laser.Start();

				_animator.SetBool("Detected", true);

				_currTime -= Time.deltaTime;

				if (_currTime <= 0)
                {
					_currTime = _repeatTime;

					_animator.SetTrigger("Shot");
					target.GetComponent<Player>().ApplyDamage(damage);
				}
			}
			else
			{
				_currTime = _repeatTime;

				laser.Stop();
				_animator.SetBool("Detected", false);
			}
		}
		else
        {
			laser.Stop();
			_animator.SetBool("Detected", false);
        }
	}

	private void LookAtTarget()
    {
		Vector3 look = target.position - ownModel.position;
		ownModel.rotation = Quaternion.RotateTowards(ownModel.rotation, Quaternion.LookRotation(look), 100f * Time.deltaTime);
		ownModel.rotation = Quaternion.Euler(0, ownModel.eulerAngles.y, 0);
	}
}
