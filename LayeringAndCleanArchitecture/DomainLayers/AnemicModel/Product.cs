﻿namespace DomainLayers.AnemicModel;

public class Product
{
	public int? Id { get; set; }
	public required string Name { get; set; }
	public int QuantityInStock { get; set; }
}