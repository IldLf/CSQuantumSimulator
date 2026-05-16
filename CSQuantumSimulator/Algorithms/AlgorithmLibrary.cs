//реестр алгоритмов

//Возвращает список.

using System.Collections.Generic;

namespace CSQuantumSimulator.Algorithms;

public static class AlgorithmLibrary
{
	public static List<IQuantumAlgorithm> GetAll()
	{
		return new()
		{
			new BellStateAlgorithm()
		};
	}
}