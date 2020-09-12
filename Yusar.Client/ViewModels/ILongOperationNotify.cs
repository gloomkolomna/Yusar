using System.ComponentModel;

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
