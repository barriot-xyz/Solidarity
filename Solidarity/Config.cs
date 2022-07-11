using Newtonsoft.Json;

namespace Solidarity
{
    public class Config
    {
        public static Settings Settings { get; private set; }
            = Settings.Read();
    }

    public class Settings
    {
        /// <summary>
        ///     The application token.
        /// </summary>
        public string Token { get; set; } = "";

        /// <summary>
        ///     To register commands to Discord or not.
        /// </summary>
        public bool RegisterCommands { get; set; }

        /// <summary>
        ///     The channel to send vote reminders to.
        /// </summary>
        public ulong VoteChannel { get; set; }

        /// <summary>
        ///     The role to ping for vote reminders.
        /// </summary>
        public ulong VoteRole { get; set; }

        /// <summary>
        ///     Emotes to automatically add on callbacks under the term 'reaction'
        /// </summary>
        public string[] Emotes { get; set; } = Array.Empty<string>();

        /// <summary>
        ///     A collection of channels that are defined under terms.
        /// </summary>
        public Dictionary<ulong, string[]> ChannelHandleCallback { get; set; } = new();

        public static Settings Read()
        {
            string path = "Config.json";
            if (!File.Exists(path))
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(new Settings(), Formatting.Indented));
                return new Settings();
            }
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path))
                ?? throw new NullReferenceException();
        }
    }
}
