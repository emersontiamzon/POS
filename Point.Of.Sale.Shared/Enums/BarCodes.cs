using System.ComponentModel;

namespace Point.Of.Sale.Shared.Enums;

public enum BarCodes
{
    [Description("None")] None = 0,
    [Description("Code39")] Code39 = 1,
    [Description("Code128")] Code128,
    [Description("Interleaved2of5")] Interleaved2of5,
    [Description("EAN13")] EAN13,
    [Description("EAN8")] EAN8,
    [Description("UPC")] UPC,
    [Description("UPCA")] UPCA,
    [Description("UPCE")] UPCE,
    [Description("ITF14")] ITF14,
    [Description("Codabar")] Codabar,
    [Description("PostNet")] PostNet,
    [Description("Bookland")] Bookland,
    [Description("PDF417")] PDF417,
    [Description("DataMatrix")] DataMatrix,
    [Description("QRCode")] QRCode,
}
