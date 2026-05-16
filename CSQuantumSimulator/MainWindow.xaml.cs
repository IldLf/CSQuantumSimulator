using System.Windows;
using CSQuantumSimulator.ViewModels;

namespace CSQuantumSimulator;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();

		DataContext = new MainViewModel();
	}
}