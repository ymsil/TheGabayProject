using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using MessageBoxOptions = System.Windows.MessageBoxOptions;

namespace UI
{
    /// <summary>
    /// Interaction logic for PrayerWindow.xaml
    /// </summary>
    public partial class AddPrayerWindow
    {
        private PrayersWindow prayersWindow;
        BL.IBL bl;
        private BE.Prayer prayer;
        public AddPrayerWindow()
        {
            InitializeComponent();
            bl = BL.FactoryBl.getBL();
            prayer = new BE.Prayer();
            Grid1.DataContext = prayer;


            BMparashaComboBox.CustomSource = Enum.GetValues(typeof(BE.Parashot));
            LastAliyaParashaComboBox.CustomSource = Enum.GetValues(typeof(BE.Parashot));
            TribeComboBox.ItemsSource = Enum.GetValues(typeof(BE.Lineage));

            BMparashaComboBox.SelectedIndex = -1;
            LastAliyaParashaComboBox.SelectedIndex = -1;
            TribeComboBox.SelectedIndex = 2;
            FirstNameTextBox.Focus();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddPrayer(prayer);
                prayer = new BE.Prayer();
                Grid1.DataContext = prayer;
                prayersWindow.PrayerDataGrid.ItemsSource = null;
                prayersWindow.PrayerDataGrid.ItemsSource = bl.GetAllPrayers();
                prayersWindow.ChoosePrayerComboBox.ItemsSource = null;
                prayersWindow.ChoosePrayerComboBox.ItemsSource = bl.GetAllPrayers();
                string message = prayer.FirstName + " " + prayer.LastName + "נוסף לרשימת המתפללים";
                MessageBox.Show(message, "המתפלל נוסף לקהילה", MessageBoxButton.OK, MessageBoxImage.Information,
                    MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                prayer = new BE.Prayer();
                Grid1.DataContext = prayer;
                BMparashaComboBox.SelectedIndex = -1;
                LastAliyaParashaComboBox.SelectedIndex = -1;
                TribeComboBox.SelectedIndex = 2;
                FirstNameTextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "אזהרה!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public PrayersWindow SetPrayersWindow
        {
            get { return prayersWindow; }
            set { prayersWindow = value; }
        }

        private void PressedEnterKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && AddButton.IsEnabled)
            {
                addButton_Click(this, e);
            }
        }
    }
}
