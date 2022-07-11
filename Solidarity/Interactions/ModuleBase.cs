using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidarity.Interactions
{
    public class ModuleBase : InteractionModuleBase<SocketInteractionContext>
    {
        public async Task UpdateAsync(string text, MessageComponent? component = null, Embed? embed = null)
        {
            if (Context.Interaction is not SocketMessageComponent interaction)
                throw new InvalidOperationException("Cannot update non-component interactions!");

            await interaction.UpdateAsync(x =>
            {
                x.Content = text;
                x.Embed = embed;
                x.Components = component ?? new ComponentBuilder().Build();
            });
        }
    }
}
