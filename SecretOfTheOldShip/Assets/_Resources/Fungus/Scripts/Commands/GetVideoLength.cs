using UnityEngine;
using UnityEngine.UI;

namespace Fungus
{
	[CommandInfo("Video",
		"Get Video Length",
		"Gets the length of a video clip and puts it in a float variable.")]
	public class GetVideoLength : Command 
	{
		public UnityEngine.Video.VideoPlayer videoPlayer;

		[Tooltip("Float variable to store the length of the video clip in.")]
		[VariableProperty(typeof(FloatVariable))]
		[SerializeField] protected FloatVariable variable;

		private UnityEngine.Video.VideoClip videoClip;

		private SetVariable setVariableCommand;

		public override void OnEnter() 
		{
			LengthToVariable ();

			Continue();
		}

		private void LengthToVariable (){
			videoClip = videoPlayer.clip;
			setVariableCommand.SetNewVariable (variable, SetOperator.Assign, videoClip.length);
			setVariableCommand.OnEnter ();
		}
	}
}