using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CSQuantumSimulator.Quantum;

namespace CSQuantumSimulator.Services;

public class CircuitSerializationService
{
	private class GateData
	{
		public string Name { get; set; } = "";
		public int TargetQubit { get; set; }
		public int? ControlQubit { get; set; }
	}

	public void Save(QuantumCircuit circuit, string path)
	{
		var data = new List<GateData>();

		foreach (var gate in circuit.Gates)
		{
			data.Add(new GateData
			{
				Name = gate.Name,
				TargetQubit = gate.TargetQubit,
				ControlQubit = gate.ControlQubit
			});
		}

		File.WriteAllText(path, JsonSerializer.Serialize(data, new JsonSerializerOptions{WriteIndented = true}));
	}

	public QuantumCircuit Load(string path)
	{
		var json = File.ReadAllText(path);

		var data = JsonSerializer.Deserialize<List<GateData>>(json) ?? new();

		var circuit = new QuantumCircuit();

		foreach (var gate in data)
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
			_ => throw new InvalidDataException($"Неизвестный гейт: {gate.Name}")
		};
	}
}