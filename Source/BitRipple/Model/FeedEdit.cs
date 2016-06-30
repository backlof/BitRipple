using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Model
{
	public enum FeedEditType
	{
		New,
		Edit
	}

	public class FeedEdit
	{
		public Feed Feed { get; set; }
		public FeedEditType Type { get; set; }
	}
}
