using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TotalAmount
{
	static public class Settings
	{

		public static string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Reservation Calculator");
		public static string settingsFile = Path.Combine(appDataFolder, "settings.xml");

		public static string[] Colors { get; set; } = { "#FFDB7093", "#FF3CB371", "#FF6495ED", "#FFADFF2F", "#FFC5CBF9", "#FFFFAFDA", "#33000000", "#FF96ffef", "#FFffef96", "#FFea96ff" };
		static int Deposit { get; }
		public static XDocument xDoc;
		public static Dictionary<string, Room> room = new Dictionary<string, Room>();
		public static Dictionary<string, BedPrices> bedPrices = new Dictionary<string, BedPrices>();
		public static Seasson[] seassons = new Seasson[5];
		public static int[] Setting = new int[Enum.GetValues(typeof(Names)).Cast<int>().Max() + 1];

		public struct Room
		{
			public byte Guests { get; set; }
			public decimal Out { get; set; }
			public decimal Low { get; set; }
			public decimal High { get; set; }

			public Room(byte g, decimal o, decimal l, decimal h)
			{
				this.Guests = g;
				this.Out = o;
				this.Low = l;
				this.High = h;
			}
		}
		public struct Seasson
		{
			public int day;
			public int month;
			public string name;
		}
		public enum Names
		{
			addBedSmallPercent,
			addBedBigPercent,
			deposit,
			selectColor
		}
		public enum ResetReason
		{
			newFile,
			syntaxError,
			pricesError,
			datesError,
			notify
		}

		public struct BedPrices
		{
			public int smallOut;
			public int smallLow;
			public int smallHigh;

			public int bigOut;
			public int bigLow;
			public int bigHigh;
		};

		static void CalculateAddBeds()
		{
			bedPrices.Clear();
			foreach (var kp in room)
			{
				if (kp.Value.Out > 0)
				{
					BedPrices Prices;

					Prices.smallOut = (int)Math.Round((double)Settings.room[kp.Key].Out / Settings.room[kp.Key].Guests * Settings.Setting[(int)Settings.Names.addBedSmallPercent] / 100);
					Prices.smallLow = (int)Math.Round((double)Settings.room[kp.Key].Low / Settings.room[kp.Key].Guests * Settings.Setting[(int)Settings.Names.addBedSmallPercent] / 100);
					Prices.smallHigh = (int)Math.Round((double)Settings.room[kp.Key].High / Settings.room[kp.Key].Guests * Settings.Setting[(int)Settings.Names.addBedSmallPercent] / 100);

					Prices.bigOut = (int)Math.Round((double)Settings.room[kp.Key].Out / Settings.room[kp.Key].Guests * Settings.Setting[(int)Settings.Names.addBedBigPercent] / 100);
					Prices.bigLow = (int)Math.Round((double)Settings.room[kp.Key].Low / Settings.room[kp.Key].Guests * Settings.Setting[(int)Settings.Names.addBedBigPercent] / 100);
					Prices.bigHigh = (int)Math.Round((double)Settings.room[kp.Key].High / Settings.room[kp.Key].Guests * Settings.Setting[(int)Settings.Names.addBedBigPercent] / 100);

					bedPrices.Add(kp.Key, Prices);
				}
			}
		}

		static void LoadSettingsFile()
		{
			if (!File.Exists(Settings.settingsFile))
			{
				ResetSettings(ResetReason.newFile);
			}
			else
			{
				try
				{
					xDoc = XDocument.Load(Settings.settingsFile);
				}
				catch
				{
					MessageBox.Show("1");
					ResetSettings(ResetReason.syntaxError);
				}
			}
		}
		
		static void LoadSettings()
		{
			bool reload;
			do
			{
				reload = false;
				try
				{
					var xDocSettings = from item in xDoc.Descendants("Setting")
									   select new
									   {
										   name = item.Value,
										   option = item.Attribute("option").Value
									   };
					foreach (var i in xDocSettings)
					{
						Setting[int.Parse(i.name)] = int.Parse(i.option);
					}
				}
				catch
				{
					ResetSettings(ResetReason.syntaxError);
					reload = true;
				}
			} while (reload);
		}

		static void LoadDates()
		{
			bool reload;
			do
			{
				reload = false;
				try
				{
					var xDocDates = from i in xDoc.Descendants("Date")
									select new
									{
										name = i.Value,
										day = int.Parse(i.Attribute("day").Value),
										month = int.Parse(i.Attribute("month").Value)
									};
					var l = xDocDates.ToList();
					l.Sort((x, y) => x.month.CompareTo(y.month));
					int counter = 0;
					foreach (var i in l)
					{
						seassons[counter].day = i.day;
						seassons[counter].month = i.month;
						seassons[counter].name = i.name;
						counter++;
					}
				}
				catch
				{
					ResetSettings(ResetReason.datesError);
					reload = true;
				}
			} while (reload);
		}
		
		static bool LoadRoomTypes()
		{
			bool reload, fileReset = false;
			do
			{
				reload = false;
				try
				{
					var types = from i in xDoc.Descendants("Room")
								select new
								{
									name = i.Value,
									guests = i.Attribute("guests").Value,
									_out = i.Attribute("out").Value,
									low = i.Attribute("low").Value,
									high = i.Attribute("high").Value
								};
					room.Clear();
					foreach (var i in types)
					{
						Room r = new Room(
							byte.Parse(i.guests),
							decimal.Parse(i._out),
							decimal.Parse(i.low),
							decimal.Parse(i.high));
						room.Add(i.name.ToString(), r);
					}
				}
				catch (Exception e)
				{
					ResetSettings(ResetReason.pricesError);
					reload = fileReset = true;
				}
			} while (reload);
			CalculateAddBeds();
			//returns true if file has been reset
			return fileReset;
		}

		static public void FirstLoad()
		{
			LoadSettingsFile();
			LoadSettings();
			LoadRoomTypes();
			LoadDates();
		}

		static public bool ReloadRoomTypes()
		{
			LoadSettingsFile();
			return LoadRoomTypes();
		}

		static public void ResetSettings(ResetReason r)
		{
			try
			{
				Directory.CreateDirectory(appDataFolder);
				//string path = @"settings.xml";
				//if (File.Exists(path))
				//{
				//    File.Delete(path);
				//}

				xDoc = new XDocument(
							new XDeclaration("1.0", "utf-16", "true"),
							new XElement("Settings",
								new XComment("----------ТОВА СА НАСТРОЙКИТЕ НА ПРОГРАМАТА ЗА КАЛКУЛИРАНЕ НА ЦЕНИ----------"),
								new XComment("Values must be decimals since decimal.TryParse is used!The rooms count can vary"),
								new XElement("Room", "DBLSV", new XAttribute("guests", 2), new XAttribute("low", 119), new XAttribute("high", "149"), new XAttribute("out", "79")),
								new XElement("Room", "DBL", new XAttribute("guests", 2), new XAttribute("low", 99), new XAttribute("high", "119"), new XAttribute("out", "59")),
								new XElement("Room", "DBLsSV", new XAttribute("guests", 2), new XAttribute("low", 109), new XAttribute("high", "139"), new XAttribute("out", "69")),
								new XElement("Room", "---------", new XAttribute("guests", 0), new XAttribute("low", 0), new XAttribute("high", "0"), new XAttribute("out", "0")),
								new XElement("Room", "SNGLSV", new XAttribute("guests", 1), new XAttribute("low", 99), new XAttribute("high", "119"), new XAttribute("out", "69")),
								new XElement("Room", "SNGL", new XAttribute("guests", 1), new XAttribute("low", 79), new XAttribute("high", "99"), new XAttribute("out", "49")),
								new XElement("Room", "SNGLsSV", new XAttribute("guests", 1), new XAttribute("low", 89), new XAttribute("high", "109"), new XAttribute("out", "59")),
								new XElement("Room", "--------", new XAttribute("guests", 0), new XAttribute("low", 0), new XAttribute("high", "0"), new XAttribute("out", "0")),
								new XElement("Room", "TRPL", new XAttribute("guests", 3), new XAttribute("low", 149), new XAttribute("high", "189"), new XAttribute("out", "99")),
								new XElement("Room", "-------", new XAttribute("guests", 0), new XAttribute("low", 0), new XAttribute("high", "0"), new XAttribute("out", "0")),
								new XElement("Room", "AP3", new XAttribute("guests", 4), new XAttribute("low", 199), new XAttribute("high", "249"), new XAttribute("out", "159")),
								new XElement("Room", "AP4", new XAttribute("guests", 4), new XAttribute("low", 199), new XAttribute("high", "249"), new XAttribute("out", "159")),
								new XElement("Room", "PresAP", new XAttribute("guests", 4), new XAttribute("low", 249), new XAttribute("high", "299"), new XAttribute("out", "239")),
								new XComment("Values must be integers since int.Parse is used!Exactly 4 dates must be defined!"),
								new XElement("Date", "Силен летен", new XAttribute("day", 1), new XAttribute("month", 7)),
								new XElement("Date", "Зимен сезон", new XAttribute("day", 1), new XAttribute("month", 10)),
								new XElement("Date", "Слаб летен - есен", new XAttribute("day", 16), new XAttribute("month", 9)),
								new XElement("Date", "Слаб летен - пролет", new XAttribute("day", 25), new XAttribute("month", 5)),
								new XElement("Setting", (int)Names.deposit, new XAttribute("option", 30)),
								new XElement("Setting", (int)Names.selectColor, new XAttribute("option", 3)),
								new XElement("Setting", (int)Names.addBedBigPercent, new XAttribute("option", 35)),
								new XElement("Setting", (int)Names.addBedSmallPercent, new XAttribute("option", 30))));
				xDoc.Save(Settings.settingsFile);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			switch (r)
			{
				case ResetReason.newFile:
					MessageBox.Show("Нов файл с настройки беше създаден.");
					break;
				case ResetReason.syntaxError:
					MessageBox.Show("Файлът с настройки беше рестартиран поради грешка в текста.");
					break;
				case ResetReason.pricesError:
					MessageBox.Show("Файлът с настройки беше рестартиран, защото една или повече от ценните не бяха зададена коректно.\nЦените бяха променени на тези за 2018г.");
					break;
				case ResetReason.datesError:
					MessageBox.Show("Файлът с настройки беше рестартиран, защото една или повече от началните дати на сезоните не бяха зададени коректно.");
					break;
				default:
					MessageBox.Show("Файлът с настройките беше рестартиран.\nВсички дати, типове стаи и цени бяха рестартирани!\nЦените бяха променени на тези за 2018");
					break;
			}
		}
	}
}
