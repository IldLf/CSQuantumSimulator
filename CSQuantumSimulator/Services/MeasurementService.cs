//логика измерения

using CSQuantumSimulator.Helpers;
using CSQuantumSimulator.Models;
using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Services;

public class MeasurementService
{
	public StateEntryModel Measure(QuantumRegister register)
	{
		int value = register.Measure();

		return new StateEntryModel
		{
			Basis = BasisStateHelper.Format(value, register.QubitCount),

			Amplitude = "1.000 + 0.000i",

			Probability = 100
		};
	}
}