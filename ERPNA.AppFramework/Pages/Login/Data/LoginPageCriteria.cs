using Browser.Core.Framework;

namespace ERPNA.AppFramework
{
    public class LoginPageCriteria
    {
        public readonly ICriteria<LoginPage> UsernameTxtEnabled = new Criteria<LoginPage>(p =>
        {
            return p.Exists(Bys.LoginPage.UserNameTxt, ElementCriteria.IsEnabled);

        }, "Username text box enabled");

        public readonly ICriteria<LoginPage> PasswordTxtEnabled = new Criteria<LoginPage>(p =>
        {
            return p.Exists(Bys.LoginPage.PasswordTxt, ElementCriteria.IsEnabled);

        }, "Password text box enabled");

        public readonly ICriteria<LoginPage> SignInBtnEnabled = new Criteria<LoginPage>(p =>
        {
            return p.Exists(Bys.LoginPage.SignInBtn, ElementCriteria.IsEnabled);

        }, "Password text box enabled");

        public readonly ICriteria<LoginPage> PageReady;

        public LoginPageCriteria()
        {
           PageReady = UsernameTxtEnabled.AND(PasswordTxtEnabled).AND(SignInBtnEnabled);
        }
    }
}
