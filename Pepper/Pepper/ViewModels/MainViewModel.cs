using Pepper.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Xam.Wikia.Models.SearchSuggestions;
using Xam.Wikia.Models.PhpQuery;
using Xamarin.Forms;
using Plugin.Connectivity;

namespace Pepper.ViewModels
{
    public class MainViewModel : BindableObject
    {
        private static MainViewModel _Instance;
        /// <summary>
        /// MainViewModel variable
        /// </summary>
        public static MainViewModel Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MainViewModel();
                return _Instance;
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            SearchCommand = new Command(PerformSearch);
            ShowCommand = new Command<SearchSuggestionsItems>(ShowSelectedData);
            ShowMarvelDetailCommand = new Command<Xam.Marvelous.Model.Base.Character>(ShowMarvelDetail);
            ShowMarvelComicsImageCommand = new Command<Xam.Marvelous.Model.Base.Comic>(ShowMarvelComicsImage);
            ShowTumblrDetailCommand = new Command<Xam.Tumblr.Models.Photo>(ShowTumblrDetail);
            ShowSimilarTumblrDetailCommand = new Command<Xam.Tumblr.Models.Photo>(ShowSimilarTumblrDetail);
            ShowRssDetailCommand = new Command<Xam.Rss.FeedItem>(ShowRssDetail);
        }        

        #region Commands

        /// <summary>
        /// Command search when enter text
        /// </summary>
        public ICommand SearchCommand { get; private set; }

        /// <summary>
        /// Command show rss from marvel news
        /// </summary>
        public ICommand ShowRssDetailCommand { get; private set; }

        /// <summary>
        /// Command show data from suggestion
        /// </summary>
        public ICommand ShowCommand { get; private set; }

        /// <summary>
        /// Command show detail data from character marvel
        /// </summary>
        public ICommand ShowMarvelDetailCommand { get; private set; }

        /// <summary>
        /// Command show marvel comics images from comic selected
        /// </summary>
        public ICommand ShowMarvelComicsImageCommand { get; private set; }

        /// <summary>
        /// Command show tumblr detail
        /// </summary>
        public ICommand ShowTumblrDetailCommand { get; private set; }

        /// <summary>
        /// Command Show similar tumblr fotos
        /// </summary>
        public ICommand ShowSimilarTumblrDetailCommand { get; private set; }        

        #endregion

        private String _SuggestName;
        /// <summary>
        /// Searched name
        /// </summary>
        public String SuggestName
        {
            get
            {
                return _SuggestName;
            }
            set
            {
                _SuggestName = value;
                OnPropertyChanged("SuggestName");
            }
        }

        private SearchSuggestionsItems _SelectedSuggestion;
        /// <summary>
        /// Main suggestion selected for character search marvel api
        /// </summary>
        public SearchSuggestionsItems SelectedSuggestion
        {
            get
            {
                return _SelectedSuggestion;
            }
            set
            {
                _SelectedSuggestion = value;
                OnPropertyChanged("SelectedSuggestion");
            }
        }

        private SearchSuggestionsItems _SelectedSuggestionToShow;
        /// <summary>
        /// Selected suggestion to show marvel wikia
        /// </summary>
        public SearchSuggestionsItems SelectedSuggestionToShow
        {
            get
            {
                return _SelectedSuggestionToShow;
            }
            set
            {
                _SelectedSuggestionToShow = value;
                OnPropertyChanged("SelectedSuggestionToShow");
            }
        }

        Xam.Marvelous.Model.Base.Character _SelectedCharacter;
        /// <summary>
        /// Selected character Marvel API to detail
        /// </summary>
        public Xam.Marvelous.Model.Base.Character SelectedCharacter
        {
            get
            {
                return _SelectedCharacter;
            }
            set
            {
                _SelectedCharacter = value;
                OnPropertyChanged("SelectedCharacter");
            }
        }

        Xam.Tumblr.Models.Photo _SelectedTumblrPhoto;
        /// <summary>
        /// Selected Photo Tumblr to detail
        /// </summary>
        public Xam.Tumblr.Models.Photo SelectedTumblrPhoto
        {
            get
            {
                return _SelectedTumblrPhoto;
            }
            set
            {
                _SelectedTumblrPhoto = value;
                OnPropertyChanged("SelectedTumblrPhoto");
            }
        }

