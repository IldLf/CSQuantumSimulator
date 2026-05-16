//factory стандартных гейтов

//Методы:

//Hadamard()
//PauliX()
//PauliY()
//PauliZ()
//Phase()
//T()
//CNOT()
//SWAP()

using System;
using System.Numerics;

namespace CSQuantumSimulator.Quantum;

public static class Gates
{
	private static readonly double InvSqrt2 =
		1.0 / Math.Sqrt(2);

	public static QuantumGate Hadamard(int target)
	{
		return new QuantumGate(
			"H",
			new ComplexMatrix(new Complex[,]
			{
				{InvSqrt2, InvSqrt2},
				{InvSqrt2,-InvSqrt2}
			}),
			target);
	}

	public static QuantumGate PauliX(int target)
	{
		return new QuantumGate(
			"X",
			new ComplexMatrix(new Complex[,]
			{
				{0,1},
				{1,0}
			}),
			target);
	}

	public static QuantumGate PauliY(int target)
	{
		return new QuantumGate(
			"Y",
			new ComplexMatrix(new Complex[,]
			{
				{0,-Complex.ImaginaryOne},
				{Complex.ImaginaryOne,0}
			}),
			target);
	}

	public static QuantumGate PauliZ(int target)
	{
		return new QuantumGate(
			"Z",
			new ComplexMatrix(new Complex[,]
			{
				{1,0},
				{0,-1}
			}),
			target);
	}

	public static QuantumGate CNOT(
		int control,
		int target)
	{
		return new QuantumGate(
			"CNOT",
			null!,
			target,
			control);
	}
}