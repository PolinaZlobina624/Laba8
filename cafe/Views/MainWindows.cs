using Avalonia.Controls;
using System.Threading.Tasks;

namespace RestaurantApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                // Загрузка данных и установка в DataContext
                var employeeService = new EmployeeService(ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString);
                var employees = await employeeService.GetAllEmployeesAsync();
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    DataContext = employees;
                });
            });
        }
    }
}