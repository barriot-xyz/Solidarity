using Solidarity.Interactions.Types;

namespace Solidarity.Interactions.Converters
{
    internal class PointerReader<T> : TypeReader<T> where T : IPointer
    {
        public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, string option, IServiceProvider services)
        {
            if (Guid.TryParse(option, out Guid id))
                if (Pointer.TryParse<T>(id, out var value))
                    return Task.FromResult(TypeConverterResult.FromSuccess(value));
            return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, "Failed to convert from guid."));
        }
    }
}
