namespace Solidarity.Interactions.Modals
{
    public class MessageModal : IModal
    {
        public string Title
            => "Set message content:";

        [InputLabel("Message")]
        [RequiredInput(true)]
        [ModalTextInput("entry", TextInputStyle.Paragraph)]
        public string? Content { get; set; }
    }
}
