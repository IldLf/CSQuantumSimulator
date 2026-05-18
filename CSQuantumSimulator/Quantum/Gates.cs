using System;
using System.Numerics;

namespace CSQuantumSimulator.Quantum;

public static class Gates
{
	private static readonly double InvSqrt2 = 1.0 / Math.Sqrt(2);

	public static QuantumGate Hadamard(int target)
	{
		return new QuantumGate("H", new ComplexMatrix(new Complex[,]
		{
			{InvSqrt2, InvSqrt2},
			{InvSqrt2,-InvSqrt2}
		}), target);
	}

	public static QuantumGate PauliX(int target)
	{
		return new QuantumGate("X", new ComplexMatrix(new Complex[,]
		{
			{0,1},
			{1,0}
		}), target);
	}

	public static QuantumGate PauliY(int target)
	{
		return new QuantumGate("Y", new ComplexMatrix(new Complex[,]
		{
			{0,-Complex.ImaginaryOne},
			{Complex.ImaginaryOne,0}
		}), target);
	}

	public static QuantumGate PauliZ(int target)
	{
		return new QuantumGate("Z", new ComplexMatrix(new Complex[,]
		{
			{1,0},
			{0,-1}
		}), target);
	}

	public static QuantumGate PhaseS(int target)
	{
		return new QuantumGate("S", new ComplexMatrix(new Complex[,]
		{
			{1,0},
			{0,Complex.ImaginaryOne}
		}), target);
	}

	public static QuantumGate PhaseT(int target)
	{
		var phase = Complex.FromPolarCoordinates(1, Math.PI / 4);

		return new QuantumGate("T", new ComplexMatrix(new Complex[,]
		{
			{1,0},
			{0,phase}
		}), target);
	}

	public static QuantumGate Rx(int target, double angle)
	{
		var c = Math.Cos(angle / 2);
		var s = -Complex.ImaginaryOne * Math.Sin(angle / 2);

		return new QuantumGate("Rx", new ComplexMatrix(new Complex[,]
		{
			{c,s},
			{s,c}
		}), target);
	}

	public static QuantumGate Ry(int target, double angle)
	{
		var c = Math.Cos(angle / 2);
		var s = Math.Sin(angle / 2);

		return new QuantumGate("Ry", new ComplexMatrix(new Complex[,]
		{
			{c,-s},
			{s,c}
		}), target);
	}

	public static QuantumGate Rz(int target, double angle)
	{
		var p1 = Complex.FromPolarCoordinates(1, -angle / 2);
		var p2 = Complex.FromPolarCoordinates(1, angle / 2);

		return new QuantumGate("Rz", new ComplexMatrix(new Complex[,]
		{
			{p1,0},
			{0,p2}
		}), target);
	}

	public static QuantumGate CNOT(int control, int target)
	{
		return new QuantumGate("CNOT", null!, target, control);
	}

	public static QuantumGate CZ(int control, int target)
	{
		return new QuantumGate("CZ", null!, target, control);
	}

	public static QuantumGate Swap(int first, int second)
	{
		return new QuantumGate("SWAP", null!, first, second);
	}
}