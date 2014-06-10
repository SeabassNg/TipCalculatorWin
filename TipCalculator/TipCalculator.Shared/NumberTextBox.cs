using System;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TipCalculator
{
    class NumberTextBox : TextBox
    {
        public static readonly DependencyProperty WholeNumberProperty = DependencyProperty.Register("WholeNumbers", typeof(int), typeof(NumberTextBox), null);
        public static readonly DependencyProperty DecimalNumberProperty = DependencyProperty.Register("DecimalNumbers", typeof(int), typeof(NumberTextBox), null);
        public static readonly DependencyProperty ShowMessageProperty = DependencyProperty.Register("ShowMessage", typeof(bool), typeof(NumberTextBox), null);
        public static readonly DependencyProperty BreakingNumberRulesIsAllowedProperty = DependencyProperty.Register("BreakingNumberRulesIsAllowed", typeof(bool), typeof(NumberTextBox), null);

        public NumberTextBox() : base()
        {
            this.InputScope = new InputScope { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };
            OldText = "";
        }

        private String OldText { get; set; }

        public int WholeNumbers
        {
            get { return (int)GetValue(WholeNumberProperty); }
            set { SetValue(WholeNumberProperty, value); }
        }

        public int DecimalPlaces
        {
            get { return (int)GetValue(DecimalNumberProperty); }
            set { SetValue(DecimalNumberProperty, value); }
        }

        public bool ShowMessage
        {
            get { return (bool)GetValue(ShowMessageProperty); }
            set { SetValue(ShowMessageProperty, value); }
        }

        public bool BreakingNumberRulesIsAllowed
        {
            get { return (bool)GetValue(BreakingNumberRulesIsAllowedProperty); }
            set { SetValue(BreakingNumberRulesIsAllowedProperty, value); }
        }

        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            if (VirtualKey.Decimal == e.Key)
            {
                if (Text.Contains("."))
                {
                    e.Handled = true;
                }
                return;
            }
            if (VirtualKey.Back == e.Key)
            {
                return;
            }
            if (!e.Handled && Text.Contains(".") && 0 < DecimalPlaces)
            {

                string check = Text.Substring(Text.IndexOf(".") + 1);
                if (check.Length > DecimalPlaces)
                {
                    Text = string.Format("{0}.{1}",
                                         Text.Substring(0, Text.IndexOf(".")),
                                         Text.Substring(Text.IndexOf(".") + 1, DecimalPlaces));
                    return;
                }
            }
            if (!e.Handled && WholeNumbers > 0)
            {
                string check = Text.Substring(0, Text.IndexOf(".") + 1);
                if (check.Length >= WholeNumbers)
                {
                    var startIndex = Text.IndexOf(".") - WholeNumbers;
                    Text = string.Format("{0}.{1}",
                                         Text.Substring(startIndex, WholeNumbers),
                                         Text.Substring(Text.IndexOf(".") + 1));
                    return;
                }
            }
            base.OnKeyUp(e);
        }

        void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal d = 0;
            if (0 < this.Text.Length && ("." != Text || ("." == Text && 0 == DecimalPlaces)))
            {
                if (this.Text.Count(x => '.' == x) > 1)
                {
                    // Too many decimal points
                    this.Text = this.OldText;
                    return;
                }
            }

            if (!decimal.TryParse(this.Text, out d))
            {
                //Not a number
                this.Text = this.OldText;
                return;
            }

            if (Text.Contains('.') && DecimalPlaces > 0)
            {
                string check = Text.Substring(this.Text.IndexOf('.'));
                if (check.Length > DecimalPlaces)
                {
                    this.Text = this.OldText;
                    return;
                }
            }

            if (WholeNumbers > 0)
            {
                if (!Text.Contains('.') && Text.Length >= WholeNumbers)
                {
                    this.Text = this.OldText;
                    return;
                }

                if (Text.Contains('.'))
                {
                    string check = Text.Substring(0, Text.IndexOf('.'));
                    if (check.Length > WholeNumbers)
                    {
                        this.Text = this.OldText;
                        return;
                    }
                }
            }
            this.OldText = this.Text;
        }
    }
}
