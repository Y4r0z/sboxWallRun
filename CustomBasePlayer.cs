using Sandbox.UI;

namespace Sandbox
{
	public partial class CustomBasePlayer : Player
	{
		[Net, Predicted]
		public CustomController customController { get; set; }

		public override CustomController GetActiveController()
		{	
			return customController;
		}
		
	}
}

