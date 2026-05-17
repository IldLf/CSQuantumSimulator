using System;

namespace CSQuantumSimulator.Quantum;

public class QuantumRegister
{
	public QuantumState State { get; }

	public int QubitCount => State.QubitCount;

	public QuantumRegister(int qubits)
	{
		State = new QuantumState(qubits);
	}

	public void ApplyGate(QuantumGate gate)
	{
		if (gate.Name == "CNOT")
		{
			ApplyCNOT(gate.ControlQubit!.Value, gate.TargetQubit);
			return;
		}

		if (gate.Name == "CZ")
		{
			ApplyCZ(gate.ControlQubit!.Value, gate.TargetQubit);
			return;
		}

		if (gate.Name == "SWAP")
		{
			ApplySwap(gate.TargetQubit, gate.ControlQubit!.Value);
			return;
		}

		if (gate.Matrix is null)
			throw new InvalidOperationException($"Гейт {gate.Name} не имеет матрицы");

		ApplySingleQubit(gate);
	}

	private void ApplySingleQubit(QuantumGate gate)
	{
		int size = State.Amplitudes.Length;
		var result = (System.Numerics.Complex[])State.Amplitudes.Clone();
		int bit = 1 << gate.TargetQubit;

		for (int i = 0; i < size; i++)
		{
			if ((i & bit) != 0)
				continue;

			int zero = i;
			int one = i | bit;

			var a = State.Amplitudes[zero];
			var b = State.Amplitudes[one];

			result[zero] = gate.Matrix[0, 0] * a + gate.Matrix[0, 1] * b;
			result[one] = gate.Matrix[1, 0] * a + gate.Matrix[1, 1] * b;
		}

		State.SetAmplitudes(result);
	}

	private void ApplyCNOT(int control, int target)
	{
		int size = State.Amplitudes.Length;
		int cMask = 1 << control;
		int tMask = 1 << target;

		var result = (System.Numerics.Complex[])State.Amplitudes.Clone();

		for (int i = 0; i < size; i++)
		{
			if ((i & cMask) != 0 && (i & tMask) == 0)
			{
				int swapped = i | tMask;
				(result[i], result[swapped]) = (result[swapped], result[i]);
			}
		}

		State.SetAmplitudes(result);
	}

	private void ApplyCZ(int control, int target)
	{
		int size = State.Amplitudes.Length;
		int cMask = 1 << control;
		int tMask = 1 << target;

		var result = (System.Numerics.Complex[])State.Amplitudes.Clone();

		for (int i = 0; i < size; i++)
		{
			if ((i & cMask) != 0 && (i & tMask) != 0)
				result[i] *= -1;
		}

		State.SetAmplitudes(result);
	}

	private void ApplySwap(int first, int second)
	{
		int size = State.Amplitudes.Length;

		int firstMask = 1 << first;
		int secondMask = 1 << second;

		var result = (System.Numerics.Complex[])State.Amplitudes.Clone();

		for (int i = 0; i < size; i++)
		{
			int firstBit = i & firstMask;
			int secondBit = i & secondMask;

			if ((firstBit == 0 && secondBit != 0) || (firstBit != 0 && secondBit == 0))
			{
				int swapped = i ^ firstMask ^ secondMask;

				if (i < swapped)
					(result[i], result[swapped]) = (result[swapped], result[i]);
			}
		}

		State.SetAmplitudes(result);
	}

	public int Measure()
	{
		var probs = State.GetProbabilities();
		double rand = Random.Shared.NextDouble();
		double cumulative = 0;

		for (int i = 0; i < probs.Length; i++)
		{
			cumulative += probs[i];

			if (rand <= cumulative)
			{
				State.Reset();
				State.Amplitudes[i] = 1;
				return i;
			}
		}

		return 0;
	}

	public int SampleMeasurement()
	{
		var probs = State.GetProbabilities();

		double rand = Random.Shared.NextDouble();
		double cumulative = 0;

		for (int i = 0; i < probs.Length; i++)
		{
			cumulative += probs[i];

			if (rand <= cumulative) { return i; }
		}

		return 0;
	}
}