//using System.Numerics;

//состояние регистра

//Что внутри:

//Complex[] Amplitudes
//int QubitCount

//Методы:

//Normalize()
//Measure()
//Clone()
//Reset()

using System;
using System.Linq;
using System.Numerics;

namespace CSQuantumSimulator.Quantum;

public class QuantumState
{
	public Complex[] Amplitudes { get; private set; }

	public int QubitCount { get; }

	public QuantumState(int qubitCount)
	{
		QubitCount = qubitCount;

		int size = 1 << qubitCount;

		Amplitudes = new Complex[size];
		Amplitudes[0] = Complex.One;
	}

	public void SetAmplitudes(Complex[] amplitudes)
	{
		Amplitudes = amplitudes;
		Normalize();
	}

	public void Normalize()
	{
		double norm = Math.Sqrt(
			Amplitudes.Sum(a => a.Magnitude * a.Magnitude));

		if (norm == 0)
			throw new InvalidOperationException();

		for (int i = 0; i < Amplitudes.Length; i++)
			Amplitudes[i] /= norm;
	}

	public double[] GetProbabilities()
	{
		return Amplitudes
			.Select(a => a.Magnitude * a.Magnitude)
			.ToArray();
	}

	public QuantumState Clone()
	{
		var clone = new QuantumState(QubitCount);

		clone.SetAmplitudes((Complex[])Amplitudes.Clone());

		return clone;
	}

	public void Reset()
	{
		Array.Clear(Amplitudes);

		Amplitudes[0] = Complex.One;
	}
}