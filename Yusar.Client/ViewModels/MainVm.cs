using AutoMapper;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Yusar.Client.Models;
using Yusar.Client.Services;
using Yusar.Core;
using Yusar.Core.Entities;

namespace Yusar.Client.ViewModels
{

    public interface IMainVm : IBaseVm
    {
        IBaseVm SelectedView { get; set; }
        SynchronizationContext GetContext();
        ObservableCollection<SimpleStringModel> SimpleStringItems { get; set; }
        Task Init();
    }

    public class MainVm : ObservableObject, IMainVm, ILongOperationNotify
    {
        private static readonly SynchronizationContext SynchronizationContext = SynchronizationContext.Current;
        private readonly ILongOperationService _longOperationService;
        private readonly IDialogService _dialogService;
        private readonly IMapper _mapper;
        private readonly IYusarRepository<SimpleString> _repository;
        private bool _longOperationInProgress;
        private string _longOperationText;
        private IBaseVm _selectedView;
        private ObservableCollection<SimpleStringModel> _simpleStringItems;

        public MainVm(ILongOperationService longOperationService, IDialogService dialogService, IMapper mapper, IYusarRepository<SimpleString> repository)
        {
            _longOperationService = longOperationService;
            _dialogService = dialogService;
            _mapper = mapper;
            _repository = repository;
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

        public ObservableCollection<SimpleStringModel> SimpleStringItems
        {
            get => _simpleStringItems;
            set
            {
                _simpleStringItems = value;
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

        public async Task Init()
        {
            SimpleStringItems = new ObservableCollection<SimpleStringModel>();
            var allItems = await _longOperationService.ExecuteAsync(async () =>
            {
                 return await _repository.GetAllAsync();                
            }, "");

            foreach (var item in allItems.Result)
            {
                var mapItem = _mapper.Map<SimpleStringModel>(item);
                SimpleStringItems.Add(mapItem);
            }
        }
    }
}
