using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

namespace PersonnelManagement.Helper
{
    public class DetectAvatar
    {
        public static async Task<IFormFile?> Detect(IFormFile imageFile)
        {
            try
            {
                // Tạo một tệp tạm thời để lưu hình ảnh đã tải lên
                string tempFilePath = Path.GetTempFileName() + ".jpg";

                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                if (!File.Exists(tempFilePath))
                {
                    Console.WriteLine("Không thể tạo file tạm thời: " + Path.GetFullPath(tempFilePath));
                    return null;
                }

                // Load model phát hiện khuôn mặt (OpenCV)
                //string haarcascadePath = "haarcascade_frontalface_default.xml";
                string haarcascadePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Helper", "haarcascade_frontalface_default.xml");


                // Load ảnh gốc
                Mat image = CvInvoke.Imread(tempFilePath, ImreadModes.Color);
                if (image.IsEmpty)
                {
                    Console.WriteLine("Không thể mở ảnh.");
                    return null;
                }

                // Tạo bản sao để hiển thị kết quả
                Mat outputImage = image.Clone();

                // Phương pháp 1: Tập trung vào vùng bên trái của CCCD
                // Định nghĩa vùng quan tâm (ROI) ở bên trái ảnh, chiếm khoảng 1/3 chiều rộng
                int roiWidth = image.Width / 3;
                Rectangle leftRegion = new Rectangle(0, 0, roiWidth, image.Height);

                // Cắt vùng ROI
                Mat leftROI = new Mat(image, leftRegion);

                // Chuyển ảnh ROI sang grayscale
                Mat gray = new Mat();
                CvInvoke.CvtColor(leftROI, gray, ColorConversion.Bgr2Gray);

                // Áp dụng các kỹ thuật xử lý ảnh để tăng độ tương phản
                CvInvoke.EqualizeHist(gray, gray);

                // Phát hiện khuôn mặt trong vùng ROI
                CascadeClassifier faceDetector = new CascadeClassifier(haarcascadePath);
                var faces = faceDetector.DetectMultiScale(gray, 1.1, 5, new Size(20, 20));

                // Biến để lưu vùng avatar được phát hiện
                Rectangle avatarRect = Rectangle.Empty;

                if (faces.Length > 0)
                {
                    // Giả định khuôn mặt đầu tiên là khuôn mặt trong avatar
                    var face = faces[0];

                    // Điều chỉnh tọa độ để phù hợp với ảnh gốc
                    face.X += leftRegion.X;
                    face.Y += leftRegion.Y;

                    // Dùng khuôn mặt để ước tính vùng avatar 3x4
                    // Ước tính tỷ lệ: ảnh 3x4 thường có chiều cao gấp 4/3 chiều rộng
                    int faceExpansion = (int)(face.Width * 0.4); // Mở rộng để lấy toàn bộ avatar

                    avatarRect = new Rectangle(
                        Math.Max(0, face.X - faceExpansion),
                        Math.Max(0, face.Y - faceExpansion),
                        (int)(face.Width + faceExpansion * 2),
                        (int)((face.Width + faceExpansion * 2) * 4.0 / 3.0)
                    );

                    // Đảm bảo rectangle nằm trong ảnh
                    avatarRect.Width = Math.Min(image.Width - avatarRect.X, avatarRect.Width);
                    avatarRect.Height = Math.Min(image.Height - avatarRect.Y, avatarRect.Height);
                }
                else
                {
                    // Nếu không phát hiện được khuôn mặt, thử phương pháp tìm kiếm contour
                    Mat edges = new Mat();
                    CvInvoke.Canny(gray, edges, 50, 150);

                    using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                    {
                        CvInvoke.FindContours(edges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

                        // Tìm các contour hình chữ nhật có tỷ lệ gần với 3:4
                        double targetRatio = 3.0 / 4.0; // Tỷ lệ ảnh 3x4
                        double bestRatioDiff = double.MaxValue;
                        Rectangle bestRect = Rectangle.Empty;

                        for (int i = 0; i < contours.Size; i++)
                        {
                            using (VectorOfPoint approx = new VectorOfPoint())
                            {
                                CvInvoke.ApproxPolyDP(contours[i], approx,
                                    CvInvoke.ArcLength(contours[i], true) * 0.02, true);

                                // Kiểm tra nếu là hình chữ nhật (4 đỉnh)
                                if (approx.Size == 4)
                                {
                                    Rectangle rect = CvInvoke.BoundingRectangle(contours[i]);

                                    // Bỏ qua các hình chữ nhật quá nhỏ
                                    if (rect.Width < 40 || rect.Height < 50)
                                        continue;

                                    // Kiểm tra tỷ lệ kích thước
                                    double ratio = (double)rect.Height / rect.Width;
                                    double ratioDiff = Math.Abs(ratio - targetRatio);

                                    // Tìm hình chữ nhật có tỷ lệ gần với 3:4 nhất
                                    if (ratioDiff < bestRatioDiff)
                                    {
                                        bestRatioDiff = ratioDiff;
                                        bestRect = rect;
                                    }
                                }
                            }
                        }

                        // Nếu tìm thấy hình chữ nhật phù hợp
                        if (bestRect != Rectangle.Empty && bestRatioDiff < 0.3)
                        {
                            // Điều chỉnh tọa độ để phù hợp với ảnh gốc
                            bestRect.X += leftRegion.X;
                            bestRect.Y += leftRegion.Y;
                            avatarRect = bestRect;
                        }
                    }
                }

                // Nếu không tìm thấy avatar bằng cả hai phương pháp, thử phương pháp cuối: 
                // giả định avatar nằm ở góc trái phía trên với tỷ lệ 3:4
                if (avatarRect.IsEmpty)
                {
                    // Giả định avatar chiếm khoảng 1/4 chiều cao của CCCD
                    int estimatedAvatarHeight = image.Height / 4;
                    int estimatedAvatarWidth = (int)(estimatedAvatarHeight * 3.0 / 4.0);

                    // Giả định avatar nằm ở góc trái trên với một số padding
                    int paddingX = image.Width / 50; // 2% chiều rộng
                    int paddingY = image.Height / 25; // 4% chiều cao

                    avatarRect = new Rectangle(
                        paddingX,
                        paddingY,
                        estimatedAvatarWidth,
                        estimatedAvatarHeight
                    );
                }

                // Vẽ hình chữ nhật xung quanh avatar đã phát hiện
                //CvInvoke.Rectangle(outputImage, avatarRect, new MCvScalar(0, 255, 0), 2);

                // Cắt vùng avatar
                Mat avatarImage = new Mat(image, avatarRect);

                // Lưu ảnh avatar
                //CvInvoke.Imwrite("avatar_3x4.jpg", avatarImage);

                // Hiển thị ảnh avatar và ảnh gốc có đánh dấu
                //CvInvoke.Imshow("Avatar 3x4", avatarImage);
                //CvInvoke.Imshow("CCCD with Detected Avatar", outputImage);
                //CvInvoke.WaitKey(0);

                // Lưu ảnh avatar vào một file tạm
                string avatarTempPath = Path.GetTempFileName() + ".jpg";
                CvInvoke.Imwrite(avatarTempPath, avatarImage);

                // Tạo IFormFile từ file ảnh avatarTempPath
                var fileStream = new FileStream(avatarTempPath, FileMode.Open, FileAccess.Read);
                var formFile = new FormFile(
                    fileStream,
                    0,
                    fileStream.Length,
                    "avatar",
                    Path.GetFileName(avatarTempPath))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };

                // Xóa file tạm
                File.Delete(tempFilePath);

                return formFile;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi nhận diện ảnh: " + ex.Message);
                return null;
            }
        }
    }
}
