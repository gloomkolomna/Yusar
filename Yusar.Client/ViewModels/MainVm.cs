using AutoMapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
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
        SimpleStringModel SelectedString { get; set; }
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
        private IAsyncCommand _addOrUpdateItemCommand;
        private SimpleStringModel _selectedString;
        private IAsyncCommand _deleteItemCommand;

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

        public IAsyncCommand AddOrUpdateItemCommand
        {
            get
            {
                return _addOrUpdateItemCommand ?? (_addOrUpdateItemCommand = new AsyncCommand(async param => await AddOrUpdateItemAsync()));
            }
        }

        public IAsyncCommand DeleteItemCommand
        {
            get
            {
                return _deleteItemCommand ?? (_deleteItemCommand = new AsyncCommand(async param => await DeleteItemAsync()));
            }
        }

        public SimpleStringModel SelectedString
        {
            get => _selectedString;
            set
            {
                _selectedString = value;
                OnPropertyChanged();
            }
        }

        private async Task DeleteItemAsync()
        {
            if (SelectedString != null)
            {
                var res = await _repository.DeleteAsync(SelectedString.Id);
                if(res)
                {
                    SynchronizationContext.Post(post =>
                    {
                        SimpleStringItems.Remove(SelectedString);
                        SimpleStringItems = new ObservableCollection<SimpleStringModel>(SimpleStringItems);
                    }, null);
                }
            }
        }

        private async Task AddOrUpdateItemAsync()
        {
            if (SelectedString != null)
            {
                var simpleString = _mapper.Map<SimpleString>(SelectedString);
                if (SelectedString.Id == 0)
                {
                    var retSimpleString = await _repository.CreateAsync(simpleString);
                    var simpleStringModel = _mapper.Map<SimpleStringModel>(retSimpleString);
                    SynchronizationContext.Post(post =>
                    {
                        SimpleStringItems.Remove(SelectedString);
                        SimpleStringItems = new ObservableCollection<SimpleStringModel>(SimpleStringItems);
                        SimpleStringItems.Add(simpleStringModel);
                    }, null);
                }
                else
                {
                    var res = await _repository.UpdateAsync(simpleString);
                    if (res)
                    {
                        SynchronizationContext.Post(post =>
                        {
                            SimpleStringItems = new ObservableCollection<SimpleStringModel>(SimpleStringItems);
                        }, null);
                        
                    }
                }
            }

        }
    }
}
