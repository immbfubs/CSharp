using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace TotalAmount
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Reservation Reservation { get; } = new Reservation();
		
        // binding array for rooms quantity ComboBox
        public int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		public decimal tempPopupTotal = 0;

        public MainWindow()
        {
			Settings.FirstLoad();
            InitializeComponent();
            DataContext = this;
            cbQuantity.ItemsSource = arr.ToArray();
            #region
            ///can set ItemSource directly to rooms.Keys since it returns a collection
            ///List<string> keyList = new List<string>(rooms.Keys);
            cbName.ItemsSource = Settings.room.Keys;
			#endregion
			Reservation.CollectionChanged += Reservation_CollectionChanged;
			this.Resources["SelectedColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Settings.Colors[Settings.Setting[(int)Settings.Names.selectColor]]));

		}

		private void Reservation_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (Reservation.Count < 1) lBox.Visibility = Visibility.Hidden;
			else lBox.Visibility = Visibility.Visible;
		}

		public void HideZeroValues(bool[] isZero)
		{
			//Hides zero columns 
			Style hidden = new Style { TargetType = typeof(TextBlock) };
            Style visible = new Style { TargetType = typeof(TextBlock) };

            visible.Setters.Add(new Setter(TextBlock.VisibilityProperty, Visibility.Visible));
            hidden.Setters.Add(new Setter(TextBlock.VisibilityProperty, Visibility.Hidden));

            if (isZero[0]) this.Resources["Out"] = hidden;
                else this.Resources["Out"] = visible;
            if (isZero[1]) this.Resources["Low"] = hidden;
                else this.Resources["Low"] = visible;
            if (isZero[2]) this.Resources["High"] = hidden;
                else this.Resources["High"] = visible;
        }
		
        private bool IsCorrectPeriodPicked()
        {
            if (dpCheckIn.SelectedDate != null && dpCheckOut.SelectedDate != null)
            {
                if (((DateTime)dpCheckIn.SelectedDate).CompareTo((DateTime)dpCheckOut.SelectedDate) < 0) return true;
            }
            MessageBox.Show("Датите нещо не ме кефят");
            return false;
        }

        private void dpCheckIn_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpCheckOut.SelectedDate == null)
				dpCheckOut.SelectedDate = ((DateTime)dpCheckIn.SelectedDate).AddDays(1);
			else if (((DateTime)dpCheckIn.SelectedDate).Month != ((DateTime)dpCheckOut.SelectedDate).Month)
				dpCheckOut.SelectedDate = ((DateTime)dpCheckIn.SelectedDate).AddDays(1);
			else if (((DateTime)dpCheckIn.SelectedDate).CompareTo((DateTime)dpCheckOut.SelectedDate) >= 0)
                dpCheckOut.SelectedDate = ((DateTime)dpCheckIn.SelectedDate).AddDays(1);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
			if (IsCorrectPeriodPicked())
			{
				for (int i = 0; i < (int)cbQuantity.SelectedValue; i++)
				{
					Item itm = new Item(
						cbName.SelectedValue.ToString(),
						(DateTime)dpCheckIn.SelectedDate,
						(DateTime)dpCheckOut.SelectedDate
					);
					Reservation.AddToReservation(itm);
				}
				cbQuantity.SelectedIndex = 0;
			}
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(lBox.SelectedIndex != -1)
            {
                Reservation.RemoveAt(lBox.SelectedIndex);
                Reservation.TotalRefresh();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            int j = lBox.Items.Count;
            for (int i = 0; i < j; i++)
            {
                Reservation.RemoveAt(0);
            }
            Reservation.TotalRefresh();
        }

        private void MenuClick_Settings(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        private void MenuClick_RoomTypes(object sender, RoutedEventArgs e)
        {
            RoomTypes roomsWindow = new RoomTypes();
            roomsWindow.ShowDialog();
        }

        private void MenuClick_ResetSettingsFile(object sender, RoutedEventArgs e)
        {
            var message = MessageBox.Show("Всички сезони и цени ще бъдат рестартирани на първоначално зададените!\nСигурни ли сте?", "Внимание!", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes) Settings.ResetSettings(Settings.ResetReason.notify);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((TextBox)sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                Keyboard.ClearFocus();
            }
        }

		private void ListBox_MouseEnter(object sender, MouseEventArgs e)
		{
			if(lBox.HasItems) PopupWindow.IsOpen = true;
		}

		private void ListBox_MouseLeave(object sender, MouseEventArgs e)
		{ 
			PopupWindow.IsOpen = false;
		}

		private void ListBox_MouseMove(object sender, MouseEventArgs e)
		{
			var mousePos = e.GetPosition(this.mainWin);
			PopupWindow.HorizontalOffset = mousePos.X + 15;
			PopupWindow.VerticalOffset = mousePos.Y + 15;
		}

		private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
		{
			StackPanel stackPan = sender as StackPanel;
			Object item = stackPan.DataContext;
			//int index = lBox.Items.IndexOf(item);
			RefreshPopupInfo(item as Item);
		}

		private void TextBox_SelectAll(object sender, object e)
		{
			if (((TextBox)sender).Text == "0") ((TextBox)sender).Clear();
			((TextBox)sender).SelectAll();
		}

		private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (((TextBox)sender).Text == "") ((TextBox)sender).Text = "0";
		}

		public void RefreshPopupInfo(Item item)
		{
			//Item item = Reservation[index];
			PopupPanel.Children.Clear();

			if(item.BedsSmall > 0 || item.BedsBig > 0) PopupPanel.Children.Add(new TextBlock { Text = String.Format((char)0x25CF + " Цена без доп. легло"), FontWeight = FontWeights.SemiBold });
			else PopupPanel.Children.Add(new TextBlock { Text = String.Format((char)0x25CF + " Формиране на цената"), FontWeight = FontWeights.SemiBold });
			if (item.CleanPrice[0] > 0) PopupPanel.Children.Add(new TextBlock { Text = String.Format("    Извън: {0} x {1} = {2}", item.Days[0], Settings.room[item.Name].Out, item.CleanPrice[0]) });
			if (item.CleanPrice[1] > 0) PopupPanel.Children.Add(new TextBlock { Text = String.Format("    Слаб: {0} x {1} = {2}", item.Days[1], Settings.room[item.Name].Low, item.CleanPrice[1]) });
			if (item.CleanPrice[2] > 0) PopupPanel.Children.Add(new TextBlock { Text = String.Format("    Силен: {0} x {1} = {2}", item.Days[2], Settings.room[item.Name].High, item.CleanPrice[2]) });
			if (item.coveredSeassons > 1) PopupPanel.Children.Add(new TextBlock { Text = String.Format("    Общо: {0}", item.CleanPrice[3]) });

			if (item.BedsSmall > 0)
			{
				PopupPanel.Children.Add(new TextBlock { Text = String.Format((char)0x25CF + " Допълнително легло (до 12г)"), FontWeight = FontWeights.SemiBold });
				if (item.coveredSeassons == 1)
				{
					if (item.Days[0] > 0) PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsSmall, item.Days[0], Settings.bedPrices[item.Name].smallOut, 0) });
					else if (item.Days[1] > 0) PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsSmall, item.Days[1], Settings.bedPrices[item.Name].smallLow, 0) });
					else PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsSmall, item.Days[2], Settings.bedPrices[item.Name].smallHigh, 0) });
				}
				else if (item.coveredSeassons == 2)
				{
					if (item.Days[0] > 0)
					{
						PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsSmall, item.Days[0], Settings.bedPrices[item.Name].smallOut, 1) });
						PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsSmall, item.Days[1], Settings.bedPrices[item.Name].smallLow, 2) });

					}
					else
					{
						PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsSmall, item.Days[1], Settings.bedPrices[item.Name].smallLow, 1) });
						PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsSmall, item.Days[2], Settings.bedPrices[item.Name].smallHigh, 2) });
					}
				}
				else
				{
					PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsSmall, item.Days[0], Settings.bedPrices[item.Name].smallOut, 1) });
					PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsSmall, item.Days[1], Settings.bedPrices[item.Name].smallLow, 1) });
					PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsSmall, item.Days[2], Settings.bedPrices[item.Name].smallHigh, 2) });
				}
			}

			if (item.BedsBig > 0)
			{
				PopupPanel.Children.Add(new TextBlock { Text = String.Format((char)0x25CF + " Допълнително легло (12+)"), FontWeight = FontWeights.SemiBold });
				if (item.coveredSeassons == 1)
				{
					if (item.Days[0] > 0) PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsBig, item.Days[0], Settings.bedPrices[item.Name].bigOut) });
					else if (item.Days[1] > 0) PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsBig, item.Days[1], Settings.bedPrices[item.Name].bigLow) });
					else PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsBig, item.Days[2], Settings.bedPrices[item.Name].bigHigh) });
				}
				else if (item.coveredSeassons == 2)
				{
					if (item.Days[0] > 0)
					{
						PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsBig, item.Days[0], Settings.bedPrices[item.Name].bigOut, 1) });
						PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsBig, item.Days[1], Settings.bedPrices[item.Name].bigLow, 2) });

					}
					else
					{
						PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsBig, item.Days[1], Settings.bedPrices[item.Name].bigLow, 1) });
						PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsBig, item.Days[2], Settings.bedPrices[item.Name].bigHigh, 2) });
					}
				}
				else
				{
					PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsBig, item.Days[0], Settings.bedPrices[item.Name].bigOut, 1) });
					PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsBig, item.Days[1], Settings.bedPrices[item.Name].bigLow, 1) });
					PopupPanel.Children.Add(new TextBlock { Text = CreateBedsInfo(item.BedsBig, item.Days[2], Settings.bedPrices[item.Name].bigHigh, 2) });
				}
			}

			if (item.Discount > 0)
			{
				PopupPanel.Children.Add(new TextBlock { Text = String.Format((char)0x25CF + " Обща цена без отстъпка"), FontWeight = FontWeights.SemiBold });
				PopupPanel.Children.Add(new TextBlock { Text = "    " + item.TotalNoDiscount.ToString() });
			}
		}

		string CreateBedsInfo(int beds, int days, int price, short option = 0)
		{
			string str = "";
			if (beds == 1)
			{
				str = String.Format("    {0} дни x {1} = {2}", days, price, (days * price));
				tempPopupTotal += days * price;
			}
				else
			{
				str += String.Format(" x {0} легла = {1}", beds, (days * price * beds));
				tempPopupTotal += days * price * beds;
			}

			if (option == 0)
			{
				tempPopupTotal = 0;
			}
			else if (option == 2)
			{
				str += "\n    Общо: " + tempPopupTotal.ToString();
				tempPopupTotal = 0;
			}
			return str;
		}
	}
}
