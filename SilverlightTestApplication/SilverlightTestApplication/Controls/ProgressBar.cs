using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;

namespace SilverlightTestApplication.Controls
{
    [TemplatePart(Name = ProgressBar.RootElement, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ProgressBar.ProgressIndicatorElement, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ProgressBar.ProgressTextElement, Type = typeof(TextBlock))]
    public class ProgressBar : Control
    {
        public ProgressBar()
        {
            this.DefaultStyleKey = typeof(ProgressBar);
        }
        protected const string RootElement = "Root";
        protected const string ProgressIndicatorElement = "ProgressIndicator";
        protected const string ProgressTextElement = "ProgressMessage";

        protected FrameworkElement Root;
        protected FrameworkElement ProgressIndicator;
        protected TextBlock ProgressText;

        public void Start()
        {
            this.Opacity = 1;
            sb.Begin();
        }
        public void Stop()
        {
            sb.Stop();
            this.Opacity = 0;
        }
        Storyboard sb = new Storyboard();
        int count;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Root = this.GetTemplateChild(RootElement) as FrameworkElement;
            this.ProgressIndicator = this.GetTemplateChild(ProgressIndicatorElement) as FrameworkElement;
            this.ProgressText = this.GetTemplateChild(ProgressTextElement) as TextBlock;

            if (this.Height == 0 || this.Height.ToString() == "NaN")
                this.Height = 20;
            if (this.Width == 0 || this.Width.ToString() == "NaN")
                this.Width = 100;
            this.ProgressIndicator.Width = 0;
            this.Opacity = 0;
            sb.Duration = new Duration(TimeSpan.FromSeconds(2));
            this.Resources.Add("sb", sb);

            DoubleAnimation a = new DoubleAnimation();
            a.Duration = sb.Duration;

            a.To = this.Width;

            Storyboard.SetTarget(a, this.ProgressIndicator);
            Storyboard.SetTargetProperty(a, new PropertyPath("Width"));
            sb.Children.Add(a);
            sb.RepeatBehavior = RepeatBehavior.Forever;
        }

        void sb_Completed(object sender, EventArgs e)
        {
            count++;
            if (count >= 100)
            {
                count = 0;
            }
            // restart the timer
            sb.Begin();
        }

        #region Text

        /// <summary>
        /// Identifies the Text dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ProgressBar), null);

        /// <summary>
        /// Gets or sets the Text possible Value of the string object.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion Text

        #region IsComplete
        /// 
        /// Identifies the IsComplete dependency property.
        /// 
        public static readonly DependencyProperty IsCompleteProperty =
        DependencyProperty.Register(
        "IsComplete",
        typeof(bool),
        typeof(ProgressBar), new PropertyMetadata(OnIsCompletePropertyChanged));

        /// 
        /// Gets or sets the IsComplete possible Value of the bool object.
        /// 
        public bool IsComplete
        {
            get { return (bool)GetValue(IsCompleteProperty); }
            set { SetValue(IsCompleteProperty, value); }
        }

        /// 
        /// IsCompleteProperty property changed handler.
        /// 
        /// ProgressBar that changed its IsComplete.
        /// DependencyPropertyChangedEventArgs.
        private static void OnIsCompletePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProgressBar _ProgressBar = d as ProgressBar;
            if (_ProgressBar != null)
            {
                if (_ProgressBar.IsComplete)
                    _ProgressBar.Stop();
            }
        }
        #endregion IsComplete
    }
}
