// INotifyPropertyChanged

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CSQuantumSimulator.ViewModels;

public class BaseViewModel :
	INotifyPropertyChanged
{
	public event PropertyChangedEventHandler?
		PropertyChanged;

	protected void Notify(
		[CallerMemberName]
		string? property = null)
	{
		PropertyChanged?.Invoke(
			this,
			new PropertyChangedEventArgs(property));
	}
}
