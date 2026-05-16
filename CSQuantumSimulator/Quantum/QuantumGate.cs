//абстракция гейта

//Name
//Matrix
//TargetQubit
//ControlQubit?

using System;

namespace CSQuantumSimulator.Quantum;

public class QuantumGate
{
	public string Name { get; }

	public ComplexMatrix Matrix { get; }

	public int TargetQubit { get; }

	public int? ControlQubit { get; }

	public QuantumGate(
		string name,
		ComplexMatrix matrix,
		int targetQubit,
		int? controlQubit = null)
	{
		Name = name;
		Matrix = matrix;
		TargetQubit = targetQubit;
		ControlQubit = controlQubit;
	}
}