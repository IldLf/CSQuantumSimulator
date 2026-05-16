//преобразует данные для UI

//например:

//state → строки таблицы

using CSQuantumSimulator.Helpers;
using CSQuantumSimulator.Models;
using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Services;

public class VisualizationService
{
	public List<StateEntryModel> BuildStateEntries(
		QuantumRegister register)
	{
		var result = new List<StateEntryModel>();

		var probs = register.State.GetProbabilities();

		for (int i = 0; i < probs.Length; i++)
		{
			result.Add(new StateEntryModel
			{
				Basis = BasisStateHelper.Format(
					i,
					register.QubitCount),

				Amplitude =
					ComplexFormatter.Format(
						register.State.Amplitudes[i]),

				Probability =
					ProbabilityHelper.Percent(
						probs[i])
			});
		}

		return result;
	}
}
