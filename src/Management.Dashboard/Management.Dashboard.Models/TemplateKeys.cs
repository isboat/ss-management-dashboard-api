namespace Management.Dashboard.Models
{
    public class TemplateKeys
    {
        public const string MenuOverlay = "MenuOverlay";
        public const string MenuOnly = "MenuOnly";
        public const string MediaOnly = "MediaOnly";
        public const string MenuTopAndMediaBottom = "MenuTopAndMediaBottom";
        public const string MediaTopAndMenuBottom = "MediaTopAndMenuBottom";
        public const string Text = "Text";
        public const string DateTime = "CurrentDateTime";
        public const string Weather = "Weather";
        public const string MediaPlaylist = "MediaPlaylist";
    }


    public class MenuSubTypeKeys
    {
        public const string Basic = "Basic";
        public const string Premium = "Premium";
        public const string Deluxe = "Deluxe";
    }

    public class DateTimeSubTypeKeys
    {
        public const string British = "dd/MM/yyyy HH:mm:ss";
        public const string American = "MM/dd/yyyy HH:mm:ss";
        public const string TimeFirstAmerican = "hh 'o''clock' a, zzzz";
        public const string DateFirstBritish = "EEE, d MMM yyyy HH:mm:ss";
    }
}