﻿namespace OperationResult.SingleErrorWithValue;

public class Executor
{
	public OperationResult Operation()
	{
		// Randomize the success indicator
		// This should be real logic
		var randomNumber = Random.Shared.Next(100);
		var success = randomNumber % 2 == 0;

		// Return the operation result
		return success
				   ? new OperationResult { Value = randomNumber }
				   : new OperationResult
					 {
						 ErrorMessage = $"Something went wrong with the number '{randomNumber}'.",
						 Value = randomNumber
					 };
	}
}

public record class OperationResult
{
	public bool Succeeded => string.IsNullOrWhiteSpace(ErrorMessage);
	public string? ErrorMessage { get; init; }
	public int? Value { get; init; }
}