namespace CoreLibrary.Interface
{
    /// <summary>
    /// Genel bir işlem durumu, bir mesaj, bir değer ve isteğe bağlı olarak bir hata içeren bir bilgi yapısı.
    /// </summary>
    public interface IOperationResult
    {
        /// <summary>
        /// İşlem durumunu gösteren bir değer. True başarılı bir işlemi, false başarısız bir işlemi temsil eder.
        /// </summary>
        bool Success { get; set; }
        /// <summary>
        /// İşlemle ilgili bir mesaj.
        /// </summary>
        string? Message { get; set; }

        /// <summary>
        /// İşlem sırasında oluşabilecek özel bir istisnai durumu temsil eden özel bir hata nesnesi.
        /// </summary>
        OperationError? Error { get; set; }
    }

    /// <summary>
    /// Genel bir işlem durumu, bir mesaj, bir değer ve isteğe bağlı olarak bir hata içeren bir bilgi yapısı.
    /// </summary>
    /// <typeparam name="T">Değer tipi.</typeparam>
    public interface IOperationResult<T> : IOperationResult
    {
        /// <summary>
        /// İşlem sonucunda elde edilen değer.
        /// </summary>
        T? Result { get; set; }
    }
}
