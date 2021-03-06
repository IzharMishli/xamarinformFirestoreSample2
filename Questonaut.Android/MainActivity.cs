﻿using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Prism;
using Prism.Ioc;
using System.Threading.Tasks;
using System.IO;
using Plugin.CurrentActivity;
using PanCardView.Droid;
using FFImageLoading.Forms.Platform;
using Plugin.Permissions;
using Plugin.LocalNotifications;
using Plugin.FirebasePushNotification;

namespace Questonaut.Droid
{
    [Activity(Label = "Questonaut", Icon = "@mipmap/icon", Theme = "@style/Theme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        #region instances
        internal static MainActivity Instance { get; private set; }
        #endregion

        #region properties
        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { get; set; }
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //set the notification icon
            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.Questonaut;

            // Name of the MainActivity theme you had there before.
            // Or you can use global::Android.Resource.Style.ThemeHoloLight
            base.SetTheme(Resource.Style.MainTheme);

            //initialize the media plugin
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            //initalize the ffimageloading framework
            CachedImageRenderer.Init(enableFastRenderer: true);


            //initalize the collection view
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");

            base.OnCreate(savedInstanceState);
            Instance = this;

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //initalize the cards view framework
            CardsViewRenderer.Preserve();

            //using the ffloading for the standard Xamarin.Forms.Image
            CachedImageRenderer.InitImageViewHandler();

            LoadApplication(new App(new AndroidInitializer()));

            //intialize the firebase push notification framework
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Shiny.AndroidShinyHost.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public class AndroidInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {

            }
        }
    }
}