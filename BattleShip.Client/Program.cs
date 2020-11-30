using System;
using System.Threading.Tasks;
using CQRSlite.Commands;
using BattleShip.Domain.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BattleShip.Client.Validators;
using BattleShip.Client.Extensions;
using BattleShip.Projections;

namespace BattleShip.Client
{
    class Program
    {
        public static bool PlayerWon { get; set; }

        private const int _defaultBoardSize = 10;
        public static bool WithHints = true;
        private static ICommandSender _commandSender;
        private static IGameInputValidator _letterBasedBoardInputValidator;
        private static Guid _boardId;
        static async Task Main(string[] args)
        {
            IHost host = HostBuilder.CreateHostBuilder(args).Build();

            initInfrastructure(host.Services);
            await initBoard(_defaultBoardSize);
            await initShips();
            await startGame();
            await host.RunAsync();
        }

        private static void initInfrastructure(IServiceProvider services)
        {
            using IServiceScope serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;
            _commandSender = provider.GetRequiredService<ICommandSender>();
            _letterBasedBoardInputValidator = provider.GetRequiredService<IGameInputValidator>();
        }
        private static async Task startGame()
        {
            PlayerWon = false;
            while (!PlayerWon)
            {
                Console.WriteLine("Enter Letter Coordinates To Shoot (i.e. A1):");
                string letterCoordinate = Console.ReadLine();

                if (_letterBasedBoardInputValidator.IsCoordinateValid(letterCoordinate))
                    await _commandSender.Send(new ShootTarget(_boardId, letterCoordinate.ToBoardPoint()));
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Invalid Target!");
                }
            }
        }
        private static async Task initBoard(int size)
        {
            if(!_letterBasedBoardInputValidator.IsBoardSizeValid(size))
                throw new ArgumentException($"Board Size Not Valid");

            var createBoardCommand = new CreateBoard(size);
            _boardId = createBoardCommand.Id;
            await _commandSender.Send(createBoardCommand);
        }

        private static async Task initShips()
        {
            await _commandSender.Send(new AddBattleShip(_boardId));
            await _commandSender.Send(new AddDestroyer(_boardId));
            await _commandSender.Send(new AddDestroyer(_boardId));                           
        }
               
    }

}
