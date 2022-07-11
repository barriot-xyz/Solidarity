using Solidarity.Interactions.Types;

namespace Solidarity.Interactions.Converters
{
    internal class ComponentPointerConverter : ComponentTypeConverter<Pointer>
    {
        public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IComponentInteractionData option, IServiceProvider services)
        {
            if (Guid.TryParse(option.Value, out Guid id))
                if (Pointer.TryParse(id, out var value))
                    return Task.FromResult(TypeConverterResult.FromSuccess(value));
            return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, "Failed to convert from guid."));
        }
    }
}
