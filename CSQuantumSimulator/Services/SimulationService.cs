//запуск схемы

//делает:

//Circuit → Register evolution

using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Services;

public class SimulationService
{
	public QuantumRegister Run(QuantumCircuit circuit, int qubitCount)
	{
		circuit.Validate(qubitCount);

		var register = new QuantumRegister(qubitCount);

		foreach (var gate in circuit.Gates)
			register.ApplyGate(gate);

		return register;
	}
}