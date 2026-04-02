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

namespace Pr_15
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary> 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var manufacturers = Core.Context.Manufacturers.ToList();
            manufacturers.Insert(0, new Manufacturers { Name = "Все производители", ID = 0 });
            ComboBrand.ItemsSource = manufacturers;
            ComboBrand.SelectedIndex = 0;


            var categories = Core.Context.Categories.ToList();
            categories.Insert(0, new Categories { Name = "Все категории", ID = 0 });
            ComboCategory.ItemsSource = categories;
            ComboCategory.SelectedIndex = 0;

            UpdateFilters(null, null);
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var selected = LvComponents.SelectedItem as Components;
            if (selected == null) return;

            var currentItems = LbCurrentBuild.Items.Cast<Components>().ToList();

            if (selected.IsCpu)
            {
                var mb = currentItems.FirstOrDefault(x => x.IsMotherboard);
                if (mb != null && mb.Socket != selected.Socket)
                {
                    MessageBox.Show("Процессор не подходит к сокету материнской платы!");
                    return;
                }
            }

            if (selected.IsRAM)
            {
                var mb = currentItems.FirstOrDefault(x => x.IsMotherboard);
                if (mb != null && mb.RamType != selected.RamType)
                {
                    MessageBox.Show("Тип оперативной памяти не поддерживается платой!");
                    return;
                }
            }

            LbCurrentBuild.Items.Add(selected);
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            decimal total = LbCurrentBuild.Items.Cast<Components>().Sum(x => x.Price);
            TbTotal.Text = $"Итого: {total} руб.";
        }

        private void UpdateFilters(object sender, EventArgs e)
        {
            if (Core.Context == null) return;
            var list = Core.Context.Components.ToList();

            if (!string.IsNullOrWhiteSpace(TxtSearch.Text))
                list = list.Where(x => x.ModelName.ToLower().Contains(TxtSearch.Text.ToLower())).ToList();

            if (ComboBrand.SelectedItem is Manufacturers brand && brand.ID > 0)
                list = list.Where(x => x.ManufacturerID == brand.ID).ToList();

            if (ComboCategory.SelectedItem is Categories cat && cat.ID > 0)
                list = list.Where(x => x.CategoryID == cat.ID).ToList();

            LvComponents.ItemsSource = list;
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBuildName.Text))
            {
                MessageBox.Show("Введите название сборки!");
                return;
            }

            if (LbCurrentBuild.Items.Count == 0)
            {
                MessageBox.Show("Сборка пуста!");
                return;
            }

            try
            {
                var newBuild = new Builds
                {
                    BuildName = TxtBuildName.Text,
                    AuthorName = "Abazov", 
                    DateCreated = DateTime.Now
                };

                Core.Context.Builds.Add(newBuild);
                Core.Context.SaveChanges(); 


                foreach (Components comp in LbCurrentBuild.Items)
                {
                    var link = new BuildComponents
                    {
                        BuildID = newBuild.ID,
                        ComponentID = comp.ID
                    };
                    Core.Context.BuildComponents.Add(link);
                }

                Core.Context.SaveChanges();
                MessageBox.Show("Сборка успешно сохранена в БД!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }
        private void BtnViewSaved_Click(object sender, RoutedEventArgs e)
        {
            SavedBuildsWindow win = new SavedBuildsWindow();
            win.ShowDialog();
        }
        private void BtnOpenHistory_Click(object sender, RoutedEventArgs e)
        {
            SavedBuildsWindow historyWindow = new SavedBuildsWindow();

            historyWindow.ShowDialog();
        }
    }
}
