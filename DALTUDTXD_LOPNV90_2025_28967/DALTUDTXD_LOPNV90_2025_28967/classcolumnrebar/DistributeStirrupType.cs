using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DALTUDTXD_LOPNV90_2025_28967.classcolumnrebar
{
    public class DistributeStirrupType
    {
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public ImageSource ImageSource => LoadImage(ImagePath);
    private ImageSource LoadImage(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        try
        {
            return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
        }
        catch
        {
            return null;
        }
    }
    }
}
