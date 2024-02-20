using App.Models.DB;
using App.Repositories;

namespace App.Services.Commands;

public class SubscribeUserCommand(
    IUserRepository userRepository,
    ISubscriptionRepository subscriptionRepository) : IAsyncCommand
{
    private string subscriberId = string.Empty;
    private string broadcasterId = string.Empty;
    
    public SubscribeUserCommand Setup(string subscriberId, string broadcasterId)
    {
        this.subscriberId = subscriberId;
        this.broadcasterId = broadcasterId;
        return this;
    }
    
    public async Task InvokeAsync(CancellationToken cancellationToken = default)
    {
        var subscriberUser = await userRepository.GetEntityAsync(subscriberId, cancellationToken);
        var broadcasterUser = await userRepository.GetEntityAsync(broadcasterId, cancellationToken);
        var subscription = new Subscription
        {
            Id = $"{broadcasterUser.Id}_{subscriberUser.Id}",
            SubscriberUserId = subscriberUser.Id,
            BroadcasterUserId = broadcasterUser.Id,
        };
        await subscriptionRepository.AddEntityAsync(subscription, cancellationToken);
    }
}