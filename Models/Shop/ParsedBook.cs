using ReedBooks.Core;
using System.Collections.ObjectModel;

namespace ReedBooks.Models.Shop
{
    public class ParsedBook : ObservableObject
    {
		private string _name;

		public string Name
		{
			get { return _name; }
			set 
			{ 
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		private string _author;
		public string Author
		{
			get { return _author; }
			set 
			{
				_author = value;
				OnPropertyChanged(nameof(Author));
			}
		}

		private double _rating;
		public double Rating
		{
			get { return _rating; }
			set 
			{
				_rating = value; 
				OnPropertyChanged(nameof(Rating));
			}
		}

		private int _ratedUsersNumber;
		public int RatedUsersNumber
		{
			get => _ratedUsersNumber;
			set
			{
				_ratedUsersNumber = value;
				OnPropertyChanged(nameof(RatedUsersNumber));
			}
		}

		private ObservableCollection<string> _genres;
		public ObservableCollection<string> Genres
		{
			get { return _genres; }
			set
			{
				_genres = value;
				OnPropertyChanged(nameof(Genres));
			}
		}

		private int _year;
		public int Year
		{
			get { return _year; }
			set 
			{
				_year = value;
				OnPropertyChanged(nameof(Year));
			}
		}

		private string _description;
		public string Description
        {
			get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

		private string _cover;

		public string Cover
		{
			get { return _cover; }
			set 
			{ 
				_cover = value;
				OnPropertyChanged(nameof(Cover));
			}
		}


		public ParsedBook()
		{
			Genres = new ObservableCollection<string>();	
		}
	}
}
