using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Syncfusion.Windows.Tools;

namespace UI
{
    /// <summary>
    /// Interaction logic for NextShabatWindow.xaml
    /// </summary>
    public partial class NextShabatWindow
    {
        readonly BL.IBL bl;

        public NextShabatWindow()
        {
            InitializeComponent();
            bl = BL.FactoryBl.getBL();
            int[] numbers = new int[9];
            for (int i = 0; i < 9; i++) numbers[i] = i;

            ParashaComboBox.Source = SourceMode.Custom;
            ParashaComboBox.CustomSource = Enum.GetValues(typeof(BE.Parashot));

            CohenComboBox.ItemsSource = numbers;
            LeviComboBox.ItemsSource = numbers;
            IsraelComboBox.ItemsSource = numbers;

            if (bl.GetAllCohanim().Count() >= 3)
            {
                CohenDataGrid.ItemsSource = bl.NumberOfAliyot(bl.GetAllCohanim(), 3);
                CohenComboBox.SelectedIndex = 3;
            }
            else
                CohenDataGrid.ItemsSource = bl.GetAllCohanim();

            if (bl.GetAllLevits().Count() >= 3)
            {
                LeviDataGrid.ItemsSource = bl.NumberOfAliyot(bl.GetAllLevits(), 3);
                LeviComboBox.SelectedIndex = 3;
            }
            else
                LeviDataGrid.ItemsSource = bl.GetAllLevits();

            if (bl.GetAllIsraels().Count() >= 8)
            {
                IsraelDataGrid.ItemsSource = bl.NumberOfAliyot(bl.GetAllIsraels(), 8);
                IsraelComboBox.SelectedIndex = 8;
            }
            else
                IsraelDataGrid.ItemsSource = bl.GetAllIsraels();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ParashaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParashaDataGrid.ItemsSource = bl.BmPrayers(ParashaComboBox.SelectedItem.ToString());
            //bl.GetAllPrayers(prayer => prayer.BMparasha.ToString() == ParashaComboBox.SelectedItem.ToString());
        }

        private void CohenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int numberOfCohanim = (int) CohenComboBox.SelectedItem;
                CohenDataGrid.ItemsSource =
                    bl.NumberOfAliyot(bl.GetAllCohanim(), numberOfCohanim);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "אין מספיק כהנים");
            }
        }

        private void LeviComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int numberOfLevits = (int) LeviComboBox.SelectedItem;
                LeviDataGrid.ItemsSource = bl.NumberOfAliyot(bl.GetAllLevits(), numberOfLevits);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "אין מספיק לויים");
            }
        }

        private void IsraelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int numberOfIsraels = (int) IsraelComboBox.SelectedItem;
                IsraelDataGrid.ItemsSource =
                    bl.NumberOfAliyot(bl.GetAllIsraels(), numberOfIsraels);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "אין מספיק מתפללים");
            }
        }
    }
}