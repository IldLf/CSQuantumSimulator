using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Algorithms;

public class BernsteinVaziraniAlgorithm : IQuantumAlgorithm
{
	public string Name => "Алгоритм Бернштейна–Вазирани";

	public QuantumCircuit BuildCircuit()
	{
		var circuit = new QuantumCircuit();

		circuit.AddGate(Gates.PauliX(2));

		circuit.AddGate(Gates.Hadamard(0));
		circuit.AddGate(Gates.Hadamard(1));
		circuit.AddGate(Gates.Hadamard(2));

		circuit.AddGate(Gates.CNOT(0, 2));
		circuit.AddGate(Gates.CNOT(1, 2));

		circuit.AddGate(Gates.Hadamard(0));
		circuit.AddGate(Gates.Hadamard(1));

		return circuit;
	}
}