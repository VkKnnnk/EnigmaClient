using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Enigma_Client_V2.View_Model
{
    public class Animation_Functions
    {
        //Анимация появления и исчезновения
        public static void Highlight_animation(UIElement uIElement, TimeSpan appearance_duration, TimeSpan disappearance_duration, bool run)
        {
            DoubleAnimation appearance_animation = Appearance_animation(uIElement, appearance_duration);
            DoubleAnimation disappearance_animation = Disappearance_animation(uIElement, disappearance_duration, appearance_duration);
            Storyboard.SetTargetProperty(appearance_animation, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTargetProperty(disappearance_animation, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTarget(appearance_animation, uIElement);
            Storyboard.SetTarget(disappearance_animation, uIElement);
            Storyboard storyboard = new() { Children = new TimelineCollection { appearance_animation, disappearance_animation } };
            storyboard.Begin();
        }
        private static DoubleAnimation Appearance_animation(UIElement uIElement, TimeSpan duration)
        {
            DoubleAnimation appearance = new()
            {
                From = 0.0,
                To = 0.8,
                Duration = duration
            };
            return appearance;
        }
        public static void Appearance_animation(UIElement uIElement, TimeSpan duration, bool run)
        {
            DoubleAnimation appearance = new()
            {
                From = 0.0,
                To = 0.8,
                Duration = duration
            };
            uIElement.BeginAnimation(UIElement.OpacityProperty, appearance);
        }
        private static DoubleAnimation Appearance_animation(UIElement uIElement, TimeSpan duration, TimeSpan beginTime)
        {
            DoubleAnimation appearance = new()
            {
                BeginTime = beginTime,
                From = 0.0,
                To = 0.8,
                Duration = duration
            };
            return appearance;
        }
        public static void Appearance_animation(UIElement uIElement, TimeSpan duration, TimeSpan beginTime, bool run)
        {
            DoubleAnimation appearance = new DoubleAnimation
            {
                BeginTime = beginTime,
                From = 0.0,
                To = 0.8,
                Duration = duration
            };
            uIElement.BeginAnimation(UIElement.OpacityProperty, appearance);
        }
        private static DoubleAnimation Disappearance_animation(UIElement uIElement, TimeSpan duration)
        {
            DoubleAnimation disappearance = new DoubleAnimation
            {
                From = 0.8,
                To = 0.0,
                Duration = duration
            };
            return disappearance;
        }
        public static void Disappearance_animation(UIElement uIElement, TimeSpan duration, bool run)
        {
            DoubleAnimation disappearance = new DoubleAnimation
            {
                From = 0.8,
                To = 0.0,
                Duration = duration
            };
            uIElement.BeginAnimation(UIElement.OpacityProperty, disappearance);
        }
        private static DoubleAnimation Disappearance_animation(UIElement uIElement, TimeSpan duration, TimeSpan beginTime)
        {
            DoubleAnimation disappearance = new DoubleAnimation
            {
                BeginTime = beginTime,
                From = 0.8,
                To = 0.0,
                Duration = duration
            };
            return disappearance;
        }
        public static void Disappearance_animation(UIElement uIElement, TimeSpan duration, TimeSpan beginTime, bool run)
        {
            DoubleAnimation disappearance = new DoubleAnimation
            {
                BeginTime = beginTime,
                From = 0.8,
                To = 0.0,
                Duration = duration
            };
            uIElement.BeginAnimation(UIElement.OpacityProperty, disappearance);
        }

        //Подсвечивает логин/пароль при успешной авторизации
        private static DoubleAnimation RunningColor_animation(GradientStop gradientStop, TimeSpan duration)
        {
            DoubleAnimation run_color = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = duration
            };
            return run_color;
        }
        public static void RunningColor_animation(GradientStop gradientStop, TimeSpan duration, bool run)
        {
            DoubleAnimation run_color = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = duration
            };
            run_color.BeginAnimation(GradientStop.OffsetProperty, run_color);
        }
        private static DoubleAnimation RunningColor_animation(GradientStop gradientStop, TimeSpan duration, TimeSpan beginTime)
        {
            DoubleAnimation run_color = new DoubleAnimation
            {
                BeginTime = beginTime,
                From = 0,
                To = 1,
                Duration = duration
            };
            return run_color;
        }
        public static void RunningColor_animation(GradientStop gradientStop, TimeSpan duration, TimeSpan beginTime, bool run)
        {
            DoubleAnimation run_color = new DoubleAnimation
            {
                BeginTime = beginTime,
                From = 0,
                To = 1,
                Duration = duration
            };
            run_color.BeginAnimation(GradientStop.OffsetProperty, run_color);
        }
        public static DoubleAnimation RunningColor_animation(GradientStop gradientStop, TimeSpan duration, double from_value, double to_value)
        {
            DoubleAnimation run_color = new DoubleAnimation
            {
                From = from_value,
                To = to_value,
                Duration = duration
            };
            return run_color;
        }
        public static void RunningColor_animation(GradientStop gradientStop, TimeSpan duration, double from_value, double to_value, bool run)
        {
            DoubleAnimation run_color = new DoubleAnimation
            {
                From = from_value,
                To = to_value,
                Duration = duration
            };
            run_color.BeginAnimation(GradientStop.OffsetProperty, run_color);
        }
        //public static void ProgressBarFill(GradientStopCollection gradientStops, TimeSpan duration, double[] offsets, bool run)
        //{
        //    DoubleAnimation left_runningColor_animation = RunningColor_animation(gradientStops[0], duration, offsets[0], offsets[1]);
        //    DoubleAnimation center_runningColor_animation = RunningColor_animation(gradientStops[0], duration,  offsets[2], offsets[3]);
        //    DoubleAnimation right_runningColor_animation = RunningColor_animation(gradientStops[0], duration, offsets[4], offsets[5]);
        //    gradientStops[0].BeginAnimation(GradientStop.OffsetProperty, left_runningColor_animation);
        //    gradientStops[1].BeginAnimation(GradientStop.OffsetProperty, center_runningColor_animation);
        //    gradientStops[2].BeginAnimation(GradientStop.OffsetProperty, right_runningColor_animation);

        //}
        public static void ProgressBarFill(GradientStop gradientStop, TimeSpan duration, double[] offsets, bool run)
        {
            DoubleAnimation left_runningColor_animation = RunningColor_animation(gradientStop, duration, offsets[0], offsets[1]);
            gradientStop.BeginAnimation(GradientStop.OffsetProperty, left_runningColor_animation);
        }
        public static void LoadingAnimation(Ellipse ellipse, bool run)
        {
            DoubleAnimation rotate = new()
            {
                Duration = TimeSpan.FromSeconds(1),
                From = 0,
                To = 360,
                RepeatBehavior = RepeatBehavior.Forever
            };
            ellipse.LayoutTransform.BeginAnimation(RotateTransform.AngleProperty, rotate);
        }
        public static void Refresh(Image img, bool run)
        {
            DoubleAnimation rotate = new()
            {
                Duration = TimeSpan.FromSeconds(1),
                From = 0,
                To = 360
            };
            img.LayoutTransform.BeginAnimation(RotateTransform.AngleProperty, rotate);
        }
        public static void LoadingAnimation(Ellipse ellipse, bool run, bool stop)
        {
            ellipse.LayoutTransform.BeginAnimation(RotateTransform.AngleProperty, null);
        }
    }
}
