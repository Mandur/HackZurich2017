using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.FaceAnalysis;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.System.Display;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HZ2017
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        MediaCapture _mediaCapture;
        bool _isPreviewing;
        DisplayRequest _displayRequest = new DisplayRequest();

        private readonly EmotionServiceClient EmotionServiceClient =
          new EmotionServiceClient("[insert your key here]");


        //face emotion
        public static Emotion[] emotion;
        private ThreadPoolTimer EmotionProcessingTimer;
        private SemaphoreSlim EmotionProcessingSemaphore = new SemaphoreSlim(1);

        //to detect faces offline
        private FaceTracker faceTracker;
        private ThreadPoolTimer frameProcessingTimer;
        private SemaphoreSlim frameProcessingSemaphore = new SemaphoreSlim(1);
        public static IList<DetectedFace> detectedFaces;

      
        public MainPage()
        {
            this.InitializeComponent();
            StartPreviewAsync();
        }

        private async Task StartPreviewAsync()
        {
            try
            {

                _mediaCapture = new MediaCapture();
                
                await _mediaCapture.InitializeAsync();

                _displayRequest.RequestActive();
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
            }
            catch (UnauthorizedAccessException)
            {
                // This will be thrown if the user denied access to the camera in privacy settings
                //("The app was denied access to the camera");
                return;
            }

            try
            {
                PreviewControl.Source = _mediaCapture;
                await _mediaCapture.StartPreviewAsync();
                _isPreviewing = true;
            }
            catch (System.IO.FileLoadException)
            {
                _mediaCapture.CaptureDeviceExclusiveControlStatusChanged += _mediaCapture_CaptureDeviceExclusiveControlStatusChanged;
            }


            //initialize UI facetracking
            this.faceTracker = await FaceTracker.CreateAsync();
            TimeSpan UIUpdateTimer = TimeSpan.FromMilliseconds(66); // 15 fps
            this.frameProcessingTimer = Windows.System.Threading.ThreadPoolTimer.CreatePeriodicTimer(new Windows.System.Threading.TimerElapsedHandler(ProcessCurrentVideoFrame), UIUpdateTimer);

            //initialize Remote emotiog Detection
            TimeSpan EmotionUpdateTimer = TimeSpan.FromMilliseconds(2000); // every 2 seconds
            this.EmotionProcessingTimer = Windows.System.Threading.ThreadPoolTimer.CreatePeriodicTimer(new Windows.System.Threading.TimerElapsedHandler(AnalyzeEmotion), EmotionUpdateTimer);

    


        }

        /// <summary>
        /// process 
        /// </summary>
        /// <param name="timer"></param>
        public async void ProcessCurrentVideoFrame(ThreadPoolTimer timer)
        {


            if (!frameProcessingSemaphore.Wait(0))
            {
                return;
            }

            VideoFrame currentFrame = await GetLatestFrame(BitmapPixelFormat.Nv12);

            // Use FaceDetector.GetSupportedBitmapPixelFormats and IsBitmapPixelFormatSupported to dynamically
            // determine supported formats
            const BitmapPixelFormat faceDetectionPixelFormat =  BitmapPixelFormat.Nv12;

            if (currentFrame.SoftwareBitmap.BitmapPixelFormat != faceDetectionPixelFormat)
            {
                return;
            }

            try
            {
               detectedFaces = await faceTracker.ProcessNextFrameAsync(currentFrame);
                
                SoftwareBitmap currentFrameBGRA = SoftwareBitmap.Convert(currentFrame.SoftwareBitmap, BitmapPixelFormat.Bgra8);

                //To modify the UI we have to run this on the UI Thread
                var ignored = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    ShowDetectedFaces(currentFrameBGRA);
                });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
                frameProcessingSemaphore.Release();
            }

            currentFrame.Dispose();
        }

        private async Task<VideoFrame> GetLatestFrame(BitmapPixelFormat format)
        {
            // Get information about the preview
            var previewProperties = _mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview) as VideoEncodingProperties;

            // Create a video frame in the desired format for the preview frame
            VideoFrame videoFrame = new VideoFrame(format, (int)previewProperties.Width, (int)previewProperties.Height);
            VideoFrame previewFrame = await _mediaCapture.GetPreviewFrameAsync(videoFrame);
            return previewFrame;
        }

        private async void ShowDetectedFaces(SoftwareBitmap sourceBitmap)
        {
            ImageBrush brush = new ImageBrush();
            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(sourceBitmap);

            if (detectedFaces != null)
            {
                double widthScale = sourceBitmap.PixelWidth / this.VisualizationCanvas.ActualWidth;
                double heightScale = sourceBitmap.PixelHeight / this.VisualizationCanvas.ActualHeight;
                this.VisualizationCanvas.Children.Clear();
           
                int i = 0;
                if (emotion==null)
                    return;
                var emoTmp= emotion.OrderBy(y => y.FaceRectangle.Left).ToList();
                    foreach (DetectedFace face in detectedFaces.OrderBy(x => x.FaceBox.X))
                    {
                    
                        SolidColorBrush color = new SolidColorBrush();
                        color = new SolidColorBrush(Windows.UI.Colors.Black);

                    try
                    {
                        if (emoTmp.Count != 0)
                        {
                            var highestEmotion = emoTmp[i].Scores.ToRankedList();
                            switch (highestEmotion.First().Key.ToLower())
                            {
                                case "anger":
                                    color = new SolidColorBrush(Windows.UI.Colors.Red);
                                    break;
                                case "contempt":
                                    color = new SolidColorBrush(Windows.UI.Colors.Blue);
                                    break;
                                case "disgust":
                                    color = new SolidColorBrush(Windows.UI.Colors.Green);
                                    break;
                                case "fear":
                                    color = new SolidColorBrush(Windows.UI.Colors.Yellow);
                                    break;
                                case "happiness":
                                    color = new SolidColorBrush(Windows.UI.Colors.Pink);
                                    break;
                                case "neutral":
                                    color = new SolidColorBrush(Windows.UI.Colors.White);
                                    break;
                                case "sadness":
                                    color = new SolidColorBrush(Windows.UI.Colors.Gray);
                                    break;
                                case "surprise":
                                    color = new SolidColorBrush(Windows.UI.Colors.DarkMagenta);
                                    break;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Write(ex.ToString());
                    }
                    // Create a rectangle element for displaying the face box but since we're using a Canvas
                    // we must scale the rectangles according to the image’s actual size.
                    // The original FaceBox values are saved in the Rectangle's Tag field so we can update the
                    // boxes when the Canvas is resized.
                    Rectangle box = new Rectangle();
                    box.Tag = face.FaceBox;
                    box.Width = (uint)(face.FaceBox.Width / widthScale);
                    box.Height = (uint)(face.FaceBox.Height / heightScale);
                    box.Fill = new SolidColorBrush(Windows.UI.Colors.Transparent);
                    box.Stroke = color;
                    box.StrokeThickness = 2.0f;
                    box.Margin = new Thickness((uint)(face.FaceBox.X / widthScale), (uint)(face.FaceBox.Y / heightScale), 0, 0);

                    this.VisualizationCanvas.Children.Add(box);
                }
            }
        }

   

        public async void AnalyzeEmotion(ThreadPoolTimer timer)
        {

            if (!EmotionProcessingSemaphore.Wait(0))
            {
                return;
            }
            using (var captureStream = new InMemoryRandomAccessStream())
            {
                await _mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), captureStream);
            
                captureStream.Seek(0);

                try {
                    emotion = await EmotionServiceClient.RecognizeAsync(captureStream.AsStream());
                    System.Diagnostics.Debug.WriteLine(DateTime.Now);
                }
                // Catch and display Face API errors.
                catch (FaceAPIException f)
                {
                    System.Diagnostics.Debug.WriteLine(f.ErrorMessage, f.ErrorCode);                 
                }
                // Catch and display all other errors.
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Data);
                }

            }
            EmotionProcessingSemaphore.Release();

        }



        private async void _mediaCapture_CaptureDeviceExclusiveControlStatusChanged(MediaCapture sender, MediaCaptureDeviceExclusiveControlStatusChangedEventArgs args)
        {
            if (args.Status == MediaCaptureDeviceExclusiveControlStatus.SharedReadOnlyAvailable)
            {
                System.Diagnostics.Debug.WriteLine("The camera preview can't be displayed because another app has exclusive access");
            }
            else if (args.Status == MediaCaptureDeviceExclusiveControlStatus.ExclusiveControlAvailable && !_isPreviewing)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    await StartPreviewAsync();
                });
            }
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await CleanupCameraAsync();
        }

        private async Task CleanupCameraAsync()
        {
            if (_mediaCapture != null)
            {
                if (_isPreviewing)
                {
                    await _mediaCapture.StopPreviewAsync();
                }

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    PreviewControl.Source = null;
                    if (_displayRequest != null)
                    {
                        _displayRequest.RequestRelease();
                    }

                    _mediaCapture.Dispose();
                    _mediaCapture = null;
                });
            }

        }


    }
}
