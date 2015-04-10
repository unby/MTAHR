using System;
using Owin;

namespace OAuthClientExt.Yandex
{
    /// <summary>
    /// Extension methods for using <see cref="YandexAuthenticationMiddleware"/>
    /// </summary>
    public static class YandexAuthenticationProvider
    {
        /// <summary>
        /// Authenticate users using Vkontakte
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/> passed to the configuration method</param>
        /// <param name="options">Middleware configuration options</param>
        /// <returns>The updated <see cref="IAppBuilder"/></returns>
        public static IAppBuilder UseYandexAuthentication(this IAppBuilder app, YandexAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            app.Use(typeof(YandexAuthenticationMiddleware), app, options);
            return app;
        }

        /// <summary>
        /// Authenticate users using Vkontakte
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/> passed to the configuration method</param>
        /// <param name="appId">The appId assigned by Vkontakte</param>
        /// <param name="appSecret">The appSecret assigned by Vkontakte</param>
        /// <param name="scope">The permissions list. Comma separated. Like "audio,video,photos"</param>
        /// <returns>The updated <see cref="IAppBuilder"/></returns>
        public static IAppBuilder UseYandexAuthentication(
            this IAppBuilder app,
            string appId,
            string appSecret,
            string scope)
        {
            return UseYandexAuthentication(
                app,
                new YandexAuthenticationOptions()
                {
                    AppId = appId,
                    AppSecret = appSecret,
                    Scope = scope
                });
        }
    }
}
