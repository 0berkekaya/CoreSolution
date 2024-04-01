using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public static class StringExtensions
    {
        /// <summary>
        /// Belirtilen stringin boş veya null olup olmadığını kontrol eder. Boşluk karakterleri dikkate alınır.
        /// </summary>
        /// <param name="_">Kontrol edilecek string.</param>
        /// <returns>Belirtilen string boş veya null ise true, aksi halde false döner.</returns>
        public static bool IsNullOrEmpty(this string? _) => _ == null || _?.Trim() == string.Empty;
    }
}
