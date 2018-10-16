using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;



public class iTweenTests
{
	private const float delay = 0.2f;
	private const float time = 0.1f;
	private static Vector3 targetLocaiton = Vector3.one;
	private static Hashtable defaultArgs => new Hashtable {
				{ "ignoretimescale", true},
				{ "delay", delay},
				{ "time", time},
				{ "position", targetLocaiton},
		};

	private static void MoveGMViaTween(GameObject obj) => iTween.MoveTo(obj, defaultArgs);

	private static GameObject Primitive;
	private static GameObject PrimitiveAtZero
	{
		get
		{
			if (Primitive == null)
			{
				Primitive = new GameObject();
			}
			Primitive.transform.position = Vector3.zero;
			return Primitive;
		}
	}

	// A Test behaves as an ordinary method
	[Test]
	public void TestPrimitiveAtZero()
	{
		Assert.AreEqual(Vector3.zero, PrimitiveAtZero.transform.position);
	}

	// A Test behaves as an ordinary method
	[UnityTest]
	public IEnumerator TestTweenMovement()
	{

		MoveGMViaTween(PrimitiveAtZero);
		yield return new WaitForSecondsRealtime(delay + time + 0.05f);
		Assert.AreEqual(targetLocaiton, Primitive.transform.position);
	}

	// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
	// `yield return null;` to skip a frame.
	[UnityTest]
	public IEnumerator TweenIgnoresTimescaleZero()
	{

		Time.timeScale = 0;

		MoveGMViaTween(PrimitiveAtZero);
		var startTime = Time.unscaledTime;
		while (Time.unscaledTime - startTime < delay + time)
			yield return null;
		Assert.AreNotEqual(Vector3.zero, Primitive);
		// Use the Assert class to test conditions
	}


	[UnityTest]
	public IEnumerator TweenIgnoresTimescaleNotZero()
	{

		Time.timeScale = 0.8f;

		MoveGMViaTween(PrimitiveAtZero);

		var startTime = Time.unscaledTime;
		while (Time.unscaledTime - startTime < (delay + time) / Time.timeScale)
			yield return null;
		Assert.AreNotEqual(Vector3.zero, Primitive.transform.position);
		// Use the Assert class to test conditions
	}
}
