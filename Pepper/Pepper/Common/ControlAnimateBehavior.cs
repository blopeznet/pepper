using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Pepper.Common.Behavior
{

    /// <summary>
    /// Behaviour for animate elements
    /// </summary>
    public enum CustomAnimationType
    {
        None,
        Rotate,
        Shake,
        Hearth,
    }


    /// <summary>
    /// Class for use item selected as behaviour
    /// </summary>
    public class ControlAnimateBehavior : Behavior<View>
    {
        public static readonly BindableProperty AnimationProperty = BindableProperty.Create("Animation", typeof(CustomAnimationType), typeof(ControlAnimateBehavior), CustomAnimationType.None);

        public CustomAnimationType Animation
        {
            get { return (CustomAnimationType)GetValue(AnimationProperty); }
            set { SetValue(AnimationProperty, value); }
        }

        public View AssociatedObject { get; private set; }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            if (Animation == CustomAnimationType.Rotate)
            {
                RotateElement(AssociatedObject, new CancellationToken());
            }
            else if (Animation != CustomAnimationType.Rotate)
            {
                if (bindable is Image)
                    AssociateToTapIntoImage(bindable);
                else if (bindable is Grid)
                {
                    AssociateToTapIntoGrid(bindable);
                }
                else
                    ShakeElement(bindable, new CancellationToken());
            }
        }

        private void AssociateToTapIntoImage(View bindable)
        {
            Image img = (Image)bindable;
            if (img.GestureRecognizers.Count > 0)
            {
                TapGestureRecognizer tg = (TapGestureRecognizer)img.GestureRecognizers.Where(t => t.GetType() == typeof(TapGestureRecognizer)).FirstOrDefault();
                tg.Tapped += Tg_Tapped;
            }
        }

        private void AssociateToTapIntoGrid(View bindable)
        {
            Grid grid = (Grid)bindable;
            if (grid.GestureRecognizers.Count > 0)
            {
                TapGestureRecognizer tg = (TapGestureRecognizer)grid.GestureRecognizers.Where(t => t.GetType() == typeof(TapGestureRecognizer)).FirstOrDefault();
                tg.Tapped += Tg_Tapped;
            }
            else
                ShakeElement(bindable, new CancellationToken());
        }

        private void Tg_Tapped(object sender, EventArgs e)
        {
            if (Animation == CustomAnimationType.Shake)
                ShakeElement(AssociatedObject, new CancellationToken());
            if (Animation == CustomAnimationType.Hearth)
                HearthElement(AssociatedObject, new CancellationToken());
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
        }

        private async Task RotateElement(VisualElement element, CancellationToken cancellation)
        {

            while (!cancellation.IsCancellationRequested)
            {                
                   await element.RotateTo(360, 800, Easing.Linear);
                   await element.RotateTo(0, 0); // reset to initial position                
            }
        }

        private async Task ShakeElement(VisualElement element, CancellationToken cancellation)
        {
            await Xamanimation.AnimationExtension.Animate(element, new Xamanimation.ShakeAnimation());
        }

        private async Task HearthElement(VisualElement element, CancellationToken cancellation)
        {
            await Xamanimation.AnimationExtension.Animate(element, new Xamanimation.HeartAnimation());
        }

    }
}
