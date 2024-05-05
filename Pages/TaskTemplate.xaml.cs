using ExperimentInterface.CustomControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExperimentInterface.Pages
{
    /// <summary>
    /// Interaction logic for TaskTemplate.xaml
    /// </summary>
    public partial class TaskTemplate : Page
    {
        public TaskTemplate()
        {
            InitializeComponent();
        }

        private void OnNextTaskButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnFeedbackSubmitted(object sender, RoutedEventArgs e)
        {
            TaskButton? ClickedButton = e.OriginalSource as TaskButton;

            if (ClickedButton == HumanButton)
                Trace.WriteLine("Send this item to a human worker in the future");
            else
                Trace.WriteLine("Send this item to the robot arm in the future");
        }
    }
}
