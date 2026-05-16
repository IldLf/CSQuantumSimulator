//перевод индекса в

//| 0101⟩

namespace CSQuantumSimulator.Helpers;

public static class BasisStateHelper
{
	public static string Format(int index, int qubits)
	{
		return "|" + Convert.ToString(index, 2).PadLeft(qubits, '0') + "⟩";
	}
}