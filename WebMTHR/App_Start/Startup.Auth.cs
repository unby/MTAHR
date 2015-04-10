using System;
using BaseType;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using OAuthClientExt.MailRu;
using OAuthClientExt.VKontakte;
using OAuthClientExt.Yandex;
using Owin;
using WebMTHR.EmailSender;
using WebMTHR.Models;
using WebMTHR.Settings;

namespace WebMTHR
{
    public partial class Startup
    {
        // Дополнительные сведения о настройке проверки подлинности см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Настройка контекста базы данных, диспетчера пользователей и диспетчера входа для использования одного экземпляра на запрос
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Включение использования файла cookie, в котором приложение может хранить информацию для пользователя, выполнившего вход,
            // и использование файла cookie для временного хранения информации о входах пользователя с помощью стороннего поставщика входа
            // Настройка файла cookie для входа
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Позволяет приложению проверять метку безопасности при входе пользователя.
                    // Эта функция безопасности используется, когда вы меняете пароль или добавляете внешнее имя входа в свою учетную запись.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, Guid>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                        getUserIdCallback: (id) => (Guid.Parse(id.GetUserId())))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Позволяет приложению временно хранить информацию о пользователе, пока проверяется второй фактор двухфакторной проверки подлинности.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Позволяет приложению запомнить второй фактор проверки имени входа. Например, это может быть телефон или почта.
            // Если выбрать этот параметр, то на устройстве, с помощью которого вы входите, будет сохранен второй шаг проверки при входе.
            // Точно так же действует параметр ServerAuthorization при входе.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Раскомментируйте приведенные далее строки, чтобы включить вход с помощью сторонних поставщиков входа
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "000000004814B445",
            //    clientSecret: "lNYw4-5l3QIS70KBedNJqNdUTCmHOqcb");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");
            var yandex = WebMTHRConfiguration.Config.OAuthServices.GetService("Yandex");
            if (yandex!=null)
            app.UseYandexAuthentication(new YandexAuthenticationOptions()
            {
                AppId =yandex.AppKey,// "5466b8798b3c46e3a5b90590b3adc804",
                AppSecret = yandex.SecretKey// "fc08f8eacf1d4650b5ef819ff1120e5d"
            });
            var vkontakte = WebMTHRConfiguration.Config.OAuthServices.GetService("vkontakte");
            if (vkontakte != null)
            app.UseVkontakteAuthentication(new VkAuthenticationOptions()
            {
                AppId =vkontakte.AppKey, //"4822926",
                AppSecret =vkontakte.SecretKey,// "6hIWsG1PKkKv3DDAW6CR",
                Scope = "email"

            });
            var mailRu = WebMTHRConfiguration.Config.OAuthServices.GetService("mailRu");
            if (mailRu != null)
            app.UseMailRuAuthentication(new MailRuAuthenticationOptions()
            {
                AppId =mailRu.AppKey,// "731715",
                AppSecret = mailRu.SecretKey,//"d9d3744b2e3e2af82a3566230e012f9e",
                Scope = "email"
            });
            var google = WebMTHRConfiguration.Config.OAuthServices.GetService("google");
            if (google != null)
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId =google.AppKey,// "198355310473-c976tclmvsgpto6jrhas3rgjv8t9nsib.apps.googleusercontent.com",
                ClientSecret =google.SecretKey// "2XfvYtc-5N4IU5bK_d09SEij"
            });

            //EmailNotivicationInit
            EmailSenderMTHR.EmailNotivicationInit();
        }
    }
}