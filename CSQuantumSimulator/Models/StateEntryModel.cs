//Basis
//Amplitude
//Probability

namespace CSQuantumSimulator.Models;

public class StateEntryModel
{
	public string Basis { get; set; } = "";
	public string Amplitude { get; set; } = "";
	public double Probability { get; set; }
	public double BarWidth => Probability * 4;
}