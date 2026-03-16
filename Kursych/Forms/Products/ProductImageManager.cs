using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Kursych.Forms.Products
{
    public static class ProductImageManager
    {
        // Путь к папке с изображениями
        private static readonly string ImagesFolder = Path.Combine(Application.StartupPath, "ProductImages");

        static ProductImageManager()
        {
            // Создаем папку для изображений, если её нет
            if (!Directory.Exists(ImagesFolder))
            {
                Directory.CreateDirectory(ImagesFolder);
            }
        }

        // Получить полный путь к файлу изображения
        public static string GetImagePath(string fileName)
        {
            return Path.Combine(ImagesFolder, fileName + ".jpg");
        }

        // Загрузить изображение из файла
        public static Image LoadImage(string fileName)
        {
            string filePath = GetImagePath(fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    return Image.FromFile(filePath);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        // Проверить существует ли изображение
        public static bool ImageExists(string fileName)
        {
            return File.Exists(GetImagePath(fileName));
        }

        // Получить список всех файлов изображений
        public static string[] GetAllImageFiles()
        {
            if (Directory.Exists(ImagesFolder))
            {
                return Directory.GetFiles(ImagesFolder, "*.jpg");
            }
            return new string[0];
        }

        // ============= ДОБАВЛЕННЫЙ МЕТОД =============
        // Создать изображение-заглушку "Нет фото"
        public static Bitmap CreateNoImageBitmap(int width = 80, int height = 80)
        {
            Bitmap noImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(noImage))
            {
                g.Clear(Color.LightGray);

                // Рисуем рамку
                using (Pen pen = new Pen(Color.Gray, 1))
                {
                    g.DrawRectangle(pen, 0, 0, width - 1, height - 1);
                }

                // Рисуем крест
                using (Pen pen = new Pen(Color.Gray, 2))
                {
                    g.DrawLine(pen, 5, 5, width - 5, height - 5);
                    g.DrawLine(pen, width - 5, 5, 5, height - 5);
                }

                // Текст "Нет фото"
                string text = "Нет фото";
                using (Font font = new Font("Arial", 8))
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    SizeF textSize = g.MeasureString(text, font);
                    g.DrawString(text, font, brush,
                        (width - textSize.Width) / 2,
                        (height - textSize.Height) / 2);
                }
            }
            return noImage;
        }
    }
}