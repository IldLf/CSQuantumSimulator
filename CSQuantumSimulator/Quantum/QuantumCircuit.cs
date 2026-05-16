//using CSQuantumSimulator.Quantum;

//модель схемы

//Хранит:

//List<QuantumGate>

//Методы:

//AddGate
//RemoveGate
//Execute
//Clear

using System.Collections.Generic;

namespace CSQuantumSimulator.Quantum;

public class QuantumCircuit
{
	public List<QuantumGate> Gates { get; } = new();

	public void AddGate(QuantumGate gate)
	{
		Gates.Add(gate);
	}

	public void Clear()
	{
		Gates.Clear();
	}

	public void Execute(QuantumRegister register)
	{
		foreach (var gate in Gates)
			register.ApplyGate(gate);
	}
}