using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using L5Shared.DTO;
using L5Shared.MessageBox;
using L5Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5MAUI.ViewModels
{
    [QueryProperty(nameof(Director), nameof(Director))]
    [QueryProperty(nameof(AddMovieViewModel), nameof(AddMovieViewModel))]
    public partial class AddDirectorViewModel : ObservableObject
    {
        private readonly IDirectorService _directorService;
        private readonly IMessageDialogService _messageDialogService;

        [ObservableProperty]
        private CreateDirectorDTO director = new CreateDirectorDTO();

        [ObservableProperty]
        private ObservableCollection<DirectorDTO> directors;

        private AddMovieViewModel _addMovieViewModel;
        public AddMovieViewModel AddMovieViewModel
        {
            get { return _addMovieViewModel; }
            set { _addMovieViewModel = value; }
        }

        public AddDirectorViewModel(IDirectorService directorService, IMessageDialogService messageDialogService)
        {
            _directorService = directorService;  
            _messageDialogService = messageDialogService;

            GetDirectorsAsync();
        }


        public async Task GetDirectorsAsync()
        {
            var result = await _directorService.GetDirectorsAsync();
            if (result.Success)
            {
                Directors = new ObservableCollection<DirectorDTO>(result.Data);
            }
            else
            {
                _messageDialogService.ShowMessage(result.Message);
            }
        }

        [RelayCommand]
        public async Task Add()
        {
            var result = await _directorService.AddDirectorAsync(Director);
            _messageDialogService.ShowMessage(result.Message);

            await GetDirectorsAsync();
            await AddMovieViewModel.GetDirectorsAsync();
            await Shell.Current.GoToAsync("../", true);
        }
    }
}
