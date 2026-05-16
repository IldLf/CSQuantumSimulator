// команды WPF

using System.Windows.Input;

namespace CSQuantumSimulator.ViewModels;

public class RelayCommand : ICommand
{
	private readonly Action _execute;

	public event EventHandler? CanExecuteChanged;

	public RelayCommand(Action execute)
	{
		_execute = execute;
	}

	public bool CanExecute(object? parameter)
	{
		return true;
	}

	public void Execute(object? parameter)
	{
		_execute();
	}
}