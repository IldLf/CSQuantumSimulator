//запуск схемы

//делает:

//Circuit → Register evolution

using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Services;

public class SimulationService
{
	public QuantumRegister Run(
		QuantumCircuit circuit,
		int qubits)
	{
		var register =
			new QuantumRegister(qubits);

		circuit.Execute(register);

		return register;
	}
}