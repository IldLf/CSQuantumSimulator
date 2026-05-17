using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Algorithms;

public class QuantumTeleportationAlgorithm : IQuantumAlgorithm
{
	public string Name => "Квантовая телепортация";

	public QuantumCircuit BuildCircuit()
	{
		var circuit = new QuantumCircuit();

		circuit.AddGate(Gates.Hadamard(0));

		circuit.AddGate(Gates.Hadamard(1));
		circuit.AddGate(Gates.CNOT(1, 2));

		circuit.AddGate(Gates.CNOT(0, 1));
		circuit.AddGate(Gates.Hadamard(0));

		circuit.AddGate(Gates.CNOT(1, 2));
		circuit.AddGate(Gates.CZ(0, 2));

		return circuit;
	}
}