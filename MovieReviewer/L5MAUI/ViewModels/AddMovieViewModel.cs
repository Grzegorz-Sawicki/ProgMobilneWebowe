using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using L5MAUI.Pages;
using L5Shared.DTO;
using L5Shared.MessageBox;
using L5Shared.Models;
using L5Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5MAUI.ViewModels
{
    [QueryProperty(nameof(Movie), nameof(Movie))]
    [QueryProperty(nameof(MoviesViewModel), nameof(MoviesViewModel))]
    public partial class AddMovieViewModel : ObservableObject
    {
        private readonly IMovieService _movieService;
        private readonly IDirectorService _directorService;
        private readonly IActorService _actorService;

        private readonly IMessageDialogService _messageDialogService;
        private MoviesViewModel _moviesViewModel;

        [ObservableProperty]
        private MovieDTO movie;

        [ObservableProperty]
        private ObservableCollection<DirectorDTO> _directors;
        [ObservableProperty]
        private DirectorDTO selectedDirector;

        [ObservableProperty]
        private ObservableCollection<ActorDTO> _actors;
        [ObservableProperty]
        private ActorDTO selectedActor1;
        [ObservableProperty]
        private ActorDTO selectedActor2;
        [ObservableProperty]
        private ActorDTO selectedActor3;

        public AddMovieViewModel(IMovieService movieService, IDirectorService directorService, IActorService actorService , IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService;
            _movieService = movieService;
            _actorService = actorService;
            _directorService = directorService;

            GetDirectorsAsync();
            GetActorsAsync();
            
        }

        public MoviesViewModel MoviesViewModel
        {
            get { return _moviesViewModel; }
            set { _moviesViewModel = value; }
        }

        public void OnNavigatedTo()
        {
            SetDirector();
            SetActors();
        }

        public void SetDirector()
        {
            if (Movie.ID != 0 && Directors != null && Directors.Count > 0)
            {
                SelectedDirector = Directors.FirstOrDefault(d => d.ID == Movie.DirectorID);
            }
            else
            {
                SelectedDirector = null;
            }
        }

        public void SetActors()
        {
            if (Movie.ID != 0 && Actors != null && Actors.Count > 0)
            {
                if (Movie.Actors.Count > 0) SelectedActor1 = Actors.FirstOrDefault(a => a.ID == Movie.Actors.ElementAt(0).ID);
                if (Movie.Actors.Count > 1) SelectedActor2 = Actors.FirstOrDefault(a => a.ID == Movie.Actors.ElementAt(1).ID);
                if (Movie.Actors.Count > 2) SelectedActor3 = Actors.FirstOrDefault(a => a.ID == Movie.Actors.ElementAt(2).ID);
            }
            else
            {
                SelectedActor1 = null;
                SelectedActor2 = null;
                SelectedActor3 = null;
            }
        }

        public async Task GetDirectorsAsync()
        {
            var result = await _directorService.GetDirectorsAsync();
            if (result.Success)
            {
                Directors = new ObservableCollection<DirectorDTO>(result.Data);
                SetDirector();
            }
            else
            {
                _messageDialogService.ShowMessage(result.Message);
            }
        }

        public async Task GetActorsAsync()
        {
            var result = await _actorService.GetActorsAsync();
            if (result.Success)
            {
                Actors = new ObservableCollection<ActorDTO>(result.Data);
                SetActors();
            }
            else
            {
                _messageDialogService.ShowMessage(result.Message);
            }
        }


        [RelayCommand]
        public async Task Delete()
        {
            await DeleteMovieAsync();
            await Shell.Current.GoToAsync("../", true);
        }

        public async Task DeleteMovieAsync()
        {
            var result = await _movieService.DeleteMovieAsync(Movie.ID);
            if (result.Success)
            {
                await _moviesViewModel.GetMoviesAsync();
            }
            else
            {
                _messageDialogService.ShowMessage(result.Message);
            }
        }

        [RelayCommand]
        public async Task Save()
        {
            if (Movie.ID == 0)
            {           
                await AddMovieAsync();
            }
            else
            {
                await UpdateMovieAsync();
            }
         
        }



        public async Task AddMovieAsync()
        {
            if (SelectedDirector == null)
            {
                _messageDialogService.ShowMessage("SELECT A DIRECTOR");
                return;
            }
            if (Movie.Title == null)
            {
                _messageDialogService.ShowMessage("WRITE A TITLE");
                return;
            }
            if (Movie.Review == null)
            {
                Movie.Review = "";
            }
            CreateMovieDTO createMovie = new CreateMovieDTO
            {
                Title = Movie.Title,
                ReleaseDate = Movie.ReleaseDate,
                Length = Movie.Length,
                Rating = Movie.Rating,
                Review = Movie.Review,
                DirectorID = SelectedDirector.ID,
                ActorIDs = new List<int>()
                {
                    SelectedActor1?.ID ?? default,
                    SelectedActor2?.ID ?? default,
                    SelectedActor3?.ID ?? default
                }.Where(id => id != default).ToList()
            };

            var result = await _movieService.AddMovieAsync(createMovie);
            if (result.Success)
            {
                await _moviesViewModel.GetMoviesAsync();
            }
            else
            {
                _messageDialogService.ShowMessage(result.Message);
            }
            await Shell.Current.GoToAsync("../", true);
        }

        public async Task UpdateMovieAsync()
        {
            if (SelectedDirector == null)
            {
                _messageDialogService.ShowMessage("SELECT A DIRECTOR");
                return;
            }
            UpdateMovieDTO updatedMovie = new UpdateMovieDTO
            {
                ID = Movie.ID,
                Title = Movie.Title,
                ReleaseDate = Movie.ReleaseDate,
                Length = Movie.Length,
                Rating = Movie.Rating,
                Review = Movie.Review,
                DirectorID = SelectedDirector.ID,
                ActorIDs = new List<int>()
                {
                    SelectedActor1?.ID ?? default,
                    SelectedActor2?.ID ?? default,
                    SelectedActor3?.ID ?? default
                }.Where(id => id != default).ToList()
            };

            var result = await _movieService.UpdateMovieAsync(updatedMovie);
            if (result.Success)
            {
                await _moviesViewModel.GetMoviesAsync();
            }
            else
            {
                _messageDialogService.ShowMessage(result.Message);
            }
            await Shell.Current.GoToAsync("../", true);
        }

        [RelayCommand]
        public async Task AddActor()
        {

            await Shell.Current.GoToAsync(nameof(AddActorPage), true, new Dictionary<string, object>
            {
                {"Actor",new CreateActorDTO() },
                {nameof(AddMovieViewModel), this }
            });

        }

        [RelayCommand]
        public async Task AddDirector()
        {

            await Shell.Current.GoToAsync(nameof(AddDirectorPage), true, new Dictionary<string, object>
            {
                {"Director",new CreateDirectorDTO() },
                {nameof(AddMovieViewModel), this }
            });

        }


    }
}
