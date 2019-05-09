using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace OCRSDKTest
{
    public class TiffCreator
    {
        public enum TiffCompressType
        {
            NONE,
            LZW,
            CCITT_GROUP3_MH,
            CCITT_GROUP4_MMR,
            PACK_BITS
        }

        public static string GetTiffFormatName(string filepath)
        {
            using (Image tiffImg = Image.FromFile(filepath))
            {
                return GetTiffFormatName(tiffImg);
            }
        }

        public static string GetTiffFormatName(Image tiffImg)
        {
            if (!tiffImg.PropertyIdList.Contains(0x0103))
            {
                return "UNKONW";
            }
            PropertyItem item = tiffImg.GetPropertyItem(0x0103);
            int comp = (int)(item.Value[0] + (item.Value[1] << 8));
            switch (comp)
            {
                case 1:
                    return "NONE";
                case 2:
                    return "CCITT";
                case 3:
                    if (tiffImg.PropertyIdList.Contains(0x0124))
                    {
                        PropertyItem g3Prop = tiffImg.GetPropertyItem(0x0124);
                        int g3Option = (int)(g3Prop.Value[0]) & 0x01;
                        if (g3Option > 0)
                        {
                            return "CCITT_GROUP3_MR";
                        }
                        else
                        {
                            return "CCITT_GROUP3_MH";
                        }
                    }
                    return "CCITT_GROUP3_MH";
                case 4:
                    return "CCITT_GROUP4_MMR";
                case 5:
                    return "LZW";
                case 32773:
                    return "PACK BITS";
                default:
                    return "UNKONW";
            }
        }
        public static TiffCompressType GetTiffFormat(Image tiffImg)
        {
            if (!tiffImg.PropertyIdList.Contains(0x0103))
            {
                return TiffCompressType.NONE;
            }
            PropertyItem item = tiffImg.GetPropertyItem(0x0103);
            int comp = (int)(item.Value[0] + (item.Value[1] << 8));
            switch (comp)
            {
                case 1:
                    return TiffCompressType.NONE;
                case 2:
                    return TiffCompressType.CCITT_GROUP3_MH;
                case 3:
                    if (tiffImg.PropertyIdList.Contains(0x0124))
                    {
                        PropertyItem g3Prop = tiffImg.GetPropertyItem(0x0124);
                        int g3Option = (int)(g3Prop.Value[0]) & 0x01;
                        if (g3Option > 0)
                        {
                            return TiffCompressType.CCITT_GROUP4_MMR;
                        }
                        else
                        {
                            return TiffCompressType.CCITT_GROUP4_MMR;
                        }
                    }
                    return TiffCompressType.CCITT_GROUP3_MH;
                case 4:
                    return TiffCompressType.CCITT_GROUP4_MMR;
                case 5:
                    return TiffCompressType.LZW;
                case 32773:
                    return TiffCompressType.PACK_BITS;
                default:
                    return TiffCompressType.NONE;
            }
        }

        public static void CreateTiffImage(Image[] images, string path, TiffCompressType compressType)
        {

            if (images == null || images.Length == 0)
            {
                return;
            }
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            var coderInfo = getCodecForstring("image/tiff");
            EncoderParameters encoderParams = null;
            Image tiffImg = null;

            if (images.Length == 1)
            {
                encoderParams = new EncoderParameters(1);
                var param1 = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, GetCompressType(compressType));
                encoderParams.Param[0] = param1;
                tiffImg = new Bitmap(images[0]);
                tiffImg.Save(path, coderInfo, encoderParams);
            }
            else
            {
                encoderParams = new EncoderParameters(2);
                var param1 = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                var param2 = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, GetCompressType(compressType));
                encoderParams.Param[0] = param1;
                encoderParams.Param[1] = param2;
                using (System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.CreateNew))
                {
                    tiffImg = new Bitmap(images[0]);
                    tiffImg.Save(stream, coderInfo, encoderParams);
                    for (int i = 1; i < images.Length; i++)
                    {
                        encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                        tiffImg.SaveAdd(images[i], encoderParams);
                    }
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.Flush);
                    tiffImg.SaveAdd(encoderParams);
                }

            }

        }

        private static long GetCompressType(TiffCompressType compressType)
        {
            switch (compressType)
            {
                case TiffCompressType.LZW:
                    return (long)EncoderValue.CompressionLZW;
                case TiffCompressType.CCITT_GROUP3_MH:
                    return (long)EncoderValue.CompressionCCITT3;
                case TiffCompressType.CCITT_GROUP4_MMR:
                    return (long)EncoderValue.CompressionCCITT4;
                case TiffCompressType.PACK_BITS:
                    return (long)EncoderValue.CompressionRle;
                default:
                    return (long)EncoderValue.CompressionNone;
            }
        }

        private static ImageCodecInfo getCodecForstring(string type)
        {
            var info = ImageCodecInfo.GetImageEncoders();
            string mimetype;
            if (string.IsNullOrEmpty(type))
            {
                mimetype = "image/tiff";
            }
            else
            {
                mimetype = type.ToLower();
            }
            mimetype = mimetype.ToLower();
            var encoders = info.Where(t => t.MimeType.ToLower().Equals(mimetype));
            if (encoders.Count() > 0)
            {
                return encoders.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public static Image[] GetImages(Image tiffImage)
        {
            Guid objGuid = tiffImage.FrameDimensionsList[0];
            FrameDimension frameDim = new FrameDimension(objGuid);
            List<Image> images = new List<Image>();
            System.Drawing.Imaging.Encoder encode = System.Drawing.Imaging.Encoder.Compression;
            int pageCount = tiffImage.GetFrameCount(frameDim);
            if (pageCount == 1)
            {
                images.Add((Image)tiffImage.Clone());
                return images.ToArray();
            }
            for (int i = 0; i < pageCount; i++)
            {
                tiffImage.SelectActiveFrame(frameDim, i);

                using (MemoryStream stream = new MemoryStream())
                {
                    tiffImage.Save(stream, ImageFormat.Tiff);
                    Image frameImage = Image.FromStream(stream);
                    images.Add((Image)frameImage.Clone());
                }
            }
            return images.ToArray();
        }

        public static void SaveTiff(string filePath,Bitmap bmp)
        {
            // BmpBitmapEncoderを作成する
            TiffBitmapEncoder encoder = new TiffBitmapEncoder();
            encoder.Compression = TiffCompressOption.Lzw;

            // ページに追加する
            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Bmp);
                BitmapFrame bmpFrame = BitmapFrame.Create(stream);
                encoder.Frames.Add(bmpFrame);
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(fs);
                }
            }
        }

        public static void SlipTiffFile(string file)
        {
            // 画像ファイルデータをStreamで開く
            using (FileStream imageFileStrm = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // BitmapDecoderを作成する
                BitmapDecoder decoder = BitmapDecoder.Create(imageFileStrm, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

                // ページ数を取得する
                int pageCount = decoder.Frames.Count;


                for (int iLoop = 0; iLoop < pageCount; iLoop++)
                {
                    // ページを選択する
                    BitmapFrame bmpFrame = decoder.Frames[iLoop];

                    // BmpBitmapEncoderを作成する
                    TiffBitmapEncoder encoder = new TiffBitmapEncoder();
                    encoder.Compression = TiffCompressOption.Lzw;

                    // ページに追加する
                    encoder.Frames.Add(bmpFrame);

                    string pagefileName = System.IO.Path.GetFileNameWithoutExtension(file) + "_" + iLoop + ".tif";
                    string filePath = Path.Combine(Path.GetDirectoryName(file), pagefileName);
                    // シーケンスフォルダへ出力
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        encoder.Save(fs);
                    }
                }

            }
        }

        public static string SlipPfdfFile(string pdffile)
        {
            const string conPdfToTifProc = "magick.exe";      // pdf→tif変換の実行ファイル
            const string conPdfToTifParam = "-density {0} -compress LZW {1} {2}";    // pdf→tif変換のパラメータ

            // ファイル名にスペースが含まれているとプロセスが動作しないため、一時ファイルを作成する
            string tiffFilePath = Path.ChangeExtension(pdffile, ".tif");
         
            // 解像度、及び入出力ファイル名の取得とセット、引数の組み立て
             string strCommand = string.Format(conPdfToTifParam, "200",pdffile,tiffFilePath);

            // プロセスの設定
            using (System.Diagnostics.Process p = new System.Diagnostics.Process())
            {
                p.StartInfo.FileName = conPdfToTifProc;
                p.StartInfo.Arguments = strCommand;
                p.StartInfo.CreateNoWindow = true;      // コンソールウィンドウを開かない
                p.StartInfo.UseShellExecute = false;    // シェル機能を使用しない

                // プロセス起動
                p.Start();
                p.WaitForExit();
            }

            return tiffFilePath;
        }
    }
}
