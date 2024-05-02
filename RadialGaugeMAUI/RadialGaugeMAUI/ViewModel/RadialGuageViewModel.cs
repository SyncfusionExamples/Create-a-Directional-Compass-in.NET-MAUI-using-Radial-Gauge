using System.ComponentModel;

namespace RadialGaugeMAUI
{
    public class RadialGuageViewModel : INotifyPropertyChanged
    {
        public RadialGuageViewModel()
        {
            this.ToggleCompass();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private double reading;
        public double Reading
        {
            get
            {
                return reading;
            }
            set
            {
                reading = value;
                this.RaisePropertyChanged(nameof(Reading));
            }
        }

        private double rotationAngle;
        public double RotationAngle
        {
            get
            {
                return rotationAngle;
            }
            set
            {
                rotationAngle = value;
                this.RaisePropertyChanged(nameof(RotationAngle));
            }
        }

        private string readingText;
        public string ReadingText
        {
            get
            {
                return readingText;
            }
            set
            {
                readingText = value;
                this.RaisePropertyChanged(nameof(ReadingText));
            }
        }
      
        private void OnCompassReadingChanged(object sender, CompassChangedEventArgs e)
        {
            this.Reading = e.Reading.HeadingMagneticNorth;
            var degree = (int)this.Reading;
            string direction = string.Empty;
            this.RotationAngle = 360 - e.Reading.HeadingMagneticNorth;

            if (degree < 30)
            {
                direction = "N";
            }
            else if (degree >= 30 && degree < 90)
            {
                direction = "NE";
            }
            else if (degree >= 90 && degree < 120)
            {
                direction = "E";
            }
            else if (degree >= 120 && degree < 180)
            {
                direction = "SE";
            }
            else if (degree >= 180 && degree < 210)
            {
                direction = "S";
            }
            else if (degree >= 210 && degree < 270)
            {
                direction = "SW";
            }
            else if (degree >= 270 && degree < 300)
            {
                direction = "W";
            }
            else if (degree >= 300 && degree <= 360)
            {
                direction = "NW";
            }

            ReadingText = $"{degree}\u00B0 {direction} ";
        }
        private void ToggleCompass()
        {
            if (Compass.Default.IsSupported)
            {
                if (!Compass.Default.IsMonitoring)
                {
                    Compass.Default.ReadingChanged += OnCompassReadingChanged;
                    Compass.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    Compass.Default.Stop();
                    Compass.Default.ReadingChanged -= OnCompassReadingChanged;
                }
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(PropertyChanged, new PropertyChangedEventArgs(propertyName));
        }
    }

}
