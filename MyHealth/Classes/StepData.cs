using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MyHealth
{
    public class StepData : DependencyObject,IXmlSerializable
    {
        public enum StepTypes
        {
            WorkTime,
            ImageSlider,
            ShortBreak,
            FreshStart,
            Seperator
        }
        public enum ImageListes
        {
            Body,Eye
        }

        #region Dependency Properites
        public static readonly DependencyProperty StepTypeProperty =
            DependencyProperty.Register("StepType", typeof(StepTypes), typeof(StepData), new PropertyMetadata());
        public static readonly DependencyProperty StepNameProperty =
            DependencyProperty.Register("StepName", typeof(string), typeof(StepData), new PropertyMetadata());
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(StepData), new PropertyMetadata());
        public static readonly DependencyProperty ImageListProperty =
            DependencyProperty.Register("ImageList", typeof(ImageListes), typeof(StepData), new PropertyMetadata());
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(StepData), new PropertyMetadata());

        #endregion

        public Geometry Icon
        {
            get { return (Geometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public StepTypes StepType
        {
            get { return (StepTypes)GetValue(StepTypeProperty); }
            set { SetValue(StepTypeProperty, value); }
        }
        public string StepName
        {
            get { return (string)GetValue(StepNameProperty); }
            set { SetValue(StepNameProperty, value); }
        }
        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        public ImageListes ImageList
        {
            get { return (ImageListes)GetValue(ImageListProperty); }
            set { SetValue(ImageListProperty, value); }
        }

        public StepData()
        {
            //Style = (Style)TryFindResource("StepData.DefaultStyle");
            BindIcon();
        }
        private void BindIcon()
        {
            //MultiBinding multiBinding = new MultiBinding();
            //multiBinding.Bindings.Add(new Binding("StepType")
            //{
            //    Source = this,
            //    Mode = BindingMode.TwoWay
            //});

            //multiBinding.Bindings.Add(new Binding("ImageList")
            //{
            //    Source = this,
            //    Mode = BindingMode.TwoWay
            //});
            //multiBinding.Converter = new StepDataIconConverter();
            //SetBinding(IconProperty, multiBinding);
        }

        public XmlSchema GetSchema() => null;
        public void ReadXml(XmlReader reader)
        {
            reader.Read();

            StepType = (StepTypes)reader.ReadElementContentAsInt();
            StepName = reader.ReadElementContentAsString();
            Duration = TimeSpan.FromSeconds(reader.ReadElementContentAsInt());
            ImageList = (ImageListes)reader.ReadElementContentAsInt();

            reader.ReadEndElement();
        }
        public void WriteXml(XmlWriter writer)
        {
            void Write(string name,object value)
            {
                writer.WriteStartElement(name);
                writer.WriteValue(value ?? "");
                writer.WriteEndElement();
            }

            Write(nameof(StepType), (int)StepType);
            Write(nameof(StepName), StepName);
            Write(nameof(Duration), Duration.TotalSeconds);
            Write(nameof(ImageList), (int)ImageList);

        }

        public override string ToString()
        {
            switch (StepType)
            {
                case StepTypes.WorkTime:
                case StepTypes.ShortBreak:
                    return $"{StepName} | {Duration.ToString("hh':'mm':'ss")}";
                case StepTypes.ImageSlider:
                    return $"{StepName} | ({ImageList}) {Duration.ToString("hh':'mm':'ss")}";
                case StepTypes.FreshStart:
                    return $"{StepName}";
                default:
                    return "";
            }
        }
        internal ITimerSlice ToStep()
        {
            switch (StepType)
            {
                case StepTypes.WorkTime:
                    return new WorkTime(Duration);
                case StepTypes.ImageSlider:
                    switch (ImageList)
                    {
                        case ImageListes.Body:
                            return new ImageSlider(Duration, System.IO.Path.Combine(Environment.CurrentDirectory, "Images", "body")) { StepName = StepName };
                        case ImageListes.Eye:
                            return new ImageSlider(Duration, System.IO.Path.Combine(Environment.CurrentDirectory, "Images", "eye")) { StepName = StepName };
                    }
                    return null;
                case StepTypes.FreshStart:
                    return new FreshStart() { StepName = StepName };
                case StepTypes.ShortBreak:
                    return new ShortBreak(Duration) { StepName = StepName };
                default:
                    return null;
            }
        }
        public object ToMenuItem()
        {
            //switch (StepType)
            //{
            //    case StepTypes.Seperator:
            //        return new Separator();
            //    default:
            //        Path itemIcon = new Path()
            //        {
            //            Data = Icon,
            //            Style = (Style)TryFindResource("StepData.MenuItemIcon"),
            //        };
            //        MenuItem item = new MenuItem()
            //        {
            //            Header = ToString(),
            //            Style = (Style)TryFindResource("StepData.MenuItem"),
            //            Icon = itemIcon
            //        };
            //        itemIcon.SetBinding(Shape.FillProperty, new Binding("BorderBrush") { Source = item });
            //        return item;
            //}
            return null;
        }
    }
}
