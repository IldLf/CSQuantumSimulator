//главная логика приложения

//Содержит:

//CurrentCircuit
//CurrentRegister
//SelectedAlgorithm
//ExecuteCommand
//MeasureCommand
//ResetCommand
//AddGateCommand

//Это мозг программы.

using System.Collections.ObjectModel;
using System.Windows.Input;
using CSQuantumSimulator.Algorithms;
using CSQuantumSimulator.Models;
using CSQuantumSimulator.Quantum;
using CSQuantumSimulator.Services;

namespace CSQuantumSimulator.ViewModels;

public class MainViewModel : BaseViewModel
{
	private readonly SimulationService simulation = new();
	private readonly VisualizationService visualization = new();
	private readonly MeasurementService measurement = new();

	private QuantumRegister? register;

	public ObservableCollection<StateEntryModel> StateEntries { get; } = new();
	public ObservableCollection<string> Algorithms { get; } = new();
	public ObservableCollection<string> CircuitLines { get; } = new();
	public ObservableCollection<int> AvailableQubits { get; } = new();

	public QuantumCircuit CurrentCircuit { get; private set; } = new();

	public int QubitCount { get; set; } = 2;

	public int SelectedQubit { get; set; }

	private string selectedAlgorithm = "Состояние Белла";

	public string SelectedAlgorithm
	{
		get => selectedAlgorithm;
		set
		{
			selectedAlgorithm = value;
			Notify();
		}
	}

	public ICommand ExecuteCommand { get; }
	public ICommand ResetCommand { get; }
	public ICommand MeasureCommand { get; }

	public ICommand AddHCommand { get; }
	public ICommand AddXCommand { get; }
	public ICommand AddYCommand { get; }
	public ICommand AddZCommand { get; }
	public ICommand AddSCommand { get; }
	public ICommand AddTCommand { get; }

	public ICommand AddRxCommand { get; }
	public ICommand AddRyCommand { get; }
	public ICommand AddRzCommand { get; }

	public ICommand AddSwapCommand { get; }

	public MainViewModel()
	{
		foreach (var algorithm in AlgorithmLibrary.GetAll())
			Algorithms.Add(algorithm.Name);

		for (int i = 0; i < 8; i++)
			AvailableQubits.Add(i);

		ExecuteCommand = new RelayCommand(Execute);
		ResetCommand = new RelayCommand(Reset);
		MeasureCommand = new RelayCommand(Measure);

		AddHCommand = new RelayCommand(() => AddGate(Gates.Hadamard(SelectedQubit)));
		AddXCommand = new RelayCommand(() => AddGate(Gates.PauliX(SelectedQubit)));
		AddYCommand = new RelayCommand(() => AddGate(Gates.PauliY(SelectedQubit)));
		AddZCommand = new RelayCommand(() => AddGate(Gates.PauliZ(SelectedQubit)));
		AddSCommand = new RelayCommand(() => AddGate(Gates.PhaseS(SelectedQubit)));
		AddTCommand = new RelayCommand(() => AddGate(Gates.PhaseT(SelectedQubit)));

		AddRxCommand = new RelayCommand(() => AddGate(Gates.Rx(SelectedQubit, Math.PI / 2)));
		AddRyCommand = new RelayCommand(() => AddGate(Gates.Ry(SelectedQubit, Math.PI / 2)));
		AddRzCommand = new RelayCommand(() => AddGate(Gates.Rz(SelectedQubit, Math.PI / 2)));

		AddSwapCommand = new RelayCommand(() =>
		{
			if (QubitCount < 2)
				return;

			AddGate(Gates.Swap(0, 1));
		});
	}

	private void AddGate(QuantumGate gate)
	{
		CurrentCircuit.AddGate(gate);
		RenderCircuit();
	}

	private void Execute()
	{
		try
		{
			QubitCount = Math.Clamp(QubitCount, 1, 8);

			if (CurrentCircuit.Gates.Count == 0)
			{
				var algorithm = AlgorithmLibrary.GetAll().First(x => x.Name == SelectedAlgorithm);
				CurrentCircuit = algorithm.BuildCircuit();
			}

			register = simulation.Run(CurrentCircuit, QubitCount);

			RenderCircuit();
			Render();
		}
		catch (Exception exception)
		{
			StateEntries.Clear();

			StateEntries.Add(new StateEntryModel
			{
				Basis = "Ошибка",
				Amplitude = exception.Message,
				Probability = -1
			});
		}
	}

	private void Measure()
	{
		if (register is null)
			return;

		StateEntries.Clear();
		StateEntries.Add(measurement.Measure(register));
	}

	private void Render()
	{
		if (register is null)
			return;

		StateEntries.Clear();

		foreach (var state in visualization.BuildStateEntries(register))
			StateEntries.Add(state);
	}

	private void RenderCircuit()
	{
		CircuitLines.Clear();

		foreach (var gate in CurrentCircuit.Gates)
		{
			if (gate.ControlQubit.HasValue)
				CircuitLines.Add($"{gate.Name} q{gate.ControlQubit}, q{gate.TargetQubit}");
			else
				CircuitLines.Add($"{gate.Name} q{gate.TargetQubit}");
		}
	}

	private void Reset()
	{
		StateEntries.Clear();
		CircuitLines.Clear();
		CurrentCircuit.Clear();
		register = null;
	}
}