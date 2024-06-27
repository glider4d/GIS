namespace Kts.Gis.Reports.ViewModels
{
    /// <summary>
    /// Вид отчета.
    /// </summary>
    public enum ReportKind
    {
        /// <summary>
        /// Отчет с информацией о количестве введенных объектов.
        /// </summary>
        AddedObjectInfo = 1,

        /// <summary>
        /// Отчет о несопоставленных объектах.
        /// </summary>
        DiffObjects = 5,

        /// <summary>
        /// Отчет о гидравлике.
        /// </summary>
        Hydraulics = 3,

        /// <summary>
        /// Отчет о процентовке сопоставления.
        /// </summary>
        IntegrationStats = 4,

        /// <summary>
        /// О жилищном фонде и договорных подключениях.
        /// </summary>
        Kts = 6,

        /// <summary>
        /// Отчет о технических характеристиках.
        /// </summary>
        TechSpec = 2,
        /// Отчет о измерительных приборах
        MetersStats = 7
    }
}