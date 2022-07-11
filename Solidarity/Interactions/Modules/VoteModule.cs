namespace Solidarity.Interactions.Modules
{
    [EnabledInDm(false)]
    public class VoteModule : ModuleBase
    {
        [ComponentInteraction("novote")]
        public async Task NoVoteAsync()
        {
            var cb = new ComponentBuilder()
                .WithButton("Stop being reminded", "novote", ButtonStyle.Danger);

            if (Context.User is not SocketGuildUser user)
                return;

            var role = user.Guild.GetRole(Config.Settings.VoteRole);

            if (role is not null && !user.Roles.Any(x => x == role))
            {
                await user.AddRoleAsync(role);
                await RespondAsync(
                    text: ":heart: **You will be reminded to vote every 12 hours!**",
                    components: cb.Build(),
                    ephemeral: true);
            }
            else
                await RespondAsync(text: ":heart: **You are already being reminded to vote!**",
                    components: cb.Build(),
                    ephemeral: true);
        }

        [ComponentInteraction("vote")]
        public async Task VoteAsync()
        {
            if (Context.User is not SocketGuildUser user)
                throw new InvalidOperationException();

            var role = Context.Guild.GetRole(Config.Settings.VoteRole);

            await user.RemoveRoleAsync(role);
            await RespondAsync(
                text: ":broken_heart: **Okay, I will stop reminding you to vote.** Thank you for contributing!",
                ephemeral: true);
        }
    }
}
