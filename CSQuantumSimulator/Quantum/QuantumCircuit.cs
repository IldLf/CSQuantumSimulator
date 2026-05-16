//using CSQuantumSimulator.Quantum;

//модель схемы

//Хранит:

//List<QuantumGate>

//Методы:

//AddGate
//RemoveGate
//Execute
//Clear

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSQuantumSimulator.Quantum;

public class QuantumCircuit
{
	private readonly List<QuantumGate> gates = new();

	public IReadOnlyList<QuantumGate> Gates => gates;

	public void AddGate(QuantumGate gate)
	{
		gates.Add(gate);
	}

	public void Clear()
	{
		gates.Clear();
	}

	public void Validate(int qubitCount)
	{
		if (qubitCount < 1 || qubitCount > 8)
			throw new InvalidOperationException("Количество кубитов должно быть от 1 до 8.");

		foreach (var gate in gates)
		{
			if (gate.TargetQubit < 0 || gate.TargetQubit >= qubitCount)
				throw new InvalidOperationException($"Гейт {gate.Name} использует несуществующий кубит q{gate.TargetQubit}.");

			if (gate.ControlQubit.HasValue)
			{
				if (gate.ControlQubit.Value < 0 || gate.ControlQubit.Value >= qubitCount)
					throw new InvalidOperationException($"Гейт {gate.Name} использует несуществующий кубит q{gate.ControlQubit.Value}.");

				if (gate.ControlQubit.Value == gate.TargetQubit)
					throw new InvalidOperationException($"Гейт {gate.Name} не может использовать один и тот же control и target.");
			}
		}
	}
}