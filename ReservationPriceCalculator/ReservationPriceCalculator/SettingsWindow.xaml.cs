using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window
	{
		MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
		Rectangle currentColor;
		string[] months = { "Януари", "Февруари", "Март", "Април", "Май", "Юни", "Юли", "Август", "Септември", "Октомври", "Ноември", "Декември" };

		public SettingsWindow()
		{
			DataContext = this;
			InitializeComponent();
			btnSave.Foreground = Brushes.White;
			//foreach (Trigger t in btnSave.Template.Triggers)
			//{
			//	if (t.Property == IsMouseOverProperty)
			//	{
			//		foreach (Setter s in t.Setters)
			//		{
			//			if (s.Property == BackgroundProperty) t.Value = Brushes.Black;
			//		}
			//	}
			//}
			//	OUTPUT:		After trigger is in use (seald) it can not be modified
			this.Loaded += SettingsWindow_Loaded;
		}

		private void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
		{
			label1.Content = "Зимен сезон";
			label2.Content = "Слаб летен - пролет";
			label3.Content = "Силен летен";
			label4.Content = "Слаб летен - есен";

			cBox1.ItemsSource = months;
			cBox2.ItemsSource = months;
			cBox3.ItemsSource = months;
			cBox4.ItemsSource = months;

			cBox1.SelectedIndex = int.Parse(Settings.xDoc.Descendants("Date").Where(element => element.Value == "Зимен сезон").Single().Attribute("month").Value) - 1;
			cBox2.SelectedIndex = int.Parse(Settings.xDoc.Descendants("Date").Where(element => element.Value == "Слаб летен - пролет").Single().Attribute("month").Value) - 1;
			cBox3.SelectedIndex = int.Parse(Settings.xDoc.Descendants("Date").Where(element => element.Value == "Силен летен").Single().Attribute("month").Value) - 1;
			cBox4.SelectedIndex = int.Parse(Settings.xDoc.Descendants("Date").Where(element => element.Value == "Слаб летен - есен").Single().Attribute("month").Value) - 1;

			tBox1.Text = Settings.xDoc.Descendants("Date").Where(element => element.Value == "Зимен сезон").Single().Attribute("day").Value;
			tBox2.Text = Settings.xDoc.Descendants("Date").Where(element => element.Value == "Слаб летен - пролет").Single().Attribute("day").Value;
			tBox3.Text = Settings.xDoc.Descendants("Date").Where(element => element.Value == "Силен летен").Single().Attribute("day").Value;
			tBox4.Text = Settings.xDoc.Descendants("Date").Where(element => element.Value == "Слаб летен - есен").Single().Attribute("day").Value;

			tBox5.Text = Settings.Setting[(int)Settings.Names.addBedSmallPercent].ToString();
			tBox6.Text = Settings.Setting[(int)Settings.Names.addBedBigPercent].ToString();

			foreach (Control c in FindVisualChildren<Control>(this))
			{
				if (c is TextBox) (c as TextBox).TextChanged += SettingChanged;
				if (c is ComboBox) (c as ComboBox).SelectionChanged += SettingChanged;
			}

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(stackPanColors); i++)
			{
				var child = VisualTreeHelper.GetChild(stackPanColors, i);
				if (child is Rectangle)
				{
					((Rectangle)child).Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Settings.Colors[i]));
					((Rectangle)child).MouseDown += SettingsWindow_MouseDown;
				}
			}

			//SAME AS:
			//int i = 0;
			//foreach (Object rect in stackPanColors.Children)
			//{
			//	if (rect is Rectangle)
			//	{
			//		((Rectangle)rect).Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Settings.Colors[i]));
			//		i++;
			//	}
			//}

			currentColor = (Rectangle)VisualTreeHelper.GetChild(stackPanColors, Settings.Setting[(int)Settings.Names.selectColor]);
			currentColor.StrokeThickness = 2;
			currentColor.Stroke = Brushes.Red;
		}

		private void SettingsWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			currentColor.StrokeThickness = 1;
			currentColor.Stroke = Brushes.DarkSlateGray;
			currentColor = (Rectangle)sender;
			currentColor.StrokeThickness = 2;
			currentColor.Stroke = Brushes.Red;



			mainWindow.Resources["SelectedColor"] = currentColor.Fill;
			SetValue("Setting", "option", GetColorIndex(((Rectangle)sender).Fill.ToString()).ToString(), ((int)Settings.Names.selectColor).ToString());
		}

		int GetColorIndex(string color)
		{
			for(int i = 0; i < Settings.Colors.Length; i++)
			{
				if (color == Settings.Colors[i].ToUpper())
				{
					return i;
				}
			}
			return 666;
		}

		private void SettingChanged(object sender, object e)
		{
			switch (((Control)sender).Name)
			{
				case "cBox1":
					SetValue("Date", "month", ((sender as ComboBox).SelectedIndex + 1).ToString(), "Зимен сезон");
					break;
				case "cBox2":
					SetValue("Date", "month", ((sender as ComboBox).SelectedIndex + 1).ToString(), "Слаб летен - пролет");
					break;
				case "cBox3":
					SetValue("Date", "month", ((sender as ComboBox).SelectedIndex + 1).ToString(), "Силен летен");
					break;
				case "cBox4":
					SetValue("Date", "month", ((sender as ComboBox).SelectedIndex + 1).ToString(), "Слаб летен - есен");
					break;
				case "tBox1":
					SetValue("Date", "day", (sender as TextBox).Text, "Зимен сезон");
					break;
				case "tBox2":
					SetValue("Date", "day", (sender as TextBox).Text, "Слаб летен - пролет");
					break;
				case "tBox3":
					SetValue("Date", "day", (sender as TextBox).Text, "Силен летен");
					break;
				case "tBox4":
					SetValue("Date", "day", (sender as TextBox).Text, "Слаб летен - есен");
					break;
				case "tBox5":
					SetValue("Setting", "option", (sender as TextBox).Text, ((int)Settings.Names.addBedSmallPercent).ToString());
					break;
				case "tBox6":
					SetValue("Setting", "option", (sender as TextBox).Text, ((int)Settings.Names.addBedBigPercent).ToString());
					break;
				default: MessageBox.Show("Мамата си трака!");
					break;
			}
		}

		private void SetValue(string tag, string attribute, string value, string name)
		{
			var item = Settings.xDoc.Descendants(tag).Where(element => element.Value == name).Single();
			item.SetAttributeValue(attribute, value);

		}

		public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
		{
			if (depObj != null)
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)

				{

					DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

					if (child != null && child is T)

					{

						yield return (T)child;

					}



					foreach (T childOfChild in FindVisualChildren<T>(child))
					{
						yield return childOfChild;
					}
				}
			}
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			Settings.xDoc.Save(Settings.settingsFile);
		}
	}
}
