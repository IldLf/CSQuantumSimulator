using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSQuantumSimulator.Quantum;
using System;

namespace CSQuantumSimulator.Tests;

[TestClass]
public class CircuitValidationTests
{
	[TestMethod]
	public void InvalidTargetQubit_ShouldThrow()
	{
		var circuit = new QuantumCircuit();

		circuit.AddGate(Gates.Hadamard(5));

		try
		{
			circuit.Validate(2);

			Assert.Fail("Expected exception was not thrown.");
		}
		catch (InvalidOperationException)
		{
		}
	}

	[TestMethod]
	public void InvalidControlQubit_ShouldThrow()
	{
		var circuit = new QuantumCircuit();

		circuit.AddGate(Gates.CNOT(5, 0));

		try
		{
			circuit.Validate(2);

			Assert.Fail("Expected exception was not thrown.");
		}
		catch (InvalidOperationException)
		{
		}
	}

	[TestMethod]
	public void SameControlAndTarget_ShouldThrow()
	{
		var circuit = new QuantumCircuit();

		circuit.AddGate(Gates.CNOT(0, 0));

		try
		{
			circuit.Validate(2);

			Assert.Fail("Expected exception was not thrown.");
		}
		catch (InvalidOperationException)
		{
		}
	}
}