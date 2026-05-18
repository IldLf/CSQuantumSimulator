using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSQuantumSimulator.Quantum;
using CSQuantumSimulator.Algorithms;

namespace CSQuantumSimulator.Tests;

[TestClass]
public class QuantumGateTests
{
	private const double EPS = 0.0001;

	[TestMethod]
	public void XGate_ShouldFlipQubit()
	{
		var reg = new QuantumRegister(1);

		reg.ApplyGate(Gates.PauliX(0));

		var probs = reg.State.GetProbabilities();

		Assert.AreEqual(0, probs[0], EPS);
		Assert.AreEqual(1, probs[1], EPS);
	}

	[TestMethod]
	public void Hadamard_ShouldCreateSuperposition()
	{
		var reg = new QuantumRegister(1);

		reg.ApplyGate(Gates.Hadamard(0));

		var probs = reg.State.GetProbabilities();

		Assert.AreEqual(0.5, probs[0], EPS);
		Assert.AreEqual(0.5, probs[1], EPS);
	}

	[TestMethod]
	public void CNOT_ShouldCreateBellState()
	{
		var reg = new QuantumRegister(2);

		reg.ApplyGate(Gates.Hadamard(0));
		reg.ApplyGate(Gates.CNOT(0, 1));

		var probs = reg.State.GetProbabilities();

		Assert.AreEqual(0.5, probs[0], EPS);
		Assert.AreEqual(0.0, probs[1], EPS);
		Assert.AreEqual(0.0, probs[2], EPS);
		Assert.AreEqual(0.5, probs[3], EPS);
	}

	[TestMethod]
	public void Swap_ShouldSwapQubits()
	{
		var reg = new QuantumRegister(2);

		reg.ApplyGate(Gates.PauliX(0));

		reg.ApplyGate(Gates.Swap(0, 1));

		var probs = reg.State.GetProbabilities();

		Assert.AreEqual(0.0, probs[1], EPS);
		Assert.AreEqual(1.0, probs[2], EPS);
	}

	[TestMethod]
	public void Ry_ShouldRotateState()
	{
		var reg = new QuantumRegister(1);

		reg.ApplyGate(Gates.Ry(0, Math.PI));

		var probs = reg.State.GetProbabilities();

		Assert.AreEqual(0.0, probs[0], EPS);
		Assert.AreEqual(1.0, probs[1], EPS);
	}
}