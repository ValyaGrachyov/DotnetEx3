using Contracts;
using DataAccess;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Features.Services
{
    public class RateChangeConsumer : IConsumer<UserRateChangeRequest>
    {
        private readonly IUserRepository _userRepository;
        public RateChangeConsumer(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        public async Task Consume(ConsumeContext<UserRateChangeRequest> context)
        {
            var data = context.Message;

            var user = await _userRepository.GetUserByIdAsync(data.UserId);
            if (user == null || user.Rate < 0)
            {
                return;
            }

            
            if (data.ChangeDelta > 0)
            {
                await _userRepository.UpdateUserRateAsync(user.Id, data.ChangeDelta + user.Rate);
            }
            else
            {
                var allowedChange = user.Rate switch
                {
                    >= 3 => data.ChangeDelta,
                    2 => - 2,
                    1 => - 1,
                    _ => 0
                };
                
                await _userRepository.UpdateUserRateAsync(user.Id, allowedChange + user.Rate);
            }
        }
    }
}
