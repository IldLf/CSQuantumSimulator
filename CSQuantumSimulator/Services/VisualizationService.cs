//преобразует данные для UI

//например:

//state → строки таблицы

using System.Collections.ObjectModel;
using CSQuantumSimulator.Helpers;
using CSQuantumSimulator.Models;
using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Services;

public class VisualizationService
{
	public List<StateEntryModel> BuildStateEntries(QuantumRegister register)
	{
		var result = new List<StateEntryModel>();
		var probabilities = register.State.GetProbabilities();

		for (int i = 0; i < probabilities.Length; i++)
		{
			result.Add(new StateEntryModel
			{
				Basis = BasisStateHelper.Format(i, register.QubitCount),
				Amplitude = ComplexFormatter.Format(register.State.Amplitudes[i]),
				Probability = ProbabilityHelper.Percent(probabilities[i])
			});
		}

		return result;
	}

	public ObservableCollection<CircuitRowModel> BuildCircuit(QuantumCircuit circuit, int qubits, int step)
	{
		var rows = new ObservableCollection<CircuitRowModel>();

		for (int q = 0; q < qubits; q++)
		{
			rows.Add(new CircuitRowModel
			{
				Qubit = $"q{q}"
			});
		}

		for (int i = 0; i < circuit.Gates.Count; i++)
		{
			var gate = circuit.Gates[i];

			for (int q = 0; q < qubits; q++)
			{
				rows[q].Cells.Add(new CircuitCellModel
				{
					Label = "─",
					IsActive = i == step
				});
			}

			if (gate.ControlQubit.HasValue)
			{
				rows[gate.ControlQubit.Value].Cells[i].Label = "●";
				rows[gate.TargetQubit].Cells[i].Label = gate.Name == "SWAP" ? "×" : gate.Name;
			}
			else
			{
				rows[gate.TargetQubit].Cells[i].Label = gate.Name;
			}
		}

		return rows;
	}
}