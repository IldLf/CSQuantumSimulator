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

	public QuantumCircuit CurrentCircuit { get; private set; } = new();

	public int QubitCount { get; set; } = 2;

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
	public ICommand AddSCommand { get; }
	public ICommand AddTCommand { get; }
	public ICommand AddSwapCommand { get; }

	public MainViewModel()
	{
		foreach (var algorithm in AlgorithmLibrary.GetAll())
			Algorithms.Add(algorithm.Name);

		ExecuteCommand = new RelayCommand(Execute);
		ResetCommand = new RelayCommand(Reset);
		MeasureCommand = new RelayCommand(Measure);

		AddHCommand = new RelayCommand(() => AddGate(Gates.Hadamard(0)));
		AddXCommand = new RelayCommand(() => AddGate(Gates.PauliX(0)));
		AddSCommand = new RelayCommand(() => AddGate(Gates.PhaseS(0)));
		AddTCommand = new RelayCommand(() => AddGate(Gates.PhaseT(0)));
		AddSwapCommand = new RelayCommand(() => AddGate(Gates.Swap(0, 1)));
	}

	private void AddGate(QuantumGate gate)
	{
		CurrentCircuit.AddGate(gate);
		RenderCircuit();
	}

	private void Execute()
	{
		if (CurrentCircuit.Gates.Count == 0)
		{
			var algorithm = AlgorithmLibrary.GetAll().First(a => a.Name == SelectedAlgorithm);
			CurrentCircuit = algorithm.BuildCircuit();
		}

		register = simulation.Run(CurrentCircuit, QubitCount);

		RenderCircuit();
		Render();
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
			CircuitLines.Add($"{gate.Name} q{gate.TargetQubit}");
	}

	private void Reset()
	{
		StateEntries.Clear();
		CircuitLines.Clear();
		CurrentCircuit.Clear();
		register = null;
	}
}