        /// <summary>
        /// Method to GET all data from sources
        /// </summary>
        /// <returns></returns>
        public async Task<HtmlWebViewSource> GetDataFromSuggestion()
        {
            ShowWiki = false;
            var html = new HtmlWebViewSource();
            bool error = false;

            try
            {
                String searchtitle = String.Empty;
                bool is616 = false;
                if (SelectedSuggestionToShow != null)
                {
                    if (!SelectedSuggestionToShow.Title.EndsWith("(Earth-616)"))
                    {
                        searchtitle = SelectedSuggestionToShow.Title;
                    }
                    else
                    {
                        searchtitle = SelectedSuggestionToShow.Title.Replace(" (Earth-616)", "");
                        is616 = true;
                    }
                }
                else
                {
                    return null;
                }

                var names = await WikiaApiService.Instance.GetNames(SelectedSuggestionToShow.Title);
                names = names.OrderBy(n => n.NumericId).ToList();
                string titlewithoutspecialchars = SelectedSuggestionToShow.Title.Replace("_", "");
                var found = names.Where(n => n.Title == titlewithoutspecialchars).FirstOrDefault();
                CharacterPhpInfo art = null;
                if (found != null)
                    art = await WikiaApiService.Instance.GetArticleById(found.Id);
                else
                {
                    art = await WikiaApiService.Instance.GetArticleByName(SelectedSuggestionToShow.Title);
                }

                if (is616)
                {
                    found = names.Where(n => (n.Url.EndsWith("_(Earth-616)"))).FirstOrDefault();
                    if (found != null)
                    {
                        var history = names.Where(n => n.Url.EndsWith("(Earth-616)/Expanded_History")).FirstOrDefault();
                        if (history != null)
                        {
                            found.Title = found.Title.Replace(" (Earth-616)", "");
                            art = await WikiaApiService.Instance.GetArticleById(history.Id);
                        }
                        else
                        {
                            art = await WikiaApiService.Instance.GetArticleByName(SelectedSuggestionToShow.Title);

                        }
                    }
                    else
                    {
                        art = await WikiaApiService.Instance.GetArticleByName(SelectedSuggestionToShow.Title);
                    }
                }

                String name = String.Empty;
                if (SelectedSuggestion != null)
                    name = SelectedSuggestion.Title;
                else
                    if (SelectedSuggestionToShow != null)
                    name = SelectedSuggestionToShow.Title;

                FindAnotherData(name);

                html.Html = art.text;
                ShowWiki = true;
                return html;

            } catch (Exception ex)
            {
                error = true;
            }

            if (error)
            {
                var art = await WikiaApiService.Instance.GetArticleByName(SelectedSuggestion.Title);
                ShowWiki = true;
                html.Html = art.text;
            }

            ShowWiki = true;
            return html;
        }

