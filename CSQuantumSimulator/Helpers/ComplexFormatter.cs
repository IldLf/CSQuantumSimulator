//красивый вывод

//0.707 + 0.000i

using System.Numerics;

namespace CSQuantumSimulator.Helpers;

public static class ComplexFormatter
{
	public static string Format(Complex value)
	{
		return $"{value.Real:F3} {(value.Imaginary < 0 ? "-" : "+")} {Math.Abs(value.Imaginary):F3}i";
	}
}
