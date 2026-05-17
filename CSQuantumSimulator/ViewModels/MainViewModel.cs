using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Win32;
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
	private readonly CircuitSerializationService serialization = new();

	private readonly List<IQuantumAlgorithm> algorithmObjects;

	private QuantumRegister? register;
	private int currentStep;

	private int qubitCount = 2;
	private int selectedQubit;
	private int controlQubit = 1;

	private double executionTime;
	private double memoryUsage;

	public ObservableCollection<StateEntryModel> StateEntries { get; } = new();
	public ObservableCollection<string> Algorithms { get; } = new();
	public ObservableCollection<CircuitRowModel> CircuitRows { get; } = new();

	public QuantumCircuit CurrentCircuit { get; private set; } = new();

	public int QubitCount
	{
		get => qubitCount;
		set
		{
			if (value < 1)
				value = 1;

			if (value > 20)
				value = 20;

			qubitCount = value;

			if (selectedQubit >= qubitCount)
				selectedQubit = qubitCount - 1;

			if (controlQubit >= qubitCount)
				controlQubit = qubitCount - 1;

			SwitchToCustom();
			Notify();
			Notify(nameof(SelectedQubit));
			Notify(nameof(ControlQubit));

			RenderCircuit();
		}
	}

	public int SelectedQubit
	{
		get => selectedQubit;
		set
		{
			if (value >= 0 && value < QubitCount)
			{
				selectedQubit = value;
				SwitchToCustom();
				Notify();
			}
		}
	}

	public int ControlQubit
	{
		get => controlQubit;
		set
		{
			if (value >= 0 && value < QubitCount)
			{
				controlQubit = value;
				SwitchToCustom();
				Notify();
			}
		}
	}

	public double ExecutionTime
	{
		get => executionTime;
		set
		{
			executionTime = value;
			Notify();
		}
	}

	public double MemoryUsage
	{
		get => memoryUsage;
		set
		{
			memoryUsage = value;
			Notify();
		}
	}

	private string selectedAlgorithm = "Произвольный алгоритм";

	public string SelectedAlgorithm
	{
		get => selectedAlgorithm;
		set
		{
			selectedAlgorithm = value;
			LoadAlgorithm();
			Notify();
		}
	}

	public ICommand ExecuteCommand { get; }
	public ICommand StepCommand { get; }
	public ICommand ResetCommand { get; }
	public ICommand MeasureCommand { get; }

	public ICommand SaveCommand { get; }
	public ICommand LoadCommand { get; }

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
	public ICommand AddCnotCommand { get; }
	public ICommand AddCzCommand { get; }

	public MainViewModel()
	{
		algorithmObjects = AlgorithmLibrary.GetAll();

		foreach (var algorithm in algorithmObjects)
			Algorithms.Add(algorithm.Name);

		ExecuteCommand = new RelayCommand(Execute);
		StepCommand = new RelayCommand(Step);
		ResetCommand = new RelayCommand(Reset);
		MeasureCommand = new RelayCommand(Measure);

		SaveCommand = new RelayCommand(Save);
		LoadCommand = new RelayCommand(Load);

		AddHCommand = new RelayCommand(() => AddGate(Gates.Hadamard(SelectedQubit)));
		AddXCommand = new RelayCommand(() => AddGate(Gates.PauliX(SelectedQubit)));
		AddYCommand = new RelayCommand(() => AddGate(Gates.PauliY(SelectedQubit)));
		AddZCommand = new RelayCommand(() => AddGate(Gates.PauliZ(SelectedQubit)));
		AddSCommand = new RelayCommand(() => AddGate(Gates.PhaseS(SelectedQubit)));
		AddTCommand = new RelayCommand(() => AddGate(Gates.PhaseT(SelectedQubit)));
		AddRxCommand = new RelayCommand(() => AddGate(Gates.Rx(SelectedQubit, Math.PI / 2)));
		AddRyCommand = new RelayCommand(() => AddGate(Gates.Ry(SelectedQubit, Math.PI / 2)));
		AddRzCommand = new RelayCommand(() => AddGate(Gates.Rz(SelectedQubit, Math.PI / 2)));
		AddCnotCommand = new RelayCommand(() => AddGate(Gates.CNOT(ControlQubit, SelectedQubit)));
		AddCzCommand = new RelayCommand(() => AddGate(Gates.CZ(ControlQubit, SelectedQubit)));
		AddSwapCommand = new RelayCommand(() => AddGate(Gates.Swap(SelectedQubit, ControlQubit)));

		LoadAlgorithm();
	}

	private void LoadAlgorithm()
	{
		var algorithm = algorithmObjects.First(x => x.Name == SelectedAlgorithm);

		CurrentCircuit = algorithm.BuildCircuit();

		QubitCount = SelectedAlgorithm switch
		{
			"Квантовая телепортация" => 3,
			"Алгоритм Бернштейна–Вазирани" => 3,
			_ => 2
		};

		currentStep = 0;
		register = null;

		ExecutionTime = 0;
		MemoryUsage = 0;

		RenderCircuit();
		Render();
	}

	private void Save()
	{
		var dialog = new SaveFileDialog();
		dialog.Filter = "JSON|*.json";

		if (dialog.ShowDialog() == true)
			serialization.Save(CurrentCircuit, dialog.FileName);
	}

	private void Load()
	{
		var dialog = new OpenFileDialog();
		dialog.Filter = "JSON|*.json";

		if (dialog.ShowDialog() == true)
		{
			CurrentCircuit = serialization.Load(dialog.FileName);
			currentStep = 0;
			register = null;
			RenderCircuit();
			Render();
		}
	}

	private void AddGate(QuantumGate gate)
	{
		if (gate.TargetQubit >= QubitCount)
			return;

		if (gate.ControlQubit.HasValue && gate.ControlQubit.Value >= QubitCount)
			return;

		if (gate.ControlQubit == gate.TargetQubit)
			return;

		SwitchToCustom();
		CurrentCircuit.AddGate(gate);
		RenderCircuit();
	}

	private void Execute()
	{
		GC.Collect();

		var before = GC.GetTotalMemory(true);

		var watch = Stopwatch.StartNew();

		register = simulation.Run(CurrentCircuit, QubitCount);

		watch.Stop();

		var after = GC.GetTotalMemory(false);

		ExecutionTime = watch.ElapsedMilliseconds;
		MemoryUsage = (after - before) / 1024d;

		currentStep = CurrentCircuit.Gates.Count;

		RenderCircuit();
		Render();
	}

	private void Step()
	{
		if (register is null)
			register = new QuantumRegister(QubitCount);

		if (currentStep >= CurrentCircuit.Gates.Count)
			return;

		register.ApplyGate(CurrentCircuit.Gates[currentStep]);

		currentStep++;

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
		{
			StateEntries.Add(state);
		}
	}

	private void RenderCircuit()
	{
		CircuitRows.Clear();

		foreach (var row in visualization.BuildCircuit(CurrentCircuit, QubitCount, currentStep))
		{
			CircuitRows.Add(row);
		}
	}

	private void Reset()
	{
		register = null;
		currentStep = 0;
		ExecutionTime = 0;
		MemoryUsage = 0;

		RenderCircuit();
		StateEntries.Clear();
	}

	private void SwitchToCustom()
	{
		if (SelectedAlgorithm != "Произвольный алгоритм")
		{
			selectedAlgorithm = "Произвольный алгоритм";
		}
	}
}