﻿/*
Conditional Compilation Symbols

Windows Universal 10  - WINDOWS_UWP
Windows Store 8.1     - WINDOWS_APP
Windows Phone 8.1     - WINDOWS_PHONE_APP
Windows Phone 8.0     - WINDOWS_PHONE
WPF .net45            - WINDOWS_DESKTOP

*/

#if !PORTABLE

using System;
using System.Collections.Generic;
using System.Linq;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINRT

using Windows.UI.Xaml.Data;

#endif

#if WINDOWS_DESKTOP || WINDOWS_PHONE || DESKTOP

using System.Globalization;
using System.Windows.Data;

#endif

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINRT

        public object Convert(object value, Type targetType, object parameter, string cultureOrlanguage)
#elif WINDOWS_DESKTOP || WINDOWS_PHONE || DESKTOP

        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
#endif
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, cultureOrlanguage));
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINRT

        public object ConvertBack(object value, Type targetType, object parameter, string cultureOrlanguage)
#elif WINDOWS_DESKTOP || WINDOWS_PHONE || DESKTOP

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
#endif
        {
            throw new NotSupportedException();
        }
    }
}

#endif