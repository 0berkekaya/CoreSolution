namespace CoreLibrary
{
    /// <summary>
    /// Genel bir işlem durumu, bir mesaj, bir değer ve isteğe bağlı olarak bir hata içeren bir bilgi yapısı.
    /// </summary>
    /// <typeparam name="T">Değer tipi.</typeparam>
    public class OperationResult<T>
    {
        /// <summary>
        /// İşlemle ilgili bir mesaj.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// İşlem durumunu gösteren bir değer. True başarılı bir işlemi, false başarısız bir işlemi temsil eder.
        /// </summary>
        public bool IsSuccessful { get; set; } = false;

        /// <summary>
        /// İşlem sonucunda elde edilen değer.
        /// </summary>
        public T? Result { get; set; }

        /// <summary>
        /// İşlem sırasında oluşabilecek özel bir istisnai durumu temsil eden özel bir hata nesnesi.
        /// </summary>
        public OperationError? Error { get; set; }
    }

    /// <summary>
    /// Genel bir işlem durumu, bir mesaj, bir değer ve isteğe bağlı olarak bir hata içeren bir bilgi yapısı.
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// İşlemle ilgili bir mesaj.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// İşlem durumunu gösteren bir değer. True başarılı bir işlemi, false başarısız bir işlemi temsil eder.
        /// </summary>
        public bool IsSuccessful { get; set; } = false;

        /// <summary>
        /// İşlem sonucunda elde edilen değer.
        /// </summary>
        public object? Result { get; set; }

        /// <summary>
        /// İşlem sırasında oluşabilecek özel bir istisnai durumu temsil eden özel bir hata nesnesi.
        /// </summary>
        public OperationError? Error { get; set; }
    }

    /// <summary>
    /// Sadece işlem sonuç durumu ve isteğe bağlı mesajı içeren yapı.
    /// </summary>
    public class OperationResultInfo
    {
        /// <summary>
        /// İşlemle ilgili bir mesaj.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// İşlem durumunu gösteren bir değer. True başarılı bir işlemi, false başarısız bir işlemi temsil eder.
        /// </summary>
        public bool IsSuccessful { get; set; } = false;

        /// <summary>
        /// İşlem sırasında oluşabilecek özel bir istisnai durumu temsil eden özel bir hata nesnesi.
        /// </summary>
        public OperationError? Error { get; set; }
    }

    /// <summary>
    /// Özel istisna durumunu temsil eden sınıf.
    /// </summary>
    public class OperationError
    {
        /// <summary>
        /// İstisna ile ilişkili mesaj.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// İstisna ile ilişkili detayları içeren bir liste.
        /// </summary>
        public List<ExceptionDetail>? Details { get; set; }
    }

    /// <summary>
    /// İstisna ayrıntılarını temsil eden sınıf.
    /// </summary>
    public class ExceptionDetail
    {
        /// <summary>
        /// İstisna ile ilişkili satır numarası.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// İstisna ile ilişkili sınıf adı.
        /// </summary>
        public string? Class { get; set; }
    }
}
