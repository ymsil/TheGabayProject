using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using BL;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL bl;
        private readonly System.Windows.Media.Effects.BlurEffect mainBlur;
        public bool restart { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            bl = FactoryBl.getBL();
            mainBlur = new System.Windows.Media.Effects.BlurEffect();
        }

        private void PrayersButton_Click(object sender, RoutedEventArgs e)
        {
            restart = true;
            Effect = mainBlur;
            while (restart)
            {
                PrayersWindow prayersWindow = new PrayersWindow();
                prayersWindow.GetMainWindow(this);
                prayersWindow.ShowDialog();
            }
            Effect = null;
        }

        

        private void LastShabatButton_Click(object sender, RoutedEventArgs e)
        {
            new LastShabatWindow().Show();
        }

        private void NextShabatButton_Click(object sender, RoutedEventArgs e)
        {
            new NextShabatWindow().Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String msg = "Tha Gabay Project - version 1.0.3\n" +
                         "לשאלות, הצעות, שיפורים הערות והארות ניתן לשלוח מייל לכתובת\n" +
                         "ymsil719@gmail.com.\n" +
                         "?האם תרצה לשלוח";

            MessageBoxResult mail = MessageBox.Show(msg,
                "מידע",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question,
                MessageBoxResult.None);
                //MessageBoxOptions.RightAlign);
            if (mail == MessageBoxResult.Yes)
            {
                System.Diagnostics.Process.Start("mailto:ymsil719@gmail.com");
            }

            
        }

        
    }
    public class InformationMessageBox
    {
        public InformationMessageBox()
        {
            Window w = new Window();
            DockPanel panel = new DockPanel();
            TextBlock message = new TextBlock();
            Paragraph par = new Paragraph();
            Run version = new Run("The Gabay Project - version 1.0.3\n" +
                                  "לשאלות, הצעות, שיפורים הערות והארות ניתן לשלוח מייל לכתובת\n");
            Run email = new Run("ymsil719@gmail.com");
            Hyperlink hyperlink = new Hyperlink(email);
            hyperlink.NavigateUri = new Uri("mailto:ymsil719@gmail.com");
            message.Inlines.Add(hyperlink);
            panel.Children.Add(message);
            w.Content = panel;
            w.Show();
        }
    }
}
