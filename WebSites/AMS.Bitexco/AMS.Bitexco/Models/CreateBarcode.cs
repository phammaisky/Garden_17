using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using Spire.Barcode;
using System.Drawing;
using System.Drawing.Imaging;

namespace AMS.Models
{
    public class CreateBarcode
    {
        private string _barcodePath = "~/Barcode/";
        public string barcodePath
        {
            get { return _barcodePath; }
        }
        public string CreateBarcodeInSpire(string BarcodeText)
        {    
            string barcodeSavePath = HttpContext.Current.Server.MapPath(this._barcodePath + BarcodeText + ".png");

            if (!File.Exists(barcodeSavePath))
            {
                BarcodeSettings setting = new BarcodeSettings();
                setting.Data = BarcodeText;
                setting.Type = BarCodeType.Code128;
                BarCodeGenerator bar = new BarCodeGenerator(setting);
                Image image = bar.GenerateImage();
                try
                {
                    image.Save(barcodeSavePath, ImageFormat.Png);
                }
                catch { }                
            }
            return BarcodeText + ".png";
        }
    }
}