        /// <summary>
        /// Find data marvel api and tumblr
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task FindAnotherData(String name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                if (name.EndsWith("(Earth-616)"))
                    name = name.Replace("(Earth-616)", "");

                Characters = MarvelApiService.Instance.FindCharByNameStart(name);
                TumblrPhotos = await TumblrApiService.Instance.GetPhotos(name);
            }
        }

        List<SearchSuggestionsItems> _Suggestions;
        /// <summary>
        /// Suggestions items for initial search
        /// </summary>
        public List<SearchSuggestionsItems> Suggestions {
            get
            {

                if (_Suggestions == null)
                    _Suggestions = new List<SearchSuggestionsItems>();
                return _Suggestions;
            }
            set
            {
                _Suggestions = value;
                OnPropertyChanged("Suggestions");
            }
        }

        List<Xam.Rss.FeedItem> _RssItems;
        /// <summary>
        /// List of Rss items news
        /// </summary>
        public List<Xam.Rss.FeedItem> RssItems
        {
            get
            {

                if (_RssItems == null)
                    _RssItems = new List<Xam.Rss.FeedItem>();
                return _RssItems;
            }
            set
            {
                _RssItems = value;
                OnPropertyChanged("RssItems");
            }
        }

        List<Xam.Marvelous.Model.Base.Character> _Characters;
        /// <summary>
        /// Marvel API characters related to search
        /// </summary>
        public List<Xam.Marvelous.Model.Base.Character> Characters
        {
            get
            {

                if (_Characters == null)
                    _Characters = new List<Xam.Marvelous.Model.Base.Character>();
                return _Characters;
            }
            set
            {
                _Characters = value;
                OnPropertyChanged("Characters");
            }
        }        

        List<Xam.Tumblr.Models.Photo> _TumblrPhotos;
        /// <summary>
        /// Tumblr Photos related to search
        /// </summary>
        public List<Xam.Tumblr.Models.Photo> TumblrPhotos
        {
            get
            {

                if (_TumblrPhotos == null)
                    _TumblrPhotos = new List<Xam.Tumblr.Models.Photo>();
                return _TumblrPhotos;
            }
            set
            {
                _TumblrPhotos = value;
                OnPropertyChanged("TumblrPhotos");
            }
        }

        List<Xam.Tumblr.Models.Photo> _SimilarTumblrPhotos;
        /// <summary>
        /// Similar Photos (with same URL) Tumblr to display into detail
        /// </summary>
        public List<Xam.Tumblr.Models.Photo> SimilarTumblrPhotos
        {
            get
            {

                if (_SimilarTumblrPhotos == null)
                    _SimilarTumblrPhotos = new List<Xam.Tumblr.Models.Photo>();
                return _SimilarTumblrPhotos;
            }
            set
            {
                _SimilarTumblrPhotos = value;
                OnPropertyChanged("SimilarTumblrPhotos");
            }
        }
        
        List<Xam.Marvelous.Model.Base.Comic> _Comics;
        /// <summary>
        /// Comic related Marvel API to show with character marvel detail
        /// </summary>
        public List<Xam.Marvelous.Model.Base.Comic> Comics
        {
            get
            {

                if (_Comics == null)
                    _Comics = new List<Xam.Marvelous.Model.Base.Comic>();
                return _Comics;
            }
            set
            {
                _Comics = value;
                OnPropertyChanged("Comics");
            }
        }

        List<Xam.Marvelous.Model.Base.Story> _Stories;
        /// <summary>
        /// Stories (Not used)
        /// </summary>
        public List<Xam.Marvelous.Model.Base.Story> Stories
        {
            get
            {

                if (_Stories == null)
                    _Stories = new List<Xam.Marvelous.Model.Base.Story>();
                return _Stories;
            }
            set
            {
                _Stories = value;
                OnPropertyChanged("Stories");
            }
        }


        /// <summary>
        /// Flag inverse ShowSuggest
        /// </summary>
        private bool _InvShowSuggest;
        public bool InvShowSuggest
        {
            get
            {
                return _InvShowSuggest;
            }

            set
            {
                _InvShowSuggest = value;
                OnPropertyChanged("InvShowSuggest");
            }
        }

        /// <summary>
        /// Flag Search Error
        /// </summary>
        private bool _SearchError;
        public bool SearchError
        {
            get
            {
                return _SearchError;
            }

            set
            {
                _SearchError = value;
                OnPropertyChanged("SearchError");
            }
        }


        /// <summary>
        /// Flag Show Wiki
        /// </summary>
        private bool _ShowWiki = false;
        public bool ShowWiki
        {
            get
            {
                return _ShowWiki;
            }

            set
            {
                _ShowWiki = value;
                OnPropertyChanged("ShowWiki");
            }
        }

        /// <summary>
        /// Flag Show Search bar
        /// </summary>
        private bool _ShowSuggest = false;
        public bool ShowSuggest
        {
            get
            {
                return _ShowSuggest;
            }

            set
            {
                _ShowSuggest = value;
                InvShowSuggest = !_ShowSuggest;
                OnPropertyChanged("ShowSuggest");
            }
        }

        /// <summary>
        /// String for info
        /// </summary>
        private string _InfoMsg;
        public string InfoMsg
        {
            get
            {
                return _InfoMsg;
            }

            set
            {
                _InfoMsg = value;
                OnPropertyChanged("InfoMsg");
            }
        }

        /// <summary>
        /// Flag for know if app is busy
        /// </summary>
        private bool _IsBusy = false;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }

            set
            {
                _IsBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// Flag for know if network is ok
        /// </summary>
        private bool _IsNetworkOk = false;
        public bool IsNetworkOk
        {
            get
            {
                return _IsNetworkOk;
            }

            set
            {
                _IsNetworkOk = value;
                OnPropertyChanged("IsNetworkOk");
            }
        }

        /// <summary>
        /// Flag for know if app is suggest names
        /// </summary>
        private bool _IsSuggest;
        public bool IsSuggest
        {
            get
            {
                return _IsSuggest;
            }

            set
            {
                _IsSuggest = value;
                OnPropertyChanged("IsSuggest");
            }
        }

        /// <summary>
        /// Method search suggestions
        /// </summary>
        private async void PerformSearch()
        {
            try
            {
                                
                ShowSuggest = false;
                string suggestname = SuggestName;
                IsBusy = true;
                String searchfreesymbol = suggestname.Replace("-", "").ToUpper();
                var suggestions = await WikiaApiService.Instance.GetSuggestions(suggestname);
                var suggestion = suggestions.Where(n =>
                (n.Title == suggestname.Replace("_", " ")) || //Equal name described
                (n.Title == (suggestname.Replace("_", " ") + " (Earth-616)")) || //Characters real names
                (n.Title.Replace("-", "").ToUpper() == searchfreesymbol) //Special chars like Spider-man
                ).FirstOrDefault();
                Suggestions = suggestions;
                SelectedSuggestion = suggestion;
                IsBusy = false;
                ShowSuggest = true;
                if (String.IsNullOrEmpty(SuggestName))
                    ShowSuggest = false;
            }
            catch (Exception ex)
            {
                ShowSuggest = false;
                System.Diagnostics.Debug.WriteLine("Error on SearchDataByName: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Method show data related to Suggestion selected
        /// </summary>
        /// <param name="item"></param>
        private async void ShowSelectedData(SearchSuggestionsItems item)
        {
            if (IsBusy)
                return;

            Pepper.Helpers.HideHeyboardFromScreen();

            ShowSuggest = false;
            SelectedSuggestionToShow = item;
            IsBusy = true;
            HtmlWebViewSource source = await GetDataFromSuggestion();
            if (source != null)
            {
                ResultPage p = new ResultPage(source);
                p.BindingContext = MainViewModel.Instance;
                p.Title = SelectedSuggestionToShow.Title.ToUpper();
                await App.Current.MainPage.Navigation.PushAsync(p, true);
            }
            else
            {
                ResultPage p = new ResultPage();
                p.BindingContext = MainViewModel.Instance;
                p.Title = SelectedSuggestionToShow.Title.ToUpper();
                await App.Current.MainPage.Navigation.PushAsync(p, true);
            }
            IsBusy = false;
        }

        /// <summary>
        /// Show marvel character detail and comics
        /// </summary>
        /// <param name="item"></param>
        private async void ShowMarvelDetail(Xam.Marvelous.Model.Base.Character item)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            await Task.Delay(1000);
            SelectedCharacter = item;
            MarvelDetailPage p = new MarvelDetailPage();                        
            p.Title = SelectedCharacter.Name.ToUpper();
            UpdateData();
            p.BindingContext = MainViewModel.Instance;
            await App.Current.MainPage.Navigation.PushAsync(p, true);            
            IsBusy = false;
        }    

        /// <summary>
        /// Update data form character marvel api selected
        /// </summary>
        private async void UpdateData()
        {
            if (String.IsNullOrEmpty(SelectedCharacter.Description)) //Is empty
                SelectedCharacter.Description = "Information not available.";
            else //Sometimes have html
                SelectedCharacter.Description = System.Text.RegularExpressions.Regex.Replace(SelectedCharacter.Description, @"<[^>]*>", String.Empty).Replace("&ldquo;","").Replace("&rdquo;", "");

            SelectedCharacter.Description += "\n"+"\n"+ Pepper.Resx.AppResources.Marvel_Attributtion;

            Comics = MarvelApiService.Instance.FindComicByCharacterId(SelectedCharacter.Id.ToString());
        }

        /// <summary>
        /// Show images from marvel comic api param
        /// </summary>
        /// <param name="c"></param>
        private async void ShowMarvelComicsImage(Xam.Marvelous.Model.Base.Comic c)
        {
            if (IsBusy)
                return;


            if (c.Images.Count > 0)
            {
                IsBusy = true;                
                ContentPage p = new ContentPage();
                p.BackgroundColor = (Color)App.Current.Resources["ColorPrimary"];
                p.Title = c.Title.ToUpper();
                ImageCoverView view = new ImageCoverView();
                view.BindingContext = c;
                p.Content = view;
                p.BindingContext = MainViewModel.Instance;
                await App.Current.MainPage.Navigation.PushAsync(p, true);
                IsBusy = false;
            }
        }

        /// <summary>
        /// Show detail Tumblr photo and process info
        /// </summary>
        /// <param name="item"></param>
        private async void ShowTumblrDetail(Xam.Tumblr.Models.Photo item)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            await Task.Delay(100);
            if (!String.IsNullOrEmpty(item.description))
                item.description = 
                    System.Text.RegularExpressions.Regex.Replace(item.description, @"<[^>]*>", String.Empty)
                    .Replace("&ldquo;", "").Replace("&rdquo;", "");
            SelectedTumblrPhoto = item;            
            SimilarTumblrPhotos = TumblrPhotos.Where(i => ((i.url == item.url)&&(i.id!=item.id))).ToList();            
            TumblrDetailPage p = new TumblrDetailPage();
            p.Title = SelectedTumblrPhoto.title.ToUpper();
            p.BindingContext = MainViewModel.Instance;
            await App.Current.MainPage.Navigation.PushAsync(p, true);
            IsBusy = false;
        }

        /// <summary>
        /// Show detail Tumblr similar photo 
        /// </summary>
        private async void ShowSimilarTumblrDetail(Xam.Tumblr.Models.Photo item)
        {
            if (IsBusy)
                return;

            IsBusy = true;            
            await Task.Delay(100);
            await App.Current.MainPage.Navigation.PopAsync();
            if (!String.IsNullOrEmpty(item.description))
                item.description =
                    System.Text.RegularExpressions.Regex.Replace(item.description, @"<[^>]*>", String.Empty)
                    .Replace("&ldquo;", "").Replace("&rdquo;", "");
            SelectedTumblrPhoto = item;
            SimilarTumblrPhotos = TumblrPhotos.Where(i => ((i.url == item.url) && (i.id != item.id))).ToList();
            TumblrDetailPage p = new TumblrDetailPage();
            p.Title = SelectedTumblrPhoto.title.ToUpper();
            p.BindingContext = MainViewModel.Instance;
            await App.Current.MainPage.Navigation.PushAsync(p, true);
            IsBusy = false;
        }

        /// <summary>
        /// Show marvel rss new detail
        /// </summary>
        /// <param name="item"></param>
        private async void ShowRssDetail(Xam.Rss.FeedItem item)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            await Task.Delay(100);
            Pepper.Helpers.HideHeyboardFromScreen();
            RssItemPage p = new RssItemPage(item);
            p.Title = item.Title.ToUpper();
            p.BindingContext = MainViewModel.Instance;
            await App.Current.MainPage.Navigation.PushAsync(p, true);
            IsBusy = false;
        }

        /// <summary>
        /// Load related rss, based on https://github.com/codehollow/FeedReader        
        /// </summary>
        public async void LoadRss()
        {
            try
            {
                bool result = await CheckConnectionAndUpdate();
                if (result)
                {
                    String url = Pepper.Resx.AppResources.Url_Rss;
                    var feed = await Xam.Rss.FeedReader.ReadAsync(url);
                    List<Xam.Rss.FeedItem> items = feed.Items.ToList();
                    foreach (Xam.Rss.FeedItem item in items)
                    {
                        List<string> imgs = Xam.Rss.Helpers.GetImgSrcs(item.Description);
                        if (imgs != null && imgs.Count > 0)
                            item.UrlImg = imgs.FirstOrDefault();
                    }
                    RssItems = items;
                    ShowSuggest = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Check internet avaliable
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckConnectionAndUpdate()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    IsNetworkOk = false;
                else
                    IsNetworkOk = true;

                return IsNetworkOk;
            }
            catch (Exception ex)
            {
                IsNetworkOk = false;
                return false;
            }
        }

    }
}
