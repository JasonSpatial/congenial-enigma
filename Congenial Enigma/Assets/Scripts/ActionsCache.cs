using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Reflection;

public class ActionsCache {

	UnityEngine.Object source;
	Type sourceClass;
	MethodInfo method = null;

	Dictionary<string, Action> parentMethodInfos;
	List<string> emptyMethodInfos;

	public ActionsCache(UnityEngine.Object source)
	{
		this.source = source;
		sourceClass = source.GetType();

		parentMethodInfos = new Dictionary<string, Action>();
		emptyMethodInfos = new List<string>();
	}

	public void Invoke(string methodName)
	{
		if (parentMethodInfos.ContainsKey(methodName))
		{
			parentMethodInfos[methodName].Invoke();
			return;
		}

		if (emptyMethodInfos.Contains(methodName)) return; // there is no method

		// we don't have the method cached yet. cache it
		method = sourceClass.GetMethod(methodName);
		if (method != null)
		{
			Action convertedMethod = (Action)Delegate.CreateDelegate(typeof(Action), source, methodName); //convert methodinfo into action for a much better performance
			parentMethodInfos.Add(methodName, convertedMethod);
			parentMethodInfos[methodName].Invoke(); //invoke from here the first time
		}
		else
		{
			emptyMethodInfos.Add(methodName); //don't need to invoke an empty method
		}
	}
}