// реализация Deutsch

using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Algorithms;

public class DeutschAlgorithm : IQuantumAlgorithm
{
	public string Name => "Алгоритм Дойча";

	public QuantumCircuit BuildCircuit()
	{
		var circuit = new QuantumCircuit();

		circuit.AddGate(Gates.PauliX(1));

		circuit.AddGate(Gates.Hadamard(0));
		circuit.AddGate(Gates.Hadamard(1));

		circuit.AddGate(Gates.CNOT(0, 1));

		circuit.AddGate(Gates.Hadamard(0));

		return circuit;
	}
}
