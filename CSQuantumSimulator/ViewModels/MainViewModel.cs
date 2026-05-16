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

	private readonly MeasurementService	measurement = new();

	private QuantumRegister? register;

	public ObservableCollection<StateEntryModel> StateEntries { get; } = new();

	public ObservableCollection<string> Algorithms { get; } = new();

	public QuantumCircuit CurrentCircuit { get; private set; } = new();

	public int QubitCount { get; set; } = 2;

	private string selectedAlgorithm =	"Состояние Белла";

	public string SelectedAlgorithm
	{
		get => selectedAlgorithm;
		set	{
			selectedAlgorithm = value;
			Notify();
		}
	}

	public ICommand ExecuteCommand { get; }
	public ICommand ResetCommand { get; }
	public ICommand MeasureCommand { get; }

	public MainViewModel()
	{
		foreach (var algorithm in AlgorithmLibrary.GetAll())
		{
			Algorithms.Add(algorithm.Name);
		}

		ExecuteCommand = new RelayCommand(Execute);

		ResetCommand = new RelayCommand(Reset);

		MeasureCommand = new RelayCommand(Measure);
	}

	private void Execute()
	{
		var algorithm =	AlgorithmLibrary.GetAll().First(a => a.Name == SelectedAlgorithm);

		CurrentCircuit = algorithm.BuildCircuit();

		register = simulation.Run(CurrentCircuit, QubitCount);

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
		{
			StateEntries.Add(state);
		}
	}

	private void Reset()
	{
		StateEntries.Clear();
		CurrentCircuit.Clear();
		register = null;
	}
}
