namespace Solidarity.Interactions.Modals
{
    public class RoleModal : IModal
    {
        public string Title
            => "Specify a role to grant";

        [InputLabel("Role ID/Name")]
        [RequiredInput(true)]
        [ModalTextInput("entry", TextInputStyle.Short)]
        public string? Role { get; set; }
    }
}
