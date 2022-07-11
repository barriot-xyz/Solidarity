using Solidarity.Interactions.Types;

namespace Solidarity.Interactions.Converters
{
    public class PointerConverter : TypeConverter<Pointer>
    {
        public override ApplicationCommandOptionType GetDiscordType()
            => ApplicationCommandOptionType.String;

        public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
        {
            if (Guid.TryParse((string)option.Value, out Guid id))
                if (Pointer.TryParse(id, out var value))
                    return Task.FromResult(TypeConverterResult.FromSuccess(value));
            return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, "Failed to convert from guid."));
        }
    }
}
