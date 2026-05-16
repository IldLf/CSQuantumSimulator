//строит:

//H(0)
//CNOT(0, 1)

using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Algorithms;

public class BellStateAlgorithm : IQuantumAlgorithm
{
	public string Name => "Состояние Белла";

	public QuantumCircuit BuildCircuit()
	{
		var circuit = new QuantumCircuit();

		circuit.AddGate(Gates.Hadamard(0));
		circuit.AddGate(Gates.CNOT(0, 1));

		return circuit;
	}
}