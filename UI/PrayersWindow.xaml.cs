using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace UI
{
    /// <summary>
    /// Interaction logic for Prayers.xaml
    /// </summary>
    public partial class PrayersWindow : Window
    {
        readonly BL.IBL bl;
        private BE.Prayer prayer;
        private readonly System.Windows.Media.Effects.BlurEffect mainBlur;
        private MainWindow main;

        public PrayersWindow()
        {
            InitializeComponent();
            //addLabel.Content = "הוספת" + Environment.NewLine + "מתפלל";
            bl = BL.FactoryBl.getBL();
            prayer = new BE.Prayer();
            mainBlur = new System.Windows.Media.Effects.BlurEffect();

            BMparashaComboBox.CustomSource = Enum.GetValues(typeof(BE.Parashot));
            LastAliyaParashaComboBox.CustomSource = Enum.GetValues(typeof(BE.Parashot));
            TribeComboBox.ItemsSource = Enum.GetValues(typeof(BE.Lineage));

            PrayerDataGrid.ItemsSource = bl.GetAllPrayers();
            ChoosePrayerComboBox.ItemsSource = bl.SortByLastName(bl.GetAllPrayers());
            ChoosePrayerComboBox.DisplayMemberPath = "LastName";
            ChoosePrayerComboBox.SelectedValuePath = "Id";
            PrayerDataGrid.SelectedValuePath = "Id";
        }

        private void prayerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            prayer = PrayerDataGrid.SelectedItem as BE.Prayer;
            PrayerGrid.DataContext = prayer;
            PrayerGrid.IsEnabled = true;
            IdTextBox.IsEnabled = false;
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prayer = (BE.Prayer) PrayerGrid.DataContext;
                prayer.Id = GetSelectedPrayer().Id;
                bl.UpdatePrayer(prayer);
                PrayerDataGrid.ItemsSource = null;
                if (!bl.Empty(prayer))
                {
                    PrayerDataGrid.ItemsSource = bl.GetAllPrayers();
                }
                ChoosePrayerComboBox.ItemsSource = null;
                ChoosePrayerComboBox.ItemsSource = bl.GetAllPrayers();
                prayer = new BE.Prayer();
                PrayerGrid.DataContext = prayer;
                //this.NavigationService.GoBack();
                MessageBox.Show("פרטי המתפלל עודכנו", "עדכון בוצע", MessageBoxButton.OK, MessageBoxImage.Information,
                    MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                main.restart = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "אזהרה!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prayer = (BE.Prayer) PrayerGrid.DataContext;
                prayer.Id = GetSelectedPrayer().Id;
                MessageBoxResult check = MessageBox.Show(".אתה בטוח שברצונך למחוק??? לא תוכל לשחזר מתפלל זה",
                    "אישור מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Hand, MessageBoxResult.No,
                    MessageBoxOptions.RightAlign);
                if (check == MessageBoxResult.Yes)
                {
                    bl.RemovePrayer(prayer.Id);
                    PrayerDataGrid.ItemsSource = null;
                    PrayerDataGrid.ItemsSource = bl.GetAllPrayers();
                    ChoosePrayerComboBox.ItemsSource = null;
                    ChoosePrayerComboBox.ItemsSource = bl.GetAllPrayers();
                    prayer = new BE.Prayer();
                    PrayerGrid.DataContext = prayer;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "אזהרה!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            Effect = mainBlur;
            AddPrayerWindow addPrayerWindow = new AddPrayerWindow {SetPrayersWindow = this};
            addPrayerWindow.ShowDialog();
            Effect = null;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            main.restart = false;
            Close();
        }

        private void choosePrayerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            prayer = GetSelectedPrayer();
            PrayerGrid.DataContext = prayer;
            PrayerGrid.IsEnabled = true;
            IdTextBox.IsEnabled = false;
            PrayerDataGrid.Focus();
        }

        private BE.Prayer GetSelectedPrayer()
        {
            object t = ChoosePrayerComboBox.SelectedItem;
            BE.Prayer choice = (BE.Prayer) t;
            return choice;
        }

        private void TribeButton_Click(object sender, RoutedEventArgs e)
        {
            var myObservableCollection = new ObservableCollection<BE.Prayer>(bl.GetAllPrayers());
            ICollectionView prayersView = CollectionViewSource.GetDefaultView(myObservableCollection);
            prayersView.GroupDescriptions.Add(new PropertyGroupDescription("Tribe"));
            PrayerDataGrid.ItemsSource = null;
            PrayerDataGrid.ItemsSource = prayersView;
        }

        private void NormalButton_OnClick(object sender, RoutedEventArgs e)
        {
            PrayerDataGrid.ItemsSource = null;
            PrayerDataGrid.ItemsSource = bl.GetAllPrayers();
        }

        private void BmButton_OnClick(object sender, RoutedEventArgs e)
        {
            var myObservableCollection = new ObservableCollection<BE.Prayer>(bl.GetAllPrayers());
            ICollectionView prayersView = CollectionViewSource.GetDefaultView(myObservableCollection);
            prayersView.GroupDescriptions.Add(new PropertyGroupDescription("BMparasha"));
            PrayerDataGrid.ItemsSource = null;
            PrayerDataGrid.ItemsSource = prayersView;
        }

        public void GetMainWindow(MainWindow mainWindow)
        {
            main = mainWindow;
        }
    }
}