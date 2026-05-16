//матрица комплексных чисел

//Что будет:

//хранение Complex[,]
//Multiply
//TensorProduct
//Identity
//ApplyToVector

//Это основа всех гейтов.

using System;
using System.Numerics;

namespace CSQuantumSimulator.Quantum;

public class ComplexMatrix
{
	private readonly Complex[,] _data;

	public int Rows { get; }
	public int Columns { get; }

	public Complex this[int row, int col]
	{
		get => _data[row, col];
		set => _data[row, col] = value;
	}

	public ComplexMatrix(int rows, int columns)
	{
		Rows = rows;
		Columns = columns;
		_data = new Complex[rows, columns];
	}

	public ComplexMatrix(Complex[,] data)
	{
		Rows = data.GetLength(0);
		Columns = data.GetLength(1);
		_data = data;
	}

	public static ComplexMatrix Identity(int size)
	{
		var result = new ComplexMatrix(size, size);

		for (int i = 0; i < size; i++)
			result[i, i] = Complex.One;

		return result;
	}

	public Complex[] Multiply(Complex[] vector)
	{
		if (Columns != vector.Length)
			throw new ArgumentException("Dimension mismatch");

		Complex[] result = new Complex[Rows];

		for (int i = 0; i < Rows; i++)
		{
			Complex sum = Complex.Zero;

			for (int j = 0; j < Columns; j++)
				sum += _data[i, j] * vector[j];

			result[i] = sum;
		}

		return result;
	}

	public static ComplexMatrix operator *(ComplexMatrix a, ComplexMatrix b)
	{
		if (a.Columns != b.Rows)
			throw new ArgumentException("Dimension mismatch");

		var result = new ComplexMatrix(a.Rows, b.Columns);

		for (int i = 0; i < a.Rows; i++)
			for (int j = 0; j < b.Columns; j++)
			{
				Complex sum = Complex.Zero;

				for (int k = 0; k < a.Columns; k++)
					sum += a[i, k] * b[k, j];

				result[i, j] = sum;
			}

		return result;
	}

	public static ComplexMatrix TensorProduct(
		ComplexMatrix a,
		ComplexMatrix b)
	{
		var result = new ComplexMatrix(
			a.Rows * b.Rows,
			a.Columns * b.Columns);

		for (int i = 0; i < a.Rows; i++)
			for (int j = 0; j < a.Columns; j++)
				for (int k = 0; k < b.Rows; k++)
					for (int l = 0; l < b.Columns; l++)
					{
						result[
							i * b.Rows + k,
							j * b.Columns + l] = a[i, j] * b[k, l];
					}

		return result;
	}
}
