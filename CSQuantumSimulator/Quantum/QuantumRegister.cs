//главный квантовый регистр

//Методы:

//ApplyGate()
//Measure()
//GetProbabilities()

//Использует QuantumState.

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

		if (gate.Name == "SWAP")
		{
			ApplySwap(gate.TargetQubit, gate.ControlQubit!.Value);
			return;
		}

		ApplySingleQubit(gate);
	}

	private void ApplySingleQubit(QuantumGate gate)
	{
		int size = State.Amplitudes.Length;
		var result = new System.Numerics.Complex[size];
		int bit = 1 << gate.TargetQubit;

		for (int i = 0; i < size; i++)
		{
			int zero = i & ~bit;
			int one = zero | bit;

			result[zero] =
				gate.Matrix[0, 0] * State.Amplitudes[zero]
				+ gate.Matrix[0, 1] * State.Amplitudes[one];

			result[one] =
				gate.Matrix[1, 0] * State.Amplitudes[zero]
				+ gate.Matrix[1, 1] * State.Amplitudes[one];
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
			if ((i & cMask) != 0)
			{
				int swapped = i ^ tMask;
				(result[i], result[swapped]) = (result[swapped], result[i]);
			}
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
}