using CefSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Get_links_from_website
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string webpagelink = string.Empty;

        TreeView treeView = new TreeView();
        TreeViewItem treeViewItem = new TreeViewItem();
        List<string> list = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetLink_Button_Click(object sender, RoutedEventArgs e)
        {
            TreeviewGrid.Children.Remove(treeView);
            treeView.Items.Clear();
            list.Clear();
            Cef.GetGlobalCookieManager().DeleteCookiesAsync(string.Empty, string.Empty);

            if (ChromiumBrowser.Address == null)
            {


                ChromiumBrowser.Address = $@"{URL_TextBox.Text}";
            }


            if (ChromiumBrowser.Address != null)
            {

                ChromiumBrowser.Address = string.Empty;
                ChromiumBrowser.Reload();
                ChromiumBrowser.Address = $@"{URL_TextBox.Text}";

            }


            try
            {
                WebClient client = new WebClient();
                webpagelink = client.DownloadString($@"{URL_TextBox.Text}");




                list = LinkExtractor.GetLink(webpagelink);


                int counter2 = 0;

                foreach (var link in list)
                {


                    if (link.IndexOf("https", 0) == 0)
                    {
                        if (link.Contains("https"))
                        {
                            treeViewItem.Items.Add(link);

                            counter2++;


                        }
                    }
                }


           

                string[] slink2 = list.ToArray();

                int j2 = 1;

                treeViewItem.Header = "Links";

                foreach (var link in list)
                {
                    if (link.IndexOf("https", 0) == 0)
                    {
                        if (link.Contains("https"))
                        {
                            for (int i = 0; i < counter2; i++)
                            {
                                TreeViewItem treeViewItemchild = new TreeViewItem();

                                treeViewItemchild.Header = $"Link {j2++}";

                                treeViewItem.Items.Add(treeViewItemchild);

                                treeViewItemchild.Items.Add(link);

                                if (i < counter2)
                                    break;
                            }




                        }
                    }

                }


                treeView.Items.Add(treeViewItem);

                TreeviewGrid.Children.Add(treeView);

                URL_TextBox.Text = string.Empty;

                treeView.SelectedItemChanged += TreeView_SelectedItemChanged;

            }
            catch (Exception)
            {
                int counter = 0;

                WebClient Web = new WebClient();
                string Source = Web.DownloadString("https://www.google.com/search?q=" + URL_TextBox.Text);


                ChromiumBrowser.Address = "https://www.google.com/search?q=" + URL_TextBox.Text;

                List<string> list = LinkExtractor.GetLink(Source);


                treeView.SelectedItemChanged += TreeView_SelectedItemChanged;

                treeViewItem.Header = "Links";

                foreach (var link in list)
                {
                    if (link.IndexOf("https", 0) == 0)
                    {
                        if (link.Contains("https"))
                        {
                            treeViewItem.Items.Add(link);
                            counter++;

                        }
                    }
                }


                string[] slink = list.ToArray();

                int j = 1;

                foreach (var link in list)
                {
                    if (link.IndexOf("https", 0) == 0)
                    {
                        if (link.Contains("https"))
                        {
                            for (int i = 0; i < counter; i++)
                            {
                                TreeViewItem treeViewItemchild = new TreeViewItem();

                                treeViewItemchild.Header = $"Link {j++}";

                                treeViewItem.Items.Add(treeViewItemchild);

                                treeViewItemchild.Items.Add(link);

                            if(i < counter)
                                break;
                            }




                        }
                    }

                }



                treeView.Items.Add(treeViewItem);

                TreeviewGrid.Children.Add(treeView);

                URL_TextBox.Text = string.Empty;

            }



        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (ChromiumBrowser.Address == null)
                {

                    ChromiumBrowser.Address = treeView.SelectedItem.ToString();
                }


                if (ChromiumBrowser.Address != null)
                {

                    ChromiumBrowser.Address = string.Empty;
                    ChromiumBrowser.Reload();
                    if (treeView.SelectedItem != null)
                    {
                        ChromiumBrowser.Address = treeView.SelectedItem.ToString();
                    }

                }

            }
            catch (Exception)
            {


            }
        }

        public class LinkExtractor
        {

            public static List<string> GetLink(string link)
            {
                List<string> list = new List<string>();

                Regex regex = new Regex("(?:href|src)=[\"|']?(.*?)[\"|'|>]+", RegexOptions.Singleline | RegexOptions.CultureInvariant);

                if (regex.IsMatch(link))
                {
                    foreach (Match match in regex.Matches(link))
                    {
                        list.Add(match.Groups[1].Value);
                    }
                }

                return list;
            }
        }


    }
}
