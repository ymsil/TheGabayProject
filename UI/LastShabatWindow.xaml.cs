using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BE;

namespace UI
{
    /// <summary>
    /// Interaction logic for LastShabatWindow.xaml
    /// </summary>
    public partial class LastShabatWindow
    {
        private BL.IBL bl;
        private string lastShabat;
        private List<Prayer> prayers;

        public LastShabatWindow()
        {
            InitializeComponent();
            bl = BL.FactoryBl.getBL();
            prayers = new List<Prayer>();

            InitializeGrid();
        }

        private void InitializeGrid()
        {
            ChooseShabatComboBox.CustomSource= Enum.GetValues(typeof(Parashot));

            CohenComboBox.CustomSource = bl.GetAllCohanim();
            CohenComboBox.DisplayMemberPath = "LastName";
            CohenComboBox.IsEnabled = false;
            
            LeviComboBox.CustomSource = bl.GetAllLevits();
            LeviComboBox.DisplayMemberPath = "LastName";
            LeviComboBox.IsEnabled = false;

            ThirdComboBox.CustomSource = bl.GetAllIsraels();
            ThirdComboBox.DisplayMemberPath = "LastName";
            ThirdComboBox.IsEnabled = false;
            ForthComboBox.CustomSource = bl.GetAllIsraels();
            ForthComboBox.DisplayMemberPath = "LastName";
            ForthComboBox.IsEnabled = false;
            FifthComboBox.CustomSource = bl.GetAllIsraels();
            FifthComboBox.DisplayMemberPath = "LastName";
            FifthComboBox.IsEnabled = false;
            SixthComboBox.CustomSource = bl.GetAllIsraels();
            SixthComboBox.DisplayMemberPath = "LastName";
            SixthComboBox.IsEnabled = false;
            SeventhComboBox.CustomSource = bl.GetAllIsraels();
            SeventhComboBox.DisplayMemberPath = "LastName";
            SeventhComboBox.IsEnabled = false;
            MaftirComboBox.CustomSource = bl.GetAllPrayers();
            MaftirComboBox.DisplayMemberPath = "LastName";
            MaftirComboBox.IsEnabled = false;

            UpdateAllButton.IsEnabled = false;

        }

        private void chooseShabatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lastShabat = ChooseShabatComboBox.SelectedItem.ToString();
            UpdateAllButton.IsEnabled = true;
        }
        private void updateAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (CohenCheckBox.IsChecked == false) prayers.Add((Prayer)CohenComboBox.SelectedItem);
            if (LeviCheckBox.IsChecked == false) prayers.Add((Prayer)LeviComboBox.SelectedItem);
            if (ThirdCheckBox.IsChecked == false) prayers.Add((Prayer)ThirdComboBox.SelectedItem);
            if (ForthCheckBox.IsChecked == false) prayers.Add((Prayer)ForthComboBox.SelectedItem);
            if (FifthCheckBox.IsChecked == false) prayers.Add((Prayer)FifthComboBox.SelectedItem);
            if (SixthCheckBox.IsChecked == false) prayers.Add((Prayer)SixthComboBox.SelectedItem);
            if (SeventhCheckBox.IsChecked == false) prayers.Add((Prayer)SeventhComboBox.SelectedItem);
            if (OtherCheckBox.IsChecked == false) prayers.Add((Prayer)OtherComboBox.SelectedItem);
            if (MaftirCheckBox.IsChecked == false) prayers.Add((Prayer)MaftirComboBox.SelectedItem);

