using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garant_R_NEW
{
    public static class BufferClass

    {
        //Описание делегата - обработчика события
        public delegate void ValueChangedEventHandler(object sender, EventArgs e);

        //Событие 
        public static event ValueChangedEventHandler ValueChanged;

        public static event ValueChangedEventHandler ValueChanged1;

        //Изолированная переменная - хранилище данных, передаваемых в свойство DataBuffer
        private static String dataBuffer = String.Empty;

        //Изолированная переменная - хранилище данных, передаваемых в свойство DataBuffer
        private static bool dataBuffer1 = false;

        //Свойство DataBuffer
        public static String DataBuffer
        {
            get
            {
                return dataBuffer;
            }
            set
            {
                dataBuffer = value;

                //При изменении данных свойства вызывается событие ValueChanged
                ValueChanged(null, EventArgs.Empty);
            }
        }

        public static bool DataBuffer1
        {
            get
            {
                return dataBuffer1;
            }
            set
            {
                dataBuffer1 = value;

                //При изменении данных свойства вызывается событие ValueChanged
                ValueChanged1(null, EventArgs.Empty);
            }
        }
    }
}
