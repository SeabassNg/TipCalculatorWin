using System;
using System.Globalization;
using System.Linq;

namespace TipCalculator
{
    public delegate void UpdateSubtotal(float subtotal);
    public delegate void UpdateTaxRate(float taxRate);
    public delegate void UpdateTaxAmount(float taxAmount);
    public delegate void UpdatePretipTotal(float pretipTotal);
    public delegate void UpdateTipPercentage(float tipPercentage);
    public delegate void UpdateTipAmount(float tipAmount);
    public delegate void UpdateTotal(float total);

    class TipAttributes
    {
        public UpdateSubtotal updateSubtotal { get; set; }
        public UpdateTaxRate updateTaxRate { get; set; }
        public UpdateTaxAmount updateTaxAmount { get; set; }
        public UpdatePretipTotal updatePretipTotal { get; set; }
        public UpdateTipPercentage updateTipPercentage { get; set; }
        public UpdateTipAmount updateTipAmount { get; set; }
        public UpdateTotal updateTotal { get; set; }

        private bool includeTaxInTip;

        public bool IncludeTaxInTip
        {
            get
            {
                return includeTaxInTip;
            }
            set
            {
                includeTaxInTip = value;
                UpdateTotal();
            }
        }

        private void UpdateTaxInfo()
        {
            if (null == updateTaxAmount || null == updateTaxRate)
            {
                return;
            }
            TaxAmount = PretipTotal - Subtotal;
            TaxRate = TaxAmount / Subtotal;

            updateTaxAmount(TaxAmount);
            updateTaxRate(TaxRate);
        }

        private void UpdateTotal()
        {
            if (null == updateTipAmount || null == updateTotal)
            {
                return;
            }
            tipAmount = (IncludeTaxInTip ? PretipTotal : Subtotal) * (TipPercentage / 100);
            total = PretipTotal + tipAmount;

            updateTipAmount(tipAmount);
            updateTotal(total);
        }

        private float subtotal;
        public float Subtotal
        {
            get
            {
                return subtotal;
            }
            set
            {
                subtotal = value;
                if (pretipTotal >= subtotal)
                {
                    UpdateTaxInfo();
                }
                else if (null != updatePretipTotal)
                {
                    pretipTotal = subtotal;
                    updatePretipTotal(subtotal);
                }

                UpdateTotal();
            }
        }

        public float TaxRate { get; set; }
        public float TaxAmount { get; set; }

        private float pretipTotal;
        public float PretipTotal
        {
            get
            {
                return pretipTotal;
            }
            set
            {
                pretipTotal = value;
                if (pretipTotal >= Subtotal)
                {
                    UpdateTaxInfo();
                }
                else if (null != updateSubtotal)
                {
                    subtotal = pretipTotal;
                    updateSubtotal(pretipTotal);
                }

                UpdateTotal();
            }
        }

        private float tipPercentage;
        public float TipPercentage
        {
            get
            {
                return tipPercentage;
            }
            set
            {
                tipPercentage = value;
                UpdateTotal();
            }
        }

        private float tipAmount;
        public float TipAmount
        {
            get
            {
                return tipAmount;
            }
            set
            {
                tipAmount = value;
                if (null != updateTipPercentage && null != updateTotal)
                {
                    tipPercentage = tipAmount / (IncludeTaxInTip ? PretipTotal : Subtotal) * 100;
                    total = PretipTotal + tipAmount;

                    updateTipPercentage(tipPercentage);
                    updateTotal(total);
                }
            }
        }

        private float total;
        public float Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
                if (null != updateTipAmount && null != updateTipPercentage)
                {
                    tipAmount = total - PretipTotal;
                    tipPercentage = tipAmount / (IncludeTaxInTip ? PretipTotal : Subtotal) * 100;
                    updateTipAmount(tipAmount);
                    updateTipPercentage(tipPercentage);
                }
            }
        }

        public void Clear()
        {
            pretipTotal = 0;
            subtotal = 0;
            TaxAmount = 0;
            TaxRate = 0;
            tipAmount = 0;
            tipPercentage = 0;
            total = 0;
            updatePretipTotal(0);
            updateSubtotal(0);
            updateTaxAmount(0);
            updateTaxRate(0);
            updateTipAmount(0);
            updateTipPercentage(0);
            updateTotal(0);
        }

        public static String priceToString(float price)
        {
            return price.ToString("F2", CultureInfo.InvariantCulture);
        }

        public static String percentageToString(float percentage)
        {
            return percentage.ToString("F3", CultureInfo.InvariantCulture);
        }
    }
}