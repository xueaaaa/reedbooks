using ReedBooks.Models.Assessment;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReedBooks.Views.Controls
{
    public partial class EmotionalAssessmentControl : StackPanel
    {
        public static readonly DependencyProperty SetEmoteCommandProperty =
            DependencyProperty.Register("SetEmoteCommand", typeof(ICommand), typeof(EmotionalAssessmentControl));
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(EmotionalAssessmentControl));
        public static readonly DependencyProperty EmoteProperty =
            DependencyProperty.Register("Emote", typeof(Emote), typeof(EmotionalAssessmentControl));

        public ICommand SetEmoteCommand
        {
            get => (ICommand)GetValue(SetEmoteCommandProperty);
            set => SetValue(SetEmoteCommandProperty, value);
        }

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public Emote Emote
        {
            get => (Emote)GetValue(EmoteProperty);
            set => SetValue(EmoteProperty, value);
        }

        public EmotionalAssessmentControl()
        {
            InitializeComponent();
        }
    }
}
