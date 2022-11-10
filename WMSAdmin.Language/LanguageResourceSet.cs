using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Language
{
    public class LanguageResourceSet : ResourceSet
    {
        public LanguageResourceSet(Utility.Configuration configuration, CultureInfo culture, string languageGroupCode) : base(new LanguageResourceReader(configuration, culture, languageGroupCode))
        {
        }

        public override Type GetDefaultReader()
        {
            return typeof(LanguageResourceReader);
        }
    }
}
