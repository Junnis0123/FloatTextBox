using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Control
{
    public partial class FloatTextBox : UserControl
    {
        public FloatTextBox()
        {
            InitializeComponent();
        }

        #region property
        public static DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(object), typeof(FloatTextBox));
        public static DependencyProperty DecimalPointProperty =
            DependencyProperty.Register("DecimalPoint", typeof(object), typeof(FloatTextBox));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public int DecimalPoint
        {
            get { return (int)GetValue(DecimalPointProperty); }
            set { SetValue(DecimalPointProperty, value); }
        }
        #endregion

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //If Arrow than pass
            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Back || e.Key == Key.Delete)
                return;

            //if Front than MinusChar
            else if (textBox.SelectionStart + 1 < Text.Length && Text.Substring(textBox.SelectionStart, 1) == "-")
                e.Handled = true;

            //only number and .
            else if (!(((Key.D0 <= e.Key) && (e.Key <= Key.D9))
                 || ((Key.NumPad0 <= e.Key) && (e.Key <= Key.NumPad9))
                 || e.Key == Key.Decimal))
                e.Handled = true;

            //over Decimal number than auto remove
            if (Text.Contains('.') && textBox.SelectionStart > Text.IndexOf('.') && Text.Split('.')[1].Length >= DecimalPoint)
            {
                int cusor = textBox.SelectionStart;
                if (cusor == Text.Length)
                    textBox.Select(cusor - 1, cusor);
                else
                    textBox.Select(cusor, 1);
            }

            switch (e.Key)
            {
                case Key.Separator:
                case Key.Decimal:
                    if (Text.Contains("."))
                        textBox.Select(textBox.Text.IndexOf("."), 1);
                    break;
                case Key.OemMinus:
                case Key.Subtract:
                    int cusor = textBox.SelectionStart;
                    if (Text.Contains("-"))
                    {
                        Text = Text.Remove(0, 1);
                        textBox.Select(cusor - 1, 0);
                    }
                    else
                    {
                        Text = "-" + Text;
                        textBox.Select(cusor + 1, 0);
                    }
                    break;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox.SelectAll();
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Text = string.Format("{0:f" + DecimalPoint + "}", Convert.ToDouble(Text));
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Text = string.Format("{0:f" + DecimalPoint + "}", 0);
        }
    }
}
