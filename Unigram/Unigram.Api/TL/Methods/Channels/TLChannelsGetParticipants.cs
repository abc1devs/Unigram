// <auto-generated/>
using System;

namespace Telegram.Api.TL.Methods.Channels
{
	/// <summary>
	/// RCP method channels.getParticipants.
	/// Returns <see cref="Telegram.Api.TL.TLChannelsChannelParticipants"/>
	/// </summary>
	public partial class TLChannelsGetParticipants : TLObject
	{
		public TLInputChannelBase Channel { get; set; }
		public TLChannelParticipantsFilterBase Filter { get; set; }
		public Int32 Offset { get; set; }
		public Int32 Limit { get; set; }

		public TLChannelsGetParticipants() { }
		public TLChannelsGetParticipants(TLBinaryReader from)
		{
			Read(from);
		}

		public override TLType TypeId { get { return TLType.ChannelsGetParticipants; } }

		public override void Read(TLBinaryReader from)
		{
			Channel = TLFactory.Read<TLInputChannelBase>(from);
			Filter = TLFactory.Read<TLChannelParticipantsFilterBase>(from);
			Offset = from.ReadInt32();
			Limit = from.ReadInt32();
		}

		public override void Write(TLBinaryWriter to)
		{
			to.Write(0x24D98F92);
			to.WriteObject(Channel);
			to.WriteObject(Filter);
			to.Write(Offset);
			to.Write(Limit);
		}
	}
}