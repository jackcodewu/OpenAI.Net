using MahApps.Metro.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;
using OpenAI.Net.api;
using OpenAI.Net.Assistants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace OpenAI.Net
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        OpenAIHttp openAIHttp;
        List<AssistantsRoleCreateResponse> assistantsRoleCreateResponses = new List<AssistantsRoleCreateResponse>();
        AssistantsRoleCreateResponse assistantsRoleCreateResponse;

        AssistantsThreadCreateResponse assistantsThreadCreateResponse;

        AssistantsAddMsgResponse assistantsAddMsgResponse;
        AssistantsCreateRunResponse assistantsCreateRunResponse;
        List<AssistantsListMessagesResponse> assistantsListMessagesResponses = new List<AssistantsListMessagesResponse>();

        AssistantsListMessagesResponse assistantsListMessagesResponse;
        AssistantsListResponse assistantsListResponse;

        Stopwatch stopwatch = new Stopwatch();
        bool isClose = false;

        Timer timer;
        private string prevInput;
        private bool isSave;

        private System.Windows.Point startPoint;
        //private System.Windows.Point endPoint;
        Window screenPrintWindow;
        System.Windows.Shapes.Rectangle rectangle;
        Canvas canvas;
        bool bStart;

        public MainWindow()
        {
            InitializeComponent();
            // panelBtn.IsEnabled = false;
            Closing += MainWindow_Closing;

            //string json = File.ReadAllText("assistants.json");
            //var assistants= JsonConvert.DeserializeObject<List<AssistantsRoleCreateRequest>>(json);

            WindowState = WindowState.Maximized;
            Width = SystemParameters.WorkArea.Width;
            Height = SystemParameters.WorkArea.Height;

            openAIHttp = new OpenAIHttp();

            //Task.Run(async () =>
            //{
            //    var result = await new AssistantsRequest().InitAsync();
            //    assistantsRoleCreateResponse = result.assistantsRoleCreateResponse;
            //    assistantsThreadCreateResponse = result.assistantsThreadCreateResponse;

            //}).Wait();

            // string threadId = txtThreadId.Text;

            #region MyRegion

            Task.Run(async () =>
            {
                assistantsListResponse = await new AssistantsListRequest().SendMsg();
                if (assistantsListResponse != null && assistantsListResponse.data != null && assistantsListResponse.data.Length > 0)
                {
                    var list = assistantsListResponse.data.Where(c => !string.IsNullOrWhiteSpace(c.id));
                    if (list != null && list.Count() > 0)
                    {
                        assistantsRoleCreateResponses.AddRange(list);
                    }
                }

                if (assistantsRoleCreateResponses.Count == 0)
                {
                    assistantsRoleCreateResponse = await new AssistantsRoleCreateRequest().SendMsg();
                    assistantsRoleCreateResponses.Add(assistantsRoleCreateResponse);
                }
                assistantsThreadCreateResponse = await new AssistantsThreadCreateRequest().SendMsg();

            }).Wait();

            for (int i = 0; i < assistantsRoleCreateResponses.Count; i++)
            {
                assistantsRoleCreateResponses[i].Success = true;
            }

            cbmAssistants.DisplayMemberPath = "name";
            cbmAssistants.SelectedValuePath = "id";
            cbmAssistants.ItemsSource = assistantsRoleCreateResponses;
            cbmAssistants.SelectedIndex = 0;

            #endregion

            cmBoxTalks.Items.Add("请您");
            cmBoxTalks.Items.Add("请问 ");
            cmBoxTalks.Items.Add("请问在asp.net 7中 ");
            cmBoxTalks.Items.Add("请问在WPF中 ");
            cmBoxTalks.Items.Add("接上面的话题，请问 ");
            cmBoxTalks.Items.Add("请问在abp vnext中 ");
            cmBoxTalks.Items.Add("");
            cmBoxTalks.SelectedIndex = 0;



            timer = new Timer(1000);
            timer.Enabled = false;
            timer.Elapsed += Timer_Elapsed;

            panelUser.IsEnabled = true;
        }

        #region  methods
        private async void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            AssistantsRetrievesRunResponse assistantsRetrievesRunResponse = await new AssistantsRetrievesRunRequest().SendMsg(assistantsThreadCreateResponse.id, assistantsCreateRunResponse.id);
            if (assistantsRetrievesRunResponse != null && assistantsRetrievesRunResponse.status == "completed")
            {
                timer.Stop();
                timer.Enabled = false;

                AssistantsListMessagesResponse assistantsListMessagesResponse = await new AssistantsListMessagesRequest().SendMsg(assistantsThreadCreateResponse.id);
                if (assistantsListMessagesResponse != null)
                {
                    assistantsListMessagesResponses.Add(assistantsListMessagesResponse);

                    stopwatch.Stop();
                    if (assistantsListMessagesResponse.data != null && assistantsListMessagesResponse.data.Length > 0)
                    {
                        var item = assistantsListMessagesResponse.data[0];
                        if (item != null && item.content != null && item.content.Length > 0 && item.content[0].type == "text")
                        {
                            Dispatcher.Invoke(() =>
                            {
                                txtResult.Text += "答： " + item.content[0].text.value + "\r\n\r\n";
                                if (isSave)
                                {
                                    if (!string.IsNullOrWhiteSpace(txtResult.Text))
                                    {

                                        //save to txt file

                                        try
                                        {
                                            string dir = $"D:\\Projects\\2023\\OpenAI\\docs\\{DateTime.Now.ToString("yyyyMMdd")}";
                                            if (!Directory.Exists(dir))
                                                Directory.CreateDirectory(dir);

                                            string path = $"{dir}\\{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_{item.content[0].text.value}.md";
                                            File.WriteAllText(path, txtResult.Text);

                                            isSave = false;

                                            if (isClose && File.Exists(path))
                                                Application.Current.Shutdown();

                                            txtResult.Text = "";

                                        }
                                        catch (Exception err)
                                        {
                                            txtResult.Text += err.Message + "\r\n\r\n";
                                        }
                                    }
                                }
                                panelUser.IsEnabled = true;
                            });
                        }
                    }
                }
            }
        }

        private bool checkInput()
        {
            panelUser.IsEnabled = false;

            if (!assistantsThreadCreateResponse.Success
                || !assistantsRoleCreateResponse.Success
                || string.IsNullOrWhiteSpace(assistantsThreadCreateResponse.id)
                || string.IsNullOrWhiteSpace(assistantsRoleCreateResponse.id)
                || string.IsNullOrWhiteSpace(txtInput.Text)
                )
            {
                panelUser.IsEnabled = true;
                return false;
            }

            txtResult.Text += "\r\n\r\n";
            for (int i = 0; i < 100; i++)
            {
                txtResult.Text += "*";
            }
            txtResult.Text += $"\r\n\r\n问： {txtInput.Text}\r\n\r\n";
            prevInput = txtInput.Text;
            txtResult.CaretIndex = txtResult.Text.Length;
            txtResult.ScrollToEnd();

            return true;
        }


        private async Task SubmitMsgAsync()
        {

            if (!checkInput()) return;

            string queryStr=txtInput.Text+ "(请查找所有语言的相关资料)，";
            
            if ((bool)chkTrans.IsChecked)
            {
                queryStr += "请用中英文双语回答。";
            }

            queryStr += "谢谢您了！";

            assistantsAddMsgResponse = await new AssistantsAddMsgRequest().SendMsg(assistantsThreadCreateResponse.id, queryStr);//，请在回复的内容中，每句话返回中英对内容

            txtInput.Text = "";

            if (assistantsAddMsgResponse == null || !assistantsAddMsgResponse.Success)
            {
                panelUser.IsEnabled = true;
                return;
            }

            assistantsCreateRunResponse = await new AssistantsCreateRunRequest().SendMsg(assistantsThreadCreateResponse.id, assistantsRoleCreateResponse.id);

            if (assistantsCreateRunResponse != null && assistantsCreateRunResponse.id != null && assistantsCreateRunResponse.id.Length > 0)
            {
                timer.Enabled = true;
                timer.Start();
                stopwatch.Start();
            }
            else
            {
                panelUser.IsEnabled = true;
                return;
            }

        }

        public string BitmapToBase64(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Bitmap to byte[]
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public string ImageFileToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }

        #endregion

        #region  Controls Event

        private async void cbmAssistants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string id = cbmAssistants.SelectedValue.ToString();
            assistantsRoleCreateResponse = assistantsRoleCreateResponses.FirstOrDefault(c => c.id == id);

        }
        private async void btnOpenImage_Click(object sender, RoutedEventArgs e)
        {

            Hide();
            screenPrintWindow = null;
            canvas = null;
            rectangle = null;


            screenPrintWindow = new Window();
            screenPrintWindow.WindowStyle = WindowStyle.None;
            screenPrintWindow.Opacity = 0.09;
            screenPrintWindow.Left = 0;
            screenPrintWindow.Top = 0;
            screenPrintWindow.Height = SystemParameters.PrimaryScreenHeight;
            screenPrintWindow.Width = SystemParameters.PrimaryScreenWidth;
            screenPrintWindow.AllowsTransparency = true;
            screenPrintWindow.Background = System.Windows.Media.Brushes.Black;
            screenPrintWindow.Topmost = true;

            #region MyRegion
            canvas = new Canvas();
            canvas.Background = System.Windows.Media.Brushes.Black;
            canvas.Height = screenPrintWindow.Height;
            canvas.Width = screenPrintWindow.Width;
            canvas.Opacity = 1;

            canvas.MouseDown += canvas_MouseDown;
            canvas.MouseUp += canvas_MouseUp;
            canvas.MouseMove += canvas_MouseMove;
            //canvas.SizeChanged += canvas_SizeChanged;
            screenPrintWindow.Content = canvas;
            #endregion

            screenPrintWindow.Show();

            #region MyRegion
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "图片文件|*.jpg;*.png;*.gif;*.jpeg;*.bmp";
            //if (openFileDialog.ShowDialog() == true)
            //{
            //    string base64String = ImageFileToBase64(openFileDialog.FileName);
            //    var result = await new Gpt4VisionRequest().SendMsg($"data:image/jpeg;base64,{base64String}");
            //    txtInput.Text = result.choices[0]?.message?.content;
            //} 
            #endregion
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(canvas);
            canvas.Children.Clear();

            System.Drawing.Color color;
            #region MyRegion
            rectangle = new System.Windows.Shapes.Rectangle
            {
                Width = 10,
                Height = 10,
                Opacity = 1,
                Fill = System.Windows.Media.Brushes.White//new SolidColorBrush() { Color = System.Windows.Media.Color.FromArgb(0, (byte)(255-color.R), (byte)(255 - color.G), (byte)(255 - color.B)) }//                                                          //
            };

            Canvas.SetLeft(rectangle, startPoint.X);
            Canvas.SetTop(rectangle, startPoint.Y);
            canvas.Children.Add(rectangle);
            #endregion

            bStart = true;

            System.Diagnostics.Debug.WriteLine($"{startPoint.X} {startPoint.Y} ");
        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!bStart)
                return;

            var point = e.GetPosition(canvas);

            if (point.Y < 0 || point.X < 0 || point.Y < startPoint.Y || point.X < startPoint.X)
                return;

            #region MyRegion
            rectangle.Width = point.X - startPoint.X;
            rectangle.Height = point.Y - startPoint.Y;
            #endregion


            //System.Diagnostics.Debug.WriteLine($"{point.X} {point.Y}");
        }

        private async void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Record the end point when the mouse button is released
            // endPoint = e.GetPosition(canvas);

            //System.Diagnostics.Debug.WriteLine($"{endPoint.X} {endPoint.Y}");
            // Calculate the width and height of the screenshot
            int width = (int)rectangle.Width;// (int)Math.Abs(startPoint.X - endPoint.X);
            int height = (int)rectangle.Height;// Math.Abs(startPoint.Y - endPoint.Y);

            if (width == 0 || height == 0) return;

            // Create a new bitmap with the same size as the screenshot
            using (Bitmap bmp = new Bitmap(width, height))
            {
                #region MyRegion
                // Create a new graphics object from the bitmap
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // Capture the screen
                    g.CopyFromScreen((int)Canvas.GetLeft(rectangle), (int)Canvas.GetTop(rectangle), 0, 0, new System.Drawing.Size(width, height));
                    string base64string = BitmapToBase64(bmp);
                    screenPrintWindow.Close();
                    var result = await new Gpt4VisionRequest().SendMsg($"data:image/jpeg;base64,{base64string}");
                    txtInput.Text = result.choices[0]?.message?.content;
                }

                #endregion
            }
            GC.Collect();
            bStart = false;
            Show();
        }


        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Text = cmBoxTalks.Text + txtInput.Text;
            await SubmitMsgAsync();
        }

        private async void txtInput_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    txtInput.Text += "\r\n";
                    txtInput.CaretIndex = txtInput.Text.Length;
                }
                else
                {
                    txtInput.Text = txtInput.Text.Substring(0, txtInput.Text.Length - 2);
                    txtInput.Text = cmBoxTalks.Text + txtInput.Text;
                    txtInput.CaretIndex = txtInput.Text.Length;
                    await SubmitMsgAsync();

                }
            }
            else if (e.Key == Key.Up)
            {
                if (!string.IsNullOrWhiteSpace(prevInput))
                {
                    txtInput.Text = prevInput.Replace(cmBoxTalks.Text, "");
                    txtInput.CaretIndex = txtInput.Text.Length;
                }
            }
        }
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtResult.Text))
            {
                txtInput.Text = "请用中文一句话总结本次对话";
                isSave = true;
                isClose = false;
                chkTrans.IsChecked = false;
                await SubmitMsgAsync();
                chkTrans.IsChecked = true;

            }
        }

        private async void btnSaveClose_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtResult.Text))
            {
                txtInput.Text = "请用中文一句话总结本次对话";
                isSave = true;
                isClose = true;
                chkTrans.IsChecked = false;

                await SubmitMsgAsync();
                chkTrans.IsChecked = true;

                //Visibility = Visibility.Hidden;
            }
        }

        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            txtInput.Width = Width - panelBtn.Width - cmBoxTalks.Width;
            txtResult.Height = Height - panelUser.Height - 50;
        }

        private void MetroWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                Width = SystemParameters.WorkArea.Width;
                Height = SystemParameters.WorkArea.Height;
            }
            else if (this.WindowState == WindowState.Normal)
            {
                Width = 810;
            }

        }
        private async void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtResult.Text))
            {
                var result = MessageBox.Show("是否保存后再退出？选择 是 保存后再退出，选择 否 直接退出。", "提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                    e.Cancel = true;
                else if (result == MessageBoxResult.Yes)
                {
                    e.Cancel = true;

                    txtInput.Text = "请用中文一句话总结本次对话";
                    isSave = true;
                    isClose = true;

                    await SubmitMsgAsync();

                    //Visibility = Visibility.Hidden;
                }
            }
        }

        private async void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtResult.Text))
            {
                //txtInput.Text = "请用中文一句话总结本次对话";               
                //await SubmitMsgAsync();                
                //isClose = false;

                if (MessageBox.Show("确定要清空吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                    txtResult.Text = "";
            }
        }

        private async void txtInput_KeyDownAsync(object sender, KeyEventArgs e)
        {


        }

        #endregion

        private async void btnCreateImage_Click(object sender, RoutedEventArgs e)
        {
            if (!checkInput()) return;

            var result = await new CreateImageRequest().SendMsg(cmBoxTalks.Text + txtInput.Text);
            if (result.data != null && result.data.Length > 0)
            {
                txtResult.Text += string.Join("\r\n", result.data.Select(c => c.url).ToList());
            }
            txtInput.Text = "";
            panelUser.IsEnabled = true;

        }
    }
}
