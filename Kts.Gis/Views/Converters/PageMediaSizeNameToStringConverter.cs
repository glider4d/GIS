using System;
using System.Globalization;
using System.Printing;
using System.Windows.Data;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет конвертер, используемый для локализации названий размеров страниц.
    /// </summary>
    internal sealed partial class PageMediaSizeNameToStringConverter : IValueConverter
    {
    }

    // Реализация IValueConverter.
    internal sealed partial class PageMediaSizeNameToStringConverter
    {
        #region Открытые методы

        /// <summary>
        /// Конвертирует значение.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Локализированное название размера страницы.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((PageMediaSizeName)value)
            {
                case PageMediaSizeName.BusinessCard:
                    return "Визитная карточка";

                case PageMediaSizeName.CreditCard:
                    return "Кредитная карта";

                case PageMediaSizeName.ISOA0:
                    return "A0";

                case PageMediaSizeName.ISOA1:
                    return "A1";

                case PageMediaSizeName.ISOA10:
                    return "A10";

                case PageMediaSizeName.ISOA2:
                    return "A2";

                case PageMediaSizeName.ISOA3:
                    return "A3";

                case PageMediaSizeName.ISOA3Extra:
                    return "A3 Extra";

                case PageMediaSizeName.ISOA3Rotated:
                    return "A3 (повернуто)";

                case PageMediaSizeName.ISOA4:
                    return "A4";

                case PageMediaSizeName.ISOA4Extra:
                    return "A4 Extra";

                case PageMediaSizeName.ISOA4Rotated:
                    return "A4 (повернуто)";

                case PageMediaSizeName.ISOA5:
                    return "A5";

                case PageMediaSizeName.ISOA5Extra:
                    return "A5 Extra";

                case PageMediaSizeName.ISOA5Rotated:
                    return "A5 (повернуто)";

                case PageMediaSizeName.ISOA6:
                    return "A6";

                case PageMediaSizeName.ISOA6Rotated:
                    return "A6 (повернуто)";

                case PageMediaSizeName.ISOA7:
                    return "A7";

                case PageMediaSizeName.ISOA8:
                    return "A8";

                case PageMediaSizeName.ISOA9:
                    return "A9";

                case PageMediaSizeName.ISOB0:
                    return "B0";

                case PageMediaSizeName.ISOB1:
                    return "B1";

                case PageMediaSizeName.ISOB10:
                    return "B10";

                case PageMediaSizeName.ISOB2:
                    return "B2";

                case PageMediaSizeName.ISOB3:
                    return "B3";

                case PageMediaSizeName.ISOB4:
                    return "B4";

                case PageMediaSizeName.ISOB4Envelope:
                    return "Конверт B4";

                case PageMediaSizeName.ISOB5Envelope:
                    return "Конверт B5";

                case PageMediaSizeName.ISOB5Extra:
                    return "B5 Extra";

                case PageMediaSizeName.ISOB7:
                    return "B7";

                case PageMediaSizeName.ISOB8:
                    return "B8";

                case PageMediaSizeName.ISOB9:
                    return "B9";

                case PageMediaSizeName.ISOC0:
                    return "C0";

                case PageMediaSizeName.ISOC1:
                    return "C1";

                case PageMediaSizeName.ISOC10:
                    return "C10";

                case PageMediaSizeName.ISOC2:
                    return "C2";

                case PageMediaSizeName.ISOC3:
                    return "C3";

                case PageMediaSizeName.ISOC3Envelope:
                    return "Конверт C3";

                case PageMediaSizeName.ISOC4:
                    return "C4";

                case PageMediaSizeName.ISOC4Envelope:
                    return "Конверт С4";

                case PageMediaSizeName.ISOC5:
                    return "C5";

                case PageMediaSizeName.ISOC5Envelope:
                    return "Конверт С5";

                case PageMediaSizeName.ISOC6:
                    return "C6";

                case PageMediaSizeName.ISOC6C5Envelope:
                    return "Конверт C6C5";

                case PageMediaSizeName.ISOC6Envelope:
                    return "Конверт С6";

                case PageMediaSizeName.ISOC7:
                    return "C7";

                case PageMediaSizeName.ISOC8:
                    return "C8";

                case PageMediaSizeName.ISOC9:
                    return "C9";

                case PageMediaSizeName.ISODLEnvelope:
                    return "Длинный конверт DL";

                case PageMediaSizeName.ISODLEnvelopeRotated:
                    return "Длинный конверт DL (повернут)";

                case PageMediaSizeName.ISOSRA3:
                    return "SRA 3";

                case PageMediaSizeName.Japan2LPhoto:
                    return "2L Фото";

                case PageMediaSizeName.JapanChou3Envelope:
                    return "Конверт Чоу 3";

                case PageMediaSizeName.JapanChou3EnvelopeRotated:
                    return "Конверт Чоу 3 (повернут)";

                case PageMediaSizeName.JapanChou4Envelope:
                    return "Конверт Чоу 4";

                case PageMediaSizeName.JapanChou4EnvelopeRotated:
                    return "Конверт Чоу 4 (повернут)";

                case PageMediaSizeName.JapanDoubleHagakiPostcard:
                    return "Двойная открытка Хагаки";

                case PageMediaSizeName.JapanDoubleHagakiPostcardRotated:
                    return "Двойная открытка Хагаки (повернута)";

                case PageMediaSizeName.JapanHagakiPostcard:
                    return "Открытка Хагаки";

                case PageMediaSizeName.JapanHagakiPostcardRotated:
                    return "Открытка Хагаки (повернута)";

                case PageMediaSizeName.JapanKaku2Envelope:
                    return "Конверт Каку 2";

                case PageMediaSizeName.JapanKaku2EnvelopeRotated:
                    return "Конверт Каку 2 (повернут)";

                case PageMediaSizeName.JapanKaku3Envelope:
                    return "Конверт Каку 3";

                case PageMediaSizeName.JapanKaku3EnvelopeRotated:
                    return "Конверт Каку 3 (повернут)";

                case PageMediaSizeName.JapanLPhoto:
                    return "L Фото";

                case PageMediaSizeName.JapanQuadrupleHagakiPostcard:
                    return "Четырехкратная открытка Хагаки";

                case PageMediaSizeName.JapanYou1Envelope:
                    return "Конверт Ю 1";

                case PageMediaSizeName.JapanYou2Envelope:
                    return "Конверт Ю 2";

                case PageMediaSizeName.JapanYou3Envelope:
                    return "Конверт Ю 3";

                case PageMediaSizeName.JapanYou4Envelope:
                    return "Конверт Ю 4";

                case PageMediaSizeName.JapanYou4EnvelopeRotated:
                    return "Конверт Ю 4 (повернут)";

                case PageMediaSizeName.JapanYou6Envelope:
                    return "Конверт Ю 6";

                case PageMediaSizeName.JapanYou6EnvelopeRotated:
                    return "Конверт Ю 6 (повернут)";

                case PageMediaSizeName.JISB0:
                    return "Японский промышленный стандарт B0";

                case PageMediaSizeName.JISB1:
                    return "Японский промышленный стандарт B1";

                case PageMediaSizeName.JISB10:
                    return "B10 (Японский промышленный стандарт)";

                case PageMediaSizeName.JISB2:
                    return "B2 (Японский промышленный стандарт)";

                case PageMediaSizeName.JISB3:
                    return "B3 (Японский промышленный стандарт)";

                case PageMediaSizeName.JISB4:
                    return "B4 (Японский промышленный стандарт)";

                case PageMediaSizeName.JISB4Rotated:
                    return "Японский промышленный стандарт B4 (повернут)";

                case PageMediaSizeName.JISB5:
                    return "B5 (Японский промышленный стандарт)";

                case PageMediaSizeName.JISB5Rotated:
                    return "Японский промышленный стандарт B5 (повернут)";

                case PageMediaSizeName.JISB6:
                    return "B6 (Японский промышленный стандарт)";

                case PageMediaSizeName.JISB6Rotated:
                    return "Японский промышленный стандарт B6 (повернут)";

                case PageMediaSizeName.JISB7:
                    return "B7 (Японский промышленный стандарт)";

                case PageMediaSizeName.JISB8:
                    return "Японский промышленный стандарт B8";

                case PageMediaSizeName.JISB9:
                    return "Японский промышленный стандарт B9";

                case PageMediaSizeName.NorthAmerica10x11:
                    return "10 x 11";

                case PageMediaSizeName.NorthAmerica10x12:
                    return "10 x 12";

                case PageMediaSizeName.NorthAmerica10x14:
                    return "10 x 14";

                case PageMediaSizeName.NorthAmerica11x17:
                    return "11 x 17";

                case PageMediaSizeName.NorthAmerica14x17:
                    return "14 x 17";

                case PageMediaSizeName.NorthAmerica4x6:
                    return "4 x 6";

                case PageMediaSizeName.NorthAmerica4x8:
                    return "4 x 8";

                case PageMediaSizeName.NorthAmerica5x7:
                    return "5 x 7";

                case PageMediaSizeName.NorthAmerica8x10:
                    return "8 x 10";

                case PageMediaSizeName.NorthAmerica9x11:
                    return "9 x 11";

                case PageMediaSizeName.NorthAmericaArchitectureASheet:
                    return "Архитектурный лист A";

                case PageMediaSizeName.NorthAmericaArchitectureBSheet:
                    return "Архитектурный лист В";

                case PageMediaSizeName.NorthAmericaArchitectureCSheet:
                    return "Архитектурный лист С";

                case PageMediaSizeName.NorthAmericaArchitectureDSheet:
                    return "Архитектурный лист D";

                case PageMediaSizeName.NorthAmericaArchitectureESheet:
                    return "Архитектурный лист Е";

                case PageMediaSizeName.NorthAmericaCSheet:
                    return "Лист C";

                case PageMediaSizeName.NorthAmericaDSheet:
                    return "Лист D";

                case PageMediaSizeName.NorthAmericaESheet:
                    return "Лист E";

                case PageMediaSizeName.NorthAmericaExecutive:
                    return "Executive";

                case PageMediaSizeName.NorthAmericaGermanLegalFanfold:
                    return "Legal Fanfold (Германия)";

                case PageMediaSizeName.NorthAmericaGermanStandardFanfold:
                    return "Standard Fanfold (Германия)";

                case PageMediaSizeName.NorthAmericaLegal:
                    return "Legal";

                case PageMediaSizeName.NorthAmericaLegalExtra:
                    return "Legal Extra";

                case PageMediaSizeName.NorthAmericaLetter:
                    return "Letter";

                case PageMediaSizeName.NorthAmericaLetterExtra:
                    return "Letter Extra";

                case PageMediaSizeName.NorthAmericaLetterPlus:
                    return "Letter Plus";

                case PageMediaSizeName.NorthAmericaLetterRotated:
                    return "Letter (повернуто)";

                case PageMediaSizeName.NorthAmericaMonarchEnvelope:
                    return "Конверт Монарх";

                case PageMediaSizeName.NorthAmericaNote:
                    return "Бумага для заметок";

                case PageMediaSizeName.NorthAmericaNumber10Envelope:
                    return "Конверт 10";

                case PageMediaSizeName.NorthAmericaNumber10EnvelopeRotated:
                    return "Конверт 10 (повернут)";

                case PageMediaSizeName.NorthAmericaNumber11Envelope:
                    return "Конверт 11";

                case PageMediaSizeName.NorthAmericaNumber12Envelope:
                    return "Конверт 12";

                case PageMediaSizeName.NorthAmericaNumber14Envelope:
                    return "Конверт 14";

                case PageMediaSizeName.NorthAmericaNumber9Envelope:
                    return "Конверт 9";

                case PageMediaSizeName.NorthAmericaPersonalEnvelope:
                    return "Конверт Personal";

                case PageMediaSizeName.NorthAmericaQuarto:
                    return "Quarto";

                case PageMediaSizeName.NorthAmericaStatement:
                    return "Оператор";

                case PageMediaSizeName.NorthAmericaSuperA:
                    return "Super A";

                case PageMediaSizeName.NorthAmericaSuperB:
                    return "Super B";

                case PageMediaSizeName.NorthAmericaTabloid:
                    return "Таблоид";

                case PageMediaSizeName.NorthAmericaTabloidExtra:
                    return "Таблоид Extra";

                case PageMediaSizeName.OtherMetricA3Plus:
                    return "A3 Plus";

                case PageMediaSizeName.OtherMetricA4Plus:
                    return "A4 Plus";

                case PageMediaSizeName.OtherMetricFolio:
                    return "Folio";

                case PageMediaSizeName.OtherMetricInviteEnvelope:
                    return "Конверт Invite";

                case PageMediaSizeName.OtherMetricItalianEnvelope:
                    return "Конверт Italian";

                case PageMediaSizeName.PRC10Envelope:
                    return "Конверт 10 (Китайская Народная Республика)";

                case PageMediaSizeName.PRC10EnvelopeRotated:
                    return "Конверт 10 (Китайская Народная Республика) (повернут)";

                case PageMediaSizeName.PRC16K:
                    return "16K (Китайская Народная Республика)";

                case PageMediaSizeName.PRC16KRotated:
                    return "16K (Китайская Народная Республика) (повернут)";

                case PageMediaSizeName.PRC1Envelope:
                    return "Конверт 1 (Китайская Народная Республика)";

                case PageMediaSizeName.PRC1EnvelopeRotated:
                    return "Конверт 1 (Китайская Народная Республика) (повернут)";

                case PageMediaSizeName.PRC2Envelope:
                    return "Конверт 2 (Китайская Народная Республика)";

                case PageMediaSizeName.PRC2EnvelopeRotated:
                    return "Конверт 2 (Китайская Народная Республика) (повернут)";

                case PageMediaSizeName.PRC32K:
                    return "32K (Китайская Народная Республика)";

                case PageMediaSizeName.PRC32KBig:
                    return "32K большой (Китайская Народная Республика)";

                case PageMediaSizeName.PRC32KRotated:
                    return "32K (Китайская Народная Республика) (повернут)";

                case PageMediaSizeName.PRC3Envelope:
                    return "Конверт 3 (Китайская Народная Республика)";

                case PageMediaSizeName.PRC3EnvelopeRotated:
                    return "Конверт 3 (Китайская Народная Республика) (повернут)";

                case PageMediaSizeName.PRC4Envelope:
                    return "Конверт 4 (Китайская Народная Республика)";

                case PageMediaSizeName.PRC4EnvelopeRotated:
                    return "Конверт 4 (Китайская Народная Республика) (повернут)";

                case PageMediaSizeName.PRC5Envelope:
                    return "Конверт 5 (Китайская Народная Республика)";

                case PageMediaSizeName.PRC5EnvelopeRotated:
                    return "Конверт 5 (Китайская Народная Республика) (повернут)";

                case PageMediaSizeName.PRC6Envelope:
                    return "Конверт 6 (Китайская Народная Республика)";

                case PageMediaSizeName.PRC6EnvelopeRotated:
                    return "Конверт 6 (Китайская Народная Республика) (повернут)";

                case PageMediaSizeName.PRC7Envelope:
                    return "Конверт 7 (Китайская Народная Республика)";

                case PageMediaSizeName.PRC7EnvelopeRotated:
                    return "Конверт 7 (Китайская Народная Республика) (повернут)";

                case PageMediaSizeName.PRC8Envelope:
                    return "Конверт 8 (Китайская Народная Республика)";

                case PageMediaSizeName.PRC8EnvelopeRotated:
                    return "Конверт 8 (Китайская Народная Республика) (повернут)";

                case PageMediaSizeName.PRC9Envelope:
                    return "Конверт 9 (Китайская Народная Республика)";

                case PageMediaSizeName.PRC9EnvelopeRotated:
                    return "Конверт 9 (Китайская Народная Республика) (повернут)";

                case PageMediaSizeName.Roll04Inch:
                    return "Рулон шириной 4 дюйма";

                case PageMediaSizeName.Roll06Inch:
                    return "Рулон шириной 6 дюймов";

                case PageMediaSizeName.Roll08Inch:
                    return "Рулон шириной 8 дюймов";

                case PageMediaSizeName.Roll12Inch:
                    return "Рулон шириной 12 дюймов";

                case PageMediaSizeName.Roll15Inch:
                    return "Рулон шириной 15 дюймов";

                case PageMediaSizeName.Roll18Inch:
                    return "Рулон шириной 18 дюймов";

                case PageMediaSizeName.Roll22Inch:
                    return "Рулон шириной 22 дюйма";

                case PageMediaSizeName.Roll24Inch:
                    return "Рулон шириной 24 дюйма";

                case PageMediaSizeName.Roll30Inch:
                    return "Рулон шириной 30 дюймов";

                case PageMediaSizeName.Roll36Inch:
                    return "Рулон шириной 36 дюймов";

                case PageMediaSizeName.Roll54Inch:
                    return "Рулон шириной 54 дюйма";

                case PageMediaSizeName.Unknown:
                    return "Неизвестный размер бумаги";
            }

            throw new NotImplementedException("Неподдерживаемое название размера страницы: " + value.ToString());
        }

        /// <summary>
        /// Конвертирует значение обратно.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Конечный тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Локаль.</param>
        /// <returns>Не реализовано.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Не реализована обратная локализация названий размеров страниц");
        }

        #endregion
    }
}