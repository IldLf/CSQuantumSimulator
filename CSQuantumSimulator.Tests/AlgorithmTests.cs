using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSQuantumSimulator.Algorithms;
using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Tests;

[TestClass]
public class AlgorithmTests
{
	private const double EPS = 0.0001;

	[TestMethod]
	public void BellAlgorithm_ShouldCreateEntanglement()
	{
		var algorithm = new BellStateAlgorithm();

		var circuit = algorithm.BuildCircuit();

		var reg = new QuantumRegister(2);

		foreach (var gate in circuit.Gates)
			reg.ApplyGate(gate);

		var probs = reg.State.GetProbabilities();

		Assert.AreEqual(0.5, probs[0], EPS);
		Assert.AreEqual(0.0, probs[1], EPS);
		Assert.AreEqual(0.0, probs[2], EPS);
		Assert.AreEqual(0.5, probs[3], EPS);
	}

	[TestMethod]
	public void DeutschAlgorithm_ShouldReturnBalancedResult()
	{
		var algorithm = new DeutschAlgorithm();

		var circuit = algorithm.BuildCircuit();

		var reg = new QuantumRegister(2);

		foreach (var gate in circuit.Gates)
			reg.ApplyGate(gate);

		var probs = reg.State.GetProbabilities();

		double q0IsOne = probs[1] + probs[3];

		Assert.IsTrue(q0IsOne > 0.99);
	}

	[TestMethod]
	public void Grover_ShouldAmplifyTargetState()
	{
		var algorithm = new GroverAlgorithm();

		var circuit = algorithm.BuildCircuit();

		var reg = new QuantumRegister(2);

		foreach (var gate in circuit.Gates)
			reg.ApplyGate(gate);

		var probs = reg.State.GetProbabilities();

		double max = probs.Max();

		Assert.IsTrue(max > 0.5);
	}
}