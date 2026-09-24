using System.Windows.Forms;

namespace Presentation.Helpers;

internal static class CurrentUserAvatarHelper
{
    public static void Apply(PictureBox pictureBox)
    {
        if (DesignModeHelper.IsDesignMode(pictureBox)) return;

        int width = Math.Max(24, pictureBox.ClientSize.Width);
        int height = Math.Max(24, pictureBox.ClientSize.Height);
        string displayName = string.IsNullOrWhiteSpace(CurrentUser.HoTen)
            ? CurrentUser.TenDangNhap
            : CurrentUser.HoTen;

        Image avatar = CurrentUser.LaTaiKhoanDocGia
            ? DatabaseImageHelper.LoadReaderAvatar(CurrentUser.AnhDaiDien, displayName, width, height)
            : DatabaseImageHelper.LoadStaffAvatar(CurrentUser.AnhDaiDien, displayName, width, height);

        Image? oldImage = pictureBox.Image;
        pictureBox.BackgroundImage = null;
        pictureBox.Image = avatar;
        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox.BorderStyle = BorderStyle.None;

        if (oldImage != null && !ReferenceEquals(oldImage, avatar))
        {
            oldImage.Dispose();
        }
    }
}
