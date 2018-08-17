using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace EatAsYouGoApi.Dtos
{ 
    [TypeConverter(typeof(UserOptionsTypeConverter))]
    [DataContract(Name = "UserOptions")]
    public class UserOptionsDto
    {
        [DataMember]
        public bool ShowActiveOnly { get; set; }

        [DataMember]
        public bool ShowPermissions { get; set; }

        public static bool TryParse(string strValue, out UserOptionsDto userOptionsDto)
        {
            userOptionsDto = null;

            var parts = strValue.Split(',');
            if (parts.Length != 2)
                return false;

            bool part1Value, part2Value, showActiveOnly = false, showPermissions = false;
            int value1, value2;

            if (int.TryParse(parts[0], out value1))
                showActiveOnly = value1 > 0;
            else if (bool.TryParse(parts[0], out part1Value))
                showActiveOnly = part1Value;

            if (int.TryParse(parts[1], out value2))
                showPermissions = value2 > 0;
            else if (bool.TryParse(parts[1], out part2Value))
                showPermissions = part2Value;

            userOptionsDto = new UserOptionsDto { ShowPermissions = showPermissions, ShowActiveOnly = showActiveOnly };

            return true;
        }
    }

    public class UserOptionsTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is string))
                return base.ConvertFrom(context, culture, value);

            UserOptionsDto userOptionsDto;
            return UserOptionsDto.TryParse((string)value, out userOptionsDto) ? userOptionsDto : base.ConvertFrom(context, culture, value);
        }
    }
}