            MessageBoxResult check = MessageBox.Show("??אתה בטוח שברצונך לעדכן את המתפללים", "אישור עדכון", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
            if (check == MessageBoxResult.Yes && prayers.Count > 0)
            {
                foreach (Parashot parasha in Enum.GetValues(typeof(Parashot)))
                {
                    if (parasha.ToString() != lastShabat) continue;
                    bl.UpdateAllPrayers(parasha, prayers);
                    break;
                }
                MessageBox.Show(".המתפללים עודכנו", "אישור עדכון", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.No, MessageBoxOptions.RightAlign);
                Close();
            }
            if (prayers.Count == 0) MessageBox.Show(".לא בחרת עולים לתורה", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No, MessageBoxOptions.RightAlign);
        }
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CohenCheckBox_Checked(object sender, RoutedEventArgs e)
        {
                CohenComboBox.IsEnabled = CohenCheckBox.IsChecked != true;
        }
        private void LeviCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            LeviComboBox.IsEnabled = LeviCheckBox.IsChecked != true;
        }
        private void ThirdCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ThirdComboBox.IsEnabled = ThirdCheckBox.IsChecked != true;
        }
        private void ForthCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ForthComboBox.IsEnabled = ForthCheckBox.IsChecked != true;
        }
        private void FifthCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            FifthComboBox.IsEnabled = FifthCheckBox.IsChecked != true;
        }
        private void SixthCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SixthComboBox.IsEnabled = SixthCheckBox.IsChecked != true;
        }
        private void SeventhCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SeventhComboBox.IsEnabled = SeventhCheckBox.IsChecked != true;
        }
        private void OtherCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            OtherComboBox.IsEnabled = OtherCheckBox.IsChecked != true;
        }
        private void MaftirCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MaftirComboBox.IsEnabled = MaftirCheckBox.IsChecked != true;
        }

        private void OtherOptionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            OtherOptionCheckBox.Visibility = Visibility.Collapsed;
            OtherLabel.Visibility = Visibility.Visible;
            OtherComboBox.Visibility = Visibility.Visible;
            OtherTextBox.Visibility = Visibility.Visible;
            OtherCheckBox.Visibility = Visibility.Visible;

            OtherComboBox.CustomSource = bl.GetAllIsraels();
            OtherComboBox.DisplayMemberPath = "LastName";
            OtherComboBox.IsEnabled = false;
        }

        private void CohenComboBox_OnSelectionChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (CohenComboBox.SelectedValue != null )
            {
                CohenTextBox.Text = ((Prayer)CohenComboBox.SelectedItem).FirstName; 
            }
        }
        private void LeviComboBox_OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (LeviComboBox.SelectedValue != null)
            {
                LeviTextBox.Text = ((Prayer) LeviComboBox.SelectedItem).FirstName;
            }
        }
        private void ThirdComboBox_OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (ThirdComboBox.SelectedValue != null)
            {
                ThirdTextBox.Text = ((Prayer) ThirdComboBox.SelectedItem).FirstName;
            }
        }
        private void ForthComboBox_OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (ForthComboBox.SelectedValue != null)
            {
                ForthTextBox.Text = ((Prayer) ForthComboBox.SelectedItem).FirstName;
            }
        }
        private void FifthComboBox_OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (FifthComboBox.SelectedValue != null)
            {
                FifthTextBox.Text = ((Prayer) FifthComboBox.SelectedItem).FirstName;
            }
        }
        private void SixthComboBox_OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (SixthComboBox.SelectedValue != null)
            {
                SixthTextBox.Text = ((Prayer) SixthComboBox.SelectedItem).FirstName;
            }
        }
        private void SeventhComboBox_OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (SeventhComboBox.SelectedValue != null)
            {
                SeventhTextBox.Text = ((Prayer) SeventhComboBox.SelectedItem).FirstName;
            }
        }
        private void MaftirComboBox_OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (MaftirComboBox.SelectedValue != null)
            {
                MaftirTextBox.Text = ((Prayer) MaftirComboBox.SelectedItem).FirstName;
            }
        }
        private void OtherComboBox_OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (OtherComboBox.SelectedValue != null)
            {
                OtherTextBox.Text = ((Prayer) OtherComboBox.SelectedItem).FirstName;
            }
        }
    }
}
