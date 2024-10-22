﻿namespace Composite.Models;

public class Store : BookComposite
{
	public Store(string name, string location, string manager)
		: base(name)
	{
		Location = location;
		Manager = manager;
	}

	public string Location { get; }
	public string Manager { get; }
}