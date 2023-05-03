using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace ServiceClientWpf.Model
{
    public partial class Service: ICloneable, IDataErrorInfo, IValidatableObject
    {


        public Visibility VisibilityAdmin
        {
            get 
            {
                return MainWindow.VisibilityAdminButton;
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


        public string this[string columnName]
        {
            get 
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Discount":
                        if ((Discount < 0) || (Discount > 100))
                        {
                            error = "Скидка должна быть (0 - 100)";
                        }
                        break;
                    case "Title":
                        if (App.DB.Service.FirstOrDefault(x => x.Title == Title) != null
                            && ID == 0)
                        {
                            error = "Такое название уже есть";
                        }
                        break;
                    case "Cost":
                        if (Cost < 0 || Cost > decimal.MaxValue)
                        {
                            error = "Цена не входит в диапазон";
                        }
                        break;
                    case "DurationInSeconds":
                        if (DurationInSeconds > 240)
                        {
                            error = "Время превышает 4 часов";
                        }
                        break;
                }
                return error;
            }
        }
        public string Error => throw new NotImplementedException();


         public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
         {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Title))
                errors.Add(new ValidationResult("- Не указано название"));
            else if (App.DB.Service.FirstOrDefault(x => x.Title == Title) != null)
                errors.Add(new ValidationResult($"- Название {Title} недоступна"));
            if (string.IsNullOrWhiteSpace(Cost.ToString()))
                errors.Add(new ValidationResult("- Не указано цена"));
            if (string.IsNullOrWhiteSpace(Discount.ToString()))
                errors.Add(new ValidationResult("- Не указано скидка"));
            else if (Discount < 0 || Discount > 100)
                errors.Add(new ValidationResult("- Скидка не входит в диапазон (0 - 100)"));
            if (string.IsNullOrWhiteSpace(DurationInSeconds.ToString()))
                errors.Add(new ValidationResult("- Не указано время"));

            return errors;
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
