using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views.Controls
{
    public partial class InWindowDialog : StackPanel
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(InWindowDialog));
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(FrameworkElement), typeof(InWindowDialog));
        public static readonly DependencyProperty LeftSideFooterProperty =
            DependencyProperty.Register("LeftSideFooter", typeof(FrameworkElement), typeof(InWindowDialog));
        public static readonly DependencyProperty RightSideFooterProperty =
            DependencyProperty.Register("RightSideFooter", typeof(FrameworkElement), typeof(InWindowDialog));

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public FrameworkElement Content
        {
            get => (FrameworkElement)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public FrameworkElement LeftSideFooter
        {
            get => (FrameworkElement)GetValue(LeftSideFooterProperty);
            set => SetValue(LeftSideFooterProperty, value);
        }

        public FrameworkElement RightSideFooter
        {
            get => (FrameworkElement)GetValue(RightSideFooterProperty);
            set => SetValue(RightSideFooterProperty, value);
        }

        public InWindowDialog()
        {
            InitializeComponent();
        }
    }
}
