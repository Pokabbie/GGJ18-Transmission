﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Input profile for a basic SNES type controller
/// </summary>
public class InputProfile : MonoBehaviour
{

	private int mInternalID = -1;
	public int ID
	{
		get { return mInternalID; }
	}

	private bool[] mCurrentState = new bool [4];
	private bool[] mPreviousState = new bool[4];
	private Vector2 mAxisInput;

	private bool bRequiresAssign = false;
	private static HashSet<int> AvaliableIDs = new HashSet<int>{ 0, 1, 2, 3 };


	public enum Button
	{
		A, B, Start, Select
	}


	void Start()
	{
		// TODO - REMOVE THIS
		Bind();
	}
	
	void Update()
	{
		mCurrentState.CopyTo(mPreviousState, 0);

		if (mInternalID == -1)
		{
			mCurrentState[0] = false;
			mCurrentState[1] = false;
			mCurrentState[2] = false;
			mCurrentState[3] = false;
			

			// Hunt for input
			if (bRequiresAssign)
			{
				foreach (int id in AvaliableIDs)
				{
					// TODO - Read any input
					if (Input.GetButton("Gamepad " + id + " A"))
					{
						mInternalID = id;
						AvaliableIDs.Remove(id);
						bRequiresAssign = false;
						Debug.Log("InputProfile(" + mInternalID + ") assigned to " + gameObject.name + " (" + AvaliableIDs.Count + " profiles remaining)");
						break;
					}
				}
			}
		}
		else
		{
			mCurrentState[(int)Button.A] = Input.GetButton("Gamepad " + mInternalID + " A");
			mCurrentState[(int)Button.B] = Input.GetButton("Gamepad " + mInternalID + " B");
			mCurrentState[(int)Button.Start] = Input.GetButton("Gamepad " + mInternalID + " Start");
			mCurrentState[(int)Button.Select] = Input.GetButton("Gamepad " + mInternalID + " Select");

			mAxisInput.x = Input.GetAxis("Gamepad " + mInternalID + " Horizontal");
			mAxisInput.y = Input.GetAxis("Gamepad " + mInternalID + " Vertical");
			mAxisInput.Normalize();
		}
	}

	/// <summary>
	/// Bind this profile to the next gamepad that gives input
	/// </summary>
	public void Bind()
	{
		if(mInternalID == -1)
			bRequiresAssign = true;
	}

	/// <summary>
	/// Release the gamepad so it's avaliable for other profiles
	/// </summary>
	public void Unbind()
	{
		mInternalID = -1;
		bRequiresAssign = false;
	}



	public bool GetKeyDown(Button button)
	{
		return mCurrentState[(int)button];
	}
	public bool GetKeyUp(Button button)
	{
		return !mCurrentState[(int)button];
	}
	public bool GetKeyPressed(Button button)
	{
		return mCurrentState[(int)button] && !mPreviousState[(int)button];
	}
	public bool GetKeyReleased(Button button)
	{
		return !mCurrentState[(int)button] && mPreviousState[(int)button];
	}
	public Vector2 GetInputVector()
	{
		return mAxisInput;
	}
}
