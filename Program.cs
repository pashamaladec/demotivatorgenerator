using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace ImageGenerator
{
    class Program
    {
        static readonly int heightImage = 400;
        static readonly int widthImage = 400;

        static Image source;
        static string firstLine = "";
        static string secondLine = "";
        static readonly Font font = new Font("Times New Roman", 17, FontStyle.Bold);

        static string sourceAddress = @"";
        static string path = @"";

        static void Main()
        {
            Console.Write("source image (maybe https://...): ");
            sourceAddress = Console.ReadLine();

            if (sourceAddress.Contains("https://"))
                using (WebClient web = new WebClient())
                {
                    var bytes = web.DownloadData(sourceAddress);

                    source = Image.FromStream(new MemoryStream(bytes));
                }
            else
                source = Image.FromFile(sourceAddress);

            Console.Write("first line: ");
            firstLine = Console.ReadLine();

            Console.Write("second line: ");
            secondLine = Console.ReadLine();

            Bitmap picture = CreateDemotivator(heightImage, widthImage, firstLine, secondLine);


            Console.Write("save path: ");
            path = Console.ReadLine();
            picture.Save(path, ImageFormat.Png);

        }

        static Bitmap CreateDemotivator(int height, int width, string firstLine, string secondLine)
        {
            Bitmap bitmap = new Bitmap(height, width);
            using var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.Black);

            DrawDemotivator(graphics, firstLine, secondLine, 250, 325, 0, -50);
            return bitmap;
        }


        static Point CenterPoint(int? height = null, int? width = null)
        {
            height = height == null ? heightImage : height;
            width = width == null ? widthImage : width;

            return new Point((int)(height / 2), (int)(width / 2));
        }

        static Rectangle Rect(int width, int height, Point position)
        {
            return new Rectangle(position.X - width / 2, position.Y - height / 2, width, height);
        }
        static RectangleF RectF(float width, float height, Point position)
        {
            return new RectangleF(position.X - width / 2, position.Y - height / 2, width, height);
        }

        static Point Offset(Point point, Point offset)
        {
            point.X += offset.X;
            point.Y += offset.Y;

            return point;
        }

        static void DrawDemotivator(Graphics graphics, string firstLine, string secondLine, int height, int width, int x = 0, int y = 0)
        {
            var position = Offset(CenterPoint(), new Point(x, y));

            graphics.DrawImage(source, Rect(width, height, position));

            var linePen = new Pen(Brushes.White) { Width = 1.5f };
            var fontBrush = Brushes.White;

            // top line
            graphics.DrawLine(linePen, Offset(position, new Point(-width / 2 - 5, -height / 2 - 5)), Offset(position, new Point(width / 2 + 5, -height / 2 - 5)));

            // left line
            graphics.DrawLine(linePen, Offset(position, new Point(-width / 2 - 5, -height / 2 - 5)), Offset(position, new Point(-width / 2 - 5, height / 2 + 5)));

            // right line
            graphics.DrawLine(linePen, Offset(position, new Point(width / 2 + 5, -height / 2 - 5)), Offset(position, new Point(width / 2 + 5, height / 2 + 5)));

            // bottom lines
            graphics.DrawLine(linePen, Offset(position, new Point(-width / 2 - 5, height / 2 + 5)), Offset(position, new Point(width / 2 + 5, height / 2 + 5)));

            RectangleF textBox = RectF(width + width / 2, height / 2, Offset(position, new Point(0, height / 2 + height / 3)));
            StringFormat format = new StringFormat() { Alignment = StringAlignment.Center };
            graphics.DrawString($"{firstLine}\n{secondLine}", font, fontBrush, textBox, format);
        }
    }
}
