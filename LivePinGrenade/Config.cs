using System.ComponentModel;

namespace LivePinGrenade
{
    public class Config
    {
        [Description("Fuse duration in seconds, measured from the moment the pin is pulled. Set to 0 to keep the game's default HE fuse.")]
        public float FuseDuration { get; set; } = 0f;
    }
}
