using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Algorithms;

public class SuperdenseCodingAlgorithm : IQuantumAlgorithm
{
	public string Name => "Сверхплотное кодирование";

	public QuantumCircuit BuildCircuit()
	{
		var circuit = new QuantumCircuit();

		circuit.AddGate(Gates.Hadamard(0));
		circuit.AddGate(Gates.CNOT(0, 1));

		circuit.AddGate(Gates.PauliX(0));
		circuit.AddGate(Gates.PauliZ(0));

		circuit.AddGate(Gates.CNOT(0, 1));
		circuit.AddGate(Gates.Hadamard(0));

		return circuit;
	}
}