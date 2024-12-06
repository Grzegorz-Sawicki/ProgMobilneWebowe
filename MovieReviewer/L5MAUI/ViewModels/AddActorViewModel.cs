using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using L5Shared.DTO;
using L5Shared.MessageBox;
using L5Shared.Models;
using L5Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5MAUI.ViewModels
{
    [QueryProperty(nameof(Actor), nameof(Actor))]
    [QueryProperty(nameof(AddMovieViewModel), nameof(AddMovieViewModel))]  
    public partial class AddActorViewModel : ObservableObject
    {
        private readonly IActorService _actorService;
        private readonly IMessageDialogService _messageDialogService;

        [ObservableProperty]
        private CreateActorDTO actor = new CreateActorDTO();

        [ObservableProperty]
        private ObservableCollection<ActorDTO> actors;

        private AddMovieViewModel _addMovieViewModel;
        public AddMovieViewModel AddMovieViewModel
        {
            get { return _addMovieViewModel; }
            set { _addMovieViewModel = value; }
        }

        public AddActorViewModel(IActorService actorService, IMessageDialogService messageDialogService)
        {
            _actorService = actorService;  
            _messageDialogService = messageDialogService;

            GetActorsAsync();
        }


        public async Task GetActorsAsync()
        {
            var result = await _actorService.GetActorsAsync();
            if (result.Success)
            {
                Actors = new ObservableCollection<ActorDTO>(result.Data);
            }
            else
            {
                _messageDialogService.ShowMessage(result.Message);
            }
        }

        [RelayCommand]
        public async Task Add()
        {
            var result = await _actorService.AddActorAsync(Actor);
            _messageDialogService.ShowMessage(result.Message);

            await GetActorsAsync();
            await AddMovieViewModel.GetActorsAsync();
            await Shell.Current.GoToAsync("../", true);
        }
    }
}
