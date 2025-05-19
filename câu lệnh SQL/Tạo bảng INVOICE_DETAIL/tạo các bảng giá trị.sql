-- Bảng KI_HIEU_MAU_SO_HOA_DON
CREATE TABLE KI_HIEU_MAU_SO_HOA_DON (
    kiHieuMauSoHoaDon decimal(18, 0) NOT NULL,
    kiHieuMauSoHoaDonName nvarchar(MAX) NOT NULL
);

-- Bảng TRANG_THAI_HOA_DON
CREATE TABLE TRANG_THAI_HOA_DON (
    trangThaiHoaDon decimal(18, 0) NOT NULL,
    trangThaiHoaDonName nvarchar(MAX) NOT NULL
);

-- Bảng TRANG_THAI_PHE_DUYET
CREATE TABLE TRANG_THAI_PHE_DUYET (
    trangThaiPheDuyet decimal(18, 0) NOT NULL,
    trangThaiPheDuyetName nvarchar(MAX) NOT NULL
);

-- Bảng LOAI_HOA_DON
CREATE TABLE LOAI_HOA_DON (
    loaiHoaDon decimal(18, 0) NOT NULL,
    loaiHoaDonName nvarchar(MAX) NOT NULL
);

-- Bảng PHUONG_THUC_NHAP
CREATE TABLE PHUONG_THUC_NHAP (
    phuongThucNhap decimal(18, 0) NOT NULL,
    phuongThucNhapName nvarchar(MAX) NOT NULL
);

-- Bảng CHECK_TRANG_THAI_XU_LY
CREATE TABLE CHECK_TRANG_THAI_XU_LY (
    checkTrangThaiXuLy decimal(18, 0) NOT NULL,
    checkTrangThaiXuLyName nvarchar(MAX) NOT NULL
);

-- Bảng CHECK_MST_NGUOI_MUA
CREATE TABLE CHECK_MST_NGUOI_MUA (
    checkMstNguoiMua decimal(18, 0) NOT NULL,
    checkMstNguoiMuaName nvarchar(MAX) NOT NULL
);

-- Bảng CHECK_DIA_CHI_NGUOI_MUA
CREATE TABLE CHECK_DIA_CHI_NGUOI_MUA (
    checkDiaChiNguoiMua decimal(18, 0) NOT NULL,
    checkDiaChiNguoiMuaName nvarchar(MAX) NOT NULL
);

-- Bảng CHECK_TEN_NGUOI_MUA
CREATE TABLE CHECK_TEN_NGUOI_MUA (
    checkTenNguoiMua decimal(18, 0) NOT NULL,
    checkTenNguoiMuaName nvarchar(MAX) NOT NULL
);

-- Bảng CHECK_HOA_DON_KY_DIEN_TU
CREATE TABLE CHECK_HOA_DON_KY_DIEN_TU (
    checkHDonKyDienTu decimal(18, 0) NOT NULL,
    checkHDonKyDienTuName nvarchar(MAX) NOT NULL
);

-- Bảng KIEM_TRA_CHUNG_THU
CREATE TABLE KIEM_TRA_CHUNG_THU (
    kiemTraChungThu decimal(18, 0) NOT NULL,
    kiemTraChungThuName nvarchar(MAX) NOT NULL
);

-- Bảng KIEM_TRA_TEN_NGUOI_BAN
CREATE TABLE KIEM_TRA_TEN_NGUOI_BAN (
    kiemTraTenNban decimal(18, 0) NOT NULL,
    kiemTraTenNbanName nvarchar(MAX) NOT NULL
);

-- Bảng KIEM_TRA_MST_NGUOI_BAN
CREATE TABLE KIEM_TRA_MST_NGUOI_BAN (
    kiemTraMstNban decimal(18, 0) NOT NULL,
    kiemTraMstNbanName nvarchar(MAX) NOT NULL
);

-- Bảng KIEM_TRA_HOAT_DONG_NGUOI_BAN
CREATE TABLE KIEM_TRA_HOAT_DONG_NGUOI_BAN (
    kiemTraHoatDongNban decimal(18, 0) NOT NULL,
    kiemTraHoatDongNbanName nvarchar(MAX) NOT NULL
);
