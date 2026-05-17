using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Algorithms;

public class EmptyAlgorithm : IQuantumAlgorithm
{
	public string Name => "Произвольный алгоритм";

	public QuantumCircuit BuildCircuit()
	{
		return new QuantumCircuit();
	}
}