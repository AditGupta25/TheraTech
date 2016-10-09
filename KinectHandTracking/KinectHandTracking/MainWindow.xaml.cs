using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KinectHandTracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

         //private void DrawSkeletonsWithOrientations()
         // {
         //     foreach (Skeleton skeleton in this.skeletonData)
         //     {
         //         if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
         //         {
         //             foreach (BoneOrientation orientation in skeleton.BoneOrientations)
         //             {
         //                 // Display bone with Rotation using quaternion
         //                 DrawBonewithRotation(orientation.StartJoint, orientation.EndJoint, orientation.AbsoluteRotation.Quaternion);
         //                 // Display hierarchical rotation using matrix
         //                 DrawHierarchicalRotation(orientation.StartJoint, orientation.HierarchicalRotation.Matrix)
         //             }
         //         }
         //     }
         // }


        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    camera.Source = frame.ToBitmap();
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    canvas.Children.Clear();

                    _bodies = new Body[frame.BodyFrameSource.BodyCount];

                    frame.GetAndRefreshBodyData(_bodies);

                    foreach (var body in _bodies)
                    {
                        if (body != null)
                        {
                            if (body.IsTracked)
                            {

                                

                                // Find the joints
                                Joint handRight = body.Joints[JointType.HandRight];
                                Joint thumbRight = body.Joints[JointType.ThumbRight];
                                Joint elbowRight = body.Joints[JointType.ElbowRight];
                                Joint showlderRight = body.Joints[JointType.ShoulderRight];
                                Joint hipRight = body.Joints[JointType.HipRight];
                                Joint handTipRight = body.Joints[JointType.HandTipRight];
                                Joint footRight = body.Joints[JointType.FootRight];
                                Joint wristRight = body.Joints[JointType.WristRight];
                                Joint SpineBase = body.Joints[JointType.SpineBase];
                                Joint MidSpine = body.Joints[JointType.SpineMid];
                                
                                Joint neck = body.Joints[JointType.Neck];

                                Joint kneeLeft = body.Joints[JointType.KneeLeft];
                                Joint kneeRight = body.Joints[JointType.KneeRight];
                                Joint ankleLeft = body.Joints[JointType.AnkleLeft];
                                Joint ankleRight = body.Joints[JointType.AnkleRight];

                                Joint handLeft = body.Joints[JointType.HandLeft];
                                Joint thumbLeft = body.Joints[JointType.ThumbLeft];
                                Joint elbowLeft = body.Joints[JointType.ElbowLeft];
                                Joint showlderLeft = body.Joints[JointType.ShoulderLeft];
                                Joint hipLeft = body.Joints[JointType.HipLeft];
                                Joint handTipLeft = body.Joints[JointType.HandTipLeft];
                                Joint footLeft = body.Joints[JointType.FootLeft];
                                Joint wristLeft = body.Joints[JointType.WristLeft];


                                // Draw hands and thumbs
                                canvas.DrawHand(handRight, _sensor.CoordinateMapper);
                                canvas.DrawHand(handLeft, _sensor.CoordinateMapper);
                                canvas.DrawThumb(thumbRight, _sensor.CoordinateMapper);
                                canvas.DrawThumb(thumbLeft, _sensor.CoordinateMapper);
                                canvas.DrawThumb(elbowRight, _sensor.CoordinateMapper);
                                canvas.DrawThumb(elbowLeft, _sensor.CoordinateMapper);
                                canvas.DrawThumb(showlderLeft, _sensor.CoordinateMapper);
                                canvas.DrawThumb(showlderRight, _sensor.CoordinateMapper);
                                canvas.DrawThumb(hipLeft, _sensor.CoordinateMapper);
                                canvas.DrawThumb(hipRight, _sensor.CoordinateMapper);
                                canvas.DrawThumb(footLeft, _sensor.CoordinateMapper);
                                canvas.DrawThumb(footRight, _sensor.CoordinateMapper);
                                canvas.DrawThumb(wristLeft, _sensor.CoordinateMapper);
                                canvas.DrawThumb(wristRight, _sensor.CoordinateMapper);
                                canvas.DrawThumb(neck, _sensor.CoordinateMapper);
                                canvas.DrawThumb(MidSpine, _sensor.CoordinateMapper);
                                canvas.DrawThumb(SpineBase, _sensor.CoordinateMapper);
                                canvas.DrawThumb(ankleLeft, _sensor.CoordinateMapper);
                                canvas.DrawThumb(ankleRight, _sensor.CoordinateMapper);
                                canvas.DrawThumb(kneeLeft, _sensor.CoordinateMapper);
                                canvas.DrawThumb(kneeRight, _sensor.CoordinateMapper);

                                // Find the hand states
                                string rightHandState = "-";
                                string leftHandState = "-";
                                string rightLegState = "-";
                                string leftLegState = "-";
                                string emotionState = "-";

                                switch (body.HandRightState)
                                {
                                    case HandState.Open:
                                        rightHandState = "Open";
                                        break;
                                    case HandState.Closed:
                                        rightHandState = "Closed";
                                        break;
                                    case HandState.Lasso:
                                        rightHandState = "Thumbs Up";
                                        break;
                                    case HandState.Unknown:
                                        rightHandState = "Unknown...";
                                        break;
                                    case HandState.NotTracked:
                                        rightHandState = "Not tracked";
                                        break;
                                    default:
                                        break;
                                }


                                switch (body.HandLeftState)
                                {
                                    case HandState.Open:
                                        leftHandState = "Open";
                                        break;
                                    case HandState.Closed:
                                        leftHandState = "Closed";
                                        break;
                                    case HandState.Lasso:
                                        leftHandState = "Thumbs Up";
                                        break;
                                    case HandState.Unknown:
                                        leftHandState = "Unknown...";
                                        break;
                                    case HandState.NotTracked:
                                        leftHandState = "Not tracked";
                                        break;
                                    default:
                                        break;
                                }


                              


                                if ((kneeLeft.TrackingState == TrackingState.Inferred ||
                                    kneeLeft.TrackingState == TrackingState.Tracked) ||
                                    (kneeRight.TrackingState == TrackingState.Tracked ||
                                    kneeRight.TrackingState == TrackingState.Inferred))
                                {
                                    rightLegState = "active Form";
                                    leftLegState = "active Form";
                                }




                                tblRightHandState.Text = rightHandState;
                                tblLeftHandState.Text = leftHandState;
                                tblRightLegState.Text = leftLegState;
                                tblRightLegState.Text = rightLegState;
                                

                                if (tblLeftHandState.Text.ToString() == "THumbs Up")
                                {
                                    emotionState = "200 Points | Left Arm Stretch";
                                    tblEmotionState.Text = emotionState;
                                }

                                if (tblRightHandState.Text.ToString() == "THumbs Up")
                                {
                                    emotionState = "200 Points | Right Arm Stretch";
                                    tblEmotionState.Text = emotionState;
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
