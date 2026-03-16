using System;
using System.Drawing;
using System.Drawing.Text;

namespace Kursych.Forms.Auth
{
    internal class CaptchaRenderer
    {
        private static readonly Random random = new Random();

        public static Bitmap RenderCaptcha(string text, int width = 200, int height = 60)
        {
            Bitmap bitmap = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                // Рисуем фон
                using (Pen pen = new Pen(Color.LightGray))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int x = random.Next(width);
                        int y = random.Next(height);
                        graphics.DrawLine(pen, x, 0, x, height);
                    }
                }

                // Рисуем текст - БЕЗ ИСКАЖЕНИЙ
                using (Font font = new Font("Arial", 30, FontStyle.Bold))
                {
                    using (SolidBrush brush = new SolidBrush(Color.Black))
                    {
                        // Просто рисуем текст по центру
                        SizeF textSize = graphics.MeasureString(text, font);
                        float x = (width - textSize.Width) / 2;
                        float y = (height - textSize.Height) / 2;

                        graphics.DrawString(text, font, brush, x, y);
                    }
                }
            }

            return bitmap;
        }
    }
}