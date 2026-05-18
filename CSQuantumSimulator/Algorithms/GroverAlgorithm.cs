using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Algorithms;

public class GroverAlgorithm : IQuantumAlgorithm
{
	public string Name => "Поиск Гровера";

	public QuantumCircuit BuildCircuit()
	{
		var circuit = new QuantumCircuit();

		circuit.AddGate(Gates.Hadamard(0));
		circuit.AddGate(Gates.Hadamard(1));

		circuit.AddGate(Gates.CZ(0, 1));

		circuit.AddGate(Gates.Hadamard(0));
		circuit.AddGate(Gates.Hadamard(1));

		circuit.AddGate(Gates.PauliX(0));
		circuit.AddGate(Gates.PauliX(1));

		circuit.AddGate(Gates.Hadamard(1));
		circuit.AddGate(Gates.CNOT(0, 1));
		circuit.AddGate(Gates.Hadamard(1));

		circuit.AddGate(Gates.PauliX(0));
		circuit.AddGate(Gates.PauliX(1));

		circuit.AddGate(Gates.Hadamard(0));
		circuit.AddGate(Gates.Hadamard(1));

		return circuit;
	}
}