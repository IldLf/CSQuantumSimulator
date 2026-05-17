using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Services;

public class CircuitSerializationService
{
	public class CircuitData
	{
		public string SelectedAlgorithm { get; set; } = "";
		public int QubitCount { get; set; }
		public int SelectedQubit { get; set; }
		public int ControlQubit { get; set; }
		public List<GateData> Gates { get; set; } = new();
	}

	public class GateData
	{
		public string Name { get; set; } = "";
		public int TargetQubit { get; set; }
		public int? ControlQubit { get; set; }
	}

	public void Save(CircuitData circuit, string path)
	{
		File.WriteAllText(path, JsonSerializer.Serialize(circuit, new JsonSerializerOptions { WriteIndented = true }));
	}

	public CircuitData Load(string path)
	{
		var json = File.ReadAllText(path);
		return JsonSerializer.Deserialize<CircuitData>(json) ?? new();
	}

	public QuantumCircuit BuildCircuit(CircuitData data)
	{
		var circuit = new QuantumCircuit();

		foreach (var gate in data.Gates)
			circuit.AddGate(CreateGate(gate));

		return circuit;
	}

	private QuantumGate CreateGate(GateData gate)
	{
		return gate.Name switch
		{
			"H" => Gates.Hadamard(gate.TargetQubit),
			"X" => Gates.PauliX(gate.TargetQubit),
			"Y" => Gates.PauliY(gate.TargetQubit),
			"Z" => Gates.PauliZ(gate.TargetQubit),
			"S" => Gates.PhaseS(gate.TargetQubit),
			"T" => Gates.PhaseT(gate.TargetQubit),
			"Rx" => Gates.Rx(gate.TargetQubit, Math.PI / 2),
			"Ry" => Gates.Ry(gate.TargetQubit, Math.PI / 2),
			"Rz" => Gates.Rz(gate.TargetQubit, Math.PI / 2),
			"CNOT" => Gates.CNOT(gate.ControlQubit!.Value, gate.TargetQubit),
			"CZ" => Gates.CZ(gate.ControlQubit!.Value, gate.TargetQubit),
			"SWAP" => Gates.Swap(gate.ControlQubit!.Value, gate.TargetQubit),
			_ => throw new InvalidDataException()
		};
	}
}