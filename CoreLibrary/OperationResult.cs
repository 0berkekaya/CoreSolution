using CoreLibrary.Interface;

namespace CoreLibrary
{
    public class OperationResult<T> : IOperationResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public OperationError? Error { get; set; }
        public T? Result { get; set; } = default;

    }

    public class OperationResult : IOperationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
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
        public string? Message { get; set; } = string.Empty;

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
        public int Row { get; set; } = 0;

        /// <summary>
        /// İstisna ile ilişkili sınıf adı.
        /// </summary>
        public string? Class { get; set; }
    }
}
