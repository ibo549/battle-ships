using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using CQRSlite.Commands;
using CQRSlite.Events;
using CQRSlite.Domain;
using CQRSlite.Caching;
using CQRSlite.Queries;
using CQRSlite.Routing;
using ISession = CQRSlite.Domain.ISession;

using BattleShip.Domain;
using BattleShip.Domain.Commands;
using BattleShip.Domain.CommandHandlers;
using BattleShip.Domain.Events;
using BattleShip.Projections;
using BattleShip.Client.Validators;

namespace BattleShip.Client
{
    public static class HostBuilder
    {
     public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddSingleton<IEventStore, InMemoryEventStore>()
                    .AddMemoryCache()
                    .AddSingleton<Router>(new Router())
                    .AddSingleton<ICommandSender>(y => y.GetService<Router>())
                    .AddSingleton<IEventPublisher>(y => y.GetService<Router>())
                    .AddSingleton<IHandlerRegistrar>(y => y.GetService<Router>())
                    .AddSingleton<IQueryProcessor>(y => y.GetService<Router>())
                    .AddSingleton<IEventStore, InMemoryEventStore>()
                    .AddSingleton<ICache, MemoryCache>()
                    .AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()))
                    .AddScoped<ISession, Session>()
                    .AddSingleton<IGameInputValidator, LetterBasedBoardInputValidator>()
                    .AddSingleton<IGameDisplay, ConsoleGameDisplay>()
                    .AddSingleton<IProjectionStore, InMemoryProjection>()
                    .AddSingleton<BattleShipEventHandlers>()
                    .AddSingleton<BattleShipCommandHandlers>()
                    .AddTransient<IEventHandler<BoardCreated>, BattleShipEventHandlers>()
                    .AddTransient<IEventHandler<ShipAdded>, BattleShipEventHandlers>()
                    .AddTransient<IEventHandler<ShipHit>, BattleShipEventHandlers>()
                    .AddTransient<IEventHandler<ShotMissed>, BattleShipEventHandlers>()
                    .AddTransient<IEventHandler<ShipSunk>, BattleShipEventHandlers>()
                    .AddTransient<IEventHandler<AllShipsSunk>, BattleShipEventHandlers>()
                    .AddTransient<IEventHandler<BattleShipOperationFailed>, BattleShipEventHandlers>()
                    .AddTransient<ICommandHandler<CreateBoard>, BattleShipCommandHandlers>()
                    .AddTransient<ICommandHandler<AddBattleShip>, BattleShipCommandHandlers>()
                    .AddTransient<ICommandHandler<AddDestroyer>, BattleShipCommandHandlers>()
                    .AddTransient<ICommandHandler<ShootTarget>, BattleShipCommandHandlers>()                     
                    )
                    .UseServiceProviderFactory(new RouteRegistrarProviderFactory());
    }


    public class RouteRegistrarProviderFactory : IServiceProviderFactory<IServiceCollection>
        {
            public IServiceCollection CreateBuilder(IServiceCollection services)
            {
                return services;
            }

            public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
            {
                var serviceProvider = containerBuilder.BuildServiceProvider();
                var registrar = new RouteRegistrar(new Provider(serviceProvider));
                registrar.RegisterInAssemblyOf(typeof(BattleShipCommandHandlers));
                registrar.RegisterInAssemblyOf(typeof(BattleShipEventHandlers));
                return serviceProvider;
            }
        }

        public class Provider : IServiceProvider
        {
            private readonly ServiceProvider _serviceProvider;

            public Provider(ServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public object GetService(Type serviceType)
            {
                return _serviceProvider.GetService(serviceType);
            }
        }
}