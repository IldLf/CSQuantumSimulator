using CSQuantumSimulator.Helpers;
using CSQuantumSimulator.Models;
using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Services;

public class MeasurementService
{
	public List<StateEntryModel> MeasureMany(QuantumRegister register, int iterations)
	{
		var counts = new Dictionary<int, int>();

		for (int i = 0; i < iterations; i++)
		{
			int value = register.SampleMeasurement();

			if (!counts.ContainsKey(value))
				counts[value] = 0;

			counts[value]++;
		}

		var result = new List<StateEntryModel>();

		foreach (var pair in counts.OrderBy(x => x.Key))
		{
			double probability = (double)pair.Value / iterations * 100.0;

			result.Add(new StateEntryModel
			{
				Basis = BasisStateHelper.Format(
				pair.Key, 
				register.QubitCount),
				Amplitude = $"{pair.Value} / {iterations}",
				Probability = probability
			});
		}

		return result;
	}
}