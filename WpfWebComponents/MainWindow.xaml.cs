using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfWebComponents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SimpleButton_Clicked(object sender, EventArgs e)
        {
            if(sender is SimpleButton button)
                button.ButtonText = "Ouch, that hurt!";

            var points = new List<Point>
            {
                new(40, 40),
                new(220, 50),
                new(200, 250),
                new(200, 320),
                new(50, 200)
            };


            Shaper.DrawPoints(points);

        }
    }
}