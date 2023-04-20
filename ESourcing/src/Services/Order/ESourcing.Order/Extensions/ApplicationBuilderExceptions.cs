using ESourcing.Order.Consumers;

namespace ESourcing.Order.Extensions
{
    public static class ApplicationBuilderExceptions
    {
        public static EventBusOrderCreateConsumer Listener { get; set; }
        public static IApplicationBuilder UseEventBusListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<EventBusOrderCreateConsumer>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);
            return app;
        } 

        private static void OnStopping()
        {
            Listener.Disconnect();
        }

        private static void OnStarted()
        {
            Listener.Consume();
        }
    }
}
