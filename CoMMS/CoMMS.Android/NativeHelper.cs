using CoMMS.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(NativeHelper))]
namespace CoMMS.Droid
{
    public class NativeHelper : INativeHelper
    {
        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}