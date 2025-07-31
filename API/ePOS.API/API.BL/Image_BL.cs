using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace API.BL
{
    public class Image_BL
    {

        public byte[] GeneralQRCode(string value)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeVersion = 5;
            qrCodeEncoder.QRCodeScale = 5;
            Bitmap bitmap = qrCodeEncoder.Encode(value);

            ImageConverter imageConverter = new ImageConverter();
            byte[] buffer = (byte[])imageConverter.ConvertTo(bitmap, (typeof(byte[])));
            return buffer;
        }
    }
}