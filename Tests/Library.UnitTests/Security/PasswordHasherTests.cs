using DataLayer.Security;

namespace Library.UnitTests.Security;

/// Kiểm thử đơn vị bộ băm và xác thực mật khẩu (PasswordHasher).
public sealed class PasswordHasherTests
{
    private readonly PasswordHasher _sut = new();

    /// Kiểm tra kết quả băm không chứa mật khẩu thô và hàm Verify xác minh chính xác mật khẩu đúng/sai.
    [Fact]
    public void Hash_KhongChuaPlaintext_VaXacMinhDung()
    {
        string hash = _sut.Hash("MatKhau@123");
        Assert.DoesNotContain("MatKhau@123", hash);
        Assert.True(_sut.Verify("MatKhau@123", hash));
        Assert.False(_sut.Verify("sai", hash));
    }

    /// Kiểm tra cùng một mật khẩu nhưng mỗi lần băm tạo ra chuỗi hash khác nhau nhờ salt ngẫu nhiên.
    [Fact]
    public void Hash_CungMatKhau_TaoSaltKhacNhau() =>
        Assert.NotEqual(_sut.Hash("MatKhau@123"), _sut.Hash("MatKhau@123"));

    /// Kiểm tra khi xác minh với chuỗi hash bị hỏng hoặc không đúng định dạng thì trả về false an toàn, không ném ngoại lệ.
    [Theory]
    [InlineData("")]
    [InlineData("khong-phai-hash")]
    [InlineData("PBKDF2-SHA256$abc$x$y")]
    public void Verify_HashLoi_TuChoiKhongCrash(string hash) => Assert.False(_sut.Verify("abc", hash));

    /// Kiểm tra nhận diện mật khẩu chưa mã hóa (Legacy plaintext) cần được nâng cấp (rehash) sang dạng băm PBKDF2.
    [Fact]
    public void NeedsRehash_PlaintextCanNangCap() => Assert.True(_sut.NeedsRehash("legacy"));
}



