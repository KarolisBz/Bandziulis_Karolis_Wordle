namespace Wordle_Karolis_G00417529
{
    // this class holds useful functions for mananging fonts across entire program
    public class fontManager
    {
        static public double scaleFontSize(double fullSize, double windowHeight, double windowWidth)
        {
            // function varibles 
            double pixelDensity = DeviceDisplay.MainDisplayInfo.Density;

            // originally made at 2560p x 1440p, so we scale to that (minus the task bar ect)
            double scaledHeight = (windowHeight / pixelDensity) / 1408;
            double scaledWidth = (windowWidth / pixelDensity) / 2560;
            double scaledFontSize = ((scaledHeight + scaledWidth) / 2) * fullSize;

            return scaledFontSize;
        }

        // this function returns the max size that would fit nicely in a box constraint of X,Y
        static public double findFontSizeToConstraint(double height)
        {
            // function varibles 
            double pixelDensity = DeviceDisplay.MainDisplayInfo.Density;
            double scaledFontSize = (height / pixelDensity) / 2;

            return scaledFontSize;
        }
    }
}
