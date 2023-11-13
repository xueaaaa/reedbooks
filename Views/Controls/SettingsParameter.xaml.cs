using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views.Controls
{
    public partial class SettingsParameter : Grid
    {
        public static readonly DependencyProperty ParameterNameProperty =
            DependencyProperty.Register("ParameterName", typeof(string), typeof(SettingsParameter));
        public static readonly DependencyProperty ParameterHintProperty =
            DependencyProperty.Register("ParameterHint", typeof (string), typeof(SettingsParameter));
        public static readonly DependencyProperty ValueSelectorProperty =
            DependencyProperty.Register("ValueSelector", typeof(FrameworkElement), typeof(SettingsParameter));

        public string ParameterName
        {
            get => (string)GetValue(ParameterNameProperty);
            set => SetValue(ParameterNameProperty, value);
        }

        public string ParameterHint
        {
            get => (string)GetValue(ParameterHintProperty);
            set => SetValue(ParameterHintProperty, value);
        }

        public FrameworkElement ValueSelector
        {
            get => (FrameworkElement)GetValue(ValueSelectorProperty);
            set => SetValue(ValueSelectorProperty, value);
        }

        public SettingsParameter()
        {
            InitializeComponent();
        }
    }
}
