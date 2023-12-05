using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace Hangman
{
    public partial class MainPage : ContentPage,INotifyPropertyChanged
    {
        #region UI Properties
        public string Spotlight                                                 // For counting the letter & display "_".
        {
            get => spotlight; set
            {
                spotlight = value;  
                OnPropertyChanged();
            }
        }
        public List<char> Letters                                               // For On App Keyboard
        {
            get => letters; set
            {
                letters = value;
                OnPropertyChanged();
            }
        }
        public string Message                                                   // For Display Win or Loss
        {
            get => message; set
            {
                message = value;
                OnPropertyChanged();
            }
        }
        public string GameStatus                                              // For Showing Status like how much mistake you have done
        {
            get => gameStatus; set
            {
                gameStatus = value;
                OnPropertyChanged();
            }
        }
        public string CurrentImage                                             // For Image Changes
        {
            get => currentImage; set
            {
                currentImage = value;
                OnPropertyChanged();
            }
        }
       
        #endregion

        #region Fields
        List<string> words = new List<string>()                             // Guessing Words Lists
        {
            "cypher","file","ascii","cipher","algorithm","encode","link","software","encryption","firmware","language","rules","laws","information","prefix"
            ,"compiler","encrypt","codification","data","program","interface","script","reference","label","coding","logic","byte","debugging","server",
            "overwrite","morse","runtime","html","metadata","function","commands","linux","databases","python","javascript","application","develop","syntax","debugger",
            "ubuntu","sql","encoding","database","template","schema","character","debug","functions","gesture","servlet","spyware","malware","string","instance","ticker",
            "reminder","domain","exception","explicitly","limitations","multitask","fortran","standardized","hacker","recursion","passcode","multithreaded","multiload",
            "postfix","complier","graphician","javascript","maui","csharp","fsharp","java","html","mongodb","sql","postman","xaml","java"
        };

        List<char> guessed = new List<char>();
        string answer = "";
        private string spotlight;
        private List<char> letters = new List<char>();
        private string message;
        public int mistake = 0;
        public int maxwrong = 6;
        private string gameStatus;
        private string currentImage = "img0.jpg";
        #endregion

        public MainPage()
        {
            InitializeComponent();
            Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
            BindingContext=this;
            PickWord();
            CalculateWord(answer, guessed);
        }
        #region Game Engine
        private void PickWord()
        {
            answer = words[new Random().Next(0,words.Count)];
            Debug.WriteLine(answer);
        }

        private void CalculateWord(string answer , List<char> guessed)
        {
            var temp =
                answer.Select(x => (guessed.IndexOf(x) >= 0 ? x : '_'))
                .ToArray();
            Spotlight = string.Join(' ', temp);
        }
        #endregion

        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var letter = button.Text;
                button.IsEnabled = false;
                HandleGuess(letter[0]);
            }
        }
        private void HandleGuess(char letter)
        {
            if(guessed.IndexOf(letter) == -1)
            {
                guessed.Add(letter);
            }
            if(answer.IndexOf(letter) >= 0)
            {
                CalculateWord(answer, guessed);
                CheckedIfGameWon();
            }
            if(answer.IndexOf(letter) == -1)
            {
                mistake++;
                UpdateStatus();
                CheckedIfGameLoss();
                CurrentImage = $"img{mistake}.jpg";
            }
        }

        private void CheckedIfGameLoss()
        {
            if(mistake == maxwrong)
            {
                Message = "You Lost!!";
                DisableLetter();
            }
        }

        private void DisableLetter()
        {
            foreach (var children in LetterContainer.Children)
            {
                var btn = children as Button;
                if(btn != null)
                {
                    btn.IsEnabled = false;
                }
            }
        }
        private void EnableLetter()
        {
            foreach (var children in LetterContainer.Children)
            {
                var btn = children as Button;
                if (btn != null)
                {
                    btn.IsEnabled = true;
                }
            }
        }

        private void UpdateStatus()
        {
            GameStatus = $"Error : {mistake} of {maxwrong}";
        }
         
        private void CheckedIfGameWon()
        {
            if (spotlight.Replace(" ","") == answer)
            {
                Message = "You Won!";
                DisableLetter();
            }
        }

        private void Reset_Clicked(object sender, EventArgs e)
        {
            mistake = 0;
            guessed = new List<char>();
            PickWord();
            CurrentImage = "img0.jpg";
            CalculateWord(answer, guessed);
            Message = "";
            UpdateStatus();
            EnableLetter();
        }
    }
}