using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garant_R_NEW
{
    public static class BufferClass1

    {
        //Описание делегата - обработчика события
        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        //Событие 
        public static event ValueChangedEventHandler ValueChanged;

        private static String passwordChecked = String.Empty;

        //Свойство DataBuffer
        public static string PasswordChecked
        {
            get
            {
                return passwordChecked;
            }
            set
            {
                passwordChecked = value;

                //При изменении данных свойства вызывается событие ValueChanged
                ValueChanged(null, EventArgs.Empty);
            }
        }
    }
}
