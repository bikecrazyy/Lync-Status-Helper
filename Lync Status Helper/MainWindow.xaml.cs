using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Lync.Model;
using System.Reflection;
using System.Timers;

namespace Lync_Status_Helper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region MainWindow
        //a bunch of stuff so you don't have to install the SDK
        public MainWindow()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string resourceName = new AssemblyName(args.Name).Name + ".dll";
                string resource = Array.Find(this.GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };

            InitializeComponent();
        }
        #endregion

        #region btnOn_Click
        private void btnOn_Click(object sender, RoutedEventArgs e)
        {
            //retrive current availability
            currentAvailability.Content = GetAvailability().ToString();

            //retrive current status
            currentStatus.Content = GetActivity();
        }
        #endregion

        #region GetLyncClient
        private static LyncClient GetLyncClient()
        {
            var client = LyncClient.GetClient();
            
            if (client == null)
            {
                //Unable to obtain client interface
            }
            if (client.InSuppressedMode == true)
            {
                //Lync is uninitialized
            }

            if (client.State == ClientState.SignedIn)
            {
                //Connected to Lync
            }
            else
            {
                //Lync is NOT signed in
            }

            return client;
        }
        #endregion

        #region GetAvailability
        private static ContactAvailability GetAvailability()
        {
            //get lync client connection
            var client = GetLyncClient();

            //declare lync var's
            ContactAvailability Availability = ContactAvailability.None;
            
            //get availability status
            Availability = (ContactAvailability)client.Self.Contact.GetContactInformation(ContactInformationType.Availability);

            //return
            return Availability;
        }
        #endregion

        #region GetActivity
        private static string GetActivity()
        {
            //get lync client connection
            var client = GetLyncClient();
            
            //declare lync var
            string Activity = "";

            //get activity status
            Activity = (string)client.Self.Contact.GetContactInformation(ContactInformationType.ActivityId);

            //return
            return Activity;
        }
        #endregion
    }
}
