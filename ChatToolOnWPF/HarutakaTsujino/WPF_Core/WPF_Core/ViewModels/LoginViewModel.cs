using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Windows.Input;
using WPF_Core.Common;
using WPF_Core.Models.Services;

namespace WPF_Core.ViewModels
{
    public class LogInViewModel
    {
        public string MailAddress { get; set; }

        public string Password { get; set; }
     
        public ICommand LogInCommand { get; set; }

        public IObservable<Unit> OnLogInSucceededAsObservable => OnLogInSucceededAsSubject;

        public IObservable<Unit> OnLogInFailedAsObservable => OnLogInSucceededAsSubject;

        public LogInViewModel()
        {
            LogInCommand = new DelegateCommand(LogIn);
        }

        private void LogIn()
        {
            var logInResult = LogInService.LogIn(MailAddress, Password);

            Password = "";

            if (logInResult)
            {
                OnLogInSucceededAsSubject.OnNext(Unit.Default);
            }
            else
            {
                OnLogInFailedAsSubject.OnNext(Unit.Default);
            }
        }

        private Subject<Unit> OnLogInSucceededAsSubject { get; set; } = new Subject<Unit>();

        private Subject<Unit> OnLogInFailedAsSubject { get; set; } = new Subject<Unit>();
    }
}
