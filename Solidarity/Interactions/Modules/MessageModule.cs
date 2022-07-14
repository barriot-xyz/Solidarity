using Solidarity.Interactions.Modals;
using Tranference.Discord;
using Transference;

namespace Solidarity.Interactions.Modules
{
    public class MessageModule : ModuleBase
    {
        [SlashCommand("write", "Create a message in this channel")]
        public async Task SendMessageAsync()
            => await RespondWithModalAsync<MessageModal>("resolve");

        [ModalInteraction("resolve")]
        public async Task ResolveMessageAsync(MessageModal modal)
            => await ResolveMessageInternalAsync(modal.Content);

        [MessageCommand("Modify")]
        public async Task ModifyMessageAsync(SocketUserMessage message)
        {
            if (message.Author.Id != Context.Client.CurrentUser.Id)
                await RespondAsync(
                    text: ":x: **Cannot operate on messages that I have not sent!**",
                    ephemeral: true);

            else
                await RespondWithModalAsync<MessageModal>($"modify:{UlongPointer.Create(message)}");
        }

        [ModalInteraction("modify:*")]
        public async Task ResolveMessageAsync(Pointer<SocketUserMessage> message, MessageModal modal)
            => await ResolveMessageInternalAsync(modal.Content, message.Value);

        private async Task ResolveMessageInternalAsync(string? content, SocketUserMessage? message = null)
        {
            if (message is null)
            {
                await Context.Channel.SendMessageAsync(
                    text: content);
            }
            else
            {
                await message.ModifyAsync(x =>
                {
                    x.Content = content;
                });
            }

            await RespondAsync(
                text: $":white_check_mark: **Succesfully {((message is null) ? "created" : "modified")} message!**",
                ephemeral: true);
        }
    }
}
