using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolExportVideo.Common
{
    public static class Converter
    {
        public static T ConvertValueByType<T>(string value)
        {
            if (value != null)
            {
                object result;

                switch (Type.GetTypeCode(typeof(T)))
                {
                    case TypeCode.Int16:
                        Int16 i16Parse;
                        Int16.TryParse(value, out i16Parse);

                        result = i16Parse;
                        break;

                    case TypeCode.Int32:
                        Int32 i32Parse;
                        Int32.TryParse(value, out i32Parse);

                        result = i32Parse;
                        break;

                    case TypeCode.Int64:
                        Int64 i64Parse;
                        Int64.TryParse(value, out i64Parse);

                        result = i64Parse;
                        break;

                    case TypeCode.Decimal:
                        decimal decParse;
                        decimal.TryParse(value, out decParse);
                        result = decParse;
                        break;

                    case TypeCode.Double:
                        double dParse;
                        double.TryParse(value, out dParse);
                        result = dParse;
                        break;

                    case TypeCode.Boolean:
                        bool bParse;
                        bool.TryParse(value, out bParse);

                        result = bParse;
                        break;

                    case TypeCode.DateTime:

                        DateTimeOffset.TryParse(value, out DateTimeOffset dateTimeOffset);
                        result = dateTimeOffset.LocalDateTime;

                        break;

                    default:
                        result = value;

                        break;
                }

                return (T)result;
            }
            else
            {
                return default;
            }
        }
    }
}
