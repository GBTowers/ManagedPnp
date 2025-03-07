using Avalonia.Controls;
using Splat;

namespace ManagedPnp.Avalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    [DependencyInjectionConstructor]
    public MainWindow(MainViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}