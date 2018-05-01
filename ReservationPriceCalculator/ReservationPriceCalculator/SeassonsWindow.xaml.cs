using System;
using System.Collections;
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
using System.Xml;

namespace TotalAmount
{
    /// <summary>
    /// Interaction logic for SeassonsWindow.xaml
    /// </summary>
    public partial class SeassonsWindow : Window
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        XmlDocument doc = new XmlDocument();
        string[] seassons = new string[5];
        string[] months = { "Януари", "Февруари", "Март", "Април", "Май", "Юни", "Юли", "Август", "Септември", "Октомври", "Ноември", "Декември" };
        int oldDay;
        int oldMonth;
        public SeassonsWindow()
        {
            InitializeComponent();
            int counter = 0;
            foreach (var s in mainWindow.seassons)
            {
                seassons[counter] = s.name;
                counter++;
            }
            seassonNames.ItemsSource = seassons;
            cbSeassonMonth.ItemsSource = months;
            seassonNames.SelectedIndex = 0;
            doc.Load(App.settingsXml);

            RefreshSeasson();
        }

        private void RefreshSeasson()
        {
            tbSeassonDay.Text = mainWindow.seassons[seassonNames.SelectedIndex].day.ToString();
            cbSeassonMonth.ItemsSource = months;
            cbSeassonMonth.SelectedIndex = mainWindow.seassons[seassonNames.SelectedIndex].month - 1;
        }

        private void seassonNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshSeasson();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            IEnumerator ie = doc.SelectNodes("/Settings/Date").GetEnumerator();
            while (ie.MoveNext())
            {
                if ((ie.Current as XmlNode).Attributes["day"].Value == mainWindow.seassons[seassonNames.SelectedIndex].day.ToString() && (ie.Current as XmlNode).Attributes["month"].Value == mainWindow.seassons[seassonNames.SelectedIndex].month.ToString())
                {
                    (ie.Current as XmlNode).Attributes["day"].Value = tbSeassonDay.Text;
                    (ie.Current as XmlNode).Attributes["month"].Value = (cbSeassonMonth.SelectedIndex + 1).ToString();
                    doc.Save(App.settingsXml);
                    
                    MessageBox.Show("Saved");
                    this.Close();
                }
            }
        }
    }
}
