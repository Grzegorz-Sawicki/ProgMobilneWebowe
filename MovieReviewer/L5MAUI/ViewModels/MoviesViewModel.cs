using L5Shared.MessageBox;
using L5Shared.Services;
using L5Shared.Models;
using L5Shared.DTO;
using L5MAUI.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5MAUI.ViewModels
{
    public partial class MoviesViewModel : ObservableObject
    {
        private readonly IActorService _actorService;
        private readonly IDirectorService _directorService;
        private readonly IMovieService _movieService;
        private readonly IMessageDialogService _messageDialogService;

        [ObservableProperty]
        private ObservableCollection<MovieDTO> _movies;

        [ObservableProperty]
        private MovieDTO _selectedMovie;

        public MoviesViewModel(IMovieService movieService, IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService;
            _movieService = movieService;

            GetMoviesAsync();
        }

        public async Task GetMoviesAsync()
        {
            try
            {
                var result = await _movieService.GetMoviesAsync();
                if (result.Success)
                {
                    Movies = new ObservableCollection<MovieDTO>(result.Data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task New()
        {
            SelectedMovie = new MovieDTO();
            await Shell.Current.GoToAsync(nameof(AddMovieView), true, new Dictionary<string, object>
            {
                {"Movie",SelectedMovie },
                {nameof(MoviesViewModel), this }
            });
        }

        [RelayCommand]
        public async Task ShowDetails(MovieDTO movie)
        {
            
            SelectedMovie = movie;

            await Shell.Current.GoToAsync(nameof(AddMovieView), true, new Dictionary<string, object>
            {
                {"Movie",movie },
                {nameof(MoviesViewModel), this }
            });
            
        }

    }
}
