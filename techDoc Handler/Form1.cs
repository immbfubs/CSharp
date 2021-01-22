using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Windows.Automation;

namespace techDoc_Handler
{
    public partial class Form1 : Form
    {
        static ChromeDriverService srvc = ChromeDriverService.CreateDefaultService();
        IWebDriver driver;
        ChromeDriver cdriver;
        public Form1()
        {
            InitializeComponent();
            srvc.HideCommandPromptWindow = true;
            driver = new ChromeDriver(srvc);
            cdriver = (ChromeDriver)driver;
            driver.Url = "https://translate.google.com/#view=home&op=translate&sl=de&tl=en&text=howcouldyou";
        }
        private void ProgramExit()
        {
            try
            {
                driver.Close();
            }
            catch(Exception e)
            {
            }
            driver.Quit();
        }
        public bool TransformText()
        {
            string cutChars;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    cutChars = @"\s(\*\d){0,1}(H\*){0,1}(22){0,1}";
                    textBox1.Text = Regex.Replace(textBox1.Text, @"\r\n\r\n", "NewLinePlaceHolder");
                    textBox1.Text = Regex.Replace(textBox1.Text, @"-" + cutChars + @"\r{0,1}\n", "");
                    textBox1.Text = Regex.Replace(textBox1.Text, cutChars + @"\r{0,1}\n", " ");
                    textBox1.Text = textBox1.Text.Replace("NewLinePlaceHolder", Environment.NewLine + Environment.NewLine);
                    break;
                case 1:
                    cutChars = @"\s(\*\d){0,1}(H\*){0,1}(22){0,1}";
                    textBox1.Text = Regex.Replace(textBox1.Text, @"\r\n\r\n", "NewLinePlaceHolder");
                    textBox1.Text = Regex.Replace(textBox1.Text, cutChars + @"\r{0,1}\n-\s*", "NewLinePlaceHolder");
                    textBox1.Text = Regex.Replace(textBox1.Text, cutChars + @"\r{0,1}\n", " ");
                    textBox1.Text = textBox1.Text.Replace("NewLinePlaceHolder", Environment.NewLine + Environment.NewLine);
                    break;
                case 2:
                    MessageBox.Show("Select a valid option");
                    break;
                case 3:
                    textBox1.Text = Regex.Replace(textBox1.Text, @"\s*[*:|/]{0,2}\s*\n.{0,1}\s+\W{2,}\s+(\:\*){0,1}", " ");
                    break;
                case 4:
                    textBox1.Text = Regex.Replace(textBox1.Text, @"(\s*\*.{5,10}\n\s*(\.{0,1}\*{1,2})(\s*\*){1}\s*){2}", "NewLinePlaceHolder");
                    textBox1.Text = Regex.Replace(textBox1.Text, @"\s*\*.{5,10}\n\s*(\.{0,1}\*{1,2})(\s*\*){1}\s*", " ");
                    textBox1.Text = textBox1.Text.Replace("NewLinePlaceHolder", Environment.NewLine + Environment.NewLine);
                    break;
                default:
                    MessageBox.Show("Select an option");
                    return false;
                    break;
            }
            textBox1.Update();
            return true;
        }
        public void SwitchToBrowser()
        {
            Process[] prcs = Process.GetProcessesByName("chrome");
            IntPtr windowHandle = IntPtr.Zero;
            foreach (Process prc in prcs)
            {
                if (prc.MainWindowHandle != IntPtr.Zero)
                {
                    windowHandle = prc.MainWindowHandle;
                }
            }
            if (windowHandle != IntPtr.Zero)
            {
                AutomationElement element = AutomationElement.FromHandle(windowHandle);
                if (element != null)
                {
                    element.SetFocus();
                }
                IWebElement sourceBox = driver.FindElement(By.XPath("/html/body/c-wiz/div/div/c-wiz/div/c-wiz/div/div/div/c-wiz/span/span/div/textarea"));
                sourceBox.Clear();
                sourceBox.Click();
                //sourceBox.SendKeys(textBox1.Text);
                Clipboard.SetText(textBox1.Text);
                SendKeys.Send("^{v}");
            }
            else
            {
                MessageBox.Show("shit");
            }
        }

        private void translate_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength == 0 || !TransformText())
            {
                return;
            }
            label1.Text = textBox1.TextLength.ToString() + " characters";
            if (textBox1.TextLength > 5000)
            {
                MessageBox.Show("Characters more than 5000");
                return;
            }
            else
            {
                SwitchToBrowser();
                if (checkBox1.Checked)
                {
                    textBox1.Clear();
                }
            }
        }
    }
}