using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public static class BoolExtensions
    {
        /// <summary>
        /// Belirtilen boolean değerin tersini döndürür.
        /// </summary>
        /// <param name="_">Kontrol edilecek boolean değer.</param>
        /// <returns>Belirtilen boolean değerin tersi.</returns>
        public static bool NotThis(this bool _) => !_;
    }
}
