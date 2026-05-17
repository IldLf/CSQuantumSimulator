using System.Collections.ObjectModel;

namespace CSQuantumSimulator.Models;

public class CircuitRowModel
{
	public string Qubit { get; set; } = "";
	public ObservableCollection<CircuitCellModel> Cells { get; set; } = new();
}