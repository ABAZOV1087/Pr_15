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
using System.Windows.Shapes;

namespace Pr_15
{
    public partial class SavedBuildsWindow : Window
    {
        public SavedBuildsWindow()
        {
            InitializeComponent();
            LvBuilds.ItemsSource = Core.Context.Builds.ToList();
        }

        private void LvBuilds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvBuilds.SelectedItem is Builds selectedBuild)
            {
                var components = Core.Context.BuildComponents
                    .Where(x => x.BuildID == selectedBuild.ID)
                    .Select(x => x.Components)
                    .ToList();

                LvDetails.ItemsSource = components;
            }
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
