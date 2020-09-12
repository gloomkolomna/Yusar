using System;
using System.Threading;
using Yusar.Client.Services;

namespace Yusar.Client.ViewModels
{

    public interface IMainVm : IBaseVm
    {
        IBaseVm SelectedView { get; set; }
        SynchronizationContext GetContext();
    }

    public class MainVm : ObservableObject, IMainVm, ILongOperationNotify
    {
        private static readonly SynchronizationContext SynchronizationContext = SynchronizationContext.Current;
        private readonly ILongOperationService _longOperationService;
        private readonly IDialogService _dialogService;
        private bool _longOperationInProgress;
        private string _longOperationText;
        private IBaseVm _selectedView;

        public MainVm(ILongOperationService longOperationService, IDialogService dialogService)
        {
            _longOperationService = longOperationService;
            _dialogService = dialogService;
            Subscribe();
        }

        public bool LongOperationInProgress
        {
            get => _longOperationInProgress;
            set
            {
                _longOperationInProgress = value;
                OnPropertyChanged();
            }
        }

        public string LongOperationText
        {
            get => _longOperationText;
            set
            {
                _longOperationText = value;
                OnPropertyChanged();
            }
        }
        public IBaseVm SelectedView
        {
            get => _selectedView;
            set
            {
                _selectedView = value;
                OnPropertyChanged();
            }
        }

        private void Subscribe()
        {
            _longOperationService.LongOperationProgressChanged += OnLongOperationProgressChanged;
            _longOperationService.LongOperationTextChanged += OnLongOperationTextChanged;
            _longOperationService.LongOperationExceptioned += OnLongOperationExceptioned;
        }

        private void UnSubscribe()
        {
            _longOperationService.LongOperationProgressChanged -= OnLongOperationProgressChanged;
            _longOperationService.LongOperationTextChanged -= OnLongOperationTextChanged;
            _longOperationService.LongOperationExceptioned -= OnLongOperationExceptioned;
        }

        private void OnLongOperationExceptioned(Exception ex)
        {
            _dialogService.Show(DialogIcon.Error, "Ошибка", ex.Message);
            LongOperationInProgress = false;
        }

        private void OnLongOperationProgressChanged(bool result)
        {
            SynchronizationContext.Post(post =>
            {
                LongOperationInProgress = result;
            }, null);
        }

        private void OnLongOperationTextChanged(string text)
        {
            SynchronizationContext.Post(post =>
            {
                LongOperationText = text;
            }, null);
        }

        public SynchronizationContext GetContext()
        {
            return SynchronizationContext;
        }

        public void Close()
        {
            UnSubscribe();
            SelectedView?.Close();
        }
    }
}
