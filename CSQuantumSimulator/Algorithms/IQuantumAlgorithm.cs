//интерфейс

//Name
//BuildCircuit()
//Description

using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Algorithms;

public interface IQuantumAlgorithm
{
	string Name { get; }

	QuantumCircuit BuildCircuit();
}
