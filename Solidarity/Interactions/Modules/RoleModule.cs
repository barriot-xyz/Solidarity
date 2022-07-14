using Solidarity.Interactions.Modals;
using Tranference.Discord;
using Transference;

namespace Solidarity.Interactions.Modules
{
    [EnabledInDm(false)]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    public class RoleModule : ModuleBase
    {
        [MessageCommand("Add Role")]
        public async Task AddRoleAsync(SocketUserMessage message)
        {
            if (message.Author.Id != Context.Client.CurrentUser.Id)
                await RespondAsync(
                    text: ":x: **Cannot operate on messages that I have not sent!**",
                    ephemeral: true);

            else
                await RespondWithModalAsync<RoleModal>($"role-add:{UlongPointer.Create(message)}");
        }

        [ModalInteraction("role-add:*")]
        public async Task AddResolveAsync(Pointer<SocketUserMessage> message, RoleModal modal)
        {
            SocketRole? role = null;

            if (ulong.TryParse(modal.Role, out ulong id))
                role = Context.Guild.GetRole(id);

            else
                role = Context.Guild.Roles.FirstOrDefault(x => x?.Name == modal.Role, null);

            if (role is null)
                await RespondAsync(
                    text: ":x: **No role was found with this name/id!**",
                    ephemeral: true);

            else
            {
                var cb = ComponentBuilder.FromMessage(message.Value)
                    .WithButton(role.Name, $"role-resolve:{role.Id}", ButtonStyle.Secondary);

                await message.Value.ModifyAsync(x =>
                {
                    x.Components = cb.Build();
                });

                await RespondAsync(
                    text: ":white_check_mark: **Succesfully added role assignment button to message!**",
                    ephemeral: true);
            }
        }

        [MessageCommand("Remove Role")]
        public async Task RemoveRoleAsync(SocketUserMessage message)
        {
            if (message.Author.Id != Context.Client.CurrentUser.Id)
                await RespondAsync(
                    text: ":x: **Cannot operate on messages that I have not sent!**",
                    ephemeral: true);

            else
            {
                if (message.Components.Any())
                {
                    var sb = new SelectMenuBuilder()
                        .WithMinValues(1)
                        .WithCustomId("role-remove");

                    foreach (var rows in message.Components)
                        foreach (var component in rows.Components)
                        {
                            if (component is not ButtonComponent button)
                                throw new InvalidOperationException();

                            sb.AddOption(button.Label, button.CustomId.Split(':')[1]);
                        }

                    sb.WithMaxValues(sb.Options.Count);

                    await RespondAsync(
                        text: ":wastebasket: **Please select a number of roles to remove:**");
                }
                else
                    await RespondAsync(
                        text: ":x: **Cannot resolve any role components from this message!**",
                        ephemeral: true);
            }
        }

        [ComponentInteraction("role-remove")]
        public async Task RemoveResolveAsync(ulong[] selectedValues)
        {
            var interaction = Context.Interaction as SocketMessageComponent;

            foreach (var value in selectedValues)
            {
                var cb = ComponentBuilder.FromMessage(interaction!.Message);

                foreach (var row in cb.ActionRows)
                    foreach (var component in row.Components)
                    {
                        if (selectedValues.Any(x => component.CustomId.Contains(x.ToString())))
                            row.Components.Remove(component);
                    }

                await interaction.Message.ModifyAsync(x => x.Components = cb.Build());
            }

            await UpdateAsync(
                text: $":white_check_mark: **Succesfully removed {selectedValues.Length} items from this message.**");
        }

        [ComponentInteraction("role-resolve:*")]
        public async Task ResolveRoleAsync(ulong id)
        {
            if (Context.User is not SocketGuildUser user)
                throw new InvalidOperationException();

            bool removing = user.Roles.Any(x => x.Id == id);

            if (removing)
                await user.RemoveRoleAsync(id);

            else
                await user.AddRoleAsync(id);

            await RespondAsync(
                text: $":white_check_mark: **Succesfully {(removing ? "removed" : "added")} <@&{id}>!**",
                ephemeral: true);
        }
    }
}
