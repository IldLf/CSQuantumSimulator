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
	private readonly SimulationService _simulation =
		new();

	private readonly VisualizationService
		_visualization = new();

	public ObservableCollection<StateEntryModel>
		StateEntries
	{ get; } = new();

	public ObservableCollection<string>
		Algorithms
	{ get; } = new();

	public QuantumCircuit CurrentCircuit
	{ get; private set; } = new();

	public int QubitCount { get; set; } = 2;

	private string _selectedAlgorithm =
		"Bell State";

	public string SelectedAlgorithm
	{
		get => _selectedAlgorithm;
		set
		{
			_selectedAlgorithm = value;
			Notify();
		}
	}

	public ICommand ExecuteCommand { get; }
	public ICommand ResetCommand { get; }

	public MainViewModel()
	{
		foreach (var algorithm
			in AlgorithmLibrary.GetAll())
		{
			Algorithms.Add(
				algorithm.Name);
		}

		ExecuteCommand =
			new RelayCommand(Execute);

		ResetCommand =
			new RelayCommand(Reset);
	}

	private void Execute()
	{
		var algorithm =
			AlgorithmLibrary
			.GetAll()
			.First(a =>
				a.Name ==
				SelectedAlgorithm);

		CurrentCircuit =
			algorithm.BuildCircuit();

		var register =
			_simulation.Run(
				CurrentCircuit,
				QubitCount);

		StateEntries.Clear();

		foreach (var state
			in _visualization
			.BuildStateEntries(register))
		{
			StateEntries.Add(state);
		}
	}

	private void Reset()
	{
		StateEntries.Clear();
		CurrentCircuit.Clear();
	}
}
