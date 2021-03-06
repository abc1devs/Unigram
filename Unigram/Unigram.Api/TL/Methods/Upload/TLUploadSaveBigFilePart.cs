// <auto-generated/>
using System;

namespace Telegram.Api.TL.Methods.Upload
{
	/// <summary>
	/// RCP method upload.saveBigFilePart.
	/// Returns <see cref="Telegram.Api.TL.TLBool"/>
	/// </summary>
	public partial class TLUploadSaveBigFilePart : TLObject
	{
		public Int64 FileId { get; set; }
		public Int32 FilePart { get; set; }
		public Int32 FileTotalParts { get; set; }
		public Byte[] Bytes { get; set; }

		public TLUploadSaveBigFilePart() { }
		public TLUploadSaveBigFilePart(TLBinaryReader from)
		{
			Read(from);
		}

		public override TLType TypeId { get { return TLType.UploadSaveBigFilePart; } }

		public override void Read(TLBinaryReader from)
		{
			FileId = from.ReadInt64();
			FilePart = from.ReadInt32();
			FileTotalParts = from.ReadInt32();
			Bytes = from.ReadByteArray();
		}

		public override void Write(TLBinaryWriter to)
		{
			to.Write(0xDE7B673D);
			to.Write(FileId);
			to.Write(FilePart);
			to.Write(FileTotalParts);
			to.WriteByteArray(Bytes);
		}
	}
}