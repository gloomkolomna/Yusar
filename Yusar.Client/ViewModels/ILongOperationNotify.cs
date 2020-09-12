using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yusar.Client.ViewModels
{
    public interface ILongOperationNotify : INotifyPropertyChanged
    {
        /// <summary>
        /// Признак выполнения длительной операции
        /// </summary>
        bool LongOperationInProgress { get; set; }
        /// <summary>
        /// Текст длительной операции
        /// </summary>
        string LongOperationText { get; set; }
    }
}
