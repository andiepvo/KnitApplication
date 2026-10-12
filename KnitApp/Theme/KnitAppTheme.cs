using MudBlazor;

namespace KnitApp.Theme;

public static class KnitAppTheme
{
    public static MudTheme Theme => new MudTheme
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#6E4555",
            Secondary = "#D282A6",
            Tertiary = "#E8B4BC",
            TertiaryContrastText = "#3A3238",  
            Error = "#A3463E",
            AppbarBackground = "#3A3238",
            Background = "#F5E3E0",
            Surface = "#FFFFFF",
            TextPrimary = "#3A3238",
            TextSecondary = "#6E4555"
        }
    };
}