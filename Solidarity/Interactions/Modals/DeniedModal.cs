namespace Solidarity.Interactions.Modals
{
    public class DeniedModal : IModal
    {
        public string Title
            => "User requested notification:";

        [RequiredInput(true)]
        [InputLabel("Why are you denying this submission?")]
        [ModalTextInput("reason", TextInputStyle.Paragraph, "It it not a Terraria build.")]
        public string? Reason { get; set; }
    }
}
