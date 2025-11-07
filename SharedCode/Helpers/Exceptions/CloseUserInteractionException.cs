namespace SharedCode.Helpers.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class CloseUserInteractionException : Exception
	{
		public CloseUserInteractionException()
		{
		}

		public CloseUserInteractionException(string message)
			: base(message)
		{
		}

		public CloseUserInteractionException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected CloseUserInteractionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}