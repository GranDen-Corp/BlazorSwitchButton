using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using WpfBlazorDemo.Data;

namespace WpfBlazorDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWpfBlazorWebView();

#if DEBUG
            serviceCollection.AddBlazorWebViewDeveloperTools();

            serviceCollection.AddLogging(logging => logging.AddDebug());
#endif

            serviceCollection.AddSingleton<WeatherForecastService>();

            Resources.Add("services", serviceCollection.BuildServiceProvider());
        }
    }
}
