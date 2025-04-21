namespace AssistantContract.TgBot.Extension
{
    public static class ServiceProvider
    {
        /// <summary>
        /// Метод розширення, дозволяє взяти реалізацію інтерфейсу.
        /// Допомагає у випадках, коли є багато реалізацій які
        /// відносяться до одного інтерфейсу
        /// </summary>
        /// <typeparam name="IT">Тип інтерфейса</typeparam>
        /// <typeparam name="T">Тип реалізації</typeparam>
        /// <returns>Вертає обєкт реалізації</returns>
        public static T GetSpecificService<IT, T>(this IServiceProvider services) where T : IT 
        {
            IEnumerable<IT> allService = services.GetServices<IT>();
            foreach (IT service in allService)
            {
                if (service is not null)
                {
                    if (service.GetType() == typeof(T))
                    {
                        return (T)service;
                    } 
                }
                
            }
            throw new NullReferenceException($"Di not have a specific servise {typeof(T)} there implementation {typeof(IT)}");
        }
        
        //Добавляет в DI обект который можено получить как по интерфейсу так и по названию класса
        public static IServiceCollection AddScopedWithAlias<TInterface, TImplementation>(this IServiceCollection services)
            where TImplementation : class, TInterface
            where TInterface : class
        {
            // Добавляем реализацию как singleton для обоих типов (гарантия одинакового объекта)
            services.AddScoped<TImplementation>();
            services.AddScoped<TInterface>(provider => provider.GetRequiredService<TImplementation>());

            return services;
        }
    }
    
}
