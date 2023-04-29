using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServiceClientWpf.Model
{
    public partial class Service: ICloneable
    {

        public Visibility VisibilityAdmin
        {
            get 
            {
                if (App.AdminCode == true)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public string Color
        {
            get
            {
                if (Discount == 0 || Discount == null)
                    return "#FFFFFF";
                else
                    return "#BDFFBD";
            }
        }
        public string StrCostTime
        {
            get
            {
                if (Discount == 0 || Discount == null)
                    return $"{Cost} рублей за {DurationInSeconds / 60} минут";
                else
                    return $"{(double)Cost - (double)Cost * Discount} рублей за {DurationInSeconds / 60} минут";
            }
        }
        public Visibility VisibilityCost
        {
            get
            {
                if (Discount == null || Discount == 0)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }
        public string StrDiscount
        {
            get
            {
                if (Discount > 0)
                    return $"*скидка {Discount * 100}%";
                else
                    return $"";
            }
        }
        public Visibility VisibilityDiscount
        {
            get
            {
                if (Discount == null || Discount == 0)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }
        public decimal CostDiscount
        {
            get
            {
                if (Discount == 0 || Discount == null)
                    return (decimal)Cost;
                else
                    return (decimal)Cost - Convert.ToDecimal(Cost) * Convert.ToDecimal(Discount);
            }
        }

        public Service Clone()
        {
            return (Service)MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }
    }
}
