using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TipCalculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private TipAttributes tipAttributes;
        public MainPage()
        {
            this.InitializeComponent();
            tipAttributes = new TipAttributes
            {
                IncludeTaxInTip = true,
                Subtotal = 0,
                TaxRate = 0,
                TaxAmount = 0,
                PretipTotal = 0,
                TipPercentage = 0,
                TipAmount = 0,
                Total = 0,
                updateSubtotal = new TipCalculator.UpdateSubtotal(this.UpdateSubtotal),
                updateTaxAmount = new TipCalculator.UpdateTaxAmount(this.UpdateTaxAmount),
                updateTaxRate = new TipCalculator.UpdateTaxRate(this.UpdateTaxRate),
                updatePretipTotal = new TipCalculator.UpdatePretipTotal(this.UpdatePretip),
                updateTipPercentage = new TipCalculator.UpdateTipPercentage(this.UpdateTipPercentage),
                updateTipAmount = new TipCalculator.UpdateTipAmount(this.UpdateTipAmount),
                updateTotal = new TipCalculator.UpdateTotal(this.UpdateTotal)
            };
        }

        public void IncludeTax_RadioChanged(object sender, RoutedEventArgs e)
        {
            if (null != tipAttributes)
            {
                tipAttributes.IncludeTaxInTip = null == WithTaxesRB || true == WithTaxesRB.IsChecked;
            }
        }

        public void UpdateTaxAmount(float taxAmount)
        {
            TaxTB.Text = "$" + TipAttributes.priceToString(taxAmount);
        }

        public void UpdateTaxRate(float taxRate)
        {
            TaxRateTB.Text = TipAttributes.percentageToString(taxRate) + "%";
        }

        public void UpdateSubtotal(float subtotal)
        {
            SubtotalTB.Text = TipAttributes.priceToString(subtotal);
        }

        public void OnSubtotal_TextChanged(object sender, RoutedEventArgs e)
        {
            tipAttributes.Subtotal = "" == SubtotalTB.Text ? 0 : float.Parse(SubtotalTB.Text);
        }

        public void UpdatePretip(float pretip)
        {
            PretipTB.Text = TipAttributes.priceToString(pretip);
        }

        public void onPretip_TextChanged(object sender, RoutedEventArgs e)
        {
            tipAttributes.PretipTotal = "" == PretipTB.Text ? 0 : float.Parse(PretipTB.Text);
        }

        public void UpdateTipPercentage(float tipPercentage)
        {
            TipPercentageTB.Text = TipAttributes.percentageToString(tipPercentage);
        }

        public void onTipPercentage_TextChanged(object sender, RoutedEventArgs e)
        {
            tipAttributes.TipPercentage = "" == TipPercentageTB.Text ? 0 : float.Parse(TipPercentageTB.Text);
        }

        public void UpdateTipAmount(float tipAmount)
        {
            TipAmountTB.Text = TipAttributes.priceToString(tipAmount);
        }

        public void onTipAmount_TextChanged(object sender, RoutedEventArgs e)
        {
            tipAttributes.TipAmount = "" == TipAmountTB.Text ? 0 : float.Parse(TipAmountTB.Text);
        }

        public void UpdateTotal(float total)
        {
            TotalTB.Text = TipAttributes.priceToString(total);
        }

        public void onTotal_TextChanged(object sender, RoutedEventArgs e)
        {
            tipAttributes.Total = "" == TotalTB.Text ? 0 : float.Parse(TotalTB.Text);
        }

        public void onRoundButton_Pressed(object sender, RoutedEventArgs e)
        {
            tipAttributes.Total = (int)(tipAttributes.Total + .99);
            UpdateTotal(tipAttributes.Total);
        }

        public void onClearButton_Pressed(object sender, RoutedEventArgs e)
        {
            tipAttributes.Clear();
        }
    }
}
