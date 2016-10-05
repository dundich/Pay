using System.Text;

namespace Maybe2
{
    /// <summary>
    ///  Регулярное выражение для "день", "дней", "дня" 
    /// </summary>
    public class StringForms
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

        // Функция ФормаМножественногоЧисла(Слово1, Слово2, Слово3, Знач ЦелоеЧисло)        
        //
        //  Изменим знак целого числа, иначе отрицательные числа будут неправильно преобразовываться
        // Если ЦелоеЧисло < 0 Тогда
        // ЦелоеЧисло = -1 * ЦелоеЧисло;
        // КонецЕсли;
        //
        // Если ЦелоеЧисло <> Цел(ЦелоеЧисло) Тогда для нецелых чисел - всегда вторая форма
        // Возврат Слово2;
        // КонецЕсли;
        //
        // остаток
        //   Остаток = ЦелоеЧисло%10;
        // Если (ЦелоеЧисло >10) И (ЦелоеЧисло<20) Тогда
        //   для второго десятка - всегда третья форма
        // Возврат Слово3;
        // ИначеЕсли Остаток=1 Тогда
        // Возврат Слово1;
        // ИначеЕсли (Остаток>1) И (Остаток<5) Тогда
        // Возврат Слово2;
        // Иначе
        // Возврат Слово3;
        // КонецЕсли;
        //
        // КонецФункции

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
}
