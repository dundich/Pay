using System.Text;

namespace Maybe2
{
    /// <summary>
    ///  Регулярное выражение для "день", "дней", "дня" 
    /// </summary>
    class StringForms
    {
        string form1, form2, form3;
        /// <summary>
        ///  Регулярное выражение для "день", "дней", "дня" 
        /// </summary>
        /// <param name="form1">день</param>
        /// <param name="form2">дней</param>
        /// <param name="form3">дня</param>
        public StringForms(string form1, string form2, string form3)
        {
            this.form1 = form1;
            this.form2 = form2;
            this.form3 = form3;
        }

        public string getForm(int digit)
        {
            StringBuilder sb = new StringBuilder();
            if (digit < 0)
            {
                digit = -1 * digit;
            }

            int digitNew = digit % 100;
            sb.Append(digit.ToString());
            sb.Append(" ");

            int lastFigure = digitNew % 10;
            if (digitNew > 10 && digitNew < 20)
            {
                sb.Append(form3);
            }
            else if (lastFigure == 1)
            {
                sb.Append(form1);
            }
            else if (lastFigure > 1 && lastFigure < 5)
            {
                sb.Append(form2);
            }
            else
            {
                sb.Append(form3);
            }

            return sb.ToString();
        }
    }


    public static class StringFormsEx
    {
        /// <summary>
        /// Регулярное выражение для "день", "дней", "дня" 
        /// </summary>
        public static string getForm(this int value, string form1, string form2, string form3)
        {
            var sf = new StringForms(form1, form2, form3);
            return sf.getForm(value);
        }

        public static string getAgeForm(this int value)
        {
            return value.getForm("год", "года", "лет");
        }

    }
}
