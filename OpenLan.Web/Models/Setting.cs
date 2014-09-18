using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenLan.Web.Models
{
    public class Setting
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class StrongSettings
    {       
        public bool EnableUserRegistration { get; private set; }
        public bool EnableTeamRegistration { get; private set; }
        public bool EnableTeamJoining { get; private set; }

        public static StrongSettings GetSettings(List<Setting> settings)
        {
            var ss = new StrongSettings()
                         {
                             EnableUserRegistration = bool.Parse((settings.Find(x => x.Key == "EnableUserRegistration") ?? new Setting() { Value = bool.TrueString }).Value),
                             EnableTeamRegistration = bool.Parse((settings.Find(x => x.Key == "EnableTeamRegistration") ?? new Setting { Value = bool.TrueString }).Value),
                             EnableTeamJoining = bool.Parse((settings.Find(x => x.Key == "EnabledTeamJoining") ?? new Setting() { Value = bool.TrueString}).Value),
                         };

            return ss;
        }
    }